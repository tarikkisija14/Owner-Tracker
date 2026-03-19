using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using System;

namespace OwnerTrack.Infrastructure.Database
{
    public class OwnerTrackDbContext : DbContext
    {
        public OwnerTrackDbContext() { }

        public OwnerTrackDbContext(DbContextOptions<OwnerTrackDbContext> options)
            : base(options) { }

        public DbSet<Djelatnost> Djelatnosti { get; set; } = null!;
        public DbSet<Klijent> Klijenti { get; set; } = null!;
        public DbSet<Vlasnik> Vlasnici { get; set; } = null!;
        public DbSet<Direktor> Direktori { get; set; } = null!;
        public DbSet<Ugovor> Ugovori { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureRelationships(modelBuilder);
            ConfigureIndexes(modelBuilder);
            ConfigureQueryFilters(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                throw new InvalidOperationException(
                    "DbContext nije konfigurisan. Proslijedi DbContextOptions pri kreiranju instance.");
        }

        private static void ConfigureRelationships(ModelBuilder mb)
        {
            mb.Entity<Klijent>()
                .HasOne(k => k.Djelatnost)
                .WithMany(d => d.Klijenti)
                .HasForeignKey(k => k.SifraDjelatnosti)
                .HasPrincipalKey(d => d.Sifra)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<Vlasnik>()
                .HasOne(v => v.Klijent)
                .WithMany(k => k.Vlasnici)
                .HasForeignKey(v => v.KlijentId)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<Direktor>()
                .HasOne(d => d.Klijent)
                .WithMany(k => k.Direktori)
                .HasForeignKey(d => d.KlijentId)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<Ugovor>()
                .HasOne(u => u.Klijent)
                .WithOne(k => k.Ugovor)
                .HasForeignKey<Ugovor>(u => u.KlijentId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureIndexes(ModelBuilder mb)
        {
            mb.Entity<Klijent>().HasIndex(k => k.Naziv);
            mb.Entity<Klijent>().HasIndex(k => k.IdBroj);
            mb.Entity<Vlasnik>().HasIndex(v => new { v.KlijentId, v.ImePrezime });
            mb.Entity<Direktor>().HasIndex(d => d.KlijentId);
            mb.Entity<Ugovor>().HasIndex(u => u.KlijentId).IsUnique();
        }

        private static void ConfigureQueryFilters(ModelBuilder mb)
        {
            mb.Entity<Klijent>().HasQueryFilter(k => k.Obrisan == null);
            mb.Entity<Vlasnik>().HasQueryFilter(v => v.Obrisan == null);
            mb.Entity<Direktor>().HasQueryFilter(d => d.Obrisan == null);
        }
    }
}