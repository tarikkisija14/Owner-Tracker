namespace OwnerTrack.App.Constants
{
    
    internal static class UiConstants
    {
      
        public const int SearchDebounceMs = 300;

        

        public const string BadgeLabelDefault = "🔔 Upozorenja";
        public const string BadgeLabelFormat = "🔔 Upozorenja ({0})";

       

        public const string PdfExportBusyLabel = "Generišem...";
        public const string PdfFilter = "PDF dokument (*.pdf)|*.pdf";
        public const string PdfExt = "pdf";
        public const string ExportDateFormat = "yyyyMMdd";

       

        public const string FilterAllDisplay = "-- Sve --";
        public const string FilterAllValue = "";
        public const string FilterAllDjelatnostDisplay = "-- Sve djelatnosti --";

       
        public const string ExcelZirvanaKeyword = "ZBIRNA";
        public const string ExcelFilter = "Excel Files (*.xlsx)|*.xlsx";

        

        public const string ResetImportConfirmMessage =
            "Ovo će obrisati SVE podatke i pokrenuti novi import.\n\nJesi li siguran?";

        public const string ResetImportTitle = "RESET BAZE";
    }
}