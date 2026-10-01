using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace OwnerTrack.Infrastructure.Database
{
    /// <summary>
    /// Poslovno pravilo prekršeno unutar transakcije (npr. duplikat koji je u
    /// međuvremenu unio drugi korisnik) — poruka je namijenjena korisniku.
    /// </summary>
    public sealed class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }

    public static class TransactionHelper
    {
        // Obična (DEFERRED) transakcija prvo čita pa tek kasnije traži zaključavanje
        // za upis — ako je u međuvremenu drugi korisnik upisao, SQLite odmah vraća
        // "database is locked" bez čekanja (busy timeout se ne primjenjuje na
        // nadogradnju čitanja u upis). BEGIN IMMEDIATE zaključava za upis odmah
        // (čeka do busy timeouta), pa se sve što se zatim čita u transakciji čita
        // nad stanjem koje niko ne može promijeniti dok traje — provjere duplikata
        // i upis su tako atomarni između korisnika.
        public static IDbContextTransaction BeginImmediate(OwnerTrackDbContext db)
        {
            var conn = (SqliteConnection)db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            var raw = conn.BeginTransaction(deferred: false);
            return db.Database.UseTransaction(raw)
                ?? throw new InvalidOperationException("Transakcija nije pokrenuta.");
        }

        public static void Execute(OwnerTrackDbContext db, Action<OwnerTrackDbContext> work)
        {
            using var tx = BeginImmediate(db);
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

        // validate se izvršava NAKON što je transakcija zaključala bazu, a PRIJE
        // upisa — tu idu provjere koje moraju biti istinite u trenutku upisa
        // (duplikati, postojanje roditeljskog zapisa). Baca BusinessRuleException.
        public static void SaveWithAudit(OwnerTrackDbContext db, Action logAudit, Action? validate = null)
        {
            Execute(db, d =>
            {
                validate?.Invoke();
                d.SaveChanges();
                logAudit();
                d.SaveChanges();
            });
        }
    }
}
