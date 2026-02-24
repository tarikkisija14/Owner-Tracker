using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OwnerTrack.Infrastructure
{
    public class PdfExportService
    {
        private readonly OwnerTrackDbContext _db;

        private static readonly string BojaPlava = "#1a3a5c";
        private static readonly string BojaPlavaLight = "#e8f0f8";
        private static readonly string BojaSiva = "#f5f5f5";
        private static readonly string BojaLinija = "#c0c8d4";
        private static readonly string BojaZelena = "#2e7d32";
        private static readonly string BojaCrvena = "#c62828";
        private static readonly string BojaOrandzasta = "#e65100";
        private static readonly string BojaTekst = "#212121";
        private static readonly string BojaSivaTekst = "#555555";
        private static readonly string BojaFooter = "#888888";
        private static readonly string BojaHeaderSub = "#b0c4de";

        public PdfExportService(OwnerTrackDbContext db)
        {
            _db = db;
            QuestPDF.Settings.License = LicenseType.Community;

        }

        public string GenerirajPdf(int klijentId, string outputPath)
        {
            var k = _db.Klijenti
                .AsNoTracking()
                .Include(x => x.Djelatnost)
                .Include(x => x.Vlasnici)
                .Include(x => x.Direktori)
                .Include(x => x.Ugovor)
                .FirstOrDefault(x => x.Id == klijentId)
                ?? throw new InvalidOperationException($"Klijent ID={klijentId} nije pronađen.");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(18, Unit.Millimetre);
                    page.DefaultTextStyle(t => t.FontFamily("Arial").FontSize(9).FontColor(BojaTekst));

                    page.Header().Element(c => BuildHeader(c, k));
                    page.Content().Element(c => BuildContent(c, k));
                    page.Footer().Element(BuildFooter);
                });
            }).GeneratePdf(outputPath);

            return outputPath;
        }


        private void BuildHeader(IContainer c, Klijent k)
        {
            var status = k.Status ?? "—";
            var statusBoja = status == "AKTIVAN" ? BojaZelena
                           : status == "NEAKTIVAN" ? BojaCrvena
                                                   : BojaOrandzasta;

            c.Background(BojaPlava).Padding(14).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text(k.Naziv)
                       .FontSize(20).FontColor("#ffffff").Bold().AlignCenter();
                    col.Item().PaddingTop(4)
                       .Text($"ID: {k.IdBroj}   |   {Fmt(k.VrstaKlijenta)}")
                       .FontSize(10).FontColor(BojaHeaderSub).AlignCenter();
                });

                row.ConstantItem(70).AlignMiddle().AlignRight()
                   .Background(statusBoja).Padding(5)
                   .Text(status)
                   .FontSize(9).FontColor("#ffffff").Bold().AlignCenter();
            });
        }


        private void BuildContent(IContainer c, Klijent k)
        {
            c.PaddingTop(10).Column(col =>
            {
                col.Spacing(10);

                col.Item().Element(x => SekcijaHeader(x, "OSNOVNI PODACI"));
                col.Item().Element(x => BuildOsnoviPodaci(x, k));

                col.Item().Element(x => SekcijaHeader(x, "PROCJENA RIZIKA"));
                col.Item().Element(x => BuildRizik(x, k));

                col.Item().Element(x => SekcijaHeader(x, "UGOVOR"));
                col.Item().Element(x => BuildUgovor(x, k.Ugovor));

                var vlasnici = k.Vlasnici.ToList();
                col.Item().Element(x => SekcijaHeader(x, $"VLASNICI  ({vlasnici.Count})"));
                col.Item().Element(x => BuildVlasnici(x, vlasnici));

                var direktori = k.Direktori.ToList();
                col.Item().Element(x => SekcijaHeader(x, $"DIREKTORI  ({direktori.Count})"));
                col.Item().Element(x => BuildDirektori(x, direktori));
            });
        }


        private void SekcijaHeader(IContainer c, string tekst)
        {
            c.Background(BojaPlavaLight)
             .BorderBottom(1.5f).BorderColor(BojaPlava)
             .PaddingHorizontal(10).PaddingVertical(6)
             .Text(tekst).FontSize(10).FontColor(BojaPlava).Bold();
        }


        private void BuildOsnoviPodaci(IContainer c, Klijent k)
        {
            var djelatnost = string.IsNullOrWhiteSpace(k.SifraDjelatnosti)
                ? Fmt(k.Djelatnost?.Naziv)
                : $"{k.SifraDjelatnosti} – {Fmt(k.Djelatnost?.Naziv)}";

            var redovi = new List<(string, string, string, string)>
            {
               ("Adresa:",          Fmt(k.Adresa),            "Datum osnivanja:", FmtDatum(k.DatumOsnivanja)),
               ("Djelatnost:",      djelatnost,               "Datum uspostave:", FmtDatum(k.DatumUspostave)),
               ("Veličina firme:",  Fmt(k.Velicina),          "Ovjera / CR:",     Fmt(k.OvjeraCr)),
               ("Email:",           Fmt(k.Email),             "Telefon:",         Fmt(k.Telefon)),
            };

            c.Table(tbl =>
            {
                tbl.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                });

                for (int i = 0; i < redovi.Count; i++)
                {
                    var (l1, v1, l2, v2) = redovi[i];
                    var bg = i % 2 == 0 ? "#ffffff" : BojaSiva;
                    InfoRed(tbl, bg, l1, v1, l2, v2);
                }

                if (!string.IsNullOrWhiteSpace(k.Napomena))
                {
                    var bg = redovi.Count % 2 == 0 ? "#ffffff" : BojaSiva;
                    tbl.Cell().Background(bg).BorderBottom(0.3f).BorderColor(BojaLinija)
                       .PaddingHorizontal(8).PaddingVertical(5)
                       .Text("Napomena:").FontColor(BojaSivaTekst);
                    tbl.Cell().ColumnSpan(3).Background(bg).BorderBottom(0.3f).BorderColor(BojaLinija)
                       .PaddingHorizontal(8).PaddingVertical(5)
                       .Text(k.Napomena).Bold();
                }
            });
        }


        private void BuildRizik(IContainer c, Klijent k)
        {
            c.Table(tbl =>
            {
                tbl.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                });

                var redovi = new List<(string, string, string, string)>
                {
                    ("PEP:",        Fmt(k.PepRizik),       "UBO:",        Fmt(k.UboRizik)),
                    ("Gotovina rizik:",   Fmt(k.GotovinaRizik),  "Geografski rizik:", Fmt(k.GeografskiRizik)),
                    ("Ukupna procjena:",  Fmt(k.UkupnaProcjena), "Datum procjene:",   FmtDatum(k.DatumProcjene)),
                };

                for (int i = 0; i < redovi.Count; i++)
                {
                    var (l1, v1, l2, v2) = redovi[i];
                    var bg = i % 2 == 0 ? "#ffffff" : BojaSiva;


                    bool bojano = i < 2;
                    var boja1 = bojano ? DaNeBoja(v1) : BojaTekst;
                    var boja2 = bojano ? DaNeBoja(v2) : BojaTekst;

                    tbl.Cell().Background(bg).BorderBottom(0.3f).BorderColor(BojaLinija)
                       .PaddingHorizontal(8).PaddingVertical(5)
                       .Text(l1).FontColor(BojaSivaTekst);
                    tbl.Cell().Background(bg).BorderBottom(0.3f).BorderColor(BojaLinija)
                       .PaddingHorizontal(8).PaddingVertical(5)
                       .Text(v1).Bold().FontColor(boja1);
                    tbl.Cell().Background(bg).BorderBottom(0.3f).BorderColor(BojaLinija)
                       .PaddingHorizontal(8).PaddingVertical(5)
                       .Text(l2).FontColor(BojaSivaTekst);
                    tbl.Cell().Background(bg).BorderBottom(0.3f).BorderColor(BojaLinija)
                       .PaddingHorizontal(8).PaddingVertical(5)
                       .Text(v2).Bold().FontColor(boja2);
                }
            });
        }


        private void BuildUgovor(IContainer c, Ugovor? u)
        {
            if (u == null)
            {
                c.PaddingHorizontal(8).PaddingVertical(5)
                 .Text("Nema podataka o ugovoru.").FontColor(BojaFooter).Italic();
                return;
            }

            var status = Fmt(u.StatusUgovora);
            var statusBoja = status == "POTPISAN" ? BojaZelena
                           : status is "OTKAZAN" or "NEAKTIVAN" ? BojaCrvena
                                                                            : BojaOrandzasta;

            c.Table(tbl =>
            {
                tbl.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                });


                tbl.Cell().Background("#ffffff").BorderBottom(0.3f).BorderColor(BojaLinija)
                   .PaddingHorizontal(8).PaddingVertical(5)
                   .Text("Status ugovora:").FontColor(BojaSivaTekst);
                tbl.Cell().Background("#ffffff").BorderBottom(0.3f).BorderColor(BojaLinija)
                   .PaddingHorizontal(8).PaddingVertical(5)
                   .Text(status).Bold().FontColor(statusBoja);
                tbl.Cell().Background("#ffffff").BorderBottom(0.3f).BorderColor(BojaLinija)
                   .PaddingHorizontal(8).PaddingVertical(5)
                   .Text("Datum ugovora:").FontColor(BojaSivaTekst);
                tbl.Cell().Background("#ffffff").BorderBottom(0.3f).BorderColor(BojaLinija)
                   .PaddingHorizontal(8).PaddingVertical(5)
                   .Text(FmtDatum(u.DatumUgovora)).Bold();


                if (!string.IsNullOrWhiteSpace(u.VrstaUgovora) || !string.IsNullOrWhiteSpace(u.Napomena))
                    InfoRed(tbl, BojaSiva, "Vrsta ugovora:", Fmt(u.VrstaUgovora), "Napomena:", Fmt(u.Napomena));
            });
        }


        private void BuildVlasnici(IContainer c, List<Vlasnik> vlasnici)
        {
            if (!vlasnici.Any())
            {
                c.PaddingHorizontal(8).PaddingVertical(5)
                 .Text("Nema evidentiranih vlasnika.").FontColor(BojaFooter).Italic();
                return;
            }

            c.Table(tbl =>
            {
                tbl.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(8, Unit.Millimetre);
                    cd.ConstantColumn(44, Unit.Millimetre);
                    cd.ConstantColumn(18, Unit.Millimetre);
                    cd.ConstantColumn(28, Unit.Millimetre);
                    cd.ConstantColumn(27, Unit.Millimetre);
                    cd.RelativeColumn();
                    cd.ConstantColumn(18, Unit.Millimetre);
                });

                foreach (var h in new[] { "#", "Ime i prezime", "% vlasn.", "Valjanost dok.", "Datum utvrđ.", "Izvor podatka", "Status" })
                    TabHdr(tbl, h);

                for (int i = 0; i < vlasnici.Count; i++)
                {
                    var v = vlasnici[i];
                    var bg = i % 2 == 0 ? "#ffffff" : BojaSiva;
                    var sc = v.Status == "AKTIVAN" ? BojaZelena : BojaCrvena;
                    var pct = v.ProcenatVlasnistva > 0 ? $"{v.ProcenatVlasnistva:0.##} %" : "—";

                    TabCell(tbl, bg, (i + 1).ToString(), center: true);
                    TabCell(tbl, bg, Fmt(v.ImePrezime));
                    TabCell(tbl, bg, pct, center: true);
                    TabCell(tbl, bg, FmtDatum(v.DatumValjanostiDokumenta), center: true);
                    TabCell(tbl, bg, FmtDatum(v.DatumUtvrdjivanja), center: true);
                    TabCell(tbl, bg, Fmt(v.IzvorPodatka));
                    TabCell(tbl, bg, v.Status, bold: true, boja: sc, center: true);
                }
            });
        }


        private void BuildDirektori(IContainer c, List<Direktor> direktori)
        {
            if (!direktori.Any())
            {
                c.PaddingHorizontal(8).PaddingVertical(5)
                 .Text("Nema evidentiranih direktora.").FontColor(BojaFooter).Italic();
                return;
            }

            c.Table(tbl =>
            {
                tbl.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(8, Unit.Millimetre);
                    cd.ConstantColumn(44, Unit.Millimetre);
                    cd.ConstantColumn(28, Unit.Millimetre);
                    cd.ConstantColumn(30, Unit.Millimetre);
                    cd.ConstantColumn(27, Unit.Millimetre);
                    cd.ConstantColumn(18, Unit.Millimetre);
                });

                foreach (var h in new[] { "#", "Ime i prezime", "JMBG", "Tip valjanosti", "Datum valjanosti", "Status" })
                    TabHdr(tbl, h);

                for (int i = 0; i < direktori.Count; i++)
                {
                    var d = direktori[i];
                    var bg = i % 2 == 0 ? "#ffffff" : BojaSiva;
                    var sc = d.Status == "AKTIVAN" ? BojaZelena : BojaCrvena;
                    var datVal = d.TipValjanosti == "TRAJNO" ? "TRAJNO" : FmtDatum(d.DatumValjanosti);

                    TabCell(tbl, bg, (i + 1).ToString(), center: true);
                    TabCell(tbl, bg, Fmt(d.ImePrezime));
                    TabCell(tbl, bg, Fmt(d.Jmbg), center: true);
                    TabCell(tbl, bg, Fmt(d.TipValjanosti), center: true);
                    TabCell(tbl, bg, datVal, center: true);
                    TabCell(tbl, bg, d.Status, bold: true, boja: sc, center: true);
                }
            });
        }


        private void BuildFooter(IContainer c)
        {
            c.BorderTop(0.5f).BorderColor(BojaLinija).PaddingTop(5).Row(row =>
            {
                row.RelativeItem()
                   .Text($"Izvještaj generisan: {DateTime.Now:dd.MM.yyyy. u HH:mm}")
                   .FontSize(7).FontColor(BojaFooter);

                row.ConstantItem(60).AlignRight().Text(txt =>
                {
                    txt.Span("Stranica ").FontSize(7).FontColor(BojaFooter);
                    txt.CurrentPageNumber().FontSize(7).FontColor(BojaFooter);
                    txt.Span(" / ").FontSize(7).FontColor(BojaFooter);
                    txt.TotalPages().FontSize(7).FontColor(BojaFooter);
                });
            });
        }


        private void InfoRed(TableDescriptor tbl, string bg, string l1, string v1, string l2, string v2)
        {
            tbl.Cell().Background(bg).BorderBottom(0.3f).BorderColor(BojaLinija)
               .PaddingHorizontal(8).PaddingVertical(5).Text(l1).FontColor(BojaSivaTekst);
            tbl.Cell().Background(bg).BorderBottom(0.3f).BorderColor(BojaLinija)
               .PaddingHorizontal(8).PaddingVertical(5).Text(v1).Bold();
            tbl.Cell().Background(bg).BorderBottom(0.3f).BorderColor(BojaLinija)
               .PaddingHorizontal(8).PaddingVertical(5).Text(l2).FontColor(BojaSivaTekst);
            tbl.Cell().Background(bg).BorderBottom(0.3f).BorderColor(BojaLinija)
               .PaddingHorizontal(8).PaddingVertical(5).Text(v2).Bold();
        }

        private void TabHdr(TableDescriptor tbl, string tekst)
        {
            tbl.Cell().Background(BojaPlava).PaddingHorizontal(5).PaddingVertical(5)
               .Text(tekst).FontSize(8).FontColor("#ffffff").Bold().AlignCenter();
        }

        private void TabCell(TableDescriptor tbl, string bg, string tekst,
                              bool bold = false, string? boja = null, bool center = false, bool noWrap = false)
        {
            var cell = tbl.Cell().Background(bg)
                          .BorderBottom(0.3f).BorderColor(BojaLinija)
                          .PaddingHorizontal(5).PaddingVertical(4);
            var t = cell.Text(tekst).FontSize(8);
            if (bold) t.Bold();
            if (boja != null) t.FontColor(boja);
            if (center) t.AlignCenter();
            if (noWrap) t.ClampLines(1);
        }


        private static string Fmt(string? val) =>
            string.IsNullOrWhiteSpace(val) ? "—" : val.Trim();

        private static string FmtDatum(DateTime? dt) =>
            dt.HasValue ? dt.Value.ToString("dd.MM.yyyy.") : "—";

        private static string DaNeBoja(string? val) =>
            (val ?? "").Trim().ToUpper() switch
            {
                "DA" => "#c62828",
                "NE" => "#2e7d32",
                _ => "#212121"
            };

       

        public string GenerirajTabeluKlijenata(List<int> klijentIds, string outputPath)
        {
            var klijenti = _db.Klijenti
                .AsNoTracking()
                .Include(x => x.Djelatnost)
                .Include(x => x.Vlasnici)
                .Include(x => x.Direktori)
                .Include(x => x.Ugovor)
                .Where(x => klijentIds.Contains(x.Id))
                .ToList();

            
            klijenti = klijentIds
                .Select(id => klijenti.FirstOrDefault(k => k.Id == id))
                .Where(k => k != null)
                .ToList()!;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(12, Unit.Millimetre);
                    page.DefaultTextStyle(t => t.FontFamily("Arial").FontSize(7.5f).FontColor(BojaTekst));

                    page.Header().Element(c => BuildTabelaHeader(c, klijenti.Count));
                    page.Content().Element(c => BuildTabelaSadrzaj(c, klijenti));
                    page.Footer().Element(BuildFooter);
                });
            }).GeneratePdf(outputPath);

            return outputPath;
        }

        private void BuildTabelaHeader(IContainer c, int ukupno)
        {
            c.Background(BojaPlava).Padding(10).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Pregled klijenata")
                       .FontSize(16).FontColor("#ffffff").Bold();
                    col.Item().PaddingTop(3)
                       .Text($"Ukupno klijenata: {ukupno}   |   Datum izvje\u0161taja: {DateTime.Now:dd.MM.yyyy.}")
                       .FontSize(9).FontColor(BojaHeaderSub);
                });
            });
        }

        private void BuildTabelaSadrzaj(IContainer c, List<Klijent> klijenti)
        {
            c.PaddingTop(8).Table(tbl =>
            {
                tbl.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(10, Unit.Millimetre); 
                    cd.RelativeColumn(2);                    
                    cd.ConstantColumn(27, Unit.Millimetre); 
                    cd.RelativeColumn(1.5f);                 
                    cd.ConstantColumn(32, Unit.Millimetre); 
                    cd.ConstantColumn(14, Unit.Millimetre); 
                    cd.ConstantColumn(14, Unit.Millimetre); 
                    cd.ConstantColumn(22, Unit.Millimetre); 
                    cd.ConstantColumn(20, Unit.Millimetre); 
                    cd.ConstantColumn(22, Unit.Millimetre); 
                    cd.ConstantColumn(22, Unit.Millimetre); 
                    cd.ConstantColumn(16, Unit.Millimetre); 
                });

                foreach (var h in new[] { "#", "Naziv klijenta", "ID broj", "Djelatnost", "Veličina", "PEP", "UBO", "Procjena", "Ugovor", "Dat. uspostave", "Dat. osnivanja", "Status" })
                    TabHdr(tbl, h);

                for (int i = 0; i < klijenti.Count; i++)
                {
                    var k = klijenti[i];
                    var bg = i % 2 == 0 ? "#ffffff" : BojaSiva;
                    var sc = (k.Status ?? "") == "AKTIVAN" ? BojaZelena
                           : (k.Status ?? "") == "ARHIVIRAN" ? BojaOrandzasta
                           : BojaCrvena;

                    var djelatnost = k.Djelatnost?.Naziv ?? k.SifraDjelatnosti ?? "—";
                    var statusUgovora = k.Ugovor?.StatusUgovora ?? "—";

                    TabCell(tbl, bg, (i + 1).ToString(), center: true, noWrap: true);
                    TabCell(tbl, bg, Fmt(k.Naziv));
                    TabCell(tbl, bg, Fmt(k.IdBroj), center: true, noWrap: true);
                    TabCell(tbl, bg, Fmt(djelatnost));
                    TabCell(tbl, bg, Fmt(k.Velicina), center: true, noWrap: true);
                    TabCell(tbl, bg, Fmt(k.PepRizik), bold: true, boja: DaNeBoja(k.PepRizik), center: true, noWrap: true);
                    TabCell(tbl, bg, Fmt(k.UboRizik), bold: true, boja: DaNeBoja(k.UboRizik), center: true, noWrap: true);
                    TabCell(tbl, bg, Fmt(k.UkupnaProcjena), center: true);
                    TabCell(tbl, bg, statusUgovora, center: true, noWrap: true);
                    TabCell(tbl, bg, FmtDatum(k.DatumUspostave), center: true, noWrap: true);
                    TabCell(tbl, bg, FmtDatum(k.DatumOsnivanja), center: true, noWrap: true);
                    TabCell(tbl, bg, Fmt(k.Status), bold: true, boja: sc, center: true, noWrap: true);
                }
            });
        }
    }
}