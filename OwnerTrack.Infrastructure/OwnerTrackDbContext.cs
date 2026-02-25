using OwnerTrack.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace OwnerTrack.Infrastructure
{
    public class OwnerTrackDbContext:DbContext
    {
        public OwnerTrackDbContext()
        {
        }
        public OwnerTrackDbContext(DbContextOptions<OwnerTrackDbContext> options)
          : base(options)
        {
        }
        public DbSet<Djelatnost> Djelatnosti { get; set; }
        public DbSet<Klijent> Klijenti { get; set; }
        public DbSet<Vlasnik> Vlasnici { get; set; }
        public DbSet<Direktor> Direktori { get; set; }
        public DbSet<Ugovor> Ugovori { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Djelatnost → Klijenti (1:N)
            modelBuilder.Entity<Klijent>()
                .HasOne(k => k.Djelatnost)
                .WithMany(d => d.Klijenti)
                .HasForeignKey(k => k.SifraDjelatnosti)
                .HasPrincipalKey(d => d.Sifra)
                .OnDelete(DeleteBehavior.Restrict);

            // Klijent → Vlasnici (1:N)
            modelBuilder.Entity<Vlasnik>()
                .HasOne(v => v.Klijent)
                .WithMany(k => k.Vlasnici)
                .HasForeignKey(v => v.KlijentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Klijent → Direktori (1:N)
            modelBuilder.Entity<Direktor>()
                .HasOne(d => d.Klijent)
                .WithMany(k => k.Direktori)
                .HasForeignKey(d => d.KlijentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Klijent → Ugovor (1:1)
            modelBuilder.Entity<Ugovor>()
                .HasOne(u => u.Klijent)
                .WithOne(k => k.Ugovor)
                .HasForeignKey<Ugovor>(u => u.KlijentId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Klijent>()
                .HasIndex(k => k.Naziv);
            modelBuilder.Entity<Klijent>()
                .HasIndex(k => k.IdBroj);
            modelBuilder.Entity<Vlasnik>()
                .HasIndex(v => new { v.KlijentId, v.ImePrezime });

            modelBuilder.Entity<Direktor>()
                .HasIndex(d => d.KlijentId);
            modelBuilder.Entity<Ugovor>()
                .HasIndex(u => u.KlijentId).IsUnique();

            
            modelBuilder.Entity<Klijent>()
                .HasQueryFilter(k => k.Obrisan == null);
            modelBuilder.Entity<Vlasnik>()
                .HasQueryFilter(v => v.Obrisan == null);
            modelBuilder.Entity<Direktor>()
                .HasQueryFilter(d => d.Obrisan == null);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {

                throw new InvalidOperationException(
                    "DbContext nije konfigurisan. Proslijedi DbContextOptions pri kreiranju instance.");
            }
        }

    }
}
