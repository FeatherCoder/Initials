using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Initials.Common
{
    public abstract class Substitute
    {
        public const string REGEX = @"{\S+}";

        #region Global Dictionary for Substitutes
        private static Dictionary<string, object> global;
        /// <summary>
        /// Global substitutes
        /// </summary>
        public static Dictionary<string, object> Global
        {
            get
            {
                if (global != null) return global;
                global = new Dictionary<string, object>();
                var instances = Reflections.GetInstances(typeof(Substitute));
                foreach (var instance in instances)
                {
                    global.Add(instance.GetType().Name, instance);
                }
                return global;
            }
            set { }
        }
        #endregion

        public static string ToKey(string item)
        {
            return item.Substring(1, item.Length - 2);
        }

        public static string[] ToKeys(string[] items)
        {
            for (int i = 0; i < items.Length; i++) items[i] = ToKey(items[i]);
            return items;
        }
    }
}
