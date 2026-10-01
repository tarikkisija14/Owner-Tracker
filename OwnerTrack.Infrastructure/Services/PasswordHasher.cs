using System.Security.Cryptography;
using System.Text;

namespace OwnerTrack.Infrastructure.Services
{
    // PBKDF2-HMAC-SHA256 iz .NET BCL-a (bez vanjskih paketa). Svaki korisnik
    // dobija vlastiti nasumični salt; hash i salt se u bazi čuvaju kao Base64.
    // Broj iteracija je konstanta — mijenjanje bi poništilo sve postojeće hasheve.
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 210_000;

        public static (string Hash, string Salt) Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Derive(password, salt);
            return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }

        public static bool Verify(string password, string hashBase64, string saltBase64)
        {
            byte[] expected;
            byte[] salt;
            try
            {
                expected = Convert.FromBase64String(hashBase64);
                salt = Convert.FromBase64String(saltBase64);
            }
            catch (FormatException)
            {
                return false;
            }

            return CryptographicOperations.FixedTimeEquals(Derive(password, salt), expected);
        }

        private static byte[] Derive(string password, byte[] salt) =>
            Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password), salt, Iterations, HashAlgorithmName.SHA256, HashSize);
    }
}
