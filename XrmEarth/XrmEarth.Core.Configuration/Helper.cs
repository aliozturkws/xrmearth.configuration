using XrmEarth.Core.Configuration.Target.Mssql.Base;
using XrmEarth.Core.Log;

namespace XrmEarth.Core.Configuration
{
    /// <summary>
    /// ConfigurationManager k�t�phanesi i�in kullan�labilecek yan y�ntemleri i�erir.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// MSSQL �zerinde ayarlar�n saklanaca�� tabloyu olu�turacak sorguyu haz�rlar.
        /// </summary>
        /// <param name="tableName">Tablo ad�.</param>
        /// <param name="schema">�ema, varsay�lan de�er 'dbo'.</param>
        /// <returns>Tablo sorgusu</returns>
        public static string CreateTableScriptOnMssql(string tableName, string schema = "dbo")
        {
            var query = MssqlStoragePolicy.CreateTableScript(tableName, schema);
            SimpleLog.Instance.Push(query, SimpleLogLevel.Request);

            return query;
        }
    }
}