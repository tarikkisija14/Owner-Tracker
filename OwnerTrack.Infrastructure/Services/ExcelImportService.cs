using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Models;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;

namespace OwnerTrack.Infrastructure
{
    public class ExcelImportService
    {
        

        private const int ColNaziv = 2;
        private const int ColIdBroj = 3;
        private const int ColAdresa = 4;
        private const int ColSifraDjelatnosti = 5;
        private const int ColNazivDjelatnosti = 6;
        private const int ColDatumUspostave = 7;
        private const int ColVrstaKlijenta = 8;
        private const int ColDatumOsnivanja = 9;
        private const int ColVlasnik = 10;
        private const int ColDatVazVlasnika = 11;
        private const int ColProcenat = 12;
        private const int ColDatUtvrdjivanja = 13;
        private const int ColIzvorPodatka = 14;
        private const int ColDirektor = 15;
        private const int ColDatVazDirektora = 16;
        private const int ColVelicina = 17;
        private const int ColPepRizik = 18;
        private const int ColUboRizik = 19;
        private const int ColGotovinaRizik = 20;
        private const int ColGeografskiRizik = 21;
        private const int ColUkupnaProcjena = 22;
        private const int ColDatumProcjene = 23;
        private const int ColOvjeraCr = 24;
        private const int ColStatusUgovora = 25;
        private const int ColDatumUgovora = 26;

        private const int BatchSize = 50;
        private const int ExcelHeaderRows = 2; // rows skipped before data

        

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

        private static readonly Regex FirmaKeywordRegex = new(
            @"\b(d\.o\.o\.|doo|d\.d\.|dd|a\.d\.|ltd|gmbh|inc|zajednica|dioničar|fond|komisija|općina|kanton|vlada)\b",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex DateTokenRegex = new(@"\d{1,2}\.\d{1,2}\.\d{4}\.?", RegexOptions.Compiled);
        private static readonly Regex WhitespaceRegex = new(@" {2,}", RegexOptions.Compiled);

        private readonly string _connectionString;

        public ExcelImportService(string connectionString)
        {
            _connectionString = connectionString;
        }

        

        public ImportResult ImportFromExcel(
            string filePath,
            IProgress<ImportProgress>? progress = null,
            CancellationToken cancellationToken = default)
        {
            var result = new ImportResult();
            var prog = new ImportProgress();

            try
            {
                Debug.WriteLine($"[IMPORT-START] File='{filePath}' Time={DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                using var doc = SpreadsheetDocument.Open(filePath, false);
                var wbPart = doc.WorkbookPart ?? throw new Exception("Excel fajl nema validan WorkbookPart!");
                var sheet = wbPart.Workbook.Sheets?.Cast<Sheet>()
                                       .FirstOrDefault(s => s.Name?.Value?.Contains("ZBIRNA") == true)
                                   ?? throw new Exception("Nije pronađen list sa 'ZBIRNA'!");
                var wsPart = (WorksheetPart)wbPart.GetPartById(sheet.Id!);
                var allRows = wsPart.Worksheet.Elements<SheetData>().First()
                                       .Elements<Row>().Skip(ExcelHeaderRows).ToList();

                prog.TotalRows = allRows.Count;

                using var db = CreateDbContext();
                using var tx = db.Database.BeginTransaction();

                var existingIdBrojevi = db.Klijenti.AsNoTracking().Select(k => k.IdBroj).ToHashSet(StringComparer.OrdinalIgnoreCase);
                var existingNazivi = db.Klijenti.AsNoTracking().Select(k => k.Naziv).ToHashSet(StringComparer.OrdinalIgnoreCase);
                var existingDjelatnosti = db.Djelatnosti.AsNoTracking().Select(d => d.Sifra).ToHashSet(StringComparer.OrdinalIgnoreCase);

                int pendingChanges = 0;

                for (int i = 0; i < allRows.Count; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Debug.WriteLine("[IMPORT-CANCELLED] Korisnik otkazao import.");
                        break;
                    }

                    var row = allRows[i];
                    string naziv = "";
                    string idBroj = "";

                    try
                    {
                        naziv = (GetCellValue(wbPart, row, ColNaziv) ?? "").Trim();
                        idBroj = (GetCellValue(wbPart, row, ColIdBroj) ?? "").Trim();

                        prog.CurrentRow = $"{naziv} ({idBroj})";
                        prog.ProcessedRows = i + 1;

                        if (string.IsNullOrWhiteSpace(naziv) || string.IsNullOrWhiteSpace(idBroj))
                        {
                            progress?.Report(prog);
                            continue;
                        }

                        bool skipped = ImportRow(
                            wbPart, row, i, naziv, idBroj,
                            result, db,
                            existingIdBrojevi, existingNazivi, existingDjelatnosti);

                        if (!skipped)
                        {
                            result.SuccessCount++;
                            prog.SuccessCount = result.SuccessCount;
                            pendingChanges++;

                            if (pendingChanges >= BatchSize)
                            {
                                db.SaveChanges();
                                pendingChanges = 0;
                            }
                        }
                        else
                        {
                            result.SkipCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorCount++;
                        prog.ErrorCount = result.ErrorCount;
                        result.Errors.Add(BuildRowError(i, naziv, idBroj, ex));
                        Debug.WriteLine(BuildRowError(i, naziv, idBroj, ex));
                    }

                    progress?.Report(prog);
                }

                if (pendingChanges > 0)
                    db.SaveChanges();

                tx.Commit();
            }
            catch (OperationCanceledException)
            {
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors.Add($"KRITIČNA GREŠKA: {ex.Message}");
                return result;
            }

            result.Success = true;
            return result;
        }

        

        /// <returns>True if the row was skipped (duplicate), false if it was imported.</returns>
        private bool ImportRow(
            WorkbookPart wbPart, Row row, int rowIndex,
            string naziv, string idBroj,
            ImportResult result,
            OwnerTrackDbContext db,
            HashSet<string> existingIdBrojevi,
            HashSet<string> existingNazivi,
            HashSet<string> existingDjelatnosti)
        {
            string sifraDjelatnosti = GetCellValue(wbPart, row, ColSifraDjelatnosti)?.Trim()
                                      ?? AppKonstante.DefaultSifraDjelatnosti;
            if (string.IsNullOrWhiteSpace(sifraDjelatnosti))
                sifraDjelatnosti = AppKonstante.DefaultSifraDjelatnosti;

            string? nazivDjelatnosti = GetCellValue(wbPart, row, ColNazivDjelatnosti)?.Trim();

            EnsureDjelatnostExists(db, existingDjelatnosti, sifraDjelatnosti, nazivDjelatnosti);

            if (existingIdBrojevi.Contains(idBroj))
            {
                Debug.WriteLine($"[SKIP-DUPLICATE-ID] Red={rowIndex + ExcelHeaderRows + 1} ID='{idBroj}'");
                return true;
            }

            if (existingNazivi.Contains(naziv))
                throw new Exception($"Naziv '{naziv}' već postoji s drugačijim ID brojem.");

            var klijent = new Klijent
            {
                Naziv = naziv,
                IdBroj = idBroj,
                Adresa = GetCellValue(wbPart, row, ColAdresa)?.Trim(),
                SifraDjelatnosti = sifraDjelatnosti,
                DatumUspostave = ParseDate(GetCellValue(wbPart, row, ColDatumUspostave)),
                VrstaKlijenta = NormalizeVrstaKlijenta(GetCellValue(wbPart, row, ColVrstaKlijenta)),
                DatumOsnivanja = ParseDate(GetCellValue(wbPart, row, ColDatumOsnivanja)),
                Velicina = NormalizeVelicina(GetCellValue(wbPart, row, ColVelicina)),
                PepRizik = NormalizeDaNe(GetCellValue(wbPart, row, ColPepRizik)),
                UboRizik = NormalizeDaNe(GetCellValue(wbPart, row, ColUboRizik)),
                GotovinaRizik = NormalizeDaNe(GetCellValue(wbPart, row, ColGotovinaRizik)),
                GeografskiRizik = NormalizeDaNe(GetCellValue(wbPart, row, ColGeografskiRizik)),
                UkupnaProcjena = GetCellValue(wbPart, row, ColUkupnaProcjena)?.Trim(),
                DatumProcjene = ParseDate(GetCellValue(wbPart, row, ColDatumProcjene)),
                OvjeraCr = GetCellValue(wbPart, row, ColOvjeraCr)?.Trim(),
                Status = StatusEntiteta.AKTIVAN,
                Kreiran = DateTime.Now,
            };

            db.Klijenti.Add(klijent);
            db.SaveChanges();

            existingIdBrojevi.Add(idBroj);
            existingNazivi.Add(naziv);

            ImportVlasnici(wbPart, row, klijent, result, db);
            ImportDirektori(wbPart, row, klijent, db);
            ImportUgovor(wbPart, row, klijent, db);

            Debug.WriteLine($"[OK] Red={rowIndex + ExcelHeaderRows + 1} KlijentId={klijent.Id} '{naziv}'");
            return false;
        }

        private void ImportVlasnici(WorkbookPart wbPart, Row row, Klijent klijent, ImportResult result, OwnerTrackDbContext db)
        {
            string? vlasnikRaw = GetCellValue(wbPart, row, ColVlasnik);
            string? datVazVlasnika = GetCellValue(wbPart, row, ColDatVazVlasnika);
            string? procenatRaw = GetCellValue(wbPart, row, ColProcenat);
            string? datUtvrdjivanja = GetCellValue(wbPart, row, ColDatUtvrdjivanja);
            string? izvorPodatka = GetCellValue(wbPart, row, ColIzvorPodatka);

            if (string.IsNullOrWhiteSpace(vlasnikRaw)) return;

            foreach (var v in ParseVlasnici(vlasnikRaw, datVazVlasnika, procenatRaw))
            {
                v.KlijentId = klijent.Id;
                v.DatumUtvrdjivanja = ParseDate(datUtvrdjivanja);
                v.IzvorPodatka = izvorPodatka?.Trim();
                v.Status = StatusEntiteta.AKTIVAN;
                db.Vlasnici.Add(v);
                result.VlasnikCount++;
            }
        }

        private void ImportDirektori(WorkbookPart wbPart, Row row, Klijent klijent, OwnerTrackDbContext db)
        {
            string? direktorRaw = GetCellValue(wbPart, row, ColDirektor);
            string? datVazDirektora = GetCellValue(wbPart, row, ColDatVazDirektora);

            if (string.IsNullOrWhiteSpace(direktorRaw)) return;

            foreach (var d in ParseDirektori(direktorRaw, datVazDirektora))
            {
                d.KlijentId = klijent.Id;
                d.Status = StatusEntiteta.AKTIVAN;
                db.Direktori.Add(d);
            }
        }

        private void ImportUgovor(WorkbookPart wbPart, Row row, Klijent klijent, OwnerTrackDbContext db)
        {
            string? statusUgovora = GetCellValue(wbPart, row, ColStatusUgovora);
            string? datumUgovora = GetCellValue(wbPart, row, ColDatumUgovora);

            if (string.IsNullOrWhiteSpace(statusUgovora)) return;

            db.Ugovori.Add(new Ugovor
            {
                KlijentId = klijent.Id,
                StatusUgovora = statusUgovora.Trim(),
                DatumUgovora = ParseDate(datumUgovora),
            });
        }

        private static void EnsureDjelatnostExists(
            OwnerTrackDbContext db,
            HashSet<string> existingDjelatnosti,
            string sifra,
            string? naziv)
        {
            if (existingDjelatnosti.Contains(sifra)) return;

            string displayNaziv = (string.IsNullOrWhiteSpace(naziv) || naziv.StartsWith("="))
                ? $"Djelatnost {sifra}"
                : naziv;

            db.Djelatnosti.Add(new Djelatnost { Sifra = sifra, Naziv = displayNaziv });
            existingDjelatnosti.Add(sifra);
        }

        

        private List<Vlasnik> ParseVlasnici(string raw, string? datVazRaw, string? procenatRaw)
        {
            var result = new List<Vlasnik>();
            raw = NormalizeWhitespace(raw);

            var imena = raw.Contains(',')
                ? raw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(s => s.Trim()).Where(s => s.Length > 0).ToList()
                : SplitIntoNames(raw.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            var datumi = ParseTokenList(datVazRaw, isDate: true);
            var procenti = ParseTokenList(procenatRaw, isDate: false);

            for (int i = 0; i < imena.Count; i++)
            {
                string ime = NormalizeWhitespace(imena[i]);
                if (string.IsNullOrWhiteSpace(ime)) continue;

                result.Add(new Vlasnik
                {
                    ImePrezime = ime.ToUpperInvariant(),
                    ProcenatVlasnistva = ParseDecimal(i < procenti.Count ? procenti[i] : null),
                    DatumValjanostiDokumenta = ParseDate(i < datumi.Count ? datumi[i] : null),
                    Status = StatusEntiteta.AKTIVAN,
                });
            }

            return result;
        }

        private List<string> SplitIntoNames(string[] words)
        {
            var list = new List<string>();
            if (words.Length == 0) return list;

            string full = string.Join(" ", words);

            bool hasCompanyKeyword = FirmaKeywordRegex.IsMatch(full);
            if (hasCompanyKeyword || words.Length > 4 || words.Length % 2 != 0)
            {
                list.Add(full);
                return list;
            }

            for (int i = 0; i + 1 < words.Length; i += 2)
                list.Add(words[i] + " " + words[i + 1]);

            return list;
        }

        private List<string> ParseTokenList(string? raw, bool isDate)
        {
            var list = new List<string>();
            if (string.IsNullOrWhiteSpace(raw)) return list;

            raw = NormalizeWhitespace(raw);

            if (isDate)
            {
                var matches = DateTokenRegex.Matches(raw);
                if (matches.Count > 0)
                {
                    foreach (Match m in matches) list.Add(m.Value.Trim());
                    return list;
                }
            }

            list.AddRange(Regex.Split(raw, @"\s+").Where(s => !string.IsNullOrWhiteSpace(s)));
            return list;
        }

        private List<Direktor> ParseDirektori(string raw, string? datVazRaw)
        {
            var result = new List<Direktor>();
            raw = NormalizeWhitespace(raw);

            var imena = raw.Contains(',')
                ? raw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(s => s.Trim()).Where(s => s.Length > 0).ToList()
                : new List<string> { raw };

            string datVazNorm = datVazRaw?.Trim().ToUpper() ?? "";
            bool isTrajno = datVazNorm == TipValjanostiKonstante.Trajno;
            DateTime? datVaz = isTrajno ? null : ParseDate(datVazRaw);
            string tip = (isTrajno || datVaz == null)
                ? TipValjanostiKonstante.Trajno
                : TipValjanostiKonstante.Vremenski;

            foreach (var ime in imena)
            {
                if (string.IsNullOrWhiteSpace(ime)) continue;
                result.Add(new Direktor
                {
                    ImePrezime = ime,
                    DatumValjanosti = datVaz,
                    TipValjanosti = tip,
                    Status = StatusEntiteta.AKTIVAN,
                });
            }

            return result;
        }

        

        private string GetCellValue(WorkbookPart wbPart, Row row, int colIndex)
        {
            try
            {
                var cell = row.Elements<Cell>()
                    .FirstOrDefault(c => GetColumnIndex(c.CellReference?.Value) == colIndex);
                if (cell == null) return "";

                if (cell.DataType == null)
                    return cell.CellValue?.Text ?? "";

                if (cell.DataType.Value == CellValues.SharedString)
                {
                    if (!int.TryParse(cell.CellValue?.Text, out int id)) return "";
                    return wbPart.SharedStringTablePart?.SharedStringTable
                        .Elements<SharedStringItem>().ElementAt(id).InnerText ?? "";
                }

                return cell.CellValue?.Text ?? "";
            }
            catch { return ""; }
        }

        private static int GetColumnIndex(string? cellRef)
        {
            if (string.IsNullOrWhiteSpace(cellRef)) return 0;
            string col = Regex.Replace(cellRef, @"\d", "");
            int idx = 0;
            foreach (char c in col)
                idx = idx * 26 + (c - 'A' + 1);
            return idx;
        }

        

        private static DateTime? ParseDate(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            s = s.Trim();

            if (double.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out double oa)
                && oa > 10000 && oa < 100000)
            {
                try { return DateTime.FromOADate(oa); } catch { }
            }

            string upper = s.ToUpperInvariant();
            if (upper == TipValjanostiKonstante.Trajno || upper is "STEČAJ" or "STECAJ")
                return null;

            foreach (var fmt in DateFormats)
                if (DateTime.TryParseExact(s, fmt, CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out DateTime dt))
                    return dt;

            return null;
        }

        private static decimal ParseDecimal(string? s)
        {
            if (string.IsNullOrWhiteSpace(s) || s == "-") return 0;
            s = s.Replace(",", ".").Trim();
            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal d) ? d : 0;
        }

        private string NormalizeWhitespace(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            return WhitespaceRegex.Replace(s.Replace("\u00a0", " "), " ").Trim();
        }

        private static string? NormalizeVelicina(string? v)
        {
            if (string.IsNullOrWhiteSpace(v)) return null;
            string upper = v.ToUpperInvariant().Trim();

            if (VelicinaAliases.TryGetValue(upper, out var result))
                return result.ToString();

            if (Enum.TryParse<VelicinaFirme>(upper, ignoreCase: true, out var parsed))
                return parsed.ToString();

            return v.Trim();
        }

        private static string? NormalizeDaNe(string? v)
        {
            if (string.IsNullOrWhiteSpace(v)) return null;
            string first = v.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0].ToUpperInvariant();
            return first == DaNeKonstante.Da ? DaNeKonstante.Da
                 : first == DaNeKonstante.Ne ? DaNeKonstante.Ne
                 : null;
        }

        private static VrstaKlijenta NormalizeVrstaKlijenta(string? v)
        {
            if (string.IsNullOrWhiteSpace(v)) return VrstaKlijenta.PravnoLice;
            string upper = v.ToUpperInvariant().Trim();
            if (upper.Contains("FIZIČKO") || upper.Contains("FIZICKO")) return VrstaKlijenta.FizickoLice;
            if (upper.Contains("UDRUŽ") || upper.Contains("UDRUZ")) return VrstaKlijenta.Udruzenje;
            if (upper == "OBRTNIK" || upper.Contains("OBRT")) return VrstaKlijenta.Obrtnik;
            return VrstaKlijenta.PravnoLice;
        }

        

        /// Builds a consistent error string for a failed row (for both log and result.Errors).
        private static string BuildRowError(int rowIndex, string naziv, string idBroj, Exception ex)
        {
            string inner = ex.InnerException?.Message ?? "";
            string suffix = string.IsNullOrEmpty(inner) ? "" : $" | {inner}";
            return $"Red {rowIndex + ExcelHeaderRows + 1} | {naziv} | {idBroj} | {ex.Message}{suffix}";
        }

        

        private OwnerTrackDbContext CreateDbContext()
        {
            var opts = new DbContextOptionsBuilder<OwnerTrackDbContext>()
                .UseSqlite(_connectionString)
                .Options;
            return new OwnerTrackDbContext(opts);
        }
    }
}