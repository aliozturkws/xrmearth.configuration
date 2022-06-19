using Microsoft.Xrm.Sdk;
using XrmEarth.Configuration.Data.Storage;
using XrmEarth.Configuration.Initializer;
using XrmEarth.Configuration.Initializer.Core;
using XrmEarth.Configuration.Policies;

namespace XrmEarth.Configuration.Target
{
    /// <summary>
    /// Crm Hedefi.
    /// <para></para>
    /// <para></para>
    /// <code>* Varlık üzerinde okuma yazma işlemi yapabilmek için ilgili varlığın Crm'de oluşturulmuş olması gerekmektedir.</code>
    /// </summary>
    public class CrmStorageTarget : StorageTarget
    {
        public CrmStorageTarget(IOrganizationService service)
        {
            _service = service;
        }

        public CrmStoragePolicy Policy { get; set; }

        private IOrganizationService _service;

        public IOrganizationService Service { get { return _service; } }

        public override BaseInitializer<T> CreateInitializer<T>(StoragePolicy storagePolicy, StorageObjectContainer objectContainer)
        {
            return new CrmStorageInitializer<T>(this, storagePolicy, objectContainer);
        }
    }
}
