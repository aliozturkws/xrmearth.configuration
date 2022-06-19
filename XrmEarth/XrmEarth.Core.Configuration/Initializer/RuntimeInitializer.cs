using System.Collections.Generic;
using System.Linq;
using XrmEarth.Core.Configuration.Initializer.Core;

namespace XrmEarth.Core.Configuration.Initializer
{
    public class RuntimeInitializer<T> : BaseInitializer<T>
    {
        public RuntimeInitializer(BaseInitializer<T> initializer) : base(initializer.StoragePolicy, initializer.ObjectContainer)
        {
            Initializer = initializer;
        }

        protected readonly BaseInitializer<T> Initializer;

        public override void Save(T storageObject)
        {
            Initializer.Save(storageObject);
        }

        public override T Load()
        {
            return Initializer.Load();
        }

        public override T Load(T storageInstance)
        {
            return Initializer.Load(storageInstance);
        }

        public Dictionary<string, object> GetKeyValues()
        {
            var instance = Load();
            return ObjectContainer.GetKeyAndValues(instance).Select(kv => new {kv.Key, kv.Value.Value}).ToDictionary(arg => arg.Key, arg => arg.Value);
        }

        public List<string> GetKeys()
        {
            return ObjectContainer.GetKeys().Select(kv => kv.Key).ToList();
        }
    }
}
