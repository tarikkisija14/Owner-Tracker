using OwnerTrack.Data.Entities;
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
            _db.SaveChanges();
            return true;
        }

        /// <summary>Korisničko ime: bez praznih znakova unutar imena (login ga samo trimuje).</summary>
        public static bool IsValidUsername(string username) =>
            username.Length > 0 && username.Length <= 100 && !username.Any(char.IsWhiteSpace);

        /// <summary>False ako korisnik s tim korisničkim imenom (bez obzira na velika/mala slova) već postoji.</summary>
        public bool CreateUser(string username, string? displayName, string password)
        {
            if (_db.Korisnici.Any(k => k.KorisnickoIme == username))
                return false;

            var (hash, salt) = PasswordHasher.Hash(password);
            _db.Korisnici.Add(new Korisnik
            {
                KorisnickoIme = username,
                PrikaznoIme = string.IsNullOrWhiteSpace(displayName) ? null : displayName.Trim(),
                PasswordHash = hash,
                PasswordSalt = salt,
                Aktivan = true,
                Kreiran = DateTime.Now,
            });

            try
            {
                _db.SaveChanges();
                return true;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                // UNIQUE na KorisnickoIme — istovremeni insert istog imena.
                return false;
            }
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

            return new AuthenticatedUser(
                korisnik.Id,
                korisnik.KorisnickoIme,
                string.IsNullOrWhiteSpace(korisnik.PrikaznoIme) ? korisnik.KorisnickoIme : korisnik.PrikaznoIme,
                korisnik.Aktivan);
        }
    }
}
