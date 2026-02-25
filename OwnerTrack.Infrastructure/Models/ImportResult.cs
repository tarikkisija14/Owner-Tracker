namespace OwnerTrack.Infrastructure.Models
{
    public class ImportResult
    {
        public bool Success { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public int VlasnikCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public string DebugInfo { get; set; }

    }
}