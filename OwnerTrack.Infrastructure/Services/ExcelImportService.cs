using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Models;
using OwnerTrack.Infrastructure.Parsing;
using System.Diagnostics;

namespace OwnerTrack.Infrastructure.Services
{
    public class ExcelImportService
    {
        
        private static class Column
        {
            public const int Naziv = 2;
            public const int IdBroj = 3;
            public const int Adresa = 4;
            public const int SifraDjelatnosti = 5;
            public const int NazivDjelatnosti = 6;
            public const int DatumUspostave = 7;
            public const int VrstaKlijenta = 8;
            public const int DatumOsnivanja = 9;
            public const int Vlasnik = 10;
            public const int DatVazVlasnika = 11;
            public const int Procenat = 12;
            public const int DatUtvrdjivanja = 13;
            public const int IzvorPodatka = 14;
            public const int Direktor = 15;
            public const int DatVazDirektora = 16;
            public const int Velicina = 17;
            public const int PepRizik = 18;
            public const int UboRizik = 19;
            public const int GotovinaRizik = 20;
            public const int GeografskiRizik = 21;
            public const int UkupnaProcjena = 22;
            public const int DatumProcjene = 23;
            public const int OvjeraCr = 24;
            public const int StatusUgovora = 25;
            public const int DatumUgovora = 26;
        }

        private const int BatchSize = 50;
        private const int ExcelHeaderRows = 2;
        private const string SummarySheetKeyword = "ZBIRNA";

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

                using var document = SpreadsheetDocument.Open(filePath, isEditable: false);
                var workbookPart = document.WorkbookPart
                                         ?? throw new InvalidOperationException("Excel fajl nema validan WorkbookPart!");

                var worksheetPart = OpenSummarySheet(workbookPart);
                var dataRows = worksheetPart.Worksheet
                                        .Elements<SheetData>().First()
                                        .Elements<Row>()
                                        .Skip(ExcelHeaderRows)
                                        .ToList();

                prog.TotalRows = dataRows.Count;

                using var db = CreateDbContext();
                db.Database.ExecuteSqlRaw("PRAGMA foreign_keys = OFF;");
                using var tx = db.Database.BeginTransaction();

                var existingIdBrojevi = db.Klijenti.AsNoTracking().Select(k => k.IdBroj).ToHashSet(StringComparer.OrdinalIgnoreCase);
                var existingNames = db.Klijenti.AsNoTracking().Select(k => k.Naziv).ToHashSet(StringComparer.OrdinalIgnoreCase);
                var existingActivityCodes = db.Djelatnosti.AsNoTracking().Select(d => d.Sifra).ToHashSet(StringComparer.OrdinalIgnoreCase);

                int pendingChanges = 0;

                for (int i = 0; i < dataRows.Count; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Debug.WriteLine("[IMPORT-CANCELLED] Korisnik otkazao import.");
                        break;
                    }

                    var row = dataRows[i];
                    string naziv = string.Empty;
                    string idBroj = string.Empty;

                    try
                    {
                        naziv = ReadCell(workbookPart, row, Column.Naziv);
                        idBroj = ReadCell(workbookPart, row, Column.IdBroj);

                        prog.CurrentRow = $"{naziv} ({idBroj})";
                        prog.ProcessedRows = i + 1;

                        if (string.IsNullOrWhiteSpace(naziv) || string.IsNullOrWhiteSpace(idBroj))
                        {
                            progress?.Report(prog);
                            continue;
                        }

                        if (existingIdBrojevi.Contains(idBroj) || existingNames.Contains(naziv))
                        {
                            result.SkipCount++;
                            progress?.Report(prog);
                            continue;
                        }

                        string sifraDjelatnosti = ReadCell(workbookPart, row, Column.SifraDjelatnosti);
                        string? nazivDjelatnosti = ReadCellOrNull(workbookPart, row, Column.NazivDjelatnosti);

                        if (!string.IsNullOrWhiteSpace(sifraDjelatnosti))
                            EnsureActivityCodeExists(db, existingActivityCodes, sifraDjelatnosti, nazivDjelatnosti);

                        var klijent = MapKlijent(workbookPart, row, naziv, idBroj, sifraDjelatnosti);
                        db.Klijenti.Add(klijent);
                        db.SaveChanges();

                        existingIdBrojevi.Add(idBroj);
                        existingNames.Add(naziv);

                        ImportVlasnici(workbookPart, row, klijent, result, db);
                        ImportDirektori(workbookPart, row, klijent, db);
                        ImportUgovor(workbookPart, row, klijent, db);

                        result.SuccessCount++;
                        pendingChanges++;

                        if (pendingChanges >= BatchSize)
                        {
                            db.SaveChanges();
                            pendingChanges = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = BuildRowErrorMessage(i, naziv, idBroj, ex);
                        result.Errors.Add(errorMessage);
                        result.ErrorCount++;
                        Debug.WriteLine($"[IMPORT-ROW-ERROR] {errorMessage}");
                        ClearTrackedEntities(db);
                    }

                    progress?.Report(prog);
                }

                if (pendingChanges > 0)
                    db.SaveChanges();

                db.Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON;");
                tx.Commit();

                Debug.WriteLine($"[IMPORT-END] Success={result.SuccessCount} Skip={result.SkipCount} Errors={result.ErrorCount}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[IMPORT-FATAL] {ex.Message}");
                throw;
            }

            return result;
        }

       

        private Klijent MapKlijent(WorkbookPart wbp, Row row, string naziv, string idBroj, string sifraDjelatnosti) =>
            new()
            {
                Naziv = naziv,
                IdBroj = idBroj,
                Adresa = ReadCellOrNull(wbp, row, Column.Adresa),
                SifraDjelatnosti = sifraDjelatnosti,
                DatumUspostave = ExcelValueNormalizer.ParseDate(ReadCellOrNull(wbp, row, Column.DatumUspostave)),
                VrstaKlijenta = ExcelValueNormalizer.NormalizeVrstaKlijenta(ReadCellOrNull(wbp, row, Column.VrstaKlijenta)),
                DatumOsnivanja = ExcelValueNormalizer.ParseDate(ReadCellOrNull(wbp, row, Column.DatumOsnivanja)),
                Velicina = ExcelValueNormalizer.NormalizeVelicina(ReadCellOrNull(wbp, row, Column.Velicina)),
                PepRizik = ExcelValueNormalizer.NormalizeDaNe(ReadCellOrNull(wbp, row, Column.PepRizik)),
                UboRizik = ExcelValueNormalizer.NormalizeDaNe(ReadCellOrNull(wbp, row, Column.UboRizik)),
                GotovinaRizik = ExcelValueNormalizer.NormalizeDaNe(ReadCellOrNull(wbp, row, Column.GotovinaRizik)),
                GeografskiRizik = ExcelValueNormalizer.NormalizeDaNe(ReadCellOrNull(wbp, row, Column.GeografskiRizik)),
                UkupnaProcjena = ReadCellOrNull(wbp, row, Column.UkupnaProcjena),
                DatumProcjene = ExcelValueNormalizer.ParseDate(ReadCellOrNull(wbp, row, Column.DatumProcjene)),
                OvjeraCr = ReadCellOrNull(wbp, row, Column.OvjeraCr),
                Kreiran = DateTime.Now,
                Status = StatusEntiteta.AKTIVAN,
            };

        private void ImportVlasnici(WorkbookPart wbp, Row row, Klijent klijent, ImportResult result, OwnerTrackDbContext db)
        {
            string? rawNames = ReadCellOrNull(wbp, row, Column.Vlasnik);
            string? rawDates = ReadCellOrNull(wbp, row, Column.DatVazVlasnika);
            string? rawPercentages = ReadCellOrNull(wbp, row, Column.Procenat);
            string? rawDatUtvrdjivanja = ReadCellOrNull(wbp, row, Column.DatUtvrdjivanja);
            string? rawIzvorPodatka = ReadCellOrNull(wbp, row, Column.IzvorPodatka);

            if (string.IsNullOrWhiteSpace(rawNames)) return;

            foreach (var vlasnik in ExcelEntityParser.ParseVlasnici(rawNames, rawDates, rawPercentages))
            {
                vlasnik.KlijentId = klijent.Id;
                vlasnik.DatumUtvrdjivanja = ExcelValueNormalizer.ParseDate(rawDatUtvrdjivanja);
                vlasnik.IzvorPodatka = rawIzvorPodatka?.Trim();
                vlasnik.Status = StatusEntiteta.AKTIVAN;

                db.Vlasnici.Add(vlasnik);
                result.VlasnikCount++;
            }
        }

        private void ImportDirektori(WorkbookPart wbp, Row row, Klijent klijent, OwnerTrackDbContext db)
        {
            string? rawNames = ReadCellOrNull(wbp, row, Column.Direktor);
            string? rawDate = ReadCellOrNull(wbp, row, Column.DatVazDirektora);

            if (string.IsNullOrWhiteSpace(rawNames)) return;

            foreach (var direktor in ExcelEntityParser.ParseDirektori(rawNames, rawDate))
            {
                direktor.KlijentId = klijent.Id;
                direktor.Status = StatusEntiteta.AKTIVAN;
                db.Direktori.Add(direktor);
            }
        }

        private void ImportUgovor(WorkbookPart wbp, Row row, Klijent klijent, OwnerTrackDbContext db)
        {
            string? statusUgovora = ReadCellOrNull(wbp, row, Column.StatusUgovora);
            string? datumUgovora = ReadCellOrNull(wbp, row, Column.DatumUgovora);

            if (string.IsNullOrWhiteSpace(statusUgovora)) return;

            db.Ugovori.Add(new Ugovor
            {
                KlijentId = klijent.Id,
                StatusUgovora = statusUgovora.Trim(),
                DatumUgovora = ExcelValueNormalizer.ParseDate(datumUgovora),
            });
        }

        

        private static void EnsureActivityCodeExists(
            OwnerTrackDbContext db,
            HashSet<string> existingCodes,
            string code,
            string? rawName)
        {
            if (existingCodes.Contains(code)) return;

            string displayName = string.IsNullOrWhiteSpace(rawName) || rawName.StartsWith("=")
                ? $"Djelatnost {code}"
                : rawName;

            db.Djelatnosti.Add(new Djelatnost { Sifra = code, Naziv = displayName });
            existingCodes.Add(code);
        }

       
        private static string ReadCell(WorkbookPart wbp, Row row, int column) =>
            ExcelCellReader.GetCellValue(wbp, row, column).Trim();

       
        private static string? ReadCellOrNull(WorkbookPart wbp, Row row, int column)
        {
            string value = ExcelCellReader.GetCellValue(wbp, row, column).Trim();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private static WorksheetPart OpenSummarySheet(WorkbookPart workbookPart)
        {
            var sheet = workbookPart.Workbook.Sheets
                ?.Cast<Sheet>()
                .FirstOrDefault(s => s.Name?.Value?.Contains(SummarySheetKeyword) == true)
                ?? throw new InvalidOperationException($"Nije pronađen list sa '{SummarySheetKeyword}'!");

            return (WorksheetPart)workbookPart.GetPartById(sheet.Id!);
        }

        private static string BuildRowErrorMessage(int rowIndex, string naziv, string idBroj, Exception ex)
        {
            string inner = ex.InnerException?.Message ?? string.Empty;
            string suffix = string.IsNullOrEmpty(inner) ? string.Empty : $" | {inner}";
            return $"Red {rowIndex + ExcelHeaderRows + 1} | {naziv} | {idBroj} | {ex.Message}{suffix}";
        }

        private static void ClearTrackedEntities(OwnerTrackDbContext db)
        {
            foreach (var entry in db.ChangeTracker.Entries().ToList())
                entry.State = EntityState.Detached;
        }

        private OwnerTrackDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<OwnerTrackDbContext>()
                .UseSqlite(_connectionString)
                .Options;
            return new OwnerTrackDbContext(options);
        }
    }
}