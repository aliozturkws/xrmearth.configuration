using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XrmEarth.Configuration.Data;
using XrmEarth.Configuration.Data.Core;

namespace XrmEarth.Configuration
{
    internal static class Utils
    {
        #region - Namespace -

        internal static string NamespaceUniquefier(Type type)
        {
            return type.FullName;
        }

        internal static string PropertyUniquefier(PropertyInfo property)
        {
            return property.Name;
        }

        internal static string NameUniquefier(Type type, PropertyInfo property, List<PropertyInfo> properties = null)
        {
            var ns = NamespaceUniquefier(type);

            ns = properties == null ? ns : properties.Aggregate(ns, (current, p) => current + ("." + PropertyUniquefier(p)));

            ns = string.Format("{0}.{1}", ns, PropertyUniquefier(property));

            return ns;
        }

        internal static string NameUniquefier(Type type, string propertyKey, List<PropertyInfo> properties = null)
        {
            var ns = NamespaceUniquefier(type);

            ns = properties == null ? ns : properties.Aggregate(ns, (current, p) => current + ("." + PropertyUniquefier(p)));

            ns = string.Format("{0}.{1}", ns, propertyKey);

            return ns;
        }

        internal static int PropertyDeep(Type type, string propertyNamespace)
        {
            var nsStartWith = NamespaceUniquefier(type);

            if (!propertyNamespace.StartsWith(nsStartWith))
                return -1;

            var pureNs = propertyNamespace.Remove(0, nsStartWith.Length);

            return pureNs.Split('.').Length;
        }

        #endregion - Namespace -

        #region - Converter -

        internal static object ConvertValue(IValueConverter converter, object val)
        {
            return converter != null ? converter.Convert(val) : val;
        }

        internal static object ConvertBackValue(IValueConverter converter, object val)
        {
            return converter != null ? converter.ConvertBack(val) : val;
        }

        #endregion - Converter -

        #region - Utils -

        internal static Type FindType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                var assemblies = GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    type = assembly.GetType(typeName);
                    if (type != null)
                        return type;
                }
            }
            return type;
        }

        internal static Type FindType(string typeName, IEnumerable<Assembly> referencedAssemblies)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                foreach (var assembly in referencedAssemblies)
                {
                    type = assembly.GetType(typeName);
                    if (type != null)
                        return type;
                }
            }
            return type;
        }

        /// <summary>
        /// IL Merge için
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        internal static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                var fullNameParts = typeName.Split(',');
                if (fullNameParts.Length == 0)
                    return null;

                type = Type.GetType(fullNameParts[0]);
            }
            return type;
        }

        internal static bool IsWritable(Type t)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                    return true;
                default:
                    if (t == typeof(Guid))
                    {
                        return true;
                    }
                    return false;
            }
        }

        internal static bool IsCollection(Type t)
        {
            return typeof(ICollection).IsAssignableFrom(t);
        }

        internal static bool IsNumeric(Type t)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        internal static bool IsArray(Type t)
        {
            return typeof(Array).IsAssignableFrom(t);
        }

        internal static bool IsInhereted(Type type, Type baseType)
        {
            return baseType.IsAssignableFrom(type);
        }

        internal static object TryConvertEnum(object val, Type enumType)
        {
            int intVal;
            if (int.TryParse(val.ToString(), out intVal))
            {
                foreach (var enmVal in Enum.GetValues(enumType))
                {
                    if ((int)enmVal == intVal)
                        return enmVal;
                }
            }
            else
            {
                foreach (var enmVal in Enum.GetValues(enumType))
                {
                    if (enmVal.ToString() == val.ToString())
                        return enmVal;
                }
            }
            return val;
        }
        
        internal static List<Assembly> GetAssemblies()
        {
            if (Config.Sandbox)
                return Config.RegisteredAssemblies.ToList();

            var assemblies = Config.RegisteredAssemblies.ToList();
            assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies());
            return assemblies;
        }

        #endregion - Utils -
    }
}
