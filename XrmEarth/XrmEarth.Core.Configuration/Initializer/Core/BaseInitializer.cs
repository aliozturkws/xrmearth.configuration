using XrmEarth.Core.Configuration.Data.Core;
using XrmEarth.Core.Configuration.Data.Storage;

namespace XrmEarth.Core.Configuration.Initializer.Core
{
    public abstract class BaseInitializer<T> : IStorageInitializer<T>
    {
        protected BaseInitializer(StoragePolicy policy, StorageObjectContainer objectContainer)
        {
            StoragePolicy = policy;
            ObjectContainer = objectContainer;
        }

        public StoragePolicy StoragePolicy { get; set; }
        public StorageObjectContainer ObjectContainer { get; set; }

        public abstract void Save(T storageObject);
        public abstract T Load();
        public abstract T Load(T storageInstance);
    }
}
