using System;
using System.Collections.Generic;
using Microsoft.Win32;
using XrmEarth.Core.Configuration.Data.Core;
using XrmEarth.Core.Configuration.Data.Exceptions;
using XrmEarth.Core.Configuration.Data.Storage;
using XrmEarth.Core.Configuration.Initializer.Core;
using XrmEarth.Core.Configuration.Target;

namespace XrmEarth.Core.Configuration.Initializer
{
    public class RegeditStorageInitializer<T> : StorageInitializer<T, RegeditStorageTarget>
    {
        public RegeditStorageInitializer(RegeditStorageTarget storageTarget, StoragePolicy policy, StorageObjectContainer objectContainer) : base(storageTarget, policy, objectContainer)
        {
        }

        protected override void WriteValues(Dictionary<string, ValueContainer> keyAndValues)
        {
            var mainKey = OpenBaseKey(Target.Hive);
            var key = mainKey.OpenSubKey(Target.Path, RegistryKeyPermissionCheck.ReadWriteSubTree);

            if(key == null)
                key = CreateKeyTree(mainKey, Target.Path);

            if (key == null)
                throw new InitializerCoreException("Anahtara erişilemedi. Root : " + Target.Hive + "\r\nDizin : " + Target.Path);

            foreach (var keyAndValue in keyAndValues)
            {
                key.SetValue(keyAndValue.Key, keyAndValue.Value.Value ?? string.Empty);
            }
        }

        protected override Dictionary<string, ValueContainer> ReadValues(Dictionary<string, Type> keys)
        {
            var mainKey = OpenBaseKey(Target.Hive);
            var key = mainKey.OpenSubKey(Target.Path, RegistryKeyPermissionCheck.ReadSubTree);

            if (key == null)
                throw new InitializerCoreException("Anahtara erişilemedi. Root : " + Target.Hive + "\r\nDizin : " + Target.Path);

            var keyAndValues = new Dictionary<string, ValueContainer>();
            foreach (var propertyKey in keys)
            {
                keyAndValues.Add(propertyKey.Key, new ValueContainer { Value = key.GetValue(propertyKey.Key) , Type = propertyKey.Value} );
            }
            return keyAndValues;
        }

        private RegistryKey CreateKeyTree(RegistryKey key, string path)
        {
            return key.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree);
        }

        public RegistryKey OpenBaseKey(RegistryHive registryHive)
        {
            return RegistryKey.OpenBaseKey(registryHive, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
        }
    }
}
