using OwnerTrack.Data.Enums;

namespace OwnerTrack.Infrastructure.Models
{
    public class RizikObrazacPodaci
    {
        public string? Drzava { get; set; } = "Bosna i Hercegovina";
        public DateTime? DatumProcjene { get; set; }
        public string? Odobrio { get; set; }

        public List<string?> RizikStrankeOdgovori { get; set; } = NewAnswers(RizikObrazacKriteriji.RizikStranke.Length);
        public string? ProcjenaStranke { get; set; }

        public List<string?> RizikPoslovnogOdnosaOdgovori { get; set; } = NewAnswers(RizikObrazacKriteriji.RizikPoslovnogOdnosa.Length);
        public string? ProcjenaPoslovnogOdnosa { get; set; }

        public List<string?> GeografskiRizikOdgovori { get; set; } = NewAnswers(RizikObrazacKriteriji.GeografskiRizik.Length);
        public string? ProcjenaGeografskog { get; set; }

        public string? UkupnaProcjena { get; set; }

        private static List<string?> NewAnswers(int count) => new(new string?[count]);
    }
}
