namespace OwnerTrack.Data.Enums
{
    public enum VrstaKlijenta
    {
        PravnoLice,
        FizickoLice,
        Udruzenje,
        Obrtnik,
        JavnaUstanova
    }

    /// <summary>
    /// Čitljiv naziv za prikaz (enum imena su bez razmaka: PravnoLice → "Pravno lice").
    /// Jedini izvor istine za ovaj tekst — koristi se i u gridu i u formama.
    /// </summary>
    public static class VrstaKlijentaDisplay
    {
        private static readonly Dictionary<VrstaKlijenta, string> Names = new()
        {
            [VrstaKlijenta.PravnoLice] = "Pravno lice",
            [VrstaKlijenta.FizickoLice] = "Fizičko lice",
            [VrstaKlijenta.Udruzenje] = "Udruženje",
            [VrstaKlijenta.Obrtnik] = "Obrtnik",
            [VrstaKlijenta.JavnaUstanova] = "Javna ustanova",
        };

        public static string ToDisplay(this VrstaKlijenta value) => Names[value];

        public static string? ToDisplay(this VrstaKlijenta? value) =>
            value.HasValue ? Names[value.Value] : null;
    }
}