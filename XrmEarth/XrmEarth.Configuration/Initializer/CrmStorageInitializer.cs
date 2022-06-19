using System;
using System.Collections.Generic;
using XrmEarth.Configuration.Data.Core;
using XrmEarth.Configuration.Data.Storage;
using XrmEarth.Configuration.Initializer.Core;
using XrmEarth.Configuration.Policies;
using XrmEarth.Configuration.Target;

namespace XrmEarth.Configuration.Initializer
{
    public class CrmStorageInitializer<T> : StorageInitializer<T, CrmStorageTarget>
    {
        public CrmStorageInitializer(CrmStorageTarget storageTarget, StoragePolicy policy, StorageObjectContainer objectContainer) : base(storageTarget, policy, objectContainer) { }

        public CrmStoragePolicy Policy
        {
            get { return Target.Policy; }
        }

        protected override void WriteValues(Dictionary<string, ValueContainer> values)
        {
            if (Policy == null)
                throw new NullReferenceException("Crm Storage Policy not created.");

            Policy.WriteValues(Target.Service, values);
        }

        protected override Dictionary<string, ValueContainer> ReadValues(Dictionary<string, Type> keys)
        {
            if (Policy == null)
                throw new NullReferenceException("Crm Storage Policy not created.");

            return Policy.ReadValues(Target.Service, keys);
        }
    }
}