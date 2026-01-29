using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerTrack.Data.Entities
{
    public class Djelatnost
    {
        [Key]
        [MaxLength(10)]
        public string Sifra { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string Naziv { get; set; } = null!;

        
        public ICollection<Klijent> Klijenti { get; set; } = new List<Klijent>();
    }
}
