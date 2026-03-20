using OwnerTrack.Data.Enums;

namespace OwnerTrack.Data.Entities
{
    /// <summary>
    /// Extends <see cref="ISoftDeletable"/> for entities that also carry
    /// an explicit <see cref="StatusEntiteta"/> field so they can be
    /// archived in a single, consistent operation via
    /// <c>AuditService.Arhiviraj</c>.
    /// </summary>
    public interface IArchivable : ISoftDeletable
    {
        StatusEntiteta Status { get; set; }
    }
}