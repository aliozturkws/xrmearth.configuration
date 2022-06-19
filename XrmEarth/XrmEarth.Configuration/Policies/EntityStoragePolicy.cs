using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using XrmEarth.Configuration.Common;
using XrmEarth.Configuration.Data.Core;
using XrmEarth.Configuration.Storages;

namespace XrmEarth.Configuration.Policies
{
    /// <summary>
    /// TODO : Max 200 key!
    /// </summary>
    public class EntityStoragePolicy : CrmStoragePolicy
    {
        public EntityStoragePolicy()
        {
            Prefix = "new";
            LogicalName = "configuration";
            KeyAttributeLogicalName = "name";
            ValueAttributeLogicalName = "value";
        }

        /// <summary>
        /// Default Value : 'new'
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// Entity Name
        /// Default Value : 'configuration'
        /// </summary>
        public string LogicalName { get; set; }
        /// <summary>
        /// Field Name
        /// Default Value : 'name'
        /// </summary>
        public string KeyAttributeLogicalName { get; set; }
        /// <summary>
        /// Field Name
        /// Default Value : 'value'
        /// </summary>
        public string ValueAttributeLogicalName { get; set; }

        public override void WriteValues(IOrganizationService service, Dictionary<string, ValueContainer> values)
        {
            var entityStorage = CreateCustomeEntityCollection();
            entityStorage.WriteValues(service, values);
        }

        public override Dictionary<string, ValueContainer> ReadValues(IOrganizationService service, Dictionary<string, Type> keys)
        {
            var entityStorage = CreateCustomeEntityCollection();
            return entityStorage.ReadValues(service, keys);
        }

        public string GetFullName<TProp>(Expression<Func<EntityStoragePolicy, TProp>> propertyExperssion)
        {
            var prop = GetType().GetProperty(((MemberExpression)propertyExperssion.Body).Member.Name);
            if (prop == null) return null;
            var value = prop.GetValue(this);
            return string.Format("{0}_{1}", Prefix, value);
        }

        private CustomEntityCollection CreateCustomeEntityCollection()
        {
            var template = new EntityTemplate(
                GetFullName(e => e.LogicalName),
                GetFullName(e => e.KeyAttributeLogicalName),
                GetFullName(e => e.ValueAttributeLogicalName));

            return new CustomEntityCollection(template);
        }
    }
}