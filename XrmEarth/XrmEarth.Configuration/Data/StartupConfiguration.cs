using System;

namespace XrmEarth.Configuration.Data
{
    /// <summary>
    /// Başlangıç konfigürasyonu. 
    /// <para></para>
    /// Başlangıç konfigürasyonu içerisinde kütüphanenin kullanacağı bağlantı (Target) bilgilerini saklar.
    /// <para></para>
    /// Bağlantı konfigürasyonlarını 'Key' özelliklerini kullanarak ortamlara ayırabilirsiniz. [DEVELOPMENT] - Bağlantılar, [LIVE] - Bağlantılar şeklinde.
    /// </summary>
    [Serializable]
    public class StartupConfiguration
    {
        /// <summary>
        /// Ortam tanımlayıcısı. ('DEVELOPMENT', 'LIVE' vb.)
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Bağlantılar.
        /// </summary>
        public TargetCollection Targets { get; set; }
    }
}
