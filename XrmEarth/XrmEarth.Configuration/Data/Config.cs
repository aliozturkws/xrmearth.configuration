using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using XrmEarth.Configuration.Attributes;
using XrmEarth.Configuration.Common;
using XrmEarth.Configuration.Data.Core;
using XrmEarth.Configuration.Utility;
using XrmEarth.Core;
using XrmEarth.Core.Configuration.Common;
using Formatting = Newtonsoft.Json.Formatting;

namespace XrmEarth.Configuration.Data
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
        public string Key { get; set; }

        #endregion - CONFIG -
    }
}