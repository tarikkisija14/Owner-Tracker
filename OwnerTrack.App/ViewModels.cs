using System;

namespace OwnerTrack.App
{
   
    public class KlijentViewModel
    {
        public int Id { get; set; }
        public string? Naziv { get; set; }
        public string? IdBroj { get; set; }
        public string? Adresa { get; set; }
        public string? SifraDjelatnosti { get; set; }
        public string? Djelatnost { get; set; }
        public DateTime? DatumUspostaveOdnosa { get; set; }
        public string? VrstaKlijenta { get; set; }
        public DateTime? DatumOsnivanjaFirme { get; set; }
        public string? Velicina { get; set; }
        public string? PepRizik { get; set; }
        public string? UboRizik { get; set; }
        public string? GotovinaRizik { get; set; }
        public string? GeografskiRizik { get; set; }
        public string? UkupnaProcjena { get; set; }
        public DateTime? DatumProcjeneRizika { get; set; }
        public string? OvjeraCr { get; set; }
        public string? StatusUgovora { get; set; }
        public DateTime? DatumPotpisaUgovora { get; set; }
        public int BrojVlasnika { get; set; }
        public int BrojDirektora { get; set; }
        public string? StatusKlijenta { get; set; }
        public string? Napomena { get; set; }
    }

   
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

   
    public class DirektorViewModel
    {
        public int Id { get; set; }
        public string? ImePrezime { get; set; }
        public DateTime? DatumValjanostiDokumenta { get; set; }
        public string? TipValjanosti { get; set; }
        public string? StatusDirektora { get; set; }
    }
}