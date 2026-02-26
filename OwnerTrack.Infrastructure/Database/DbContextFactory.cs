using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace OwnerTrack.Infrastructure.Database
{
    public static class DbContextFactory
    {
        public static string DbPath { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "OwnerTrack",
            "Firme.db"
        );

        public static string ConnectionString { get; } = $"Data Source={DbPath}";

        static DbContextFactory()
        {
            string? dir = Path.GetDirectoryName(DbPath);
            if (dir != null)
                Directory.CreateDirectory(dir);
        }

        public static OwnerTrackDbContext Kreiraj()
        {
            var options = new DbContextOptionsBuilder<OwnerTrackDbContext>()
                .UseSqlite(ConnectionString, sqliteOpts =>
                    sqliteOpts.CommandTimeout(30))
                .Options;

            var ctx = new OwnerTrackDbContext(options);

            
            ctx.Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON;");

            return ctx;
        }
    }
}