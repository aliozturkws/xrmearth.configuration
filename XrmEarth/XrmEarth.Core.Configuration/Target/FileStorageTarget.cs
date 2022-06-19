using System;
using XrmEarth.Core.Configuration.Data.Storage;
using XrmEarth.Core.Configuration.Initializer;
using XrmEarth.Core.Configuration.Initializer.Core;

namespace XrmEarth.Core.Configuration.Target
{
    /// <summary>
    /// Dosya hedefi.
    /// </summary>
    [Serializable]
    public class FileStorageTarget : StorageTarget
    {
        /// <summary>
        /// Dosya yolu.
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// Dosya adı.
        /// </summary>
        public string FileName { get;set; }

        public override BaseInitializer<T> CreateInitializer<T>(StoragePolicy storagePolicy, StorageObjectContainer objectContainer)
        {
            return new FileStorageInitializer<T>(this, storagePolicy, objectContainer);
        }
    }
}
