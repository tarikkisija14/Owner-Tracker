using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
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
                    new AuditService(db).Archive(
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