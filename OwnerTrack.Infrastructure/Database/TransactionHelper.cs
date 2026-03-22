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
    }
}