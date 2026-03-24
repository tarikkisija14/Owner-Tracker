namespace OwnerTrack.App.Helpers
{
   
    internal static class ImportProgressFormatter
    {
        public static string FormatProgress(Infrastructure.Models.ImportProgress p) =>
            $"Obrađeno: {p.ProcessedRows}/{p.TotalRows}\n" +
            $"Dodato: {p.SuccessCount}  |  Greške: {p.ErrorCount}\n" +
            $"{p.CurrentRow}";

        public static string FormatResult(Infrastructure.Models.ImportResult result, bool wasCancelled) =>
            wasCancelled
                ? $"Import otkazan.\nDodano: {result.SuccessCount}  |  Preskočeno: {result.SkipCount}\nGreške: {result.ErrorCount}"
                : $"Import završen!\nDodano: {result.SuccessCount}  |  Preskočeno (duplikati): {result.SkipCount}\nGreške: {result.ErrorCount}  |  Vlasnici: {result.VlasnikCount}";
    }
}