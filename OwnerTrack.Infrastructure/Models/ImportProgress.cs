namespace OwnerTrack.Infrastructure.Models
{
    public class ImportProgress
    {
        public int TotalRows { get; set; }
        public int ProcessedRows { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public string CurrentRow { get; set; } = string.Empty;
    }
}