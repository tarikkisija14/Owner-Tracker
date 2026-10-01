using System;
using System.ComponentModel.DataAnnotations;

namespace OwnerTrack.Data.Entities
{
    // Lokalni korisnik aplikacije (offline login). Lozinka se nikad ne čuva
    // u čistom tekstu — samo PBKDF2 hash i salt (Base64), vidi AuthService.
    public class Korisnik
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string KorisnickoIme { get; set; } = string.Empty;

        [StringLength(150)]
        public string? PrikaznoIme { get; set; }

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string PasswordSalt { get; set; } = string.Empty;

        // Lozinka u čitljivom obliku — namjerna odluka vlasnika aplikacije: offline
        // aplikacija nema "zaboravljena lozinka" tok, pa se lozinka može pogledati
        // na ekranu Korisnici. null = nepoznata (račun iz vremena prije ove kolone).
        public string? LozinkaTekst { get; set; }

        // Profilna slika (PNG bajtovi); null = prikazuje se placeholder.
        public byte[]? Slika { get; set; }

        public bool Aktivan { get; set; } = true;

        public DateTime Kreiran { get; set; } = DateTime.Now;

        public DateTime? ZadnjaPrijava { get; set; }
    }
}
