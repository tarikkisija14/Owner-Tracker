using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;

namespace OwnerTrack.Infrastructure.Services
{
    public class WarningAcknowledgementService
    {
        private readonly OwnerTrackDbContext _db;

        public WarningAcknowledgementService(OwnerTrackDbContext db)
        {
            _db = db;
        }

        public HashSet<(string EntityType, int EntityId, DateTime DatumIsteka)> GetAcknowledgedKeys() =>
            _db.WarningAcknowledgements
                .AsNoTracking()
                .Select(a => new { a.EntityType, a.EntityId, a.DatumIsteka })
                .AsEnumerable()
                .Select(a => (a.EntityType, a.EntityId, a.DatumIsteka))
                .ToHashSet();

        public void Acknowledge(string entityType, int entityId, DateTime datumIsteka, string? napomena, string nazivFirme, string imePrezime)
        {
            // Provjera je unutar transakcije (zaključane za upis) pa je idempotentna: ako je
            // drugi korisnik u međuvremenu označio isto upozorenje, ovo je samo prazan uspjeh
            // umjesto greške zbog UNIQUE indeksa.
            TransactionHelper.Execute(_db, db =>
            {
                bool alreadyAcknowledged = db.WarningAcknowledgements.Any(a =>
                    a.EntityType == entityType && a.EntityId == entityId && a.DatumIsteka == datumIsteka);
                if (alreadyAcknowledged) return;

                var ack = new WarningAcknowledgement
                {
                    EntityType = entityType,
                    EntityId = entityId,
                    DatumIsteka = datumIsteka,
                    Napomena = string.IsNullOrWhiteSpace(napomena) ? null : napomena.Trim(),
                    Korisnik = AuditContext.CurrentUsername,
                };
                db.WarningAcknowledgements.Add(ack);
                db.SaveChanges();

                string opis = string.IsNullOrEmpty(napomena)
                    ? $"Upozorenje označeno kao pregledano: {entityType} '{imePrezime}', klijent '{nazivFirme}'."
                    : $"Upozorenje označeno kao pregledano: {entityType} '{imePrezime}', klijent '{nazivFirme}' (napomena: {napomena.Trim()}).";

                new AuditService(db).Log("WarningAcknowledgements", ack.Id, AuditConstants.Pregledano, opis);
                db.SaveChanges();
            });
        }
    }
}
