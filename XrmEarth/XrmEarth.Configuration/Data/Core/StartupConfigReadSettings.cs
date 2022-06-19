using System.Collections.Generic;
using Newtonsoft.Json;

namespace XrmEarth.Configuration.Data.Core
{
    /// <summary>
    /// Baþlangýç konfigürasyonunun okunma ayarlarý.
    /// <para></para>
    /// Yeni eklenecek hedef <c>(StorageTarget)</c> türleri için buraya JsonConverter eklenmesi gerekebilir. 
    /// </summary>
    public class StartupConfigReadSettings
    {
        public List<JsonConverter> Converters { get; set; }
    }
}