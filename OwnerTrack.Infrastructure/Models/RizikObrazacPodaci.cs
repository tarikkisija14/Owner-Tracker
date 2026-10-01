using OwnerTrack.Data.Enums;

namespace OwnerTrack.Infrastructure.Models
{
    public class RizikObrazacPodaci
    {
        public DateTime? DatumProcjene { get; set; }
        public string? Odobrio { get; set; }

        public List<string?> OpciIndikatoriOdgovori { get; set; } = NewAnswers(RizikObrazacKriteriji.OpciIndikatori.Length);
        public string? ProcjenaOpcihIndikatora { get; set; }

        public List<string?> IdentifikacijaOdgovori { get; set; } = NewAnswers(RizikObrazacKriteriji.IndikatoriIdentifikacije.Length);
        public string? ProcjenaIdentifikacije { get; set; }

        public List<string?> TransakcijeOdgovori { get; set; } = NewAnswers(RizikObrazacKriteriji.IndikatoriTransakcija.Length);
        public string? ProcjenaTransakcija { get; set; }

        public List<string?> GeografskiRizikOdgovori { get; set; } = NewAnswers(RizikObrazacKriteriji.GeografskiRizikOstali.Length);
        public string? OstalaSumnjivaZapazanja { get; set; }
        public string? ProcjenaGeografskog { get; set; }

        public string? UkupnaProcjena { get; set; }

        private static List<string?> NewAnswers(int count) => new(new string?[count]);
    }
}
