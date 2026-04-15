using System.Security.Cryptography;
using System.Text;

namespace MojaBiblioteka.Utility.Security
{
    public static class PasswordHasher
    {
        public static string ComputeHash(string plainText)
        {
            if (plainText == null)
            {
                return string.Empty;
            }

            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(plainText);
                var hash = sha256.ComputeHash(bytes);
                var sb = new StringBuilder(hash.Length * 2);
                foreach (var b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
