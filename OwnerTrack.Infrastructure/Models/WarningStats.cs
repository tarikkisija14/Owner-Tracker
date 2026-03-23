namespace OwnerTrack.Infrastructure.Models
{
    public sealed record WarningStats(int Count, bool HasExpired)
    {
        public bool HasWarnings => Count > 0;
    }
}