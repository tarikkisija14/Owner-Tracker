namespace OwnerTrack.Infrastructure.ViewModels
{
    public class VlasnikSaFirmomViewModel
    {
        public int Id { get; set; }
        public int Redni { get; set; }
        public string? KlijentNaziv { get; set; }
        public string? KlijentIdBroj { get; set; }
        public string? ImePrezime { get; set; }
        public decimal ProcenatVlasnistva { get; set; }
        public DateTime? DatumUtvrdjivanja { get; set; }
        public string? IzvorPodatka { get; set; }
    }
}
