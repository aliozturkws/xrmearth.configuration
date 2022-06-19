using System;
using XrmEarth.Core.Configuration.Data.Connection;
using XrmEarth.Core.Configuration.Data.Storage;
using XrmEarth.Core.Configuration.Initializer;
using XrmEarth.Core.Configuration.Initializer.Core;
using XrmEarth.Core.Configuration.Target.Mssql.Base;

namespace XrmEarth.Core.Configuration.Target
{
    /// <summary>
    /// Mssql Server hedefi.
    /// <para></para>
    /// <para></para>
    /// Tablo Bilgisi
    /// <para></para>
    /// 1. KeyName - nvarchar(150)
    /// <para></para>
    /// 2. Value - nvarchar(4000)
    /// <para></para>
    /// Verilerin saklanacağı tablo yukarıdaki kolonları içermelidir, bunların dışında kolonlar olması kütüphanenin çalışmasını etkilemez.
    /// <para></para>
    /// <code>*Tablo ismi veri okuma ve yazma politikalarında belirlenir.</code>
    /// <para></para>
    /// <code>*Tablo oluşturma sorgusuna MssqlStoragePolicy.CreateTableScript üzerinden erişebilirsiniz.</code>
    /// </summary>
    [Serializable]
    public class MssqlStorageTarget : StorageTarget
    {
        /// <summary>
        /// Veri saklama politikası. 
        /// </summary>
        public MssqlStoragePolicy Policy { get; set; }

        /// <summary>
        /// Bağlantı bilgisi
        /// </summary>
        public MssqlConnection Connection { get; set; }

        public override BaseInitializer<T> CreateInitializer<T>(StoragePolicy storagePolicy, StorageObjectContainer objectContainer)
        {
            return new MssqlStorageInitializer<T>(this, storagePolicy, objectContainer);
        }
    }
}
