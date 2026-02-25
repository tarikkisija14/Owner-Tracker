using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace OwnerTrack.Infrastructure
{
    public class ExcelImportService
    {
        private readonly string _connectionString;

        public ExcelImportService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ImportResult ImportFromExcel(string filePath, IProgress<ImportProgress>? progress = null)
        {
            var result = new ImportResult();
            var prog = new ImportProgress();
            void Log(string t) => Debug.WriteLine(t);

            try
            {
                Log($"[IMPORT-START] File='{filePath}' Time={DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                using var doc = SpreadsheetDocument.Open(filePath, false);
                var wbPart = doc.WorkbookPart
                    ?? throw new Exception("Excel fajl nema validan WorkbookPart!");

                var sheet = wbPart.Workbook.Sheets?.Cast<Sheet>()
                    .FirstOrDefault(s => s.Name?.Value?.Contains("ZBIRNA") == true)
                    ?? throw new Exception("Nije pronađen list sa 'ZBIRNA'!");

                var wsPart = (WorksheetPart)wbPart.GetPartById(sheet.Id!);
                var sheetData = wsPart.Worksheet.Elements<SheetData>().First();
                var allRows = sheetData.Elements<Row>().Skip(2).ToList();

                prog.TotalRows = allRows.Count;

                for (int i = 0; i < allRows.Count; i++)
                {
                    var row = allRows[i];
                    string naziv = "";
                    string idBroj = "";

                    try
                    {
                        naziv = (GetCellValue(wbPart, row, 2) ?? "").Trim();
                        idBroj = (GetCellValue(wbPart, row, 3) ?? "").Trim();

                        prog.CurrentRow = $"{naziv} ({idBroj})";
                        prog.ProcessedRows = i + 1;

                        if (string.IsNullOrWhiteSpace(naziv) || string.IsNullOrWhiteSpace(idBroj))
                        {
                            progress?.Report(prog);
                            continue;
                        }

                        
                        ImportajRed(wbPart, row, i, naziv, idBroj, result, Log);

                        result.SuccessCount++;
                        prog.SuccessCount = result.SuccessCount;
                    }
                    catch (Exception ex)
                    {
                        result.ErrorCount++;
                        prog.ErrorCount = result.ErrorCount;
                        string inner = ex.InnerException?.Message ?? "";
                        Log($"[ERROR] Red={i + 3} '{naziv}' Msg='{ex.Message}'" +
                            (string.IsNullOrEmpty(inner) ? "" : $" Inner='{inner}'"));
                        result.Errors.Add(
                            $"Red {i + 3} | {naziv} | {idBroj} | {ex.Message}" +
                            (string.IsNullOrEmpty(inner) ? "" : $" | {inner}"));
                    }

                    progress?.Report(prog);
                }
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

        private void ImportajRed(WorkbookPart wbPart, Row row, int i,
                                   string naziv, string idBroj,
                                   ImportResult result, Action<string> Log)
        {
            
            using var db = KreirajDb();
            using var tx = db.Database.BeginTransaction();
            int privremeniVlasnikCount = 0;

            try
            {
                if (db.Klijenti.AsNoTracking().Any(k => k.IdBroj == idBroj))
                {
                    Log($"[SKIP-DUPLICATE-ID] Red={i + 3} ID='{idBroj}'");
                    return;
                }

                if (db.Klijenti.AsNoTracking().Any(k => k.Naziv == naziv))
                    throw new Exception($"Naziv '{naziv}' već postoji s drugačijim ID brojem.");

                string sifraDjelatnosti = GetCellValue(wbPart, row, 5)?.Trim() ?? "69.20";
                string? nazivDjelatnosti = GetCellValue(wbPart, row, 6)?.Trim();
                if (string.IsNullOrWhiteSpace(sifraDjelatnosti))
                    sifraDjelatnosti = "69.20";

                EnsureDjelatnostExists(db, sifraDjelatnosti, nazivDjelatnosti);

                var klijent = new Klijent
                {
                    Naziv = naziv,
                    IdBroj = idBroj,
                    Adresa = GetCellValue(wbPart, row, 4)?.Trim(),
                    SifraDjelatnosti = sifraDjelatnosti,
                    DatumUspostave = ParseDate(GetCellValue(wbPart, row, 7)),
                    VrstaKlijenta = NormalizeVrstaKlijenta(GetCellValue(wbPart, row, 8)),
                    DatumOsnivanja = ParseDate(GetCellValue(wbPart, row, 9)),
                    Velicina = NormalizeVelicina(GetCellValue(wbPart, row, 17)),
                    PepRizik = NormalizeDaNe(GetCellValue(wbPart, row, 18)),
                    UboRizik = NormalizeDaNe(GetCellValue(wbPart, row, 19)),
                    GotovinaRizik = NormalizeDaNe(GetCellValue(wbPart, row, 20)),
                    GeografskiRizik = NormalizeDaNe(GetCellValue(wbPart, row, 21)),
                    UkupnaProcjena = GetCellValue(wbPart, row, 22)?.Trim(),
                    DatumProcjene = ParseDate(GetCellValue(wbPart, row, 23)),
                    OvjeraCr = GetCellValue(wbPart, row, 24)?.Trim(),
                    Status = StatusEntiteta.AKTIVAN.ToString(),
                    Kreiran = DateTime.Now
                };

                db.Klijenti.Add(klijent);
                db.SaveChanges();

                
                string? vlasnikRaw = GetCellValue(wbPart, row, 10);
                string? datVazVlasnika = GetCellValue(wbPart, row, 11);
                string? procenatRaw = GetCellValue(wbPart, row, 12);
                string? datUtvrdjivanja = GetCellValue(wbPart, row, 13);
                string? izvorPodatka = GetCellValue(wbPart, row, 14);

                if (!string.IsNullOrWhiteSpace(vlasnikRaw))
                {
                    var vlasnici = ParseVlasnici(vlasnikRaw, datVazVlasnika, procenatRaw);


                    foreach (var v in vlasnici)
                    {
                        v.KlijentId = klijent.Id;
                        v.DatumUtvrdjivanja = ParseDate(datUtvrdjivanja);
                        v.IzvorPodatka = izvorPodatka?.Trim();
                        v.Status = StatusEntiteta.AKTIVAN.ToString();
                        db.Vlasnici.Add(v);
                        privremeniVlasnikCount++;
                    }
                }

                
                string? direktorRaw = GetCellValue(wbPart, row, 15);
                string? datVazDirektora = GetCellValue(wbPart, row, 16);

                if (!string.IsNullOrWhiteSpace(direktorRaw))
                {
                    var direktori = ParseDirektori(direktorRaw, datVazDirektora);
                    foreach (var d in direktori)
                    {
                        d.KlijentId = klijent.Id;
                        d.Status = StatusEntiteta.AKTIVAN.ToString();
                        db.Direktori.Add(d);
                    }
                }

                
                string? statusUgovora = GetCellValue(wbPart, row, 25);
                string? datumUgovora = GetCellValue(wbPart, row, 26);
                if (!string.IsNullOrWhiteSpace(statusUgovora))
                {
                    db.Ugovori.Add(new Ugovor
                    {
                        KlijentId = klijent.Id,
                        StatusUgovora = statusUgovora.Trim(),
                        DatumUgovora = ParseDate(datumUgovora)
                    });
                }

                db.SaveChanges();
                tx.Commit();
                result.VlasnikCount += privremeniVlasnikCount;

                Log($"[OK] Red={i + 3} KlijentId={klijent.Id} '{naziv}'");
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        private OwnerTrackDbContext KreirajDb()
        {
            var opts = new DbContextOptionsBuilder<OwnerTrackDbContext>()
                .UseSqlite(_connectionString)
                .Options;
            return new OwnerTrackDbContext(opts);
        }

        private void EnsureDjelatnostExists(OwnerTrackDbContext db, string sifra, string? naziv)
        {
            if (string.IsNullOrWhiteSpace(sifra)) return;
            if (db.Djelatnosti.AsNoTracking().Any(d => d.Sifra == sifra)) return;

            string ime = (string.IsNullOrWhiteSpace(naziv) || naziv.StartsWith("="))
                ? $"Djelatnost {sifra}" : naziv;

            db.Djelatnosti.Add(new Djelatnost { Sifra = sifra, Naziv = ime });
            db.SaveChanges();
        }

        private List<Vlasnik> ParseVlasnici(string raw, string? datVazRaw, string? procenatRaw)
        {
            var result = new List<Vlasnik>();
            raw = NormalizeWhitespace(raw);

            List<string> imena = raw.Contains(',')
                ? raw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => s.Trim()).Where(s => s.Length > 0).ToList()
                : SplitNaImena(raw.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            var datumi = ParseTokenLista(datVazRaw, jeDatum: true);
            var procenti = ParseTokenLista(procenatRaw, jeDatum: false);

            for (int i = 0; i < imena.Count; i++)
            {
                string ime = NormalizeWhitespace(imena[i]);
                if (string.IsNullOrWhiteSpace(ime)) continue;

                result.Add(new Vlasnik
                {
                    ImePrezime = ime.ToUpperInvariant(),
                    ProcenatVlasnistva = ParseDecimal(i < procenti.Count ? procenti[i] : null),
                    DatumValjanostiDokumenta = ParseDate(i < datumi.Count ? datumi[i] : null),
                    Status = StatusEntiteta.AKTIVAN.ToString()
                });
            }

            return result;
        }

        private List<string> SplitNaImena(string[] rijeci)
        {
            var lista = new List<string>();
            if (rijeci.Length == 0) return lista;

            string cijeli = string.Join(" ", rijeci);

            bool imaFirmaKeyword = Regex.IsMatch(cijeli,
                @"\b(d\.o\.o\.|doo|d\.d\.|dd|a\.d\.|ltd|gmbh|inc|zajednica|dioničar|fond|komisija|općina|kanton|vlada)\b",
                RegexOptions.IgnoreCase);

            if (imaFirmaKeyword || rijeci.Length > 4 || rijeci.Length % 2 != 0)
            {
                lista.Add(cijeli);
                return lista;
            }

            for (int i = 0; i + 1 < rijeci.Length; i += 2)
                lista.Add(rijeci[i] + " " + rijeci[i + 1]);

            return lista;
        }

        private List<string> ParseTokenLista(string? raw, bool jeDatum)
        {
            var lista = new List<string>();
            if (string.IsNullOrWhiteSpace(raw)) return lista;
            raw = NormalizeWhitespace(raw);

            if (jeDatum)
            {
                var matches = Regex.Matches(raw, @"\d{1,2}\.\d{1,2}\.\d{4}\.?");
                if (matches.Count > 0)
                {
                    foreach (Match m in matches) lista.Add(m.Value.Trim());
                    return lista;
                }
            }

            lista.AddRange(Regex.Split(raw, @"\s+").Where(s => !string.IsNullOrWhiteSpace(s)));
            return lista;
        }

        private List<Direktor> ParseDirektori(string raw, string? datVazRaw)
        {
            var result = new List<Direktor>();
            raw = NormalizeWhitespace(raw);

            List<string> imena = raw.Contains(',')
                ? raw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => s.Trim()).Where(s => s.Length > 0).ToList()
                : new List<string> { raw };

            string datVazNorm = datVazRaw?.Trim().ToUpper() ?? "";
            bool jeTrajno = datVazNorm == "TRAJNO";
            DateTime? datVaz = jeTrajno ? null : ParseDate(datVazRaw);
            string tip = (jeTrajno || datVaz == null)
                ? TipValjanosti.TRAJNO.ToString()
                : TipValjanosti.VREMENSKI.ToString();

            foreach (var ime in imena)
            {
                if (string.IsNullOrWhiteSpace(ime)) continue;
                result.Add(new Direktor
                {
                    ImePrezime = ime,
                    DatumValjanosti = datVaz,
                    TipValjanosti = tip,
                    Status = StatusEntiteta.AKTIVAN.ToString()
                });
            }

            return result;
        }

        private string GetCellValue(WorkbookPart wbPart, Row row, int colIndex)
        {
            try
            {
                var cell = row.Elements<Cell>()
                    .FirstOrDefault(c => GetColIndex(c.CellReference?.Value) == colIndex);
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

        private int GetColIndex(string? cellRef)
        {
            if (string.IsNullOrWhiteSpace(cellRef)) return 0;
            string col = Regex.Replace(cellRef, @"\d", "");
            int idx = 0;
            foreach (char c in col)
                idx = idx * 26 + (c - 'A' + 1);
            return idx;
        }

        private DateTime? ParseDate(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            s = s.Trim();

            if (double.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out double oa)
                && oa > 10000 && oa < 100000)
            {
                try { return DateTime.FromOADate(oa); } catch { }
            }

            if (s.ToUpper() is "TRAJNO" or "STEČAJ" or "STECAJ") return null;

            string[] formats = {
                "dd.MM.yyyy.", "dd.MM.yyyy",
                "d.M.yyyy.",  "d.M.yyyy",
                "d.MM.yyyy.", "d.MM.yyyy",
                "yyyy-MM-dd"
            };

            foreach (var fmt in formats)
                if (DateTime.TryParseExact(s, fmt, CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out DateTime dt))
                    return dt;
            return null;
        }

        private decimal ParseDecimal(string? s)
        {
            if (string.IsNullOrWhiteSpace(s) || s == "-") return 0;
            s = s.Replace(",", ".").Trim();
            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal d) ? d : 0;
        }

        private string NormalizeWhitespace(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            return Regex.Replace(s.Replace("\u00a0", " "), @" {2,}", " ").Trim();
        }

        private string? NormalizeVelicina(string? v)
        {
            if (string.IsNullOrWhiteSpace(v)) return null;
            string u = v.ToUpper().Trim();

            
            var aliasi = new Dictionary<string, VelicinaFirme>(StringComparer.OrdinalIgnoreCase)
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
                ["UDRUZ"] = VelicinaFirme.UDRUŽENJE,
            };

            if (aliasi.TryGetValue(u, out var rezultat))
                return rezultat.ToString();

            
            if (Enum.TryParse<VelicinaFirme>(u, ignoreCase: true, out var parsed))
                return parsed.ToString();

            return v.Trim(); 
        }

        private string? NormalizeDaNe(string? v)
        {
            if (string.IsNullOrWhiteSpace(v)) return null;
            string first = v.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0].ToUpper();
            return first switch { "DA" => "DA", "NE" => "NE", _ => null };
        }

        private string NormalizeVrstaKlijenta(string? v)
        {
            if (string.IsNullOrWhiteSpace(v)) return "PRAVNO LICE";
            string u = v.ToUpper().Trim();
            if (u.Contains("FIZIČKO") || u.Contains("FIZICKO")) return "FIZIČKO LICE";
            if (u.Contains("UDRUŽ") || u.Contains("UDRUZ")) return "UDRUŽENJE";
            if (u == "OBRTNIK" || u.Contains("OBRT")) return "OBRTNIK";
            return "PRAVNO LICE";
        }
    }
}