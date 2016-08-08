using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feather.Initials.Common.Extensions
{
    public static class Arrays
    {

        public static T[] Fill<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++) if (arr[i] == null) arr[i] = value;
            return arr;
        }

        public static T[] Overwrite<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++) arr[i] = value;
            return arr;
        }

        public static int FirstIndexOf<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++) if (arr[i].Equals(value)) return i;
            return -1;
        }

        public static int LastIndexOf<T>(this T[] arr, T value)
        {
            for (int i = arr.Length - 1; i >= 0; i--) if (arr[i].Equals(value)) return i;
            return -1;
        }

        public static int[] IndexesOf(this object[] arr, object value)
        {
            var indexes = new List<int>();
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == value) indexes.Add(i);
            }
            return indexes.ToArray();
        }

        public static bool Identical<T>(this T[] arra, T[] arrb)
        {
            bool identical = (arra == null && arrb == null);

            if (!identical && arra != null && arrb != null && arra.Length == arrb.Length)
            {
                for (int i = 0; i < arra.Length; i++)
                {
                    if (!EqualityComparer<T>.Default.Equals(arra[i], arrb[i]))
                    {
                        identical = false;
                        break;
                    }
                }
            }
            return identical;

        }

        public static string Join<T>(this T[] array, string delimeter = "")
        {
            if (array == null) return null;
            var values = new List<string>();
            foreach (var item in array) values.Add("" + item);
            return string.Join(delimeter, values.ToArray());
        }

        /// <summary>
        /// Copes array part
        /// </summary>
        /// <param name="src">source array</param>
        /// <param name="dst">destination array</param>
        /// <param name="start">starting index</param>
        /// <returns>dst</returns>
        public static T[] Copy<T>(this T[] src, T[] dst = null, int start = 0)
        {
            dst = dst ?? new T[src.Length - start];
            for (int i = start; i < src.Length && (i - start) < dst.Length; i++)
            {
                dst[i - start] = src[i];
            }
            return dst;
        }

        /// <summary>
        /// Skips specified number of elements and returns a new array
        /// </summary>
        public static T[] Skip<T>(this T[] src, int skip = 1)
        {
            return Copy(src, new T[src.Length - skip], skip);
        }

        public static T[] ToArray<T>(params T[] args) { return args; }

        /// <returns>An array in specified size and filled randomly from specified source array.</returns>
        public static T[] Random<T>(this T[] src, int size = 1)
        {
            return Random(src, new T[size]);
        }

        public static T[] Random<T>(this T[] src, T[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++) buffer[i] = src[Common.Random.Next(src.Length)];
            return buffer;
        }

        public static T[] Resize<T>(this T[] arr, int newSize)
        {
            Array.Resize(ref arr, newSize);
            return arr;
        }

        public static T[] Append<T>(this T[] arr, params T[] items)
        {
            int offset = arr.Length;
            arr = arr.Resize(arr.Length + items.Length);
            for (int i = 0; i < items.Length; i++)
            {
                arr[offset + i] = items[i];
            }
            return arr;
        }

        public static T[] Sort<T>(this T[] arr)
        {
            Array.Sort<T>(arr);
            return arr;
        }
    }
}
