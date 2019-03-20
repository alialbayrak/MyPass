using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace MyPass.Bll.Helper
{
    public class SecurityHelper
    {
        public static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        /*public static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }*/

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        public static string Encode(string encodeMe)
        {
            try
            {
                byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
                return Convert.ToBase64String(encoded);
            }
            catch (Exception)
            {
                return encodeMe;
            }
            
        }

        public static string Decode(string decodeMe)
        {
            try
            {
                byte[] encoded = Convert.FromBase64String(decodeMe);
                return System.Text.Encoding.UTF8.GetString(encoded);
            }
            catch (Exception)
            {
                return decodeMe;
            }
            
        }
    }
}
