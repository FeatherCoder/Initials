using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Initials.Common.Extensions
{
    public static class Bytes
    {
        public static byte[] Mix(this byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public static bool Verify(this byte[] input, byte[] hash)
        {
            var exp = hash.HEX();
            return Verify(input, exp);
        }

        public static bool Verify(this byte[] input, string hex)
        {
            var res = input.Hash().HEX();
            return 0 == StringComparer.OrdinalIgnoreCase.Compare(res, hex);
        }

        public static byte[] Hash(this byte[] input, string algName = null)
        {
            return
                String.IsNullOrEmpty(algName)
                ? MD5.Create().ComputeHash(input)
                : MD5.Create(algName).ComputeHash(input)
                ;
        }

        /// The example displays the following output to the console if the 
        /// current culture is en-us: 
        ///       'C3' format specifier: $240.000 
        ///       'D4' format specifier: 0240 
        ///       'e1' format specifier: 2.4e+002 
        ///       'E2' format specifier: 2.40E+002 
        ///       'F1' format specifier: 240.0 
        ///       'G'  format specifier: 240 
        ///       'N1' format specifier: 240.0 
        ///       'P0' format specifier: 24,000 % 
        ///       'X4' format specifier: 00F0 
        ///       'X2' format specifier: F0 
        ///       '0000.0000' format specifier: 0240.0000 
        public static string Format(this byte[] input, string format)
        {
            var sb = new StringBuilder();
            foreach (var t in input) sb.Append(t.ToString(format));
            return sb.ToString();
        }

        public static string HEX(this byte[] input)
        {
            return input.Format("X2");
        }
    }
}
