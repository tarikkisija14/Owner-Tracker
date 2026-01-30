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

            // Djelatnost → Klijenti (1:N)
            modelBuilder.Entity<Klijent>()
                .HasOne(k => k.Djelatnost)
                .WithMany(d => d.Klijenti)
                .HasForeignKey(k => k.SifraDjelativnosti)
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

            // UNIQUE constraints
            modelBuilder.Entity<Klijent>()
                .HasIndex(k => k.Naziv).IsUnique();
            modelBuilder.Entity<Klijent>()
                .HasIndex(k => k.IdBroj).IsUnique();
            modelBuilder.Entity<Vlasnik>()
                .HasIndex(v => new { v.KlijentId, v.ImePrezime }).IsUnique();
            modelBuilder.Entity<Direktor>()
                .HasIndex(d => d.KlijentId).IsUnique();
            modelBuilder.Entity<Ugovor>()
                .HasIndex(u => u.KlijentId).IsUnique();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlite($"Data Source=C:\\Users\\tarik\\Desktop\\Job\\Firme.db");
            }
        }

    }
}
