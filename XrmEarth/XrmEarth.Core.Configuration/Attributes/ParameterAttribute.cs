using System;
using XrmEarth.Core.Common;

namespace XrmEarth.Core.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ParameterAttribute : Attribute
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public bool Required { get; set; }
        public Type Converter { get; set; }
        public object DefaultValue { get; set; }

        public ArgumentSourceType Source { get; set; }
    }
}
