using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwnerTrack.Data.Entities
{
    public class Klijent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Naziv { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string IdBroj { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Adresa { get; set; }

        [Required]
        [ForeignKey("Djelatnost")]
        public string SifraDjelatnosti { get; set; } = string.Empty;

        public DateTime? DatumUspostave { get; set; }

        // 🔴 OVO JE BIO PROBLEM (ordinal 6)
        [StringLength(50)]
        public string? VrstaKlijenta { get; set; }

        public DateTime? DatumOsnivanja { get; set; }

        [StringLength(50)]
        public string? Velicina { get; set; }

        [StringLength(10)]
        public string? PepRizik { get; set; }

        [StringLength(10)]
        public string? UboRizik { get; set; }

        [StringLength(10)]
        public string? GotovinaRizik { get; set; }

        [StringLength(10)]
        public string? GeografskiRizik { get; set; }

        [StringLength(100)]
        public string? UkupnaProcjena { get; set; }

        public DateTime? DatumProcjene { get; set; }

        [StringLength(255)]
        public string? OvjeraCr { get; set; }

        [StringLength(50)]
        public string? Status { get; set; } = "AKTIVAN";

        [StringLength(1000)]
        public string? Napomena { get; set; }

        public DateTime? Kreiran { get; set; }
        public DateTime? Azuriran { get; set; }

        public virtual Djelatnost? Djelatnost { get; set; }
        public virtual ICollection<Vlasnik> Vlasnici { get; set; } = new List<Vlasnik>();
        public virtual ICollection<Direktor> Direktori { get; set; } = new List<Direktor>();
        public virtual Ugovor? Ugovor { get; set; }
    }
}
