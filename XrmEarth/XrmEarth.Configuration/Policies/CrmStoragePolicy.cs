using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using XrmEarth.Configuration.Data.Core;

namespace XrmEarth.Configuration.Policies
{
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

        public string Type { get; set; }

        public abstract void WriteValues(IOrganizationService service, Dictionary<string, ValueContainer> values);
        public abstract Dictionary<string, ValueContainer> ReadValues(IOrganizationService service, Dictionary<string, Type> keys);
    }
}