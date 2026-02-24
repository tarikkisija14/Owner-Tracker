using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace OwnerTrack.Infrastructure
{
    public static class DbContextFactory
    {
        // AppData\Local je uvijek writable, čak i bez admin prava
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
                .UseSqlite(ConnectionString)
                .Options;
            return new OwnerTrackDbContext(options);
        }
    }
}