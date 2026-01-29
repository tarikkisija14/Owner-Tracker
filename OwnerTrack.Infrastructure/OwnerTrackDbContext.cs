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

        public OwnerTrackDbContext(DbContextOptions<OwnerTrackDbContext> options)
          : base(options)
        {
        }
        public DbSet<Djelatnost> Djelatnosti { get; set; }
        public DbSet<Klijent> Klijenti { get; set; }
        public DbSet<Vlasnik> Vlasnici { get; set; }
        public DbSet<Direktor> Direktori { get; set; }
        public DbSet<Ugovor> Ugovori { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -------------------------------
            // Djelatnost → Klijenti (1:N)
            // -------------------------------
            modelBuilder.Entity<Klijent>()
                .HasOne(k => k.Djelatnost)
                .WithMany(d => d.Klijenti)
                .HasForeignKey(k => k.SifraDjelativnosti)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------------
            // Klijent → Vlasnici (1:N)
            // -------------------------------
            modelBuilder.Entity<Vlasnik>()
                .HasOne(v => v.Klijent)
                .WithMany(k => k.Vlasnici)
                .HasForeignKey(v => v.KlijentId)
                .OnDelete(DeleteBehavior.Cascade);

            // UNIQUE constraint za Vlasnik (klijent + ime)
            modelBuilder.Entity<Vlasnik>()
                .HasIndex(v => new { v.KlijentId, v.ImePrezime })
                .IsUnique();

            // -------------------------------
            // Klijent → Direktor (1:1)
            // -------------------------------
            modelBuilder.Entity<Klijent>()
                .HasMany(k => k.Direktori)
                .WithOne(d => d.Klijent)
                .HasForeignKey(d => d.KlijentId);

            // -------------------------------
            // Klijent → Ugovor (1:1)
            // -------------------------------
            modelBuilder.Entity<Klijent>()
                .HasOne(k => k.Ugovor)
                .WithOne(u => u.Klijent)
                .HasForeignKey<Ugovor>(u => u.KlijentId);

            // -------------------------------
            // Indeksi i unique constraints za Klijente
            // -------------------------------
            modelBuilder.Entity<Klijent>()
                .HasIndex(k => k.Naziv)
                .IsUnique();

            modelBuilder.Entity<Klijent>()
                .HasIndex(k => k.IdBroj)
                .IsUnique();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                
                optionsBuilder.UseSqlite(@"Data Source=C:\Users\tarik\Desktop\Job\Firme.db");
            }
        }

    }
}
