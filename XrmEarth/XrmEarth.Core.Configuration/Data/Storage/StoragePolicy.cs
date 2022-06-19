using System;
using System.ComponentModel;
using XrmEarth.Core.Configuration.Attributes;

namespace XrmEarth.Core.Configuration.Data.Storage
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable]
    public class StoragePolicy
    {
        public StorageTargetAttribute Attribute { get; set; }

        public Type Owner { get; set; }
    }
}
