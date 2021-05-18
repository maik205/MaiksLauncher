using System;
using System.Collections.Generic;
using System.Text;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Linq;

namespace MaiksLauncher.Core
{
    class Hasher
    {
        public static string SHA256(string text)
        {
            var result = default(string);

            using (var algo = new SHA256Managed())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }
        public static string MD5(string text)
        {
            var result = default(string);

            using (var algo = new MD5CryptoServiceProvider())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }
        public static string SHA512(string text)
        {
            var result = default(string);

            using (var algo = new SHA512Managed())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }
        public static string SHA384(string text)
        {
            var result = default(string);

            using (var algo = new SHA384Managed())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }
        public static string SHA1(string text)
        {
            var result = default(string);

            using (var algo = new SHA1Managed())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }
        private static string GenerateHashString(HashAlgorithm algo, string text)
        {
            algo.ComputeHash(Encoding.UTF8.GetBytes(text));
            var result = algo.Hash;
            return string.Join(
                string.Empty,
                result.Select(x => x.ToString("x2")));
        }
    }
}
