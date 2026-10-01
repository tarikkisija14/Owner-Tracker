using System.Text.Json;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Models;

namespace OwnerTrack.Infrastructure.Services
{
    public class RizikObrazacService
    {
        private readonly OwnerTrackDbContext _db;

        public RizikObrazacService(OwnerTrackDbContext db)
        {
            _db = db;
        }

        public RizikObrazacPodaci Load(int klijentId) => Load(klijentId, out _);

        // out version — Klijent.Version u trenutku učitavanja, da ga pozivalac
        // (FrmRizikObrazac) može čuvati i proslijediti nazad u Save kao
        // expectedVersion za optimistic-concurrency provjeru.
        public RizikObrazacPodaci Load(int klijentId, out int version)
        {
            var row = _db.Klijenti
                .Where(k => k.Id == klijentId)
                .Select(k => new { k.RizikObrazacJson, k.Version })
                .FirstOrDefault();

            version = row?.Version ?? 0;

            if (row is null || string.IsNullOrWhiteSpace(row.RizikObrazacJson))
                return new RizikObrazacPodaci();

            return JsonSerializer.Deserialize<RizikObrazacPodaci>(row.RizikObrazacJson) ?? new RizikObrazacPodaci();
        }

        // expectedVersion je Klijent.Version onako kako ga je vidio pozivalac
        // (npr. FrmRizikObrazac pri otvaranju forme) — postavlja se na entitetu
        // prije SaveChanges da EF-ov optimistic-concurrency check ("WHERE
        // Version = expectedVersion") stvarno uporedi sa onim što je korisnik
        // vidio, a ne sa vrijednošću koju upravo pročita ovaj isti poziv (što bi
        // učinilo provjeru beskorisnom — uvijek bi se poklapala sa samom sobom).
        // Vraća novu verziju da pozivalac može ažurirati svoje lokalno stanje za
        // sljedeći Save u istoj sesiji forme.
        public int Save(int klijentId, RizikObrazacPodaci podaci, int expectedVersion)
        {
            var klijent = _db.Klijenti.FirstOrDefault(k => k.Id == klijentId)
                ?? throw new InvalidOperationException($"Klijent ID={klijentId} nije pronađen.");

            _db.Entry(klijent).Property(k => k.Version).OriginalValue = expectedVersion;

            string? prijeUkupna = klijent.UkupnaProcjena;
            string? prijeDatum = klijent.DatumProcjene?.ToString("dd.MM.yyyy");

            klijent.RizikObrazacJson = JsonSerializer.Serialize(podaci);

            // Profil, evidencije, dashboard i PDF čitaju Klijent.UkupnaProcjena/DatumProcjene,
            // pa se rezultat obrasca prenosi na ta polja (isti SaveChanges). Prazna procjena
            // u obrascu ne briše postojeću vrijednost na klijentu.
            if (!string.IsNullOrWhiteSpace(podaci.UkupnaProcjena))
            {
                klijent.UkupnaProcjena = podaci.UkupnaProcjena;
                if (podaci.DatumProcjene.HasValue)
                    klijent.DatumProcjene = podaci.DatumProcjene;
            }

            // Procjene četiri bloka obrasca (VIŠE/NIŽE) prenose se u kolone rizika
            // na klijentu kao DA/NE; neodređena procjena daje prazno polje.
            var prijeBlokovi = (klijent.OpciIndikatoriRizika, klijent.IndikatoriIdentifikacijeRizika,
                                klijent.IndikatoriTransakcijaRizika, klijent.GeografskiRizik);
            klijent.OpciIndikatoriRizika = ToDaNe(podaci.ProcjenaOpcihIndikatora);
            klijent.IndikatoriIdentifikacijeRizika = ToDaNe(podaci.ProcjenaIdentifikacije);
            klijent.IndikatoriTransakcijaRizika = ToDaNe(podaci.ProcjenaTransakcija);
            klijent.GeografskiRizik = ToDaNe(podaci.ProcjenaGeografskog);

            klijent.Azuriran = DateTime.Now;
            klijent.Version = expectedVersion + 1;

            string? poslijeDatum = klijent.DatumProcjene?.ToString("dd.MM.yyyy");
            var promjene = new List<string>();
            void Blok(string naziv, string? prije, string? poslije)
            {
                if (prije != poslije) promjene.Add($"{naziv}: '{prije}' → '{poslije}'");
            }
            Blok("Opći indikatori", prijeBlokovi.OpciIndikatoriRizika, klijent.OpciIndikatoriRizika);
            Blok("Indikatori identifikacije", prijeBlokovi.IndikatoriIdentifikacijeRizika, klijent.IndikatoriIdentifikacijeRizika);
            Blok("Indikatori transakcija", prijeBlokovi.IndikatoriTransakcijaRizika, klijent.IndikatoriTransakcijaRizika);
            Blok("Geografski rizik", prijeBlokovi.GeografskiRizik, klijent.GeografskiRizik);
            if (prijeUkupna != klijent.UkupnaProcjena)
                promjene.Add($"Ukupna procjena: '{prijeUkupna}' → '{klijent.UkupnaProcjena}'");
            if (prijeDatum != poslijeDatum)
                promjene.Add($"Datum procjene: '{prijeDatum}' → '{poslijeDatum}'");

            string opis = promjene.Count == 0
                ? "Ažuriran obrazac za procjenu rizika"
                : "Ažuriran obrazac za procjenu rizika — " + string.Join("; ", promjene);

            new AuditService(_db).LogUpdated("Klijenti", klijentId, opis);
            _db.SaveChanges();

            return klijent.Version;
        }

        private static string? ToDaNe(string? procjena) => procjena switch
        {
            RizikObrazacKriteriji.Vise => DaNeConstants.Da,
            RizikObrazacKriteriji.Nize => DaNeConstants.Ne,
            _ => null,
        };
    }
}
