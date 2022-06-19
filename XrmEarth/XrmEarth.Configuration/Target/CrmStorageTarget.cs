using Microsoft.Xrm.Sdk;
using XrmEarth.Configuration.Data.Storage;
using XrmEarth.Configuration.Initializer;
using XrmEarth.Configuration.Initializer.Core;
using XrmEarth.Configuration.Policies;

namespace XrmEarth.Configuration.Target
{
    public class CrmStorageTarget : StorageTarget
    {
        public CrmStorageTarget(IOrganizationService service)
        {
            Service = service;
        }

        public CrmStoragePolicy Policy { get; set; }

        public IOrganizationService Service { get; set; }

        public override BaseInitializer<T> CreateInitializer<T>(StoragePolicy storagePolicy, StorageObjectContainer objectContainer)
        {
            return new CrmStorageInitializer<T>(this, storagePolicy, objectContainer);
        }
    }
}
