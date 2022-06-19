using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using XrmEarth.Core.Attributes;
using XrmEarth.Core.Common;
using XrmEarth.Core.Configuration.Common;
using XrmEarth.Core.Configuration.Data.Core;
using XrmEarth.Core.Utility;
using Formatting = Newtonsoft.Json.Formatting;

namespace XrmEarth.Core.Configuration.Data
{
    /// <summary>
    /// ConfigurationManager sýnýfýnýn deðiþkenlerini saklar.<para></para>
    /// </summary>
    public class Config
    {
        public static readonly HashSet<Assembly> RegisteredAssemblies = new HashSet<Assembly>();
        
        /// <summary>
        /// Sandbox'a uyarlý çalýþýp çalýþmayacaðýný belirtir.
        /// <para></para>
        /// Kütüphane tip arama iþlemlerinde baðlý assembly lere eriþmeye çalýþtýðýndan dolayý güvenlik ile ilgili hatalar almanýza neden olabilir. Bu özelliði 'True' set ederek bu gibi güvenlik sorunu çýkaracak iþlemler gerçekleþtirilmez, fakat bu seferde bazý iþlemler gerçekleþtirilemeyebilir.
        /// <para></para>
        /// <c>Not</c> Ayar dosyasý ve komut satýrýndan yüklenmeyi destekler.
        /// <para></para>
        /// <list type="bullet">
        ///     <item><description>Kaynak - Parametre Adý<para></para></description></item>
        ///     <item><description>Ayar dosyasý - 'Configuration:Sandbox'<para></para></description></item>
        ///     <item><description>Komut satýrý - '-Configuration:Sandbox'<para></para></description></item>
        /// </list>
        /// </summary>
        public static bool Sandbox { get; set; }

        private readonly Dictionary<string, Tuple<ConfigSourceType, LazyValueLoader>> _values = new Dictionary<string, Tuple<ConfigSourceType, LazyValueLoader>>();

        #region - CONFIG -
        /// <summary>
        /// Uygulama bilgilerinin özetini saklar.
        /// </summary>
        [Obsolete("Sandbox için devre dýþý býrakýldý.")]
        public ApplicationSummary App { get; set; }

        /// <summary>
        /// Baþlangýç konfigürasyonunun okunma ayarlarý.
        /// </summary>
        public StartupConfigReadSettings ConfigReadSettings { get; set; }

        /// <summary>
        /// Baþlangýç konfigürasyonunun okunacaðý dosyanýn yolu. (Örn: C:\Users\admin\Appdata\CrmAkademi\StartupConfig.json)
        /// <para></para>
        /// <c>Not</c> Ayar dosyasý ve komut satýrýndan yüklenmeyi destekler.
        /// <para></para>
        /// <list type="bullet">
        ///     <item><description>Kaynak - Parametre Adý<para></para></description></item>
        ///     <item><description>Ayar dosyasý - 'Configuration:Path'<para></para></description></item>
        ///     <item><description>Komut satýrý - '-Configuration:Path'<para></para></description></item>
        /// </list>
        /// </summary>
        [Argument("Configuration:Path", Source = ArgumentSourceType.AllPlatform)]
        public string ConfigurationPath { get; set; }
        /// <summary>
        /// Baþlangýç konfigürasyonunun anahtar. (Örn: DEFAULT)
        /// <para></para>
        /// <c>Not</c> Ayar dosyasý ve komut satýrýndan yüklenmeyi destekler.
        /// <para></para>
        /// <list type="bullet">
        ///     <item><description>Kaynak - Parametre Adý<para></para></description></item>
        ///     <item><description>Ayar dosyasý - 'Configuration:Key'<para></para></description></item>
        ///     <item><description>Komut satýrý - '-Configuration:Key'<para></para></description></item>
        /// </list>
        /// </summary>
        [Argument("Configuration:Key", Source = ArgumentSourceType.AllPlatform)]
        public string Key { get; set; }

        /// <summary>
        /// Yapýlan geliþtirmelerin yer aldýðý Assembly buraya eklenerek tip arama iþlemlerinde hem güvenlik hemde performans artýþý saðlanabilir.
        /// <para></para>
        /// <c>Not</c> Özellikle 'Sandbox' özelliði 'True' olduðu durumlarda tip arama iþlemleri için gerekli olabilir.
        /// <para></para>
        /// <c>Not</c> ILMerge ile kütüphane birleþtirme iþlemi yapýldýysa, varolan bütün tiplere bu ortak kütüphaneden eriþilebilir olduðu için Assembly eklemenize gerek yoktur.
        /// </summary>
        public List<Assembly> ReferencedAssemblies { get; set; }
        #endregion - CONFIG -

        /// <summary>
        /// Deðiþkenleri, kaynaklarýný ve deðerlerini yazdýrýr.
        /// <para></para>
        /// Kaynaklar;
        /// <para></para>
        /// <list type="bullet">
        /// <item><description>Varsayýlan<para></para></description></item>
        /// <item><description>Nesne<para></para></description></item>
        /// <item><description>Ayar dosyasý<para></para></description></item>
        /// <item><description>Komut satýrý<para></para></description></item>
        /// </list>
        /// </summary>
        /// <returns>{Deðiþken Adý} - {Kaynak} - {Deðer}<para>- - -</para></returns>
        public string ReportValues()
        {
            var builder = new StringBuilder();
            foreach (var key in _values.Keys)
            {
                var val = _values[key];
                var sourceKey = val.Item1;
                var propVal = val.Item2.Value;
                var safePropVal = propVal == null
                    ? "{NULL}"
                    : propVal.GetType().IsClass
                        ? JsonConvert.SerializeObject(propVal, Formatting.Indented)
                        : propVal;
                builder
                    .Append(key)
                    .Append(" - ")
                    .Append(sourceKey)
                    .Append(" - ")
                    .Append(safePropVal)
                    .AppendLine()
                    .AppendLine("- - -");
            }

            return builder.ToString();
        }
        
        /// <summary>
        /// ReferencedAssemblies deðiþkenine eklenmiþ Assembly leri iþler. Bu iþlem yapýlmadan Assembly'ler üzerinde herhangi bir iþlem yapýlmaz.
        /// </summary>
        public void InvalidateAssemblies()
        {
            if (ReferencedAssemblies != null)
            {
                foreach (var referencedAssembly in ReferencedAssemblies)
                {
                    RegisteredAssemblies.Add(referencedAssembly);
                }
            }
        }

        internal void AddValue<TProp>(Expression<Func<Config, TProp>> propertyExpression, ConfigSourceType sourceType)
        {
            var propName = ((MemberExpression)propertyExpression.Body).Member.Name;
            var prop = GetType().GetProperty(propName);

            AddValueInternal(prop, sourceType);
        }
        internal void AllValuesDefault()
        {
            foreach (var property in GetType().GetProperties())
            {
                AddValueInternal(property);
            }   
        }
        private void AddValueInternal(PropertyInfo property, ConfigSourceType sourceType = ConfigSourceType.Default)
        {
            _values[property.Name] = Tuple.Create(sourceType, new LazyValueLoader { Instance = this, Property = property });
        }

        internal void Merge(Config overrideConfig, ConfigSourceType sourceType)
        {
            if (overrideConfig != null)
            {
                if (!string.IsNullOrEmpty(overrideConfig.ConfigurationPath))
                {
                    ConfigurationPath = overrideConfig.ConfigurationPath;
                    AddValue(c => c.ConfigurationPath, sourceType);
                }


                if (!string.IsNullOrEmpty(overrideConfig.Key))
                {
                    Key = overrideConfig.Key;
                    AddValue(c => c.Key, sourceType);
                }


                if (overrideConfig.ConfigReadSettings != null)
                {
                    ConfigReadSettings = overrideConfig.ConfigReadSettings;
                    AddValue(c => c.ConfigReadSettings, sourceType);
                }
            }
        }

        internal static Config LoadOnConfigFile()
        {
            if (Sandbox) return null;
            var c = new Config();
            LoadOnSource(c, ArgumentSourceType.ConfigFile);
            return c;
        }

        internal static Config LoadOnStartupArgs()
        {
            if (Sandbox) return null;
            var c = new Config();
            LoadOnSource(c, ArgumentSourceType.StartupArgs);
            return c;
        }

        private static void LoadOnSource(Config instance, ArgumentSourceType argumentSourceType)
        {
            if (Sandbox) return;

            var arguments = ArgumentHelper.GetArguments().Where(a => a.Source == argumentSourceType).ToArray();

            var props = instance.GetType().GetProperties().Where(p => p.GetCustomAttribute<ArgumentAttribute>() != null);

            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute<ArgumentAttribute>();

                var argument = arguments.FirstOrDefault(v => attr.Key.Equals(v.Key, StringComparison.OrdinalIgnoreCase));
                if (argument == null)
                    continue;

                var val = argument.ActualValue;
                if (val != null)
                {
                    if (!prop.PropertyType.IsEnum)
                    {
                        if (Utils.IsWritable(prop.PropertyType))
                        {
                            val = Convert.ChangeType(val, prop.PropertyType);
                        }
                        else
                        {
                            //TODO [100] 08.06.2017 - object tipindeki nesneleri yüklemek için geliþtirilmeli, belki de hiç kullanýlmayacak olabilir. (JSON deserialize olabilir)
                        }
                    }
                    else
                    {
                        val = Utils.TryConvertEnum(val, prop.PropertyType);
                    }
                }
                    

                prop.SetValue(instance, val);
            }
        }

        #region - DEFAULTS -

        public class Defaults
        {
            //public static readonly string ConfigurationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM Akademi", "StartupConfig.json");
            /// <summary>
            /// %LOCALAPPDATA%\CRM Akademi\StartupConfig.json
            /// </summary>
            private static string _configurationPath;
            public static string ConfigurationPath
            {
                get
                {
                    if (!Sandbox && string.IsNullOrWhiteSpace(_configurationPath))
                    {
                        _configurationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Globals.CompanyName, "StartupConfig.json");
                    }
                    return _configurationPath;
                }
            }

            /// <summary>
            /// StorageTargetJsonConverter, MssqlDeliveryPolicyJsonConverter, MssqlRetrievePolicyJsonConverter
            /// </summary>
            public static readonly StartupConfigReadSettings StartupConfigReadSettings = new StartupConfigReadSettings
            {
                Converters = new List<JsonConverter>
                {
                    new StorageTargetJsonConverter(),
                    new MssqlDeliveryPolicyJsonConverter(),
                    new MssqlRetrievePolicyJsonConverter()
                }
            };

            /// <summary>
            /// DEFAULT
            /// </summary>
            public static readonly string Key = "DEFAULT";
        }

        #endregion - DEFAULTS -
    }
}