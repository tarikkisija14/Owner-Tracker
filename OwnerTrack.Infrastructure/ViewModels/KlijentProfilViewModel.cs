namespace OwnerTrack.Infrastructure.ViewModels
{
    public class KlijentProfilViewModel
    {
        public int Id { get; set; }
        public string? Naziv { get; set; }
        public string? IdBroj { get; set; }
        public string? Adresa { get; set; }
        public string? Djelatnost { get; set; }
        public DateTime? DatumUspostave { get; set; }
        public string? VrstaKlijenta { get; set; }
        public DateTime? DatumOsnivanja { get; set; }
        public string? Velicina { get; set; }
        public string? Email { get; set; }
        public string? Telefon { get; set; }
        public string? StatusKlijenta { get; set; }
        public string? Napomena { get; set; }

        public string? PepRizik { get; set; }
        public string? UboRizik { get; set; }
        public string? GotovinaRizik { get; set; }
        public string? GeografskiRizik { get; set; }
        public string? UkupnaProcjena { get; set; }
        public DateTime? DatumProcjeneRizika { get; set; }
        public string? OvjeraCr { get; set; }
        public string? PepImePrezime { get; set; }
        public string? PepFunkcija { get; set; }
        public string? PepPovezanost { get; set; }
        public string? PepMjerePoduzete { get; set; }
        public DateTime? PepDatumProvjere { get; set; }
        public string? OpciIndikatoriRizika { get; set; }
        public string? IndikatoriIdentifikacijeRizika { get; set; }
        public string? IndikatoriTransakcijaRizika { get; set; }

        public string? VrstaUgovora { get; set; }
        public string? StatusUgovora { get; set; }
        public DateTime? DatumUgovora { get; set; }
        public string? NapomenaUgovora { get; set; }

        public List<VlasnikProfilViewModel> Vlasnici { get; set; } = new();
        public List<DirektorProfilViewModel> Direktori { get; set; } = new();
        public List<AuditEntryViewModel> Historija { get; set; } = new();
    }

    public class VlasnikProfilViewModel
    {
        public string? ImePrezime { get; set; }
        public decimal ProcenatVlasnistva { get; set; }
        public DateTime? DatumValjanostiDokumenta { get; set; }
        public string? Status { get; set; }
    }

    public class DirektorProfilViewModel
    {
        public string? ImePrezime { get; set; }
        public DateTime? DatumValjanosti { get; set; }
        public string? TipValjanosti { get; set; }
        public string? Status { get; set; }
    }

    public class AuditEntryViewModel
    {
        public DateTime Vrijeme { get; set; }
        public string? Tabela { get; set; }
        public int? EntitetId { get; set; }
        public string? Akcija { get; set; }
        public string? Opis { get; set; }
    }
}
