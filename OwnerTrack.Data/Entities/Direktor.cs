using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerTrack.Data.Entities
{
    public class Direktor : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Klijent")]
        public int KlijentId { get; set; }

        [Required]
        [StringLength(255)]
        public string ImePrezime { get; set; }

        public DateTime? DatumValjanosti { get; set; }

        [StringLength(50)]
        public string TipValjanosti { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "AKTIVAN";

        public DateTime Kreiran { get; set; } = DateTime.Now;
        public DateTime? Obrisan { get; set; }

        [StringLength(13)]
        public string? Jmbg { get; set; }



        public virtual Klijent Klijent { get; set; }
    }
}
