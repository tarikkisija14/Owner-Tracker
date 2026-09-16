namespace OwnerTrack.Infrastructure.Database
{
    public static class TransactionHelper
    {
        public static void Execute(OwnerTrackDbContext db, Action<OwnerTrackDbContext> work)
        {
            using var tx = db.Database.BeginTransaction();
            try
            {
                work(db);
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public static void SaveWithAudit(OwnerTrackDbContext db, Action logAudit)
        {
            Execute(db, d =>
            {
                d.SaveChanges();
                logAudit();
                d.SaveChanges();
            });
        }
    }
}