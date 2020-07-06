using System;
using System.Security.Cryptography;
using System.Text;

namespace Structure.Security.Cryptography
{
    public static class EncryptionHelper
    {
        public static string Md5(string input)
        {
            var md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.ASCII.GetBytes(input));

            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static string Encrypt(string value, string key8Bytes)
        {
            using (var provider = new DESCryptoServiceProvider())
            {
                byte[] byteHash, byteBuff;
                string strTempKey = key8Bytes;

                byteHash = Encoding.ASCII.GetBytes(strTempKey);
                provider.Key = byteHash;
                provider.Mode = CipherMode.ECB;

                byteBuff = Encoding.ASCII.GetBytes(value);
                return Convert.ToBase64String(provider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            }
        }

        public static string Decrypt(string valor, string key8Bytes)
        {
            using (var provider = new DESCryptoServiceProvider())
            {

                byte[] byteHash, byteBuff;
                string strTempKey = key8Bytes;

                byteHash = Encoding.ASCII.GetBytes(strTempKey);
                provider.Key = byteHash;
                provider.Mode = CipherMode.ECB;

                byteBuff = Convert.FromBase64String(valor);
                string strDecrypted = Encoding.ASCII.GetString(provider.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));

                return strDecrypted;
            }

        }

        private static string GetHexadecimal(byte[] bytes)
        {
            var sb = new StringBuilder();

            foreach (var b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }

            return sb.ToString();
        }

        public static string GetHexadecimal(string str)
        {
            var hex = "";

            foreach (var c in str)
            {
                int tmp = c;
                hex += string.Format("{0:x2}", Convert.ToUInt32(tmp.ToString()));
            }

            return hex;
        }

        public static string GetHexadecimalSha1(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            var sha1 = SHA1.Create();
            var hashBytes = sha1.ComputeHash(bytes);
            return GetHexadecimal(hashBytes);
        }

        public static string GetHash(string str)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }
    }
}
