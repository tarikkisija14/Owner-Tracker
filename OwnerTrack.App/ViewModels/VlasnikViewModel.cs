namespace OwnerTrack.App.ViewModels
{
    public class VlasnikViewModel
    {
        public int Id { get; set; }
        public string? ImePrezime { get; set; }
        public DateTime? DatumValjanostiDokumenta { get; set; }
        public decimal ProcenatVlasnistva { get; set; }
        public DateTime? DatumUtvrdjivanja { get; set; }
        public string? IzvorPodatka { get; set; }
        public string? StatusVlasnika { get; set; }
    }
}