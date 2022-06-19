using System.Collections.Generic;
using Newtonsoft.Json;

namespace XrmEarth.Configuration.Data.Core
{
    /// <summary>
    /// Ba�lang�� konfig�rasyonunun okunma ayarlar�.
    /// <para></para>
    /// Yeni eklenecek hedef <c>(StorageTarget)</c> t�rleri i�in buraya JsonConverter eklenmesi gerekebilir. 
    /// </summary>
    public class StartupConfigReadSettings
    {
        public List<JsonConverter> Converters { get; set; }
    }
}