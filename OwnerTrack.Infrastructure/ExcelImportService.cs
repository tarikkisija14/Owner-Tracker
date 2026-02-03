using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace OwnerTrack.App
{
    public class ExcelImportService
    {
        private OwnerTrackDbContext _db;
        private const int BATCH_SIZE = 5; // (trenutno se ne koristi)

        public ExcelImportService(OwnerTrackDbContext db)
        {
            _db = db;
        }

        public ImportResult ImportFromExcel(string filePath, IProgress<ImportProgress> progress = null)
        {
            if (_db == null)
                throw new Exception("_db JE NULL");

            if (_db.Klijenti == null)
                throw new Exception("_db.Klijenti JE NULL");

            if (_db.Database == null)
                throw new Exception("_db.Database JE NULL");

            var result = new ImportResult();
            var importProgress = new ImportProgress();

            void Log(string text)
            {
                Debug.WriteLine(text);
            }

            try
            {
                Log($"[IMPORT-START] File='{filePath}' Time={DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false))
                {
                    WorkbookPart workbookPart = doc.WorkbookPart;

                    if (workbookPart == null || workbookPart.Workbook == null || workbookPart.Workbook.Sheets == null)
                        throw new Exception("Excel fajl nema validan Workbook/Sheets!");

                    Sheet sheet = workbookPart.Workbook.Sheets.Cast<Sheet>()
                        .FirstOrDefault(s => s.Name != null && s.Name.Value != null && s.Name.Value.Contains("ZBIRNA"));

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
                            naziv = (GetCellValue(workbookPart, row, 2) ?? "").Trim();
                            idBroj = (GetCellValue(workbookPart, row, 3) ?? "").Trim();

                            importProgress.CurrentRow = $"{naziv} ({idBroj})";
                            importProgress.ProcessedRows = i + 1;

                            if (string.IsNullOrWhiteSpace(naziv) || string.IsNullOrWhiteSpace(idBroj))
                            {
                                Log($"[SKIP-EMPTY] ExcelRed={i + 3} Naziv='{naziv}' ID='{idBroj}'");
                                progress?.Report(importProgress);
                                continue;
                            }

                            // BITNO: AsNoTracking da se ne nakupi tracking, i bude brže/čistije
                            if (_db.Klijenti.AsNoTracking().Any(k => k.IdBroj == idBroj))
                            {
                                Log($"[SKIP-DUPLICATE] ExcelRed={i + 3} ID='{idBroj}' Naziv='{naziv}'");
                                progress?.Report(importProgress);
                                continue;
                            }

                            var klijent = new Klijent
                            {
                                Naziv = naziv,
                                IdBroj = idBroj,
                                Adresa = GetCellValue(workbookPart, row, 4),
                                SifraDjelatnosti = (GetCellValue(workbookPart, row, 5) ?? "69.20"),
                                DatumUspostave = ParseDate(GetCellValue(workbookPart, row, 7)),
                                VrstaKlijenta = (GetCellValue(workbookPart, row, 8) ?? "PRAVNO LICE"),
                                DatumOsnivanja = ParseDate(GetCellValue(workbookPart, row, 9)),
                                Velicina = GetCellValue(workbookPart, row, 17),
                                PepRizik = NormalizeDaNe(GetCellValue(workbookPart, row, 18)),
                                UboRizik = NormalizeDaNe(GetCellValue(workbookPart, row, 19)),
                                GotovinaRizik = NormalizeDaNe(GetCellValue(workbookPart, row, 20)),
                                GeografskiRizik = NormalizeDaNe(GetCellValue(workbookPart, row, 21)),
                                UkupnaProcjena = GetCellValue(workbookPart, row, 22),
                                DatumProcjene = ParseDate(GetCellValue(workbookPart, row, 23)),
                                OvjeraCr = GetCellValue(workbookPart, row, 24),
                                Status = "AKTIVAN"
                            };

                            _db.Klijenti.Add(klijent);

                            // KRITIČNO: klijent mora dobiti Id prije Vlasnika/Direktora/Ugovora
                            SaveChangesWithRetry();

                            Log($"[KLIJENT-SAVED] ExcelRed={i + 3} KlijentId={klijent.Id} Naziv='{naziv}' ID='{idBroj}'");

                            string vlasnicString = GetCellValue(workbookPart, row, 10);
                            string procenatString = GetCellValue(workbookPart, row, 12);
                            string datumString = GetCellValue(workbookPart, row, 13);
                            string izvorString = GetCellValue(workbookPart, row, 14);

                            if (!string.IsNullOrWhiteSpace(vlasnicString))
                            {
                                var vlasnici = ParseVlasnici(vlasnicString, procenatString);
                                foreach (var vlasnik in vlasnici)
                                {
                                    vlasnik.KlijentId = klijent.Id;
                                    vlasnik.DatumUtvrdjivanja = ParseDate(datumString);
                                    vlasnik.IzvorPodatka = izvorString;
                                    vlasnik.Status = "AKTIVAN";
                                    _db.Vlasnici.Add(vlasnik);
                                    result.VlasnikCount++;
                                }
                            }

                            string direktorString = GetCellValue(workbookPart, row, 15);
                            string datumDirektora = GetCellValue(workbookPart, row, 16);

                            if (!string.IsNullOrWhiteSpace(direktorString))
                            {
                                var direktori = ParseDirektori(direktorString, datumDirektora);
                                foreach (var direktor in direktori)
                                {
                                    direktor.KlijentId = klijent.Id;
                                    direktor.Status = "AKTIVAN";
                                    _db.Direktori.Add(direktor);
                                }
                            }

                            string statusUgovora = GetCellValue(workbookPart, row, 25);
                            string datumUgovora = GetCellValue(workbookPart, row, 26);

                            if (!string.IsNullOrWhiteSpace(statusUgovora))
                            {
                                var ugovor = new Ugovor
                                {
                                    KlijentId = klijent.Id,
                                    StatusUgovora = statusUgovora,
                                    DatumUgovora = ParseDate(datumUgovora)
                                };
                                _db.Ugovori.Add(ugovor);
                            }

                            // Snimi related podatke (vlasnici/direktori/ugovor) za ovaj red
                            SaveChangesWithRetry();

                            result.SuccessCount++;
                            importProgress.SuccessCount = result.SuccessCount;

                            Log($"[OK] ExcelRed={i + 3} KlijentId={klijent.Id} Naziv='{naziv}' ID='{idBroj}'");

                            // da se tracker ne napuni (pomaže i protiv "database is locked" scenarija kod dužih importa)
                            _db.ChangeTracker.Clear();

                            progress?.Report(importProgress);
                        }
                        catch (Exception ex)
                        {
                            // OBAVEZNO: očisti tracking nakon greške da ne ostanu "pokvareni" entiteti u kontekstu
                            _db.ChangeTracker.Clear();

                            result.ErrorCount++;
                            importProgress.ErrorCount = result.ErrorCount;

                            // Pokušaj izvući SQLite detalje
                            SqliteException sqliteEx = null;
                            if (ex is SqliteException se)
                                sqliteEx = se;
                            else if (ex.InnerException is SqliteException ise)
                                sqliteEx = ise;

                            string sqliteInfo = "";
                            if (sqliteEx != null)
                                sqliteInfo = $" | SQLiteCode={sqliteEx.SqliteErrorCode} Ext={sqliteEx.SqliteExtendedErrorCode}";

                            string inner = ex.InnerException != null ? ex.InnerException.Message : "";

                            string dbg =
                                $"[ERROR] ExcelRed={i + 3} Naziv='{naziv}' ID='{idBroj}' " +
                                $"Msg='{ex.Message}'{sqliteInfo}" +
                                (string.IsNullOrWhiteSpace(inner) ? "" : $" Inner='{inner}'");

                            Log(dbg);

                            result.Errors.Add(
                                $"Red {i + 3} | Naziv={naziv} | ID={idBroj} | {ex.Message}{sqliteInfo}" +
                                (string.IsNullOrWhiteSpace(inner) ? "" : $" | Inner={inner}")
                            );

                            progress?.Report(importProgress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;

                SqliteException sqliteEx = null;
                if (ex is SqliteException se)
                    sqliteEx = se;
                else if (ex.InnerException is SqliteException ise)
                    sqliteEx = ise;

                string sqliteInfo = "";
                if (sqliteEx != null)
                    sqliteInfo = $" | SQLiteCode={sqliteEx.SqliteErrorCode} Ext={sqliteEx.SqliteExtendedErrorCode}";

                Log($"[CRITICAL] Msg='{ex.Message}'{sqliteInfo}");

                if (ex.InnerException != null)
                    Log($"[CRITICAL-INNER] {ex.InnerException.Message}");

                result.Errors.Add($"KRITIČNA GREŠKA: {ex.Message}{sqliteInfo}");
                return result;
            }

            Log($"[IMPORT-END] SuccessCount={result.SuccessCount} ErrorCount={result.ErrorCount} VlasnikCount={result.VlasnikCount}");

            result.Success = true;
            return result;
        }

        // Retry za "database is locked" (SQLiteCode=5)
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
                catch (DbUpdateException ex)
                {
                    SqliteException sqliteEx = null;
                    if (ex.InnerException is SqliteException ise)
                        sqliteEx = ise;

                    if (sqliteEx != null && sqliteEx.SqliteErrorCode == 5) // SQLITE_BUSY / locked
                    {
                        attempts++;
                        Thread.Sleep(150 * attempts);
                        continue;
                    }

                    throw;
                }
                catch (SqliteException ex)
                {
                    if (ex.SqliteErrorCode == 5)
                    {
                        attempts++;
                        Thread.Sleep(150 * attempts);
                        continue;
                    }

                    throw;
                }
            }

            // ako je i dalje zaključano nakon retry-a, pusti izuzetak da ga gore uhvati i loguje
            _db.SaveChanges();
        }

        private string GetCellValue(WorkbookPart workbookPart, Row row, int columnIndex)
        {
            try
            {
                Cell cell = row.Elements<Cell>()
                    .FirstOrDefault(c => GetColumnIndex(c.CellReference?.Value) == columnIndex);

                if (cell == null)
                    return "";

                if (cell.DataType == null)
                    return cell.CellValue?.Text ?? "";

                if (cell.DataType.Value == CellValues.SharedString)
                {
                    if (cell.CellValue == null)
                        return "";

                    int stringId;
                    if (!int.TryParse(cell.CellValue.Text, out stringId))
                        return "";

                    SharedStringTablePart stringTablePart = workbookPart.SharedStringTablePart;
                    if (stringTablePart == null)
                        return "";

                    return stringTablePart.SharedStringTable
                        .Elements<SharedStringItem>()
                        .ElementAt(stringId)
                        .InnerText;
                }

                return cell.CellValue?.Text ?? "";
            }
            catch
            {
                return "";
            }
        }

        private int GetColumnIndex(string cellReference)
        {
            if (string.IsNullOrWhiteSpace(cellReference))
                return 0;

            string columnPart = System.Text.RegularExpressions.Regex.Replace(cellReference, @"\d", "");
            int column = 0;
            foreach (char c in columnPart)
            {
                column = column * 26 + (c - 'A' + 1);
            }
            return column;
        }

        private List<Vlasnik> ParseVlasnici(string vlasnicString, string procenatString)
        {
            var result = new List<Vlasnik>();

            if (string.IsNullOrWhiteSpace(vlasnicString))
                return result;

            // procenti su često jedan ili više; probaj razdvojiti
            var procentiLista = procenatString?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

            if (!vlasnicString.Contains("  ") && vlasnicString.Split(' ').Length <= 2)
            {
                result.Add(new Vlasnik
                {
                    ImePrezime = vlasnicString.Trim(),
                    ProcetatVlasnistva = ParseDecimal(procenatString),
                    Status = "AKTIVAN"
                });
            }
            else
            {
                var imena = vlasnicString.Split(new[] { "  " }, StringSplitOptions.RemoveEmptyEntries);

                if (imena.Length == 1)
                    imena = vlasnicString.Split(new[] { "   " }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < imena.Length; i++)
                {
                    string proc = i < procentiLista.Length ? procentiLista[i] : "";
                    result.Add(new Vlasnik
                    {
                        ImePrezime = imena[i].Trim(),
                        ProcetatVlasnistva = ParseDecimal(proc),
                        Status = "AKTIVAN"
                    });
                }
            }

            return result;
        }

        private List<Direktor> ParseDirektori(string direktorString, string datumString)
        {
            var result = new List<Direktor>();

            if (string.IsNullOrWhiteSpace(direktorString))
                return result;

            var imena = direktorString.Split(new[] { "  " }, StringSplitOptions.RemoveEmptyEntries);

            if (imena.Length == 0)
                imena = new[] { direktorString };

            foreach (var ime in imena)
            {
                string tipValjanosti = "VREMENSKI";
                if (!string.IsNullOrEmpty(datumString) && datumString.ToUpper().IndexOf("TRAJNO") >= 0)
                {
                    tipValjanosti = "TRAJNO";
                }

                result.Add(new Direktor
                {
                    ImePrezime = ime.Trim(),
                    DatumValjanosti = ParseDate(datumString),
                    TipValjanosti = tipValjanosti,
                    Status = "AKTIVAN"
                });
            }

            return result;
        }

        private DateTime? ParseDate(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString) || dateString.ToUpper() == "TRAJNO")
                return null;

            string[] formats = { "dd.MM.yyyy.", "dd.MM.yyyy", "d.M.yyyy.", "d.M.yyyy", "yyyy-MM-dd" };

            foreach (var format in formats)
            {
                DateTime date;
                if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    return date;
            }

            return null;
        }

        private decimal ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            decimal result;
            if (decimal.TryParse(value.Replace(",", "."), CultureInfo.InvariantCulture, out result))
                return result;

            return 0;
        }

        private string NormalizeDaNe(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            string upper = value.ToUpper().Trim();
            if (upper.StartsWith("D"))
                return "DA";
            if (upper.StartsWith("N"))
                return "NE";

            return null;
        }
    }
}
