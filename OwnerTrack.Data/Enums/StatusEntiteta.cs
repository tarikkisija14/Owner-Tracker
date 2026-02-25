using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerTrack.Data.Enums
{
    public enum StatusEntiteta
    {
        AKTIVAN,
        NEAKTIVAN,
        ARHIVIRAN
    }
    public static class StatusKonstante
    {
        public const string Aktivan = "AKTIVAN";
        public const string Neaktivan = "NEAKTIVAN";
        public const string Arhiviran = "ARHIVIRAN";
    }
}
