using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Common
{
    public class Security
    {
        //private static string EncryptKey = "zB064QlTzhrrNqBlGh5ibIzdYGsqHleZ";
        //private static string EncryptIV = "KrhpRvYnMVA=";

        /// <summary>
        /// Simple Hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"> </param>
        /// <returns></returns>
        public static string Hash(string password, string salt = "")
        {
            return BitConverter.ToString(SHA1.Create().ComputeHash(Encoding.Default.GetBytes(password + salt))).Replace("-", "");
        }

        public static string EncryptString(String value)
        {
            value += CommonHelper.RandomString(5);
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(value);
            string encryptedString = Convert.ToBase64String(bytes).Replace("=", "").Replace('+', '-').Replace('/', '_');
            return encryptedString;
        }

        public static string DecryptString(String value)
        {
            byte[] bytes;
            string decryptedString;
            try
            {
                value = value.Replace('-', '+').Replace('_', '/');
                string padding = new String('=', 3 - (value.Length + 3) % 4);
                value += padding;
                bytes = Convert.FromBase64String(value);
                decryptedString = ASCIIEncoding.ASCII.GetString(bytes);
                decryptedString = decryptedString.Remove(decryptedString.Length - 5);
            }
            catch (FormatException)
            {
                decryptedString = "";
            }
            return decryptedString;
        }
    }
}
