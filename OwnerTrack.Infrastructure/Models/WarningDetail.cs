using System;

namespace OwnerTrack.Infrastructure.Models
{
    public class WarningDetail
    {
        public int KlijentId { get; set; }
        public string NazivFirme { get; set; } = string.Empty;
        public string ImePrezime { get; set; } = string.Empty;
        public string Tip { get; set; } = string.Empty;
        public DateTime DatumIsteka { get; set; }
    }
}