namespace OwnerTrack.Infrastructure.Models
{
    // Snapshot brojeva već prikazanih na Compliance Dashboardu — puni ga
    // pozivalac (Form1) iz DashboardQueryService/WarningQueryService prije
    // generisanja PDF-a, tako da PDF ne pokreće nikakav dodatni/paralelni
    // upit i uvijek odgovara onome što je korisnik upravo vidio na ekranu.
    public class ComplianceSummaryData
    {
        public int AktivniKlijenti { get; set; }
        public Dictionary<string, int> KlijentiPoRiziku { get; set; } = new();
        public int PepKlijenti { get; set; }
        public int KlijentiBezUgovora { get; set; }
        public int UpozorenjaUkupno { get; set; }
        public bool UpozorenjaImaIsteklih { get; set; }
    }
}
