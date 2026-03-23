using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using System.Text.RegularExpressions;

namespace OwnerTrack.Infrastructure.Parsing
{
    internal static class ExcelEntityParser
    {
        private static readonly Regex CompanyKeywordRegex = new(
            @"\b(d\.o\.o\.|doo|d\.d\.|dd|a\.d\.|ltd|gmbh|inc|zajednica|dioničar|fond|komisija|općina|kanton|vlada)\b",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex DateTokenRegex =
            new(@"\d{1,2}\.\d{1,2}\.\d{4}\.?", RegexOptions.Compiled);

        public static List<Vlasnik> ParseVlasnici(
            string rawNames,
            string? rawDatesOfValidity,
            string? rawPercentages)
        {
            var results = new List<Vlasnik>();

            rawNames = ExcelValueNormalizer.NormalizeWhitespace(rawNames);
            if (string.IsNullOrWhiteSpace(rawNames))
                return results;

            var names = SplitNames(rawNames);
            var dates = ParseTokenList(rawDatesOfValidity, isDate: true);
            var percentages = ParseTokenList(rawPercentages, isDate: false);

            for (int i = 0; i < names.Count; i++)
            {
                string name = ExcelValueNormalizer.NormalizeWhitespace(names[i]);
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                results.Add(new Vlasnik
                {
                    ImePrezime = name.ToUpperInvariant(),
                    ProcenatVlasnistva = ExcelValueNormalizer.ParseDecimal(i < percentages.Count ? percentages[i] : null),
                    DatumValjanostiDokumenta = ExcelValueNormalizer.ParseDate(i < dates.Count ? dates[i] : null),
                    Status = StatusEntiteta.AKTIVAN,
                });
            }

            return results;
        }

        public static List<Direktor> ParseDirektori(string rawNames, string? rawDateOfValidity)
        {
            var results = new List<Direktor>();

            rawNames = ExcelValueNormalizer.NormalizeWhitespace(rawNames);
            if (string.IsNullOrWhiteSpace(rawNames))
                return results;

            var names = rawNames.Contains(',')
                ? rawNames.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(s => s.Trim())
                          .Where(s => s.Length > 0)
                          .ToList()
                : new List<string> { rawNames };

            string normalizedDateRaw = rawDateOfValidity?.Trim().ToUpperInvariant() ?? string.Empty;
            bool isPermanent = normalizedDateRaw == ValidityTypeConstants.Trajno;
            DateTime? dateOfValidity = isPermanent ? null : ExcelValueNormalizer.ParseDate(rawDateOfValidity);
            string validityType = (isPermanent || dateOfValidity is null)
                ? ValidityTypeConstants.Trajno
                : ValidityTypeConstants.Vremenski;

            foreach (string name in names)
            {
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                results.Add(new Direktor
                {
                    ImePrezime = name,
                    DatumValjanosti = dateOfValidity,
                    TipValjanosti = validityType,
                    Status = StatusEntiteta.AKTIVAN,
                });
            }

            return results;
        }

        private static List<string> SplitNames(string normalized)
        {
            if (normalized.Contains(','))
                return normalized
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => s.Length > 0)
                    .ToList();

            string[] words = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return SplitWordsIntoNames(words);
        }

        private static List<string> SplitWordsIntoNames(string[] words)
        {
            var list = new List<string>();
            if (words.Length == 0)
                return list;

            string full = string.Join(" ", words);

            
            if (CompanyKeywordRegex.IsMatch(full) || words.Length > 4 || words.Length % 2 != 0)
            {
                list.Add(full);
                return list;
            }

            
            for (int i = 0; i + 1 < words.Length; i += 2)
                list.Add($"{words[i]} {words[i + 1]}");

            return list;
        }

        private static List<string> ParseTokenList(string? raw, bool isDate)
        {
            var list = new List<string>();
            if (string.IsNullOrWhiteSpace(raw))
                return list;

            raw = ExcelValueNormalizer.NormalizeWhitespace(raw);

            if (isDate)
            {
                var matches = DateTokenRegex.Matches(raw);
                if (matches.Count > 0)
                {
                    foreach (Match m in matches)
                        list.Add(m.Value.Trim());
                    return list;
                }
            }

            list.AddRange(Regex.Split(raw, @"\s+").Where(s => !string.IsNullOrWhiteSpace(s)));
            return list;
        }
    }
}