using OwnerTrack.Data.Enums;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OwnerTrack.Infrastructure.Parsing
{
   
    internal static class ExcelValueNormalizer
    {
        

        private static readonly Dictionary<string, VelicinaFirme> VelicinaAliases =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["MIKRO"] = VelicinaFirme.MIKRO,
                ["MICRO"] = VelicinaFirme.MIKRO,
                ["MALO"] = VelicinaFirme.MALO,
                ["MALI"] = VelicinaFirme.MALO,
                ["MALA"] = VelicinaFirme.MALO,
                ["SREDNJE"] = VelicinaFirme.SREDNJE,
                ["SREDNJI"] = VelicinaFirme.SREDNJE,
                ["SREDNJA"] = VelicinaFirme.SREDNJE,
                ["VELIKO"] = VelicinaFirme.VELIKO,
                ["VELIKI"] = VelicinaFirme.VELIKO,
                ["VELIKA"] = VelicinaFirme.VELIKO,
                ["OBRTNIK"] = VelicinaFirme.OBRTNIK,
                ["OBRT"] = VelicinaFirme.OBRTNIK,
                ["UDRUZENJE"] = VelicinaFirme.UDRUŽENJE,
                ["UDRUŽENJE"] = VelicinaFirme.UDRUŽENJE,
            };

        private static readonly string[] DateFormats =
        {
            "dd.MM.yyyy.", "dd.MM.yyyy",
            "d.M.yyyy.",   "d.M.yyyy",
            "d.MM.yyyy.",  "d.MM.yyyy",
            "yyyy-MM-dd",
        };

        private static readonly Regex WhitespaceRegex =
            new(@" {2,}", RegexOptions.Compiled);

        
        public static string NormalizeWhitespace(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return WhitespaceRegex
                .Replace(value.Replace("\u00a0", " "), " ")
                .Trim();
        }

        
        public static DateTime? ParseDate(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            string s = raw.Trim();

            
            if (double.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out double oaDate)
                && oaDate is > 10_000 and < 100_000)
            {
                try { return DateTime.FromOADate(oaDate); } catch { /* fall through */ }
            }

            
            string upper = s.ToUpperInvariant();
            if (upper == TipValjanostiKonstante.Trajno || upper is "STEČAJ" or "STECAJ")
                return null;

            foreach (string fmt in DateFormats)
                if (DateTime.TryParseExact(s, fmt, CultureInfo.InvariantCulture,
                                           DateTimeStyles.None, out DateTime parsed))
                    return parsed;

            return null;
        }

        
        public static decimal ParseDecimal(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw) || raw == "-")
                return 0m;

            string normalised = raw.Replace(",", ".").Trim();
            return decimal.TryParse(normalised, NumberStyles.Number,
                                    CultureInfo.InvariantCulture, out decimal result)
                ? result
                : 0m;
        }

       
        public static string? NormalizeVelicina(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            string upper = raw.ToUpperInvariant().Trim();

            if (VelicinaAliases.TryGetValue(upper, out VelicinaFirme matched))
                return matched.ToString();

            if (Enum.TryParse<VelicinaFirme>(upper, ignoreCase: true, out VelicinaFirme parsed))
                return parsed.ToString();

            return raw.Trim();
        }

        
        public static string? NormalizeDaNe(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            string first = raw.Trim()
                .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0]
                .ToUpperInvariant();

            return first == DaNeKonstante.Da ? DaNeKonstante.Da
                 : first == DaNeKonstante.Ne ? DaNeKonstante.Ne
                 : null;
        }

        
        public static VrstaKlijenta NormalizeVrstaKlijenta(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return VrstaKlijenta.PravnoLice;

            string upper = raw.ToUpperInvariant().Trim();

            if (upper.Contains("FIZIČKO") || upper.Contains("FIZICKO"))
                return VrstaKlijenta.FizickoLice;

            if (upper.Contains("UDRUŽ") || upper.Contains("UDRUZ"))
                return VrstaKlijenta.Udruzenje;

            if (upper == "OBRTNIK" || upper.Contains("OBRT"))
                return VrstaKlijenta.Obrtnik;

            return VrstaKlijenta.PravnoLice;
        }
    }
}