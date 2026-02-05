using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerTrack.Data.Entities
{
    public class Vlasnik
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Klijent")]
        public int KlijentId { get; set; }

        [Required]
        [StringLength(255)]
        public string ImePrezime { get; set; }

        public DateTime? DatumValjanostiDokumenta { get; set; }  

        [Range(0, 100)]
        public decimal ProcetatVlasnistva { get; set; }

        public DateTime? DatumUtvrdjivanja { get; set; }

        [StringLength(255)]
        public string IzvorPodatka { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "AKTIVAN";

        public DateTime Kreiran { get; set; } = DateTime.Now;


        public virtual Klijent Klijent { get; set; }
    }
}
