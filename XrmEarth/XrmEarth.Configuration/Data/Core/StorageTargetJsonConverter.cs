using System;
using Newtonsoft.Json.Linq;
using XrmEarth.Configuration.Target;

namespace XrmEarth.Configuration.Data.Core
{
    public class StorageTargetJsonConverter : JsonCreationConverter<StorageTarget>
    {
        protected override StorageTarget Create(Type objectType, JObject jObject)
        {
            var typeName = (string) jObject.Property("Type");
            var type = Utils.FindType(typeName);
            if(type == null)
                throw new NullReferenceException($"{typeName} bulunamadı. 'StorageTarget' tipi tanımlanamadı, harici geliştirmeyle eklenmiş ise 'Config.ReferencedAssemblies' özelliğine bakabilirsiniz.");

            return (StorageTarget)Activator.CreateInstance(type);
        }
    }
}
