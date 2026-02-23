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

namespace OwnerTrack.App
{
    public class ExcelImportService
    {
        private OwnerTrackDbContext _db;



        public ExcelImportService(OwnerTrackDbContext db)
        {
            _db = db;
        }

        public ImportResult ImportFromExcel(string filePath, IProgress<ImportProgress> progress = null)
        {
            if (_db == null) throw new Exception("_db JE NULL");
            if (_db.Klijenti == null) throw new Exception("_db.Klijenti JE NULL");
            if (_db.Database == null) throw new Exception("_db.Database JE NULL");

            var result = new ImportResult();
            var importProgress = new ImportProgress();

            void Log(string text) => Debug.WriteLine(text);

            try
            {
                Log($"[IMPORT-START] File='{filePath}' Time={DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false))
                {
                    WorkbookPart workbookPart = doc.WorkbookPart;

                    if (workbookPart?.Workbook?.Sheets == null)
                        throw new Exception("Excel fajl nema validan Workbook/Sheets!");

                    Sheet sheet = workbookPart.Workbook.Sheets.Cast<Sheet>()
                        .FirstOrDefault(s => s.Name?.Value != null && s.Name.Value.Contains("ZBIRNA"));

                    if (sheet == null)
                        throw new Exception("Nije pronađen list sa 'ZBIRNA'!");

                    Log($"[SHEET] Using sheet='{sheet.Name?.Value}'");

                    WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                    
                    var allRows = sheetData.Elements<Row>().Skip(2).ToList();
                    importProgress.TotalRows = allRows.Count;

                    Log($"[ROWS] TotalRows={allRows.Count}");

                    for (int i = 0; i < allRows.Count; i++)
                    {
                        var row = allRows[i];
                        string naziv = "";
                        string idBroj = "";

                        try
                        {
                            naziv = (GetCellValue(workbookPart, row, 2) ?? "").Trim();  // B
                            idBroj = (GetCellValue(workbookPart, row, 3) ?? "").Trim(); // C

                            importProgress.CurrentRow = $"{naziv} ({idBroj})";
                            importProgress.ProcessedRows = i + 1;

                            if (string.IsNullOrWhiteSpace(naziv) || string.IsNullOrWhiteSpace(idBroj))
                            {
                                Log($"[SKIP-EMPTY] ExcelRed={i + 3} Naziv='{naziv}' ID='{idBroj}'");
                                progress?.Report(importProgress);
                                continue;
                            }

                            
                            if (_db.Klijenti.AsNoTracking().Any(k => k.IdBroj == idBroj))
                            {
                                Log($"[SKIP-DUPLICATE-ID] ExcelRed={i + 3} ID='{idBroj}' Naziv='{naziv}'");
                                progress?.Report(importProgress);
                                continue;
                            }

                            if (_db.Klijenti.AsNoTracking().Any(k => k.Naziv == naziv))
                            {
                                Log($"[SKIP-DUPLICATE-NAZIV] ExcelRed={i + 3} Naziv='{naziv}' ID='{idBroj}'");
                                result.ErrorCount++;
                                result.Errors.Add($"Red {i + 3} | Naziv='{naziv}' već postoji u bazi s drugačijim ID brojem!");
                                progress?.Report(importProgress);
                                continue;
                            }


                            string sifraDjelatnosti = GetCellValue(workbookPart, row, 5)?.Trim(); // E
                            string nazivDjelatnosti = GetCellValue(workbookPart, row, 6)?.Trim(); // F

                            if (string.IsNullOrWhiteSpace(sifraDjelatnosti))
                                sifraDjelatnosti = "69.20";

                            EnsureDjelatnostExists(sifraDjelatnosti, nazivDjelatnosti);


                            var klijent = new Klijent
                            {
                                Naziv = naziv,
                                IdBroj = idBroj,
                                Adresa = GetCellValue(workbookPart, row, 4)?.Trim(),           // D
                                SifraDjelatnosti = sifraDjelatnosti,
                                DatumUspostave = ParseDate(GetCellValue(workbookPart, row, 7)), // G
                                VrstaKlijenta = NormalizeVrstaKlijenta(GetCellValue(workbookPart, row, 8)), // H
                                DatumOsnivanja = ParseDate(GetCellValue(workbookPart, row, 9)), // I
                                Velicina = NormalizeVelicina(GetCellValue(workbookPart, row, 17)), // Q - FIX #9
                                PepRizik = NormalizeDaNe(GetCellValue(workbookPart, row, 18)),  // R
                                UboRizik = NormalizeDaNe(GetCellValue(workbookPart, row, 19)),  // S
                                GotovinaRizik = NormalizeDaNe(GetCellValue(workbookPart, row, 20)), // T
                                GeografskiRizik = NormalizeDaNe(GetCellValue(workbookPart, row, 21)), // U
                                UkupnaProcjena = GetCellValue(workbookPart, row, 22)?.Trim(),   // V
                                DatumProcjene = ParseDate(GetCellValue(workbookPart, row, 23)), // W
                                OvjeraCr = GetCellValue(workbookPart, row, 24)?.Trim(),         // X
                                Status = StatusEntiteta.AKTIVAN.ToString()
                            };

                            _db.Klijenti.Add(klijent);
                            SaveChangesWithRetry();

                            Log($"[KLIJENT-SAVED] ExcelRed={i + 3} KlijentId={klijent.Id} Naziv='{naziv}' ID='{idBroj}'");


                            string vlasnikRaw = GetCellValue(workbookPart, row, 10);        // J
                            string datVazVlasnikaRaw = GetCellValue(workbookPart, row, 11); // K
                            string procenatRaw = GetCellValue(workbookPart, row, 12);       // L
                            string datUtvrdjivanja = GetCellValue(workbookPart, row, 13);   // M
                            string izvorPodatka = GetCellValue(workbookPart, row, 14);      // N

                            if (!string.IsNullOrWhiteSpace(vlasnikRaw))
                            {
                                var vlasnici = ParseVlasnici(vlasnikRaw, datVazVlasnikaRaw, procenatRaw);
                                foreach (var vlasnik in vlasnici)
                                {
                                    vlasnik.KlijentId = klijent.Id;
                                    vlasnik.DatumUtvrdjivanja = ParseDate(datUtvrdjivanja);
                                    vlasnik.IzvorPodatka = izvorPodatka?.Trim();
                                    vlasnik.Status = StatusEntiteta.AKTIVAN.ToString();
                                    _db.Vlasnici.Add(vlasnik);
                                    result.VlasnikCount++;
                                    Log($"  [VLASNIK] '{vlasnik.ImePrezime}' {vlasnik.ProcetatVlasnistva}%");
                                }
                            }


                            string direktorRaw = GetCellValue(workbookPart, row, 15);        // O
                            string datVazDirektoraRaw = GetCellValue(workbookPart, row, 16); // P

                            if (!string.IsNullOrWhiteSpace(direktorRaw))
                            {
                                var direktori = ParseDirektori(direktorRaw, datVazDirektoraRaw);
                                foreach (var direktor in direktori)
                                {
                                    direktor.KlijentId = klijent.Id;
                                    direktor.Status = StatusEntiteta.AKTIVAN.ToString();
                                    _db.Direktori.Add(direktor);
                                    Log($"  [DIREKTOR] '{direktor.ImePrezime}' tip={direktor.TipValjanosti}");
                                }
                            }


                            string statusUgovora = GetCellValue(workbookPart, row, 25); // Y
                            string datumUgovora = GetCellValue(workbookPart, row, 26);  // Z

                            if (!string.IsNullOrWhiteSpace(statusUgovora))
                            {
                                _db.Ugovori.Add(new Ugovor
                                {
                                    KlijentId = klijent.Id,
                                    StatusUgovora = statusUgovora.Trim(),
                                    DatumUgovora = ParseDate(datumUgovora)
                                });
                            }

                            SaveChangesWithRetry();

                            result.SuccessCount++;
                            importProgress.SuccessCount = result.SuccessCount;
                            Log($"[OK] ExcelRed={i + 3} KlijentId={klijent.Id} Naziv='{naziv}' ID='{idBroj}'");

                            _db.ChangeTracker.Clear();
                            progress?.Report(importProgress);
                        }
                        catch (Exception ex)
                        {
                            _db.ChangeTracker.Clear();
                            result.ErrorCount++;
                            importProgress.ErrorCount = result.ErrorCount;

                            string sqliteInfo = ExtractSqliteInfo(ex);
                            string inner = ex.InnerException?.Message ?? "";

                            Log($"[ERROR] ExcelRed={i + 3} Naziv='{naziv}' ID='{idBroj}' Msg='{ex.Message}'{sqliteInfo}"
                                + (string.IsNullOrWhiteSpace(inner) ? "" : $" Inner='{inner}'"));

                            result.Errors.Add(
                                $"Red {i + 3} | Naziv={naziv} | ID={idBroj} | {ex.Message}{sqliteInfo}"
                                + (string.IsNullOrWhiteSpace(inner) ? "" : $" | Inner={inner}")
                            );

                            progress?.Report(importProgress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                string sqliteInfo = ExtractSqliteInfo(ex);
                Debug.WriteLine($"[CRITICAL] Msg='{ex.Message}'{sqliteInfo}");
                if (ex.InnerException != null)
                    Debug.WriteLine($"[CRITICAL-INNER] {ex.InnerException.Message}");
                result.Errors.Add($"KRITIČNA GREŠKA: {ex.Message}{sqliteInfo}");
                return result;
            }

            Debug.WriteLine($"[IMPORT-END] Success={result.SuccessCount} Error={result.ErrorCount} Vlasnici={result.VlasnikCount}");
            result.Success = true;
            return result;
        }


        private void EnsureDjelatnostExists(string sifra, string naziv)
        {
            if (string.IsNullOrWhiteSpace(sifra)) return;

            bool exists = _db.Djelatnosti.AsNoTracking().Any(d => d.Sifra == sifra);
            if (exists) return;


            string nazivZapis = naziv;
            if (string.IsNullOrWhiteSpace(nazivZapis) || nazivZapis.StartsWith("="))
                nazivZapis = $"Djelatnost {sifra}";

            _db.Djelatnosti.Add(new Djelatnost
            {
                Sifra = sifra,
                Naziv = nazivZapis
            });

            SaveChangesWithRetry();
            Debug.WriteLine($"[DJELATNOST-CREATED] Sifra='{sifra}' Naziv='{nazivZapis}'");
        }

        private List<Vlasnik> ParseVlasnici(string vlasnikRaw, string datVazRaw, string procenatRaw)
        {
            var result = new List<Vlasnik>();
            if (string.IsNullOrWhiteSpace(vlasnikRaw))
                return result;

            vlasnikRaw = NormalizeWhitespace(vlasnikRaw);

            List<string> imenaLista;

            if (vlasnikRaw.Contains(","))
            {
                imenaLista = vlasnikRaw
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();
            }
            else
            {
                var rijeci = vlasnikRaw.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                imenaLista = SplitNaImena(rijeci);
            }

            var datVazLista = ParseTokenLista(datVazRaw, jeDatum: true);
            var procenatLista = ParseTokenLista(procenatRaw, jeDatum: false);

            for (int i = 0; i < imenaLista.Count; i++)
            {
                string ime = imenaLista[i];
                if (string.IsNullOrWhiteSpace(ime)) continue;

                string datVazToken = i < datVazLista.Count ? datVazLista[i] : null;
                string procToken = i < procenatLista.Count ? procenatLista[i] : null;

                
                string imeNorm = NormalizeWhitespace(ime).ToUpperInvariant();

                result.Add(new Vlasnik
                {
                    ImePrezime = imeNorm,
                    DatumValjanostiDokumenta = ParseDate(datVazToken),
                    ProcetatVlasnistva = ParseDecimal(procToken),
                    Status = StatusEntiteta.AKTIVAN.ToString()
                });
            }

            return result;
        }


      
        private List<string> SplitNaImena(string[] rijeci)
        {
            var lista = new List<string>();
            if (rijeci.Length == 0) return lista;

            if (rijeci.Length <= 2)
            {
                lista.Add(string.Join(" ", rijeci));
                return lista;
            }

            string cijeli = string.Join(" ", rijeci);

           
            if (rijeci.Length > 4 ||
                Regex.IsMatch(cijeli,
                    @"\b(d\.o\.o\.|doo|d\.d\.|dd|a\.d\.|ad|ltd|gmbh|inc|zajednica|dioničar|diničar|registar|komisija|općina|kanton|vlada|fond)\b",
                    RegexOptions.IgnoreCase))
            {
                lista.Add(cijeli);
                return lista;
            }

           
            if (rijeci.Length % 2 != 0)
            {
                lista.Add(cijeli);
                return lista;
            }

           
            for (int i = 0; i + 1 < rijeci.Length; i += 2)
                lista.Add(rijeci[i] + " " + rijeci[i + 1]);

            return lista;
        }


        private List<string> ParseTokenLista(string raw, bool jeDatum)
        {
            var lista = new List<string>();
            if (string.IsNullOrWhiteSpace(raw)) return lista;

            raw = NormalizeWhitespace(raw);

            if (jeDatum)
            {
                var matches = Regex.Matches(raw, @"\d{1,2}\.\d{1,2}\.\d{4}\.?");
                if (matches.Count > 0)
                {
                    foreach (Match m in matches)
                        lista.Add(m.Value.Trim());
                    return lista;
                }
            }

            lista.AddRange(Regex.Split(raw, @"\s+").Where(s => !string.IsNullOrWhiteSpace(s)));
            return lista;
        }


        private List<Direktor> ParseDirektori(string direktorRaw, string datVazRaw)
        {
            var result = new List<Direktor>();
            if (string.IsNullOrWhiteSpace(direktorRaw))
                return result;

            direktorRaw = NormalizeWhitespace(direktorRaw);

            List<string> imenaLista;
            if (direktorRaw.Contains(","))
            {
                imenaLista = direktorRaw
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();
            }
            else
            {
                imenaLista = new List<string> { direktorRaw };
            }

            string datVazNorm = datVazRaw?.Trim().ToUpper() ?? "";
            bool jeTrajno = datVazNorm == "TRAJNO";
            DateTime? datVaz = jeTrajno ? (DateTime?)null : ParseDate(datVazRaw);

            
            string tipValjanosti;
            if (jeTrajno || datVaz == null)
                tipValjanosti = TipValjanosti.TRAJNO.ToString();
            else
                tipValjanosti = TipValjanosti.VREMENSKI.ToString();

            foreach (var ime in imenaLista)
            {
                if (string.IsNullOrWhiteSpace(ime)) continue;
                result.Add(new Direktor
                {
                    ImePrezime = ime,
                    DatumValjanosti = datVaz,
                    TipValjanosti = tipValjanosti,
                    Status = StatusEntiteta.AKTIVAN.ToString()
                });
            }

            return result;
        }



        private string NormalizeWhitespace(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            return Regex.Replace(s.Replace("\u00a0", " "), @" {2,}", " ").Trim();
        }

        private void SaveChangesWithRetry()
        {
            int attempts = 0;
            while (attempts < 5)
            {
                try
                {
                    _db.SaveChanges();
                    return;
                }
                catch (DbUpdateException ex) when (ex.InnerException is SqliteException { SqliteErrorCode: 5 })
                {
                    attempts++;
                    if (attempts >= 5) throw;
                    Thread.Sleep(150 * attempts);
                }
                catch (SqliteException ex) when (ex.SqliteErrorCode == 5)
                {
                    attempts++;
                    if (attempts >= 5) throw;
                    Thread.Sleep(150 * attempts);
                }
            }
        }

        private string ExtractSqliteInfo(Exception ex)
        {
            var sqliteEx = ex as SqliteException ?? ex.InnerException as SqliteException;
            return sqliteEx == null
                ? ""
                : $" | SQLiteCode={sqliteEx.SqliteErrorCode} Ext={sqliteEx.SqliteExtendedErrorCode}";
        }

        private string GetCellValue(WorkbookPart workbookPart, Row row, int columnIndex)
        {
            try
            {
                Cell cell = row.Elements<Cell>()
                    .FirstOrDefault(c => GetColumnIndex(c.CellReference?.Value) == columnIndex);

                if (cell == null) return "";

                
                if (cell.DataType == null)
                    return cell.CellValue?.Text ?? "";

                
                if (cell.DataType.Value == CellValues.SharedString)
                {
                    if (cell.CellValue == null) return "";
                    if (!int.TryParse(cell.CellValue.Text, out int id)) return "";
                    return workbookPart.SharedStringTablePart?.SharedStringTable
                               .Elements<SharedStringItem>()
                               .ElementAt(id)
                               .InnerText ?? "";
                }

                
                if (cell.DataType.Value == CellValues.String)
                    return cell.CellValue?.Text ?? "";

                return cell.CellValue?.Text ?? "";
            }
            catch (Exception ex)
            {
                
                Debug.WriteLine($"[GETCELL-ERROR] CellRef problem: {ex.Message}");
                return "";
            }
        }

        private int GetColumnIndex(string cellReference)
        {
            if (string.IsNullOrWhiteSpace(cellReference)) return 0;
            string col = Regex.Replace(cellReference, @"\d", "");
            int index = 0;
            foreach (char c in col)
                index = index * 26 + (c - 'A' + 1);
            return index;
        }

        private DateTime? ParseDate(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString)) return null;
            dateString = dateString.Trim();

            
            if (double.TryParse(dateString, NumberStyles.Number, CultureInfo.InvariantCulture, out double oaDate)
                && oaDate > 10000 && oaDate < 100000)
            {
                try { return DateTime.FromOADate(oaDate); }
                catch { }
            }

            string upper = dateString.ToUpper();
            if (upper == "TRAJNO" || upper == "STEČAJ" || upper == "STECAJ")
                return null;

            string[] formats =
            {
                "dd.MM.yyyy.", "dd.MM.yyyy",
                "d.M.yyyy.",  "d.M.yyyy",
                "d.MM.yyyy.", "d.MM.yyyy",
                "yyyy-MM-dd"
            };

            foreach (var fmt in formats)
            {
                if (DateTime.TryParseExact(dateString, fmt,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                    return dt;
            }

            return null;
        }

        private decimal ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;
            value = value.Replace(",", ".").Trim();
            if (value == "-") return 0;
            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal d))
                return d;
            return 0;
        }

        
        private string NormalizeVelicina(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            string upper = value.ToUpper().Trim();
            if (upper == "MIKRO" || upper == "MICRO") return "MIKRO";
            if (upper == "MALO" || upper == "MALI" || upper == "MALA") return "MALO";
            if (upper == "SREDNJE" || upper == "SREDNJI" || upper == "SREDNJA") return "SREDNJE";
            if (upper == "VELIKA" || upper == "VELIKI" || upper == "VELIKO") return "VELIKA";
            return value.Trim();
        }

       
        private string NormalizeDaNe(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            
            string first = value.Trim().Split(new[] { ' ', '	' }, StringSplitOptions.RemoveEmptyEntries)[0].ToUpper();
            if (first == "DA") return DaNe.DA.ToString();
            if (first == "NE") return DaNe.NE.ToString();
            return null;
        }

       
        private string NormalizeVrstaKlijenta(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "PRAVNO LICE";

            string upper = value.ToUpper().Trim();

            if (upper.Contains("FIZIČKO") || upper.Contains("FIZICKO") ||
                upper.Contains("FIZIČKA") || upper.Contains("FIZICKA"))
                return "FIZIČKO LICE";
            if (upper.Contains("UDRUŽ") || upper.Contains("UDRUZ") || upper == "UDRUŽENJE")
                return "UDRUŽENJE";
            if (upper == "OBRTNIK" || upper.Contains("OBRT"))
                return "OBRTNIK";
            if (upper == "PRAVNO LICE")
                return "PRAVNO LICE";

            return value.Trim();
        }
    }
}