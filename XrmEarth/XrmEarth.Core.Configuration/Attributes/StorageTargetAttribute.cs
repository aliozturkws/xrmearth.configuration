using System;

namespace XrmEarth.Core.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
    public abstract class StorageTargetAttribute : Attribute
    {
        protected StorageTargetAttribute(Type targetType)
        {
            TargetType = targetType;
        }
        public Type TargetType { get; set; }
        public Type Type { get; set; }
        public string Key { get; set; }
    }
}
