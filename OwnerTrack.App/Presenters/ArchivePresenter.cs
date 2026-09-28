using Microsoft.EntityFrameworkCore;
using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App.Presenters
{

    public sealed class ArchivePresenter
    {
        public void ArchiveKlijent(int id, Action? onSuccess = null)
        {
            ExecuteArchive(
                db =>
                {
                    var k = db.Klijenti.Find(id);
                    if (k is null) return;

                    var audit = new AuditService(db);

                    // Cascade the archive to still-active owners/directors so their
                    // Status/Obrisan reflect the parent's archived state, instead of
                    // leaving them AKTIVAN while the client itself is ARHIVIRAN.
                    var activeVlasnici = db.Vlasnici
                        .Where(v => v.KlijentId == id && v.Status != StatusEntiteta.ARHIVIRAN)
                        .ToList();
                    foreach (var v in activeVlasnici)
                        audit.Archive(v, "Vlasnici", v.Id,
                            string.Format(UiMessages.AuditArchivedVlasnik, v.ImePrezime));

                    var activeDirektori = db.Direktori
                        .Where(d => d.KlijentId == id && d.Status != StatusEntiteta.ARHIVIRAN)
                        .ToList();
                    foreach (var d in activeDirektori)
                        audit.Archive(d, "Direktori", d.Id,
                            string.Format(UiMessages.AuditArchivedDirektor, d.ImePrezime));

                    audit.Archive(
                        k, "Klijenti", id,
                        string.Format(UiMessages.AuditArchivedKlijent, k.Naziv));
                    db.SaveChanges();
                },
                onSuccess: onSuccess,
                successMessage: UiMessages.ArchiveKlijentSuccess);
        }

        public void ArchiveVlasnik(int vlasnikId, Action? onSuccess = null)
        {
            ExecuteArchive(
                db =>
                {
                    var v = db.Vlasnici.Find(vlasnikId);
                    if (v is null) return;
                    new AuditService(db).Archive(
                        v, "Vlasnici", vlasnikId,
                        string.Format(UiMessages.AuditArchivedVlasnik, v.ImePrezime));
                    db.SaveChanges();
                },
                onSuccess: onSuccess,
                successMessage: UiMessages.ArchiveVlasnikSuccess);
        }

        public void ArchiveDirektor(int direktorId, Action? onSuccess = null)
        {
            ExecuteArchive(
                db =>
                {
                    var d = db.Direktori.Find(direktorId);
                    if (d is null) return;
                    new AuditService(db).Archive(
                        d, "Direktori", direktorId,
                        string.Format(UiMessages.AuditArchivedDirektor, d.ImePrezime));
                    db.SaveChanges();
                },
                onSuccess: onSuccess,
                successMessage: UiMessages.ArchiveDirektorSuccess);
        }

        

        private static void ExecuteArchive(
            Action<OwnerTrackDbContext> archiveAction,
            Action? onSuccess,
            string successMessage)
        {
            try
            {
                using var db = DbContextFactory.Create();
                TransactionHelper.Execute(db, archiveAction);

                if (!string.IsNullOrEmpty(successMessage))
                    MessageBox.Show(successMessage);

                onSuccess?.Invoke();
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex);
            }
        }
    }
}