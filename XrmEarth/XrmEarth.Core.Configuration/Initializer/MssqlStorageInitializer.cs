using System;
using System.Collections.Generic;
using XrmEarth.Core.Configuration.Data.Core;
using XrmEarth.Core.Configuration.Data.Storage;
using XrmEarth.Core.Configuration.Initializer.Core;
using XrmEarth.Core.Configuration.Target;
using XrmEarth.Core.Configuration.Target.Mssql.Base;

namespace XrmEarth.Core.Configuration.Initializer
{
    public class MssqlStorageInitializer<T> : StorageInitializer<T, MssqlStorageTarget>
    {
        public MssqlStorageInitializer(MssqlStorageTarget storageTarget, StoragePolicy policy, StorageObjectContainer objectContainer) : base(storageTarget, policy, objectContainer)
        {
        }

        public MssqlStoragePolicy MssqlStoragePolicy
        {
            get
            {
                return Target.Policy;
            }
        }


        protected override void WriteValues(Dictionary<string, ValueContainer> values)
        {
            MssqlStoragePolicy.DeliveryPolicy.Deliver(Target.Connection, values);
        }

        protected override Dictionary<string, ValueContainer> ReadValues(Dictionary<string, Type> keys)
        {
            return MssqlStoragePolicy.RetrievePolicy.Retrieve(Target.Connection, keys);
        }
    }
}
