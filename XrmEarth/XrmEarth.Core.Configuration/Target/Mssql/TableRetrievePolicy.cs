using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XrmEarth.Core.Configuration.Data.Connection;
using XrmEarth.Core.Configuration.Data.Core;
using XrmEarth.Core.Configuration.Target.Mssql.Base;
using XrmEarth.Core.Log;
using XrmEarth.Core.Utility;

namespace XrmEarth.Core.Configuration.Target.Mssql
{
    /// <summary>
    /// Sorgular üzerinden okuma işlemi.
    /// <para></para>
    /// <code>*Verilerin yüklendiği sorgulara; BuildQuery (OwnTable:true) ve BuildQuery(keys) (OwnTable:false) metodlarından erişebilirsiniz.</code>
    /// </summary>
    [Serializable]
    public class TableRetrievePolicy : MssqlRetrievePolicy
    {
        /// <summary>
        /// Tablo adı.
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Tablo kullanımı sadece bu sınıfa veya tek nesne tipine ait mi? Eğer ortak kullanım için kullanılıyorsa, false atanması gerekir.
        /// <para></para>
        /// Eğer ortak kullanım içinse kullanılacak nesneye ait olmayan anahtarlarda (key) bulunabileceği için sorgu buna göre (kriterli) düzenlenir. Bu sınıfa aitse daha optimize (kritersiz) sorgu hazırlanır.
        /// </summary>
        public bool OwnTable { get; set; }

        public override Dictionary<string, ValueContainer> Retrieve(MssqlConnection connection, Dictionary<string, Type> keyAndTypes)
        {
            var conStr = connection.CreateConnectionString();
            SimpleLog.Instance.Push("Connection String : " + conStr, SimpleLogLevel.Debug);
            var keys = keyAndTypes.Keys;
            var query = OwnTable ? BuildQuery() : BuildQuery(keys);
            SimpleLog.Instance.Push("Query : " + query, SimpleLogLevel.Debug);
            var dtValues = SqlHelper.GetDataTable(conStr, query, CommandType.Text);
            SimpleLog.Instance.Push("Result Count : " + dtValues.Rows.Count, SimpleLogLevel.Debug);

            var result = new Dictionary<string, ValueContainer>();
            foreach (DataRow row in dtValues.Rows)
            {
                var keyName = row[MssqlStoragePolicy.KeyColumnName].ToString();

                if(keys.All(k => k != keyName))
                    continue;

                var val = row[MssqlStoragePolicy.ValueColumnName];
                if (val == DBNull.Value)
                    val = null;

                result.Add(keyName, new ValueContainer(val));
            }
            return result;
        }

        /// <summary>
        /// Ortak kullanım tablosu için sorgu oluşturur.
        /// </summary>
        /// <param name="keyNames">Yüklenecek anahtarlar.</param>
        /// <returns>Veri yükleme sorgusu</returns>
        public string BuildQuery(IEnumerable<string> keyNames)
        {
            var sb = new StringBuilder
                ("SELECT ").AppendLine().
                Append(MssqlStoragePolicy.KeyColumnName).AppendLine().
                Append(",").Append(MssqlStoragePolicy.ValueColumnName).AppendLine().
                Append("FROM ").Append(TableName).AppendLine().
                Append("WHERE ").AppendLine().
                Append(MssqlStoragePolicy.KeyColumnName).Append(" IN ('").Append(string.Join("', '", keyNames)).Append("')").AppendLine();

            return sb.ToString();
        }

        /// <summary>
        /// Sadece tek tipe ait tablolar için sorgu oluşturur.
        /// </summary>
        /// <returns>Veri yükleme sorgusu</returns>
        public string BuildQuery()
        {
            var sb = new StringBuilder
                ("SELECT ").AppendLine().
                Append(MssqlStoragePolicy.KeyColumnName).AppendLine().
                Append(",").Append(MssqlStoragePolicy.ValueColumnName).AppendLine().
                Append("FROM ").Append(TableName).AppendLine();

            return sb.ToString();
        }
    }
}
