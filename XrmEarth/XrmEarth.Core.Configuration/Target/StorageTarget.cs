using System;
using XrmEarth.Core.Configuration.Data.Core;
using XrmEarth.Core.Configuration.Data.Storage;
using XrmEarth.Core.Configuration.Initializer.Core;

namespace XrmEarth.Core.Configuration.Target
{
    /// <summary>
    /// Hedef ortamla ilgili bütün bilgileri saklar. Genel olarak bağlantı bilgilerini saklamak için kullanılır ama gerekirse oturum vb. bilgileride içerebilir.
    /// <para></para> 
    /// </summary>
    [Serializable]
    public abstract class StorageTarget : IStorageTarget
    {
        protected StorageTarget()
        {
            Type = GetType().FullName;
        }

        /// <summary>
        /// Serialize işlemleri için sınıfa ait tip bilgisini saklar.
        /// </summary>
        public string Type { get; private set; }

        //TODO [5] 12.06.2017 - Kaldırılması herhangi bir sorun çıkarmıyorsa komple silinmeli.
        //public string Key { get; set; }

        /// <summary>
        /// Ortam için Initializer oluşturan metod.
        /// <para></para>
        /// Kütüphane üzerine yeni ortamlar (olabildiğince kısıtsız) eklenebilmesi için her ortam kendi Initializer'ını oluşturur. 
        /// </summary>
        /// <typeparam name="T">Initializer'ın çalışacağı nesne</typeparam>
        /// <param name="storagePolicy">Policy bilgisi.</param>
        /// <param name="objectContainer">Çalışılacak nesne için oluşturulmuş konteyner, Initializer'ın kullancığı nesneye ait bütün bilgiler burada saklanır.</param>
        /// <returns></returns>
        public abstract BaseInitializer<T> CreateInitializer<T>(StoragePolicy storagePolicy, StorageObjectContainer objectContainer);
    }
}
