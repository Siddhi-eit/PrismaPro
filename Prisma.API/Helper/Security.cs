using System.Security.Cryptography;
using System.Text;

namespace MDFusionLabHaute.API.Helper
{
    public class Security
    {
        public static string Hash(string password, string salt = "")
        {
            using (var algorithm = SHA512.Create()) //or MD5 SHA256 etc.
            {
                var hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));

                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
