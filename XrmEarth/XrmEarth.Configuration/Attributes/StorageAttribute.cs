using System;

namespace XrmEarth.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class StorageAttribute : Attribute
    {
        public string Key { get; set; }
        public Type Converter { get; set; }
        public bool Exclude { get; set; }
        public object DefaultValue { get; set; }
    }
}
