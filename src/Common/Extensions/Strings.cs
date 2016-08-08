using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Initials.Common.Extensions
{
    public static class Strings
    {
        private static readonly ConcurrentDictionary<string, object> _locks = new ConcurrentDictionary<string, object>();
        public static object GetLockObject(this string s, object context = null)
        { return _locks.GetOrAdd(context == null ? s : context + s, k => new object()); }

        /// <summary>
        /// {0},{1}....{n}
        /// </summary>
        public static string Format(string format, object[] substitutes)
        {
            if (substitutes == null) return format;
            for (int i = 0; i < substitutes.Length; i++) format = format.Replace("{" + i + "}", "" + substitutes[i]);
            return format;
        }

        public static string[] FormatAll(this string[] formats, params object[] args)
        {
            return (from format in formats select string.Format(format, args)).ToArray();
        }

        /// <param name="format">"Hello my name is {My.Name}."</param>
        /// <param name="substitutes">
        /// { 
        ///     {"My.Name"      , "Feather Coder"}, 
        ///     {"My.Proffesion", "Developer"}
        /// }
        /// If not specified Strings.Substitutes will be used for translation.
        /// </param>
        /// <returns>Hello my name is Serkan BAYHAN.</returns>
        public static string Format(string format, Dictionary<string, string> substitutes = null)
        {
            substitutes = substitutes ?? Substitute.Global.ToLookup();
            var subs = format.Extract(Substitute.REGEX, true);

            foreach (var sub in subs)
            {
                var key = Substitute.ToKey(sub);
                var replace = substitutes.ContainsKey(key);
                if (replace) format = format.Replace(sub, substitutes[key]);
            }

            return format;
        }

        public static string[] Extract(this string text, string pattern, bool distinct = false, RegexOptions options = RegexOptions.IgnoreCase)
        {
            var list = new List<string>();
            var matches = new Regex(pattern, options).Matches(text);
            foreach (var match in matches)
            {
                var item = match.ToString();
                var add = !distinct || !list.Contains(item);

                if (add) list.Add(item);
            }
            return list.ToArray();
        }

        public static string[] Substitutes(this string text)
        {
            return text.Extract(Substitute.REGEX, true);
        }

        public static string[] ToStrings<T>(this T[] args)
        {
            var strarr = new string[args.Length];
            for (int i = 0; i < strarr.Length; i++) strarr[i] = args[i] == null ? null : args[i].ToString();
            return strarr;
        }

        public static bool IsMatch(this string text, string pattern, RegexOptions options = default(RegexOptions))
        {
            return new Regex(pattern, options).IsMatch(text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding">Default is UTF8</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string input, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            return encoding.GetBytes(input);
        }

        public static string Obfuscate(this string input, string seed = null)
        {
            return (seed + input).ToBytes().Hash().HEX();
        }

        public static string[] Obfuscate(this string[] inputs)
        {
            var outputs = new string[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
                outputs[i] = inputs[i] == null ? null : inputs[i].Obfuscate();

            return outputs;
        }

        public static string[] CleanUp(this string[] arr)
        {
            var list = new List<string>();
            foreach (var item in arr)
            {
                var trimed = item == null ? string.Empty : item.Trim();
                if (string.IsNullOrEmpty(trimed)) continue;
                list.Add(trimed);
            }
            return list.ToArray();
        }

        public static Dictionary<string, string[]> ToParameters(this string query, string prefix = "--", string suffix = " ")
        {
            var parameters = new Dictionary<string, string[]>();

            var parts = query.Trim().Split(new[] { prefix }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                part.Split(new[] { suffix }, StringSplitOptions.RemoveEmptyEntries);
                var idx = part.IndexOf(suffix);
                var key = idx >= 0 ? part.Substring(0, idx).Trim() : part.Trim();
                var val = part.Substring(idx + suffix.Length).Trim();

                if (!parameters.ContainsKey(key)) parameters.Add(key, new[] { string.IsNullOrEmpty(val) ? string.Empty : val });
                else parameters[key] = parameters[key].Append(string.IsNullOrEmpty(val) ? string.Empty : val);
            }

            return parameters;
        }

        public static string Encrypt(this string plainText, string passPhrase = null)
        {
            passPhrase = passPhrase ?? string.Empty;
            return Cryptography.Encrypt(plainText, passPhrase);
        }

        public static string Decrypt(this string cipherText, string passPhrase = null)
        {
            passPhrase = passPhrase ?? string.Empty;
            return Cryptography.Decrypt(cipherText, passPhrase);
        }

        public static string[] GetRoots(this string[] paths, bool sorted = false)
        {
            var sordertedpaths = sorted ? paths : paths.Copy(paths).Sort();
            var selectedpaths = new List<string>();
            foreach (var path in sordertedpaths)
            {
                var covered = selectedpaths.Any(root => path.StartsWith(root));
                if (!covered) selectedpaths.Add(path);
            }
            return selectedpaths.ToArray();
        }
    }
}
