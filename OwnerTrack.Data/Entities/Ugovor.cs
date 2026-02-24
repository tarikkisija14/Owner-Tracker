using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerTrack.Data.Entities
{
   public class Ugovor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Klijent")]
        public int KlijentId { get; set; }

        [StringLength(100)]
        public string VrstaUgovora { get; set; }

        [StringLength(100)]
        public string StatusUgovora { get; set; }

        public DateTime? DatumUgovora { get; set; }

        [StringLength(500)]
        public string Napomena { get; set; }

        public DateTime Kreiran { get; set; } = DateTime.Now;
        public DateTime? Obrisan { get; set; }


        public virtual Klijent Klijent { get; set; }
    }
}
