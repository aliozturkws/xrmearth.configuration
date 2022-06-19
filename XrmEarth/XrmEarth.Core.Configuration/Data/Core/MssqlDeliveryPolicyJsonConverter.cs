using System;
using Newtonsoft.Json.Linq;
using XrmEarth.Core.Configuration.Target.Mssql.Base;

namespace XrmEarth.Core.Configuration.Data.Core
{
    public class MssqlDeliveryPolicyJsonConverter : JsonCreationConverter<MssqlDeliveryPolicy>
    {
        protected override MssqlDeliveryPolicy Create(Type objectType, JObject jObject)
        {
            var typeName = (string)jObject.Property("Type");
            var type = Type.GetType(typeName);
            if (type == null)
                throw new NullReferenceException($"{typeName} bulunamadı.");

            return (MssqlDeliveryPolicy)Activator.CreateInstance(type);
        }
    }
}
