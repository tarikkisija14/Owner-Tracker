using System;
using System.ComponentModel.DataAnnotations;

namespace OwnerTrack.Data.Entities
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Tabela { get; set; } = string.Empty;

        public int? EntitetId { get; set; }

        [Required]
        [StringLength(20)]
        public string Akcija { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Opis { get; set; } = string.Empty;

        public DateTime Vrijeme { get; set; } = DateTime.Now;
    }
}