namespace OwnerTrack.Data.Enums
{
    public enum VelicinaFirme
    {
        MIKRO,
        MALO,
        SREDNJE,
        VELIKO,
        OBRTNIK,
        UDRUŽENJE
    }

    /// <summary>
    /// Čitljiv naziv za prikaz (enum imena su velikim slovima: MIKRO → "Mikro").
    /// Isti pristup kao VrstaKlijentaDisplay — jedini izvor istine za ovaj tekst.
    /// </summary>
    public static class VelicinaFirmeDisplay
    {
        private static readonly Dictionary<VelicinaFirme, string> Names = new()
        {
            [VelicinaFirme.MIKRO] = "Mikro",
            [VelicinaFirme.MALO] = "Malo",
            [VelicinaFirme.SREDNJE] = "Srednje",
            [VelicinaFirme.VELIKO] = "Veliko",
            [VelicinaFirme.OBRTNIK] = "Obrtnik",
            [VelicinaFirme.UDRUŽENJE] = "Udruženje",
        };

        public static string ToDisplay(this VelicinaFirme value) => Names[value];

        public static string? ToDisplay(this VelicinaFirme? value) =>
            value.HasValue ? Names[value.Value] : null;
    }
}