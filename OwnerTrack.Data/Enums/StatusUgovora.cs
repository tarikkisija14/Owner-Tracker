namespace OwnerTrack.Data.Enums
{
    
    public static class StatusUgovora
    {
        public const string Potpisan = "POTPISAN";
        public const string Aneks = "ANEKS";
        public const string Otkazan = "OTKAZAN";
        public const string NemaUgovor = "NEMA UGOVOR";
        public const string Neaktivan = "NEAKTIVAN";

        
        public static readonly string[] Svi =
        {
            Potpisan, Aneks, Otkazan, NemaUgovor, Neaktivan
        };
    }
}