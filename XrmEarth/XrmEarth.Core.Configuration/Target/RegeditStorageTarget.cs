using System;
using Microsoft.Win32;
using XrmEarth.Core.Configuration.Data.Storage;
using XrmEarth.Core.Configuration.Initializer;
using XrmEarth.Core.Configuration.Initializer.Core;

namespace XrmEarth.Core.Configuration.Target
{
    /// <summary>
    /// Kayıt defteri hedefi.
    /// </summary>
    [Serializable]
    public class RegeditStorageTarget : StorageTarget
    {
        /// <summary>
        /// Hive
        /// </summary>
        public RegistryHive Hive { get; set; }

        /// <summary>
        /// Ana dizin (Hive, Root) hariç yol.
        /// <para></para>
        /// Örn: 'SOFTWARE\WOW6432Node\CrmAkademi'
        /// </summary>
        public string Path { get; set; }

        public override BaseInitializer<T> CreateInitializer<T>(StoragePolicy storagePolicy, StorageObjectContainer objectContainer)
        {
            return new RegeditStorageInitializer<T>(this, storagePolicy, objectContainer);
        }
    }
}
