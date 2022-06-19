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
    /// ConfigurationManager s�n�f�n�n de�i�kenlerini saklar.<para></para>
    /// </summary>
    public class Config
    {
        public static readonly HashSet<Assembly> RegisteredAssemblies = new HashSet<Assembly>();
        
        /// <summary>
        /// Sandbox'a uyarl� �al���p �al��mayaca��n� belirtir.
        /// <para></para>
        /// K�t�phane tip arama i�lemlerinde ba�l� assembly lere eri�meye �al��t���ndan dolay� g�venlik ile ilgili hatalar alman�za neden olabilir. Bu �zelli�i 'True' set ederek bu gibi g�venlik sorunu ��karacak i�lemler ger�ekle�tirilmez, fakat bu seferde baz� i�lemler ger�ekle�tirilemeyebilir.
        /// <para></para>
        /// <c>Not</c> Ayar dosyas� ve komut sat�r�ndan y�klenmeyi destekler.
        /// <para></para>
        /// <list type="bullet">
        ///     <item><description>Kaynak - Parametre Ad�<para></para></description></item>
        ///     <item><description>Ayar dosyas� - 'Configuration:Sandbox'<para></para></description></item>
        ///     <item><description>Komut sat�r� - '-Configuration:Sandbox'<para></para></description></item>
        /// </list>
        /// </summary>
        public static bool Sandbox { get; set; }

        private readonly Dictionary<string, Tuple<ConfigSourceType, LazyValueLoader>> _values = new Dictionary<string, Tuple<ConfigSourceType, LazyValueLoader>>();

        #region - CONFIG -
        /// <summary>
        /// Uygulama bilgilerinin �zetini saklar.
        /// </summary>
        [Obsolete("Sandbox i�in devre d��� b�rak�ld�.")]
        public ApplicationSummary App { get; set; }

        /// <summary>
        /// Ba�lang�� konfig�rasyonunun okunma ayarlar�.
        /// </summary>
        public StartupConfigReadSettings ConfigReadSettings { get; set; }

        /// <summary>
        /// Ba�lang�� konfig�rasyonunun okunaca�� dosyan�n yolu. (�rn: C:\Users\admin\Appdata\CrmAkademi\StartupConfig.json)
        /// <para></para>
        /// <c>Not</c> Ayar dosyas� ve komut sat�r�ndan y�klenmeyi destekler.
        /// <para></para>
        /// <list type="bullet">
        ///     <item><description>Kaynak - Parametre Ad�<para></para></description></item>
        ///     <item><description>Ayar dosyas� - 'Configuration:Path'<para></para></description></item>
        ///     <item><description>Komut sat�r� - '-Configuration:Path'<para></para></description></item>
        /// </list>
        /// </summary>
        [Argument("Configuration:Path", Source = ArgumentSourceType.AllPlatform)]
        public string ConfigurationPath { get; set; }
        /// <summary>
        /// Ba�lang�� konfig�rasyonunun anahtar. (�rn: DEFAULT)
        /// <para></para>
        /// <c>Not</c> Ayar dosyas� ve komut sat�r�ndan y�klenmeyi destekler.
        /// <para></para>
        /// <list type="bullet">
        ///     <item><description>Kaynak - Parametre Ad�<para></para></description></item>
        ///     <item><description>Ayar dosyas� - 'Configuration:Key'<para></para></description></item>
        ///     <item><description>Komut sat�r� - '-Configuration:Key'<para></para></description></item>
        /// </list>
        /// </summary>
        [Argument("Configuration:Key", Source = ArgumentSourceType.AllPlatform)]
        public string Key { get; set; }

        /// <summary>
        /// Yap�lan geli�tirmelerin yer ald��� Assembly buraya eklenerek tip arama i�lemlerinde hem g�venlik hemde performans art��� sa�lanabilir.
        /// <para></para>
        /// <c>Not</c> �zellikle 'Sandbox' �zelli�i 'True' oldu�u durumlarda tip arama i�lemleri i�in gerekli olabilir.
        /// <para></para>
        /// <c>Not</c> ILMerge ile k�t�phane birle�tirme i�lemi yap�ld�ysa, varolan b�t�n tiplere bu ortak k�t�phaneden eri�ilebilir oldu�u i�in Assembly eklemenize gerek yoktur.
        /// </summary>
        public List<Assembly> ReferencedAssemblies { get; set; }
        #endregion - CONFIG -

        /// <summary>
        /// De�i�kenleri, kaynaklar�n� ve de�erlerini yazd�r�r.
        /// <para></para>
        /// Kaynaklar;
        /// <para></para>
        /// <list type="bullet">
        /// <item><description>Varsay�lan<para></para></description></item>
        /// <item><description>Nesne<para></para></description></item>
        /// <item><description>Ayar dosyas�<para></para></description></item>
        /// <item><description>Komut sat�r�<para></para></description></item>
        /// </list>
        /// </summary>
        /// <returns>{De�i�ken Ad�} - {Kaynak} - {De�er}<para>- - -</para></returns>
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
        /// ReferencedAssemblies de�i�kenine eklenmi� Assembly leri i�ler. Bu i�lem yap�lmadan Assembly'ler �zerinde herhangi bir i�lem yap�lmaz.
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
                            //TODO [100] 08.06.2017 - object tipindeki nesneleri y�klemek i�in geli�tirilmeli, belki de hi� kullan�lmayacak olabilir. (JSON deserialize olabilir)
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