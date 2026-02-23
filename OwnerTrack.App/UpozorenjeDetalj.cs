namespace OwnerTrack.App
{
    public partial class FrmUpozorenja
    {
        
        private class UpozorenjeDetalj
        {
            public int KlijentId { get; set; }
            public string NazivFirme { get; set; }
            public string ImePrezime { get; set; }
            public string Tip { get; set; }
            public DateTime DatumIsteka { get; set; }
        }
    }
}