using System;

namespace OwnerTrack.Infrastructure.Database
{
    /// <summary>
    /// Wraps a unit of work in a database transaction, rolling back automatically on failure.
    /// </summary>
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