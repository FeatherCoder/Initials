using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feather.Initials.Common.Extensions
{
    public static class Dictionaries
    {
        public static Dictionary<string, string> ToLookup<K, V>(this Dictionary<K, V> dictionary)
        {
            var lookup = new Dictionary<string, string>();
            foreach (var item in dictionary) lookup.Add("" + item.Key, "" + item.Value);
            return lookup;
        }

        public static Dictionary<K, V> Clone<K, V>(this Dictionary<K, V> dictionary)
        {
            return new Dictionary<K, V>(dictionary);
        }
    }
}
