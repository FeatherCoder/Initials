using Feather.Initials.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Feather.Initials.Common
{
    public static class Reflections
    {
        public static FileVersionInfo GetVersionInfo(object o)
        {
            if (o is Type) return GetVersionInfo(o.GetType());
            if (o is Assembly) return GetVersionInfo((Assembly)o);
            return GetVersionInfo(o.GetType());
        }

        public static FileVersionInfo GetVersionInfo<T>()
        {
            return GetVersionInfo(typeof(T));
        }

        public static FileVersionInfo GetVersionInfo(Type type)
        {
            return GetVersionInfo(type.Assembly);
        }

        public static FileVersionInfo GetVersionInfo(this Assembly asm)
        {
            return FileVersionInfo.GetVersionInfo(asm.Location);
        }

        public static object CreateInstance(Type type, params object[] args)
        {
            //create the instance of class using System.Activator class 
            object obj = args == null ? Activator.CreateInstance(type) : Activator.CreateInstance(type, args);
            return obj;
        }

        public static Type GetType(Assembly assembly, string type)
        {
            //get the class type information in which late bindig applied 
            Type implementation = assembly.GetType(type);
            return implementation;
        }

        /// <param name="baseType">Selects only this type of types</param>
        /// <param name="interfaces">Selects interfaces if it is true</param>
        /// <param name="abstracts">Selects abstracts if its true </param>
        /// <param name="ns">Namespace regex pattern</param>
        /// <param name="name">Name regex pattern</param>
        /// <returns>All defined types in application domain</returns>
        public static Type[] GetTypes(Type baseType, bool interfaces = true, bool abstracts = true, string ns = null, string name = null)
        {
            var types = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return GetTypes(assemblies, baseType, interfaces, abstracts, ns, name);
        }

        public static Type[] GetTypes<T>(bool interfaces = true, bool abstracts = true, string ns = null, string name = null)
        {
            return GetTypes(baseType: typeof(T), interfaces: interfaces, abstracts: abstracts, ns: ns, name: name);
        }

        public static Type[] GetTypes(Assembly[] assemblies, Type baseType, bool interfaces = true, bool abstracts = true, string ns = null, string name = null)
        {
            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                Type[] typs = GetTypes(assembly, baseType, interfaces, abstracts, ns, name);
                types.AddRange(typs);
            }
            return types.ToArray();
        }

        /// <param name="assembly">Selects from this assebmly</param>
        /// <param name="baseType">Selects only this type of types</param>
        /// <param name="interfaces">Selects interfaces if it is true</param>
        /// <param name="abstracts">Selects abstracts if its true </param>
        /// <param name="ns">Namespace regex pattern</param>
        /// <param name="name">Name regex pattern</param>
        /// <returns>Selected types defined in assembly</returns>
        public static Type[] GetTypes(Assembly assembly, Type baseType, bool interfaces = true, bool abstracts = true, string ns = null, string name = null)
        {
            if (assembly.IsDynamic) return new Type[0];
            Type[] types = (from t in assembly.GetExportedTypes()
                            where (interfaces || !t.IsInterface) && (abstracts || !t.IsAbstract)
                            where baseType.IsAssignableFrom(t)
                            where (ns == null || t.Namespace.IsMatch(ns))
                            where (name == null || t.Name.IsMatch(name))
                            select t).ToArray();
            return types;
        }

        /// <param name="baseType">Selects only this type of types</param>
        /// <returns>All defined types in application domain</returns>
        public static object[] GetInstances(Type baseType, params object[] args)
        {
            Type[] types = GetTypes(baseType, false, false);
            return GetInstances(types, args);
        }

        public static object[] GetInstances(Assembly assembly, Type baseType, params object[] args)
        {
            Type[] types = GetTypes(assembly, baseType, false, false);
            return GetInstances(types, args);
        }

        public static object[] GetInstances(Type[] types, params object[] args)
        {
            var list = new List<object>();
            foreach (var type in types)
            {
                var instance = CreateInstance(type, args);
                list.Add(instance);
            }
            return list.ToArray();
        }

    }
}
