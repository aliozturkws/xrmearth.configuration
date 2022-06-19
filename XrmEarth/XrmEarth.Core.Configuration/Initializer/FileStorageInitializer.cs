using System;
using System.Collections.Generic;
using System.IO;
using XrmEarth.Core.Configuration.Data.Core;
using XrmEarth.Core.Configuration.Data.Exceptions;
using XrmEarth.Core.Configuration.Data.Storage;
using XrmEarth.Core.Configuration.Initializer.Core;
using XrmEarth.Core.Configuration.Target;
using XrmEarth.Core.Utility;

namespace XrmEarth.Core.Configuration.Initializer
{
    public class FileStorageInitializer<T> : StorageInitializer<T, FileStorageTarget>
    {
        public FileStorageInitializer(FileStorageTarget storageTarget, StoragePolicy policy, StorageObjectContainer objectContainer) : base(storageTarget, policy, objectContainer)
        {
        }

        protected override void WriteValues(Dictionary<string, ValueContainer> values)
        {
            var fullPath = Path.Combine(Target.Path, Target.FileName);

            if (!Directory.Exists(Target.Path))
                Directory.CreateDirectory(Target.Path);

            JsonSerializerUtil.SerializeFile(values, fullPath);
            //XmlSerializeUtil.SerializeFile(values, fullPath);
        }

        protected override Dictionary<string, ValueContainer> ReadValues(Dictionary<string, Type> keys)
        {
            if (!Directory.Exists(Target.Path))
                throw new InitializerCoreException("Dizin bulunamadı. Detay için 'InnerException' bakınız.", new DirectoryNotFoundException(string.Concat("Dizin : \"", Target.Path, "\"")));

            var fullPath = Path.Combine(Target.Path, Target.FileName);

            if (!File.Exists(fullPath))
                throw new InitializerCoreException("Ayar dosyası bulunamadı. Detay için 'InnerException' bakınız", new FileNotFoundException("Dosya bulunamadı. ", fullPath));

            return JsonSerializerUtil.DeserializeFile<Dictionary<string, ValueContainer>>(fullPath);
            //return XmlSerializeUtil.DeserializeFile<Dictionary<string, ValueContainer>>(fullPath);
        }
    }
}
