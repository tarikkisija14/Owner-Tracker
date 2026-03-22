using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace OwnerTrack.Infrastructure.Services
{

    internal static class PdfRenderHelpers
    {

        public static string Fmt(string? value) => string.IsNullOrWhiteSpace(value) ? "—" : value.Trim();
        public static string Fmt(StatusEntiteta s) => s.ToString();
        public static string Fmt(VrstaKlijenta? v) => v.HasValue ? v.Value.ToString() : "—";

        public static string FmtDate(DateTime? date) =>
            date.HasValue ? date.Value.ToString("dd.MM.yyyy.") : "—";


        public static string AlternatingBackground(int rowIndex) =>
            rowIndex % 2 == 0 ? PdfColours.White : PdfColours.Grey;

        public static void RenderTableHeader(TableDescriptor table, string text)
        {
            table.Cell()
                 .Background(PdfColours.Navy)
                 .PaddingHorizontal(5).PaddingVertical(5)
                 .Text(txt =>
                 {
                     txt.AlignCenter();
                     txt.Span(text).FontSize(8).FontColor(PdfColours.White).Bold();
                 });
        }

        public static void RenderTableCell(
            TableDescriptor table,
            string background,
            string text,
            bool bold = false,
            string? colour = null,
            bool center = false,
            bool noWrap = false)
        {
            var container = table.Cell()
                                 .Background(background)
                                 .PaddingHorizontal(5).PaddingVertical(4);

            IContainer cell = noWrap ? container.ShowOnce() : container;

            cell.Text(txt =>
            {
                var span = txt.Span(text).FontSize(8);
                if (bold) span.Bold();
                if (colour != null) span.FontColor(colour);
                if (center) txt.AlignCenter();
            });
        }

        public static void RenderInfoRow(
            TableDescriptor table,
            string background,
            string label1, string value1,
            string label2, string value2)
        {
            table.Cell().Background(background).PaddingHorizontal(8).PaddingVertical(5)
                 .Text(txt => txt.Span(label1).FontColor(PdfColours.TextMuted));
            table.Cell().Background(background).PaddingHorizontal(8).PaddingVertical(5)
                 .Text(txt => txt.Span(value1).Bold());
            table.Cell().Background(background).PaddingHorizontal(8).PaddingVertical(5)
                 .Text(txt => txt.Span(label2).FontColor(PdfColours.TextMuted));
            table.Cell().Background(background).PaddingHorizontal(8).PaddingVertical(5)
                 .Text(txt => txt.Span(value2).Bold());
        }

        public static void RenderSectionHeader(IContainer container, string text)
        {
            container.Background(PdfColours.NavyLight)
                     .PaddingHorizontal(10).PaddingVertical(7)
                     .Text(txt => txt.Span(text).FontSize(10).FontColor(PdfColours.Navy).Bold());
        }

        public static void RenderEmptyNote(IContainer container, string message)
        {
            container.PaddingHorizontal(8).PaddingVertical(5)
                     .Text(txt => txt.Span(message).FontColor(PdfColours.Footer).Italic());
        }


        public static string EntityStatusColour(StatusEntiteta status) =>
            status == StatusEntiteta.AKTIVAN ? PdfColours.Green :
            status == StatusEntiteta.NEAKTIVAN ? PdfColours.Red : PdfColours.Orange;

        public static string DaNeColour(string? value) =>
            (value ?? string.Empty).Trim().ToUpperInvariant() switch
            {
                var s when s == DaNeKonstante.Da => PdfColours.Red,
                var s when s == DaNeKonstante.Ne => PdfColours.Green,
                _ => PdfColours.Text,
            };

        public static string ContractStatusColour(string? statusText) =>
            statusText == StatusUgovora.Potpisan ? PdfColours.Green :
            statusText is StatusUgovora.Otkazan or StatusUgovora.Neaktivan ? PdfColours.Red :
            PdfColours.Orange;
    }
}