namespace OwnerTrack.Infrastructure.Models
{
   
    public sealed record WarningStats(int Count, bool ImaIsteklih)
    {
        public bool ImaUpozorenja => Count > 0;
    }
}