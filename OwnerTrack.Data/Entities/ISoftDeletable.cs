using System;

namespace OwnerTrack.Data.Entities
{
    public interface ISoftDeletable
    {
        DateTime? Obrisan { get; set; }
    }
}