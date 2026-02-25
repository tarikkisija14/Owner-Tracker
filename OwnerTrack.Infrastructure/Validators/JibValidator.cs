using System.Linq;

namespace OwnerTrack.Infrastructure.Validators
{
    public static class JibValidator
    {
        private static readonly int[] Tezine = { 8, 9, 2, 3, 4, 5, 6, 7, 8, 9, 2, 3 };

        public static bool JeValidan(string jib)
        {
            if (string.IsNullOrWhiteSpace(jib)) return false;
            jib = jib.Trim();
            if (jib.Length != 13) return false;
            if (!jib.All(char.IsDigit)) return false;

            int[] cifre = jib.Select(c => c - '0').ToArray();
            int suma = 0;
            for (int i = 0; i < 12; i++)
                suma += cifre[i] * Tezine[i];

            int ostatak = suma % 11;
            int kontrolna = ostatak == 0 || ostatak == 1 ? 0 : 11 - ostatak;
            return cifre[12] == kontrolna;
        }

        public static string? GreškaValidacije(string jib)
        {
            if (string.IsNullOrWhiteSpace(jib))
                return "ID broj je obavezan.";

            jib = jib.Trim();

            if (jib.Length != 13)
                return $"ID broj mora imati tačno 13 cifara (uneseno: {jib.Length}).";

            if (!jib.All(char.IsDigit))
                return "ID broj smije sadržavati samo cifre.";

            if (!JeValidan(jib))
                return "ID broj nije ispravan — kontrolna cifra se ne podudara. Provjeri unos.";

            return null;
        }
    }
}