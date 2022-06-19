using System;
using XrmEarth.Core.Common;

namespace XrmEarth.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ArgumentAttribute : Attribute
    {
        public ArgumentAttribute(string key)
        {
            Key = key;
        }

        public ArgumentAttribute(string key, object defaultValue)
        {
            Key = key;
            DefaultValue = defaultValue;
        }

        public ArgumentAttribute(string key, object defaultValue, ArgumentSourceType source)
        {
            Key = key;
            DefaultValue = defaultValue;
            Source = source;
        }

        public ArgumentAttribute()
        {

        }

        public string Key { get; set; }
        public string Name { get; set; }
        public bool Required { get; set; }
        public Type Converter { get; set; }
        public object DefaultValue { get; set; }

        public ArgumentSourceType Source { get; set; }
    }
}
