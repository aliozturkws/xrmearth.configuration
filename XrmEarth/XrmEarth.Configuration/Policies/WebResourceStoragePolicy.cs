using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Serialization;
using XrmEarth.Configuration.Common;
using XrmEarth.Configuration.Data.Core;
using XrmEarth.Configuration.Storages;

namespace XrmEarth.Configuration.Policies
{
    /// <summary>
    /// It stores and reads the settings on the web resource
    /// </summary>
    public class WebResourceStoragePolicy : CrmStoragePolicy
    {
        public WebResourceStoragePolicy()
        {
            Prefix = "new";
            Name = "configuration";
            DisplayName = "XrmEarth.Configuration";
            Description = "XrmEarth.Configuration WebResource";
            Encoding = Encoding.UTF8;
        }

        /// <summary>
        /// Default Value : 'new'
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// By entering the solution name, the web resource to be created can be connected to the solution.
        /// <para></para>
        /// <code>Note: Applies to creation only. The solution cannot be changed during the update process.</code>
        /// </summary>
        public string SolutionUniqueName { get; set; }
        /// <summary>
        /// Default Value : 'configuration'
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Default Value : 'XrmEarth.Configuration'
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Default Value : 'XrmEarth.Configuration WebResource'
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Default Value : UTF8
        /// <para></para>
        /// It is used for web resource read-write operations
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public Encoding Encoding { get; set; }

        public int EncodingCodePage
        {
            get { return Encoding.CodePage; }
            set { Encoding = Encoding.GetEncoding(value); }
        }

        public override void WriteValues(IOrganizationService service, Dictionary<string, ValueContainer> values)
        {
            var webResourceStorage = CreateWebResource();
            webResourceStorage.WriteValues(service, values);
        }

        public override Dictionary<string, ValueContainer> ReadValues(IOrganizationService service, Dictionary<string, Type> keys)
        {
            var webResourceStorage = CreateWebResource();
            return webResourceStorage.ReadValues(service, keys);
        }

        public string GetFullName<TProp>(Expression<Func<WebResourceStoragePolicy, TProp>> propertyExperssion)
        {
            var prop = GetType().GetProperty(((MemberExpression)propertyExperssion.Body).Member.Name);
            if (prop == null) return null;
            var value = prop.GetValue(this);
            return string.Format("{0}_{1}", Prefix, value);
        }

        private WebResource CreateWebResource()
        {
            return new WebResource
            {
                Name = GetFullName(wr => wr.Name),
                DisplayName = DisplayName,
                Description = Description,
                ResourceType = WebResourceType.JScript,
                SolutionUniqueName = SolutionUniqueName,
                Encoding = Encoding
            };
        }
    }
}
