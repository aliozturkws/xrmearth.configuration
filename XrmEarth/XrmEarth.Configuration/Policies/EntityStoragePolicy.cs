using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using XrmEarth.Configuration.Common;
using XrmEarth.Configuration.Storages;
using XrmEarth.Configuration.Data.Core;

namespace XrmEarth.Configuration.Policies
{
    //TODO [-] 31.10.2017 - Fetch yap�s�na sayfalama geli�tirmesi yap�larak. 200 de�erden daha fazla saklanabilir hale getirilmeli.
    /// <summary>
    /// Ayarlar� varl�k �zerinde saklar ve okur.
    /// <para></para>
    /// <c>Not:</c> Maksimum 200 de�er saklayabilir.
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
        /// Varsay�lan de�er: 'rms'
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// Varl�k mant�ksal ad�.
        /// Varsay�lan de�er: 'configuration'
        /// </summary>
        public string LogicalName { get; set; }
        /// <summary>
        /// Varl�k �zerindeki key alan�n�n mant�ksal ad�.
        /// Varsay�lan de�er: 'key'
        /// </summary>
        public string KeyAttributeLogicalName { get; set; }
        /// <summary>
        /// Varl�k �zerindeki de�er alan�n�n mant�ksal ad�.
        /// Varsay�lan de�er: 'value'
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