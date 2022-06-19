using System;
using System.Collections.Generic;
using XrmEarth.Core.Configuration.Data.Connection;
using XrmEarth.Core.Configuration.Data.Core;

namespace XrmEarth.Core.Configuration.Target.Mssql.Base
{
    /// <summary>
    /// Yazma (saklama) politikası.
    /// </summary>
    [Serializable]
    public abstract class MssqlDeliveryPolicy
    {
        protected MssqlDeliveryPolicy()
        {
            Type = GetType().FullName;

        }

        /// <summary>
        /// Serialize işlemleri için sınıfa ait tip bilgisini saklar.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// SQL'e verileri yazar.
        /// </summary>
        /// <param name="connection">Bağlantı bilgisi</param>
        /// <param name="keyAndValues">Anatar ve değerini içeren Dictionary, bu verilerin SQL'e yazılması beklenir.</param>
        public abstract void Deliver(MssqlConnection connection, Dictionary<string, ValueContainer> keyAndValues);
    }
}
