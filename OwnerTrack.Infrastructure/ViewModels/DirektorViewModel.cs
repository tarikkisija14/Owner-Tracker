namespace OwnerTrack.Infrastructure.ViewModels
{
    public class DirektorViewModel
    {
        public int Id { get; set; }
        public string? ImePrezime { get; set; }
        public DateTime? DatumValjanostiDokumenta { get; set; }
        public string? TipValjanosti { get; set; }
        public string? StatusDirektora { get; set; }
    }
}