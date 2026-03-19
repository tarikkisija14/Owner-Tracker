using System.Linq;

namespace OwnerTrack.Infrastructure.Validators
{
    public static class JibValidator
    {
        private const int RequiredLength = 13;

        public static bool JeValidan(string jib)
        {
            if (string.IsNullOrWhiteSpace(jib)) return false;
            jib = jib.Trim();
            return jib.Length == RequiredLength && jib.All(char.IsDigit);
        }

        
        public static string? GreskaValidacije(string jib)
        {
            if (string.IsNullOrWhiteSpace(jib))
                return "ID broj je obavezan.";

            jib = jib.Trim();

            if (!jib.All(char.IsDigit))
                return "ID broj smije sadržavati samo cifre.";

            if (jib.Length != RequiredLength)
                return $"ID broj mora imati tačno {RequiredLength} cifara (uneseno: {jib.Length}).";

            return null;
        }
    }
}