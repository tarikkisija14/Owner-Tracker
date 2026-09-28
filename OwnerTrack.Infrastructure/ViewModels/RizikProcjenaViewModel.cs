namespace OwnerTrack.Infrastructure.ViewModels
{
    public class RizikProcjenaViewModel
    {
        public int Id { get; set; }
        public int Redni { get; set; }
        public string? Naziv { get; set; }
        public string? IdBroj { get; set; }
        public string? Djelatnost { get; set; }
        public string? VrstaKlijenta { get; set; }
        public string? VlasnikImena { get; set; }
        public string? DirektorImena { get; set; }
        public string? Velicina { get; set; }
        public string? OpciIndikatoriRizika { get; set; }
        public string? IndikatoriIdentifikacijeRizika { get; set; }
        public string? IndikatoriTransakcijaRizika { get; set; }
        public string? GeografskiRizik { get; set; }
        public string? UkupnaProcjena { get; set; }
        public DateTime? DatumProcjeneRizika { get; set; }
        public string? OvjeraCr { get; set; }
        public string? VrstaUgovora { get; set; }
        public DateTime? DatumUgovora { get; set; }
    }
}
