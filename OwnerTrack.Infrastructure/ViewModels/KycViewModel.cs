namespace OwnerTrack.Infrastructure.ViewModels
{
    public class KycViewModel
    {
        public int Id { get; set; }
        public int Redni { get; set; }
        public string? Naziv { get; set; }
        public string? IdBroj { get; set; }
        public string? Adresa { get; set; }
        public string? SifraDjelatnosti { get; set; }
        public string? Djelatnost { get; set; }
        public DateTime? DatumUspostaveOdnosa { get; set; }
        public string? VrstaKlijenta { get; set; }
        public string? PepRizik { get; set; }
        public string? UkupnaProcjena { get; set; }
    }
}
