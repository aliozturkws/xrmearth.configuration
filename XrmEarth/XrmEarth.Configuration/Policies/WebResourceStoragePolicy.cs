using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using XrmEarth.Configuration.Data.Core;
using XrmEarth.Configuration.Common;
using XrmEarth.Configuration.Storages;

namespace XrmEarth.Configuration.Policies
{
    /// <summary>
    /// Ayarları web kaynağı üzerinde saklar ve okur.
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
        /// Varsayılan değer: 'rms'
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// Çözüm adı girilerek oluşturulacak web kaynağının çözüme bağlanması sağlanabilir.
        /// <para></para>
        /// <code>Not: Sadece oluşturma işleminde geçerlidir. Güncelleme işlemlerinde çözüm değiştirilemez.</code>
        /// </summary>
        public string SolutionUniqueName { get; set; }
        /// <summary>
        /// Varsayılan değer: 'configuration'
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Varsayılan değer: 'XrmEarth.Configuration'
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Varsayılan değer: 'XrmEarth.Configuration kaynağı'
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Varsayılan değer: UTF8
        /// <para></para>
        /// Web kaynağı okuma yazma işlemlerinde kullanılır.
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
