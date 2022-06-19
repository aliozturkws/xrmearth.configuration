using System;
using System.Collections.Generic;
using XrmEarth.Core.Configuration.Data.Connection;
using XrmEarth.Core.Configuration.Data.Core;

namespace XrmEarth.Core.Configuration.Target.Mssql.Base
{
    /// <summary>
    /// Okuma politikası.
    /// </summary>
    [Serializable]
    public abstract class MssqlRetrievePolicy
    {
        protected MssqlRetrievePolicy()
        {
            Type = GetType().FullName;
        }

        /// <summary>
        /// Serialize işlemleri için sınıfa ait tip bilgisini saklar.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// SQL'den verileri getirir.
        /// </summary>
        /// <param name="connection">Bağlantı bilgisi.</param>
        /// <param name="keys">Yüklenecek anahtarların (key) listesi. Dictionary tipinde anatarın tipiyle verilmiştir.</param>
        /// <returns>Anatar ve değerini içeren Dictionary</returns>
        public abstract Dictionary<string, ValueContainer> Retrieve(MssqlConnection connection, Dictionary<string, Type> keys);
    }
}
