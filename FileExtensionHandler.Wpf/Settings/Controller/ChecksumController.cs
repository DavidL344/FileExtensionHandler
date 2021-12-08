using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Settings.Controller
{
    internal class ChecksumController
    {
        // Source: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=netframework-4.7.2
        internal static string GetHash(string input, HashAlgorithm hashAlgorithm = null)
        {
            if (hashAlgorithm == null) hashAlgorithm = SHA256.Create();
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++) sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        internal static bool VerifyHash(string input, string hash, HashAlgorithm hashAlgorithm = null)
        {
            if (hashAlgorithm == null) hashAlgorithm = SHA256.Create();
            string hashOfInput = GetHash(input, hashAlgorithm);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}
