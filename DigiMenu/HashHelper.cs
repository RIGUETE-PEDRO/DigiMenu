using System;
using System.Security.Cryptography;
using System.Text;

namespace DigiMenu
{
    public class HashHelper
    {
        public string GerarHashSHA256(string texto)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(texto);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}