using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace XrmEarth.Configuration.Data.Core
{
    public class ValueContainer
    {
        public ValueContainer()
        {
            
        }

        public ValueContainer(object value)
        {
            Value = value;
        }

        [XmlIgnore]
        [JsonIgnore]
        public bool IsNullable { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public Type Type { get; set; }
        public object Value { get; set; }
    }
}
