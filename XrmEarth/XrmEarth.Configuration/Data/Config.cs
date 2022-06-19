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
        public string Key { get; set; }

        #endregion - CONFIG -
    }
}