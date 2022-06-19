using XrmEarth.Core.Configuration.Target.Mssql.Base;
using XrmEarth.Core.Log;

namespace XrmEarth.Core.Configuration
{
    /// <summary>
    /// ConfigurationManager kütüphanesi için kullanýlabilecek yan yöntemleri içerir.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// MSSQL üzerinde ayarlarýn saklanacaðý tabloyu oluþturacak sorguyu hazýrlar.
        /// </summary>
        /// <param name="tableName">Tablo adý.</param>
        /// <param name="schema">Þema, varsayýlan deðer 'dbo'.</param>
        /// <returns>Tablo sorgusu</returns>
        public static string CreateTableScriptOnMssql(string tableName, string schema = "dbo")
        {
            var query = MssqlStoragePolicy.CreateTableScript(tableName, schema);
            SimpleLog.Instance.Push(query, SimpleLogLevel.Request);

            return query;
        }
    }
}