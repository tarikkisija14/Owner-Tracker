using System;
using System.ComponentModel.DataAnnotations;

namespace OwnerTrack.Data.Entities
{
    public class WarningAcknowledgement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string EntityType { get; set; } = string.Empty;

        public int EntityId { get; set; }

        public DateTime DatumIsteka { get; set; }

        [StringLength(500)]
        public string? Napomena { get; set; }

        [StringLength(100)]
        public string? Korisnik { get; set; }

        public DateTime Vrijeme { get; set; } = DateTime.Now;
    }
}
