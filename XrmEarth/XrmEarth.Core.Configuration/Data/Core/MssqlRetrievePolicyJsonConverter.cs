using System;
using Newtonsoft.Json.Linq;
using XrmEarth.Core.Configuration.Target.Mssql.Base;

namespace XrmEarth.Core.Configuration.Data.Core
{
    public class MssqlRetrievePolicyJsonConverter : JsonCreationConverter<MssqlRetrievePolicy>
    {
        protected override MssqlRetrievePolicy Create(Type objectType, JObject jObject)
        {
            var typeName = (string)jObject.Property("Type");
            var type = Type.GetType(typeName);
            if (type == null)
                throw new NullReferenceException($"{typeName} bulunamadı.");

            return (MssqlRetrievePolicy)Activator.CreateInstance(type);
        }
    }
}
