using System;
using System.Collections.Generic;
using XrmEarth.Configuration.Data.Core;
using XrmEarth.Configuration.Data.Storage;

namespace XrmEarth.Configuration.Initializer.Core
{
    public abstract class StorageInitializer<T, T1> : BaseInitializer<T>, IStorageTarget
    {
        protected StorageInitializer(T1 storageTarget, StoragePolicy policy, StorageObjectContainer objectContainer) : base(policy, objectContainer)
        {
            if (Equals(storageTarget, null))
                throw new NullReferenceException("storageTarget");

            Target = storageTarget;
        }

        public T1 Target { get; protected set; }

        public override void Save(T storageObject)
        {
            if (Equals(storageObject, null))
                throw new NullReferenceException("storageObject");

            var keyAndValues = ObjectContainer.GetKeyAndValues(storageObject);

            WriteValues(keyAndValues);
        }

        public override T Load()
        {
            var storageInstance = (T)Activator.CreateInstance(typeof(T));

            return Load(storageInstance);
        }

        public override T Load(T storageInstance)
        {
            if (Equals(storageInstance, null))
                throw new NullReferenceException("storageInstance");

            ObjectContainer.SetValues(storageInstance, ReadValues(ObjectContainer.GetKeys()));

            return storageInstance;
        }

        protected abstract void WriteValues(Dictionary<string, ValueContainer> values);
        protected abstract Dictionary<string, ValueContainer> ReadValues(Dictionary<string, Type> keys);
    }
}
