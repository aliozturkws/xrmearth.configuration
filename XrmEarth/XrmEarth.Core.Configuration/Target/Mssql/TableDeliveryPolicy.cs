using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XrmEarth.Core.Configuration.Data.Connection;
using XrmEarth.Core.Configuration.Data.Core;
using XrmEarth.Core.Configuration.Data.Exceptions;
using XrmEarth.Core.Configuration.Target.Mssql.Base;
using XrmEarth.Core.Log;
using XrmEarth.Core.Utility;

namespace XrmEarth.Core.Configuration.Target.Mssql
{
    /// <summary>
    /// Sorgular üzerinden yazma işlemi.
    /// <para></para>
    /// <code>*Belirtilen anahtar yoksa ekler mevcutsa güncelleme işlemi yapar.</code>
    /// <para></para>
    /// <code>*Sınıfın kullandığı sorgulara; BuildQuery, BuildUpdateQuery ve BuildInsertQuery metodları üzerinden erişebilirsiniz.</code>
    /// <para></para>
    /// <code>*Sorgu Transaction için çalıştırılır, herhangi bir anahtarda hata alınırsa bütün işlem geri alınır.</code>
    /// </summary>
    [Serializable]
    public class TableDeliveryPolicy : MssqlDeliveryPolicy
    {
        /// <summary>
        /// Tablo adı.
        /// </summary>
        public string TableName { get; set; }

        public override void Deliver(MssqlConnection connection, Dictionary<string, ValueContainer> keyAndValues)
        {
            if(keyAndValues == null)
                throw new NullReferenceException("CrmAkademi.Configuration.Initializer.Mssql.TableDeliveryPolicy.Deliver > keyAndValues");

            if(keyAndValues.Count == 0)
                return;

            var conStr = connection.CreateConnectionString();
            SimpleLog.Instance.Push("Connection String : " + conStr, SimpleLogLevel.Debug);
            var query = BuildQuery(keyAndValues);
            SimpleLog.Instance.Push("Query : " + query, SimpleLogLevel.Debug);
            var result = SqlHelper.ExecuteNonQuery(conStr, query, CommandType.Text);
            SimpleLog.Instance.Push("Execution Result : " + result, SimpleLogLevel.Debug);

            if (result == 0)
                throw new MssqlStorageException(ConfigurationCoreException.InitializerMssqlNotChangeException);
        }

        /// <summary>
        /// Yazma sorgusunu oluşturur.
        /// <para></para>
        /// <code>*İşleme göre (ekleme ve güncelleme) BuildUpdateQuery ve BuildInsertQuery metodlarını kullanır.</code>
        /// <para></para>
        /// <code>*Sorgu Transaction için çalıştırılır, herhangi bir anahtarda hata alınırsa bütün işlem geri alınır.</code>
        /// <para></para>
        /// <code>*Mevcut anahtar kontrolü (=) eşittir operatörüyle gerçekleştirilir, büyük, küçük harf duyarlılığı veya herhangi bir konfigürasyon içermez. Örn: [KeyColumnName] = 'Key'</code>
        /// </summary>
        /// <param name="keyAndValues">SQL'e yazılacak anahtarları ve değerlerini içeren Dictionary.</param>
        /// <returns>İşlem sorgusu.</returns>
        public string BuildQuery(Dictionary<string, ValueContainer> keyAndValues)
        {
            var sb = new StringBuilder
                ("BEGIN TRANSACTION").AppendLine().
                Append("DECLARE @IsExist INT = 0").AppendLine().AppendLine();

            foreach (var keyValuePair in keyAndValues)
            {
                sb.Append("SET @IsExist = (SELECT COUNT(0) FROM ").Append(TableName).Append(" WHERE ").Append(MssqlStoragePolicy.KeyColumnName).Append(" = '").Append(keyValuePair.Key).Append("')").AppendLine().
                Append("IF @IsExist = 0").AppendLine().
                Append("BEGIN").AppendLine().
                Append(BuildInsertQuery(keyValuePair.Key, keyValuePair.Value.Value)).AppendLine().
                Append("END ELSE").AppendLine().
                Append(BuildUpdateQuery(keyValuePair.Key, keyValuePair.Value.Value)).AppendLine().AppendLine();
            }
            sb.Append
                ("COMMIT TRANSACTION");

            return sb.ToString();
        }

        /// <summary>
        /// Güncelleme sorgusunu oluşturur.
        /// </summary>
        /// <param name="keyName">Anahtar.</param>
        /// <param name="value">Güncellenecek değer.</param>
        /// <returns>Güncelleme sorgusu</returns>
        public string BuildUpdateQuery(string keyName, object value)
        {
            var sb = new StringBuilder
                ("UPDATE ").Append(TableName).AppendLine().
                Append("SET ").Append(MssqlStoragePolicy.ValueColumnName).Append(" = '").Append(value).Append("'").AppendLine().
                Append("WHERE ").AppendLine().
                Append(MssqlStoragePolicy.KeyColumnName).Append(" = '").Append(keyName).Append("'").AppendLine();

            return sb.ToString();
        }

        /// <summary>
        /// Ekleme sorgusunu oluşturur.
        /// </summary>
        /// <param name="keyName">Eklenecek anahtar.</param>
        /// <param name="value">Değer.</param>
        /// <returns>Ekleme sorgusu</returns>
        public string BuildInsertQuery(string keyName, object value)
        {
            var sb = new StringBuilder
                ("INSERT INTO ").Append(TableName).AppendLine().
                Append("(").Append(MssqlStoragePolicy.KeyColumnName).AppendLine().
                Append(",").Append(MssqlStoragePolicy.ValueColumnName).Append(")").AppendLine().
                Append("VALUES").AppendLine().
                Append("('").Append(keyName).Append("'").AppendLine().
                Append(", '").Append(value).Append("')").AppendLine();

            return sb.ToString();
        }
    }
}
