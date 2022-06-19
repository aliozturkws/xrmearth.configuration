using System;
using Microsoft.Xrm.Sdk;
using XrmEarth.Configuration.Common;

namespace XrmEarth.Configuration.Storages
{
    public class CustomEntity
    {
        public CustomEntity()
        {
            
        }
        public CustomEntity(Entity entity, EntityTemplate template)
        {
            Bind(entity, template);
        }

        public Guid ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public Entity Create(EntityTemplate template)
        {
            return new Entity(template.Name, ID)
            {
                Attributes = {{template.KeyName, Key}, {template.ValueName, Value}}
            };
        }

        public void Bind(Entity entity, EntityTemplate template)
        {
            ID = entity.Id;
            Key = entity.GetAttributeValue<string>(template.KeyName);
            Value = entity.GetAttributeValue<string>(template.ValueName);
        }
    }
}