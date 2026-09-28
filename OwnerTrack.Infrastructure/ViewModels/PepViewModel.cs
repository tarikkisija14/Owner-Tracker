namespace OwnerTrack.Infrastructure.ViewModels
{
    public class PepViewModel
    {
        public int Id { get; set; }
        public int Redni { get; set; }
        public string? NazivKlijenta { get; set; }
        public string? PepImePrezime { get; set; }
        public string? PepFunkcija { get; set; }
        public string? PepPovezanost { get; set; }
        public string? PepMjerePoduzete { get; set; }
        public DateTime? PepDatumProvjere { get; set; }
    }
}
