using System.Linq;

namespace OwnerTrack.Infrastructure.Validators
{
    public static class JibValidator
    {
        public static bool JeValidan(string jib)
        {
            if (string.IsNullOrWhiteSpace(jib)) return false;
            jib = jib.Trim();
            return jib.Length == 13 && jib.All(char.IsDigit);
        }

        public static string? GreškaValidacije(string jib)
        {
            if (string.IsNullOrWhiteSpace(jib))
                return "ID broj je obavezan.";

            jib = jib.Trim();

            if (!jib.All(char.IsDigit))
                return "ID broj smije sadržavati samo cifre.";

            if (jib.Length != 13)
                return $"ID broj mora imati tačno 13 cifara (uneseno: {jib.Length}).";

            return null;
        }
    }
}