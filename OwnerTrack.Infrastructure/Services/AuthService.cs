using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Models;

namespace OwnerTrack.Infrastructure.Services
{
    public class AuthService
    {
        // Hash lažne lozinke, izračunat jednom: kad korisnik ne postoji i dalje
        // radimo isti PBKDF2 posao, da vrijeme odgovora ne otkriva postoji li username.
        private static readonly Lazy<(string Hash, string Salt)> DummyCredentials =
            new(() => PasswordHasher.Hash("dummy-password-for-timing"));

        private readonly OwnerTrackDbContext _db;

        public AuthService(OwnerTrackDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Vraća prijavljenog korisnika ili null — namjerno ne razlikuje
        /// nepostojeći username, pogrešnu lozinku i neaktivan račun.
        /// </summary>
        public const int PasswordLength = 4;

        /// <summary>
        /// Nova lozinka: tačno 4 znaka, bez razmaka/praznih znakova (razmak se ne
        /// računa kao znak). Slova, brojevi i ostali znakovi su dozvoljeni, ali nisu obavezni.
        /// </summary>
        public static bool IsValidNewPassword(string password) =>
            password.Length == PasswordLength && !password.Any(char.IsWhiteSpace);

        /// <summary>False ako trenutna lozinka nije ispravna.</summary>
        public bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            var korisnik = _db.Korisnici.FirstOrDefault(k => k.Id == userId);
            if (korisnik is null || !PasswordHasher.Verify(currentPassword, korisnik.PasswordHash, korisnik.PasswordSalt))
                return false;

            var (hash, salt) = PasswordHasher.Hash(newPassword);
            korisnik.PasswordHash = hash;
            korisnik.PasswordSalt = salt;
            korisnik.LozinkaTekst = newPassword;
            new AuditService(_db).LogUpdated("Korisnici", korisnik.Id,
                $"Promijenjena lozinka korisnika '{korisnik.KorisnickoIme}'");
            _db.SaveChanges();
            return true;
        }

        public enum SetActiveResult { Ok, NotFound, CannotDeactivateSelf, CannotDeactivateLastActive }

        public List<KorisnikInfo> GetUsers() =>
            _db.Korisnici
                .AsNoTracking()
                .OrderBy(k => k.KorisnickoIme)
                .AsEnumerable()
                .Select(k => new KorisnikInfo(
                    k.Id, k.KorisnickoIme,
                    string.IsNullOrWhiteSpace(k.PrikaznoIme) ? k.KorisnickoIme : k.PrikaznoIme,
                    k.Aktivan, k.ZadnjaPrijava, k.LozinkaTekst))
                .ToList();

        /// <summary>
        /// Soft delete korisnika: račun ostaje u bazi (audit zapisi ga i dalje referenciraju),
        /// ali se više ne može prijaviti. Ne dozvoljava deaktivaciju vlastitog računa
        /// ni zadnjeg aktivnog korisnika, da aplikacija ne ostane bez pristupa.
        /// </summary>
        public SetActiveResult SetActive(int actingUserId, int targetUserId, bool active)
        {
            // Provjera "zadnji aktivni korisnik" i upis moraju biti atomarni: bez transakcije
            // bi dva administratora koji istovremeno deaktiviraju jedan drugog oba prošla provjeru.
            var result = SetActiveResult.Ok;
            TransactionHelper.Execute(_db, _ => result = SetActiveCore(actingUserId, targetUserId, active));
            return result;
        }

        private SetActiveResult SetActiveCore(int actingUserId, int targetUserId, bool active)
        {
            var target = _db.Korisnici.FirstOrDefault(k => k.Id == targetUserId);
            if (target is null) return SetActiveResult.NotFound;
            if (target.Aktivan == active) return SetActiveResult.Ok;

            if (!active)
            {
                if (targetUserId == actingUserId)
                    return SetActiveResult.CannotDeactivateSelf;
                if (!_db.Korisnici.Any(k => k.Id != targetUserId && k.Aktivan))
                    return SetActiveResult.CannotDeactivateLastActive;
            }

            target.Aktivan = active;
            new AuditService(_db).Log("Korisnici", target.Id,
                active ? AuditConstants.Aktivirano : AuditConstants.Deaktivirano,
                active
                    ? $"Aktiviran korisnik '{target.KorisnickoIme}'"
                    : $"Deaktiviran korisnik '{target.KorisnickoIme}'");
            _db.SaveChanges();
            return SetActiveResult.Ok;
        }

        /// <summary>Korisničko ime: bez praznih znakova unutar imena (login ga samo trimuje).</summary>
        public static bool IsValidUsername(string username) =>
            username.Length > 0 && username.Length <= 100 && !username.Any(char.IsWhiteSpace);

        /// <summary>False ako korisnik s tim korisničkim imenom (bez obzira na velika/mala slova) već postoji.</summary>
        public bool CreateUser(string username, string? displayName, string password)
        {
            bool created = false;
            try
            {
                TransactionHelper.Execute(_db, db =>
                {
                    if (db.Korisnici.Any(k => k.KorisnickoIme == username))
                        return;

                    var (hash, salt) = PasswordHasher.Hash(password);
                    var novi = new Korisnik
                    {
                        KorisnickoIme = username,
                        PrikaznoIme = string.IsNullOrWhiteSpace(displayName) ? null : displayName.Trim(),
                        PasswordHash = hash,
                        PasswordSalt = salt,
                        LozinkaTekst = password,
                        Aktivan = true,
                        Kreiran = DateTime.Now,
                    };
                    db.Korisnici.Add(novi);
                    db.SaveChanges();
                    new AuditService(db).LogAdded("Korisnici", novi.Id, $"Dodan korisnik '{novi.KorisnickoIme}'");
                    db.SaveChanges();
                    created = true;
                });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                // UNIQUE na KorisnickoIme — istovremeni insert istog imena.
                return false;
            }

            return created;
        }

        public byte[]? GetPhoto(int userId) =>
            _db.Korisnici.Where(k => k.Id == userId).Select(k => k.Slika).FirstOrDefault();

        /// <summary>null uklanja sliku (vraća se placeholder).</summary>
        public void SetPhoto(int userId, byte[]? photo)
        {
            var korisnik = _db.Korisnici.FirstOrDefault(k => k.Id == userId)
                ?? throw new InvalidOperationException("Korisnik nije pronađen.");
            korisnik.Slika = photo;
            _db.SaveChanges();
        }

        public AuthenticatedUser? Authenticate(string username, string password)
        {
            string normalized = username.Trim();
            var korisnik = _db.Korisnici.FirstOrDefault(k => k.KorisnickoIme == normalized);

            if (korisnik is null)
            {
                PasswordHasher.Verify(password, DummyCredentials.Value.Hash, DummyCredentials.Value.Salt);
                return null;
            }

            bool valid = PasswordHasher.Verify(password, korisnik.PasswordHash, korisnik.PasswordSalt);
            if (!valid || !korisnik.Aktivan)
                return null;

            var previousLogin = korisnik.ZadnjaPrijava;
            korisnik.ZadnjaPrijava = DateTime.Now;
            _db.SaveChanges();

            return new AuthenticatedUser(
                korisnik.Id,
                korisnik.KorisnickoIme,
                string.IsNullOrWhiteSpace(korisnik.PrikaznoIme) ? korisnik.KorisnickoIme : korisnik.PrikaznoIme,
                korisnik.Aktivan,
                previousLogin);
        }
    }
}
