using System;
using System.ComponentModel;

namespace XrmEarth.Configuration.Data.Storage
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable]
    public class StorageObjectPolicy
    {
        public StorageObjectPolicy()
        {
            IgnoreRecursiveProperties = true;
        }

        public bool IgnoreRecursiveProperties { get; set; }
    }
}
