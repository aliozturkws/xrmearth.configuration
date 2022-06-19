using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using XrmEarth.Core.Configuration.Data.Core;
using XrmEarth.Core.Utility;

namespace XrmEarth.Core.Configuration.Data.Connection
{
    /// <summary>
    /// Mssql Bağlantısı
    /// </summary>
    [Serializable]
    public class MssqlConnection : IConnection
    {
        /// <summary>
        /// Sunucu
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// Veritabanı
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Güvenilir bağlantı
        /// </summary>
        public bool TrustedConnection { get; set; }

        /// <summary>
        /// Kullanıcı Adı
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Parola
        /// <para></para>
        /// <code>*Şifre bilgisi kodlanarak saklanır. Xml ve JSON serialize destekler.</code>
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string Password { get; set; }

        public string CryptedPassword
        {
            get { return CryptHelper.Crypt(Password); }
            set { Password = CryptHelper.Decrypt(value); }
        }

        /// <summary>
        /// Bağlantı cümlesi oluşturur.
        /// </summary>
        /// <returns>Bağlantı cümlesi</returns>
        public string CreateConnectionString()
        {
            if (TrustedConnection)
            {
                return string.Format("Server={0};Database={1};Trusted_Connection=SSPI;",
                    Server,
                    Database);
            }

            return string.Format("Server={0};Database={1};User Id={2};Password={3};",
                Server,
                Database,
                Username,
                Password);
        }
    }
}
