using Newtonsoft.Json.Linq;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace Application.WebAPI.AppCode.Extensions
{
    public static partial class Extension
    {
        const string key = "zinrzinr";

        public static string ToMD5(this string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return string.Empty;

            byte[] buffer = Encoding.UTF8.GetBytes(content);

            byte[] hash = MD5.HashData(buffer);

            // 1) First way.
            //StringBuilder sb = new();

            //foreach (byte item in hash)
            //{
            //    sb.Append(item.ToString("x2"));
            //}

            //return sb.ToString();

            // 2) Second way.
            return string.Join("", buffer.Select(b => b.ToString("x2")));
        }

        public static string Encrypt(this string content, string key = key)
        {
            try
            {
                // TripleDES-de "key" - min. 16 max. 32 byte yer tutarken, "iv" ise 8 byte yer tutmalidir.

                TripleDES provider = TripleDES.Create();
                byte[] keyBuffer = MD5.HashData(Encoding.UTF8.GetBytes($"@`{key}/<"));
                byte[] ivBuffer = MD5.HashData(Encoding.UTF8.GetBytes(key));
                Array.Resize(ref ivBuffer, 8);

                provider.Padding = PaddingMode.PKCS7;
                provider.Mode = CipherMode.ECB;

                ICryptoTransform transformation = provider.CreateEncryptor(keyBuffer, ivBuffer);

                using MemoryStream ms = new();
                using CryptoStream cs = new(ms, transformation, CryptoStreamMode.Write);

                byte[] buffer = Encoding.UTF8.GetBytes(content);
                cs.Write(buffer, 0, buffer.Length);
                cs.FlushFinalBlock();

                ms.Position = 0;
                byte[] result = new byte[ms.Length];
                ms.Read(result, 0, result.Length);

                return Convert.ToBase64String(result);
            }
            finally
            {
                GC.Collect();
            }
        }

        public static string Decrypt(this string content, string key = key)
        {
            try
            {
                using TripleDES provider = TripleDES.Create();
                byte[] keyBuffer = MD5.HashData(Encoding.UTF8.GetBytes($"@`{key}/<"));
                byte[] ivBuffer = MD5.HashData(Encoding.UTF8.GetBytes(key));
                Array.Resize(ref ivBuffer, 8);

                provider.Padding = PaddingMode.PKCS7;
                provider.Mode = CipherMode.ECB;

                ICryptoTransform transformation = provider.CreateDecryptor(keyBuffer, ivBuffer);

                using MemoryStream ms = new();
                using CryptoStream cs = new(ms, transformation, CryptoStreamMode.Write);

                byte[] buffer = Convert.FromBase64String(content);
                cs.Write(buffer, 0, buffer.Length);
                cs.FlushFinalBlock();

                ms.Position = 0;
                byte[] result = new byte[ms.Length];
                ms.Read(result, 0, result.Length);

                return Encoding.UTF8.GetString(result);
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
