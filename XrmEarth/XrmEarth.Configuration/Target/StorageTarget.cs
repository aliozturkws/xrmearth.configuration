using System;
using XrmEarth.Configuration.Data.Core;
using XrmEarth.Configuration.Data.Storage;
using XrmEarth.Configuration.Initializer.Core;

namespace XrmEarth.Configuration.Target
{
    [Serializable]
    public abstract class StorageTarget : IStorageTarget
    {
        protected StorageTarget()
        {
            Type = GetType().FullName;
        }

        public string Type { get; private set; }

        public abstract BaseInitializer<T> CreateInitializer<T>(StoragePolicy storagePolicy, StorageObjectContainer objectContainer);
    }
}
