using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using XrmEarth.Configuration.Data.Core;

namespace XrmEarth.Configuration.Policies
{
    /// <summary>
    /// Crm üzerinde veri saklama politikası. Genel olarak verilerin saklanma ve okunma yöntemlerini içerir.
    /// <para></para>
    /// Bilinen tipler;
    /// <para></para>
    /// 1- WebResource (WebResourceStoragePolicy)
    /// <para></para>
    /// 2- Entity (EntityStoragePolicy)
    /// </summary>
    public abstract class CrmStoragePolicy
    {
        protected CrmStoragePolicy()
        {
            Type = GetType().FullName;
        }

        /// <summary>
        /// Serialize işlemleri için sınıfa ait tip bilgisini saklar.
        /// </summary>
        public string Type { get; set; }

        public abstract void WriteValues(IOrganizationService service, Dictionary<string, ValueContainer> values);
        public abstract Dictionary<string, ValueContainer> ReadValues(IOrganizationService service, Dictionary<string, Type> keys);
    }
}