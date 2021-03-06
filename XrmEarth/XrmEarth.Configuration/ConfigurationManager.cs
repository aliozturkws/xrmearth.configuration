using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using XrmEarth.Configuration.Attributes;
using XrmEarth.Configuration.Data;
using XrmEarth.Configuration.Data.Storage;
using XrmEarth.Configuration.Initializer.Core;
using XrmEarth.Configuration.Target;

namespace XrmEarth.Configuration
{
    /// <summary>
    /// Ayarları yükler ve kaydeder.
    /// </summary>
    public class ConfigurationManager
    {
        #region - SingleTon -
        private class Nested
        {
            internal static readonly ConfigurationManager Instance;

            static Nested()
            {
                Instance = new ConfigurationManager();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static ConfigurationManager Instance
        {
            get
            {
                return Nested.Instance;
            }
        }
        #endregion

        protected readonly Type BaseStorageAttributeType = typeof(StorageTargetAttribute);
        protected readonly Type BaseFieldAttributeType = typeof(StorageAttribute);

        protected readonly Dictionary<Type, StorageObjectContainer> ContainerCaches = new Dictionary<Type, StorageObjectContainer>();

        private Config _config;
        public Config Config
        {
            get
            {
                if (_config == null) InitConfig();

                return _config;
            }
        }

        public void InitConfig(Config overrideConfig = null)
        {
            _config = new Config();

            //_config.InvalidateAssemblies();
        }

        public void SaveSettings<T>(T instance, string key = null)
        {
            var storageInitializer = CreateInitializer<T>(key);
            storageInitializer.Save(instance);
        }
        public void SaveSettings<T>(T instance, StartupConfiguration startupConfiguration)
        {
            var storageInitializer = CreateInitializer<T>(startupConfiguration);
            storageInitializer.Save(instance);
        }

        public void SaveObjectSettings(object instance, string key = null)
        {
            var storageInitializer = CreateInitializer<object>(key, instance.GetType());
            storageInitializer.Save(instance);
        }
        public void SaveObjectSettings(object instance, StartupConfiguration startupConfiguration)
        {
            var storageInitializer = CreateInitializer<object>(startupConfiguration, instance.GetType());
            storageInitializer.Save(instance);
        }

        public T LoadSettings<T>(string key = null)
        {
            var storageInitializer = CreateInitializer<T>(key);
            return storageInitializer.Load();
        }
        public void LoadSettings<T>(T instance, string key = null)
        {
            var storageInitializer = CreateInitializer<T>(key);
            storageInitializer.Load(instance);
        }
        public T LoadSettings<T>(StartupConfiguration startupConfiguration)
        {
            var storageInitializer = CreateInitializer<T>(startupConfiguration);
            return storageInitializer.Load();
        }
        public void LoadSettings<T>(T instance, StartupConfiguration startupConfiguration)
        {
            var storageInitializer = CreateInitializer<T>(startupConfiguration);
            storageInitializer.Load(instance);
        }

        public void LoadObjectSettings(object instance, string key = null)
        {
            var storageInitializer = CreateInitializer<object>(key, instance.GetType());
            storageInitializer.Load(instance);
        }
        public void LoadObjectSettings(object instance, StartupConfiguration startupConfiguration)
        {
            var storageInitializer = CreateInitializer<object>(startupConfiguration, instance.GetType());
            storageInitializer.Load(instance);
        }

        public ConfigurationManager CreateConfigurationManager()
        {
            return new ConfigurationManager();
        }

        #region - WORKERS -

        public BaseInitializer<T> CreateInitializer<T>(string key = null, Type type = null)
        {
            type = type ?? typeof(T);

            //var startupConfiguration = StartupConfigInitializer.LoadConfiguration(key);

            //return CreateInitializer<T>(null, type);
            return null;
        }

        public BaseInitializer<T> CreateInitializer<T>(StartupConfiguration startupConfiguration, Type type = null)
        {
            type = type ?? typeof(T);
            var objectContainer = CreateContainer(type);

            //TODO [5] 🗸 Varsayılan hedefi seçme durumu test edilmeli.
            StorageTarget target;
            if (startupConfiguration.Targets.Count == 1)
            {
                target = startupConfiguration.Targets.First();
                objectContainer.TargetType = target.GetType();
            }
            else
            {
                if (objectContainer.TargetType == null)
                    throw new Exception(string.Format("'{0}' tipi '{1}' özelliklerinden birini içermelidir. Sınıf için birden fazla hedef bulunduğu için tip belirtilmeli.", type.FullName, BaseStorageAttributeType));

                target = startupConfiguration.Targets.FirstOrDefault(t => t.GetType() == objectContainer.TargetType);
                if (target == null)
                    throw new Exception($"'{type.FullName}' tipi için kullanılan '{objectContainer.TargetType}' hedef için bağlantı bilgileri ayar dosyasında bulunamadı. Mevcut hedefler{string.Join(", ", startupConfiguration.Targets.Select(sc => string.Concat("'", sc.GetType(), "'")))}");
            }

            var storageInitializer = target.CreateInitializer<T>(new StoragePolicy(), objectContainer);
            if (storageInitializer == null)
                throw new Exception($"'{objectContainer.TargetType.Name}' hedefi için konteyner oluşturulamadı.");

            return storageInitializer;
        }

        public StorageObjectContainer CreateContainer(Type type, StoragePolicy policy = null, bool ignoreCache = false)
        {
            if (type == null)
                throw new NullReferenceException("'type' null olamaz.");

            if (ContainerCaches.ContainsKey(type) && !ignoreCache)
                return ContainerCaches[type];

            if (policy == null)
                policy = new StoragePolicy();

            var stAttr = type.GetCustomAttributes(BaseStorageAttributeType, false).FirstOrDefault() as StorageTargetAttribute;


            var stObjCont = new StorageObjectContainer
            {
                OwnerType = type,
                TargetType = stAttr?.TargetType,
                Fields = new HashSet<StorageFieldContainer>(),
                Policy = new StorageObjectPolicy()
            };

            stObjCont.Initialize();

            ContainerCaches[type] = stObjCont;
            return stObjCont;
        }

        #endregion - WORKERS - 

        #region - STATIC -
        /// <summary>
        /// Ayarları yapılandırır.
        /// <para></para>
        /// Önceliklendirme sırasıyla aşağıdaki gibidir. 
        /// <para></para>
        /// <list type="bullet">
        ///     <item>
        ///         <description><c>Varsayılan</c> - Kütüphanenin varsayılan değerleri.<para></para></description>
        ///     </item>
        ///     <item>
        ///         <description><c>Parametre</c> - Bu metoda gönderilen config nesnesi.<para></para></description>
        ///     </item>
        ///     <item>
        ///         <description><c>Yapılandırma Dosyası</c> - Uygulama ayarlarının yüklendiği dosya (App.config vb.).<para></para></description>
        ///     </item>
        ///     <item>
        ///         <description><c>Komut Satırı</c> - Komut satırından gönderilen parametreler.<para></para></description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="config">Verilen değerlere göre mevcut değerleri güncellenir. Atanmayan değerler için varsayılan değerler kullanılır.</param>
        public static void Init(Config config = null)
        {
            Instance.InitConfig(config);
        }

        /// <summary>
        /// Verilen ayar nesnesi belirlenmiş konfigürasyonlara göre hedef sisteme kaydeder.
        /// </summary>
        /// <typeparam name="T">Ayar nesnesin tipi</typeparam>
        /// <param name="instance">Kaydedilecek ayar nesnesi.</param>
        /// <param name="startupConfiguration">Hedef sisteme ait konfigürasyon. Boş geçilmesi durumunda Init olmuş anahtara göre yüklenen konfigürasyon üzerinden işlem yapılır.</param>
        public static void Save<T>(T instance, StartupConfiguration startupConfiguration)
        {
            Instance.SaveSettings(instance, startupConfiguration);
        }
        /// <summary>
        /// Verilen ayar nesnesi belirlenmiş konfigürasyonlara göre hedef sisteme kaydeder.
        /// </summary>
        /// <typeparam name="T">Ayar nesnesin tipi</typeparam>
        /// <param name="instance">Kaydedilecek ayar nesnesi.</param>
        /// <param name="key">Konfigürasyon içinden özel bir anahtara göre işlem yapmak için kullanılabilir. Boş geçilmesi durumunda Init olmuş anahtara göre işlem yapar (<c>bkz. ConfigurationManager.Init(Config)</c>).</param>
        public static void Save<T>(T instance, string key = null)
        {
            Instance.SaveSettings(instance, key);
        }

        /// <summary>
        /// Verilen ayar nesnesi belirlenmiş konfigürasyonlara göre hedef sisteme kaydeder.
        /// <para></para>
        /// <c>ConfigurationManager.Save&lt;T&gt;()</c> metodundan farklı olarak tipi bilinmeyen nesneler için kullanılmalıdır. Eğer <c>ConfigurationManager.Save&lt;object&gt;()</c> olarak kullanılırsa, verilen sınıf <c>object</c> gibi yazılacak ve hiçbir özelliği saklanmayacaktır.
        /// </summary>
        /// <param name="instance">Kaydedilecek ayar nesnesi.</param>
        /// <param name="key">Konfigürasyon içinden özel bir anahtara göre işlem yapmak için kullanılabilir. Boş geçilmesi durumunda Init olmuş anahtara göre işlem yapar (<c>bkz. ConfigurationManager.Init(Config)</c>).</param>
        public static void SaveObject(object instance, string key = null)
        {
            Instance.SaveObjectSettings(instance, key);
        }
        /// <summary>
        /// Verilen ayar nesnesi belirlenmiş konfigürasyonlara göre hedef sisteme kaydeder.
        /// <para></para>
        /// <c>ConfigurationManager.Save&lt;T&gt;()</c> metodundan farklı olarak tipi bilinmeyen nesneler için kullanılmalıdır. Eğer <c>ConfigurationManager.Save&lt;object&gt;()</c> olarak kullanılırsa, verilen sınıf <c>object</c> gibi yazılacak ve hiçbir özelliği saklanmayacaktır.
        /// </summary>
        /// <param name="instance">Kaydedilecek ayar nesnesi.</param>
        /// /// <param name="startupConfiguration">Hedef sisteme ait konfigürasyon. Boş geçilmesi durumunda Init olmuş anahtara göre yüklenen konfigürasyon üzerinden işlem yapılır.</param>
        public static void SaveObject(object instance, StartupConfiguration startupConfiguration)
        {
            Instance.SaveObjectSettings(instance, startupConfiguration);
        }

        /// <summary>
        /// Belirlenmiş konfigürasyonlara göre hedef sistemden ayar nesnesini yükler ve oluşturur.
        /// </summary>
        /// <typeparam name="T">Ayar nesnesin tipi</typeparam>
        /// <param name="key">Konfigürasyon içinden özel bir anahtara göre işlem yapmak için kullanılabilir. Boş geçilmesi durumunda Init olmuş anahtara göre işlem yapar (<c>bkz. ConfigurationManager.Init(Config)</c>).</param>
        /// <returns>Yüklenmiş ayar nesnesi</returns>
        public static T Load<T>(string key = null)
        {
            return Instance.LoadSettings<T>(key);
        }
        /// <summary>
        /// Belirlenmiş konfigürasyonlara göre hedef sistemden ayar nesnesini yükler.
        /// </summary>
        /// <typeparam name="T">Ayar nesnesin tipi</typeparam>
        /// <param name="instance">Ayarların yükleneceği nesne</param>
        /// <param name="key">Konfigürasyon içinden özel bir anahtara göre işlem yapmak için kullanılabilir. Boş geçilmesi durumunda Init olmuş anahtara göre işlem yapar (<c>bkz. ConfigurationManager.Init(Config)</c>).</param>
        public static void Load<T>(T instance, string key = null)
        {
            Instance.LoadSettings(instance, key);
        }
        /// <summary>
        /// Belirlenmiş konfigürasyonlara göre hedef sistemden ayar nesnesini yükler ve oluşturur.
        /// </summary>
        /// <typeparam name="T">Ayar nesnesin tipi</typeparam>
        /// <param name="startupConfiguration">Hedef sisteme ait konfigürasyon. Boş geçilmesi durumunda Init olmuş anahtara göre yüklenen konfigürasyon üzerinden işlem yapılır.</param>
        /// <returns>Yüklenmiş ayar nesnesi</returns>
        public static T Load<T>(StartupConfiguration startupConfiguration)
        {
            return Instance.LoadSettings<T>(startupConfiguration);
        }
        /// <summary>
        /// Belirlenmiş konfigürasyonlara göre hedef sistemden ayar nesnesini yükler.
        /// </summary>
        /// <typeparam name="T">Ayar nesnesin tipi</typeparam>
        /// <param name="instance">Ayarların yükleneceği nesne</param>
        /// <param name="startupConfiguration">Hedef sisteme ait konfigürasyon. Boş geçilmesi durumunda Init olmuş anahtara göre yüklenen konfigürasyon üzerinden işlem yapılır.</param>
        /// <returns>Yüklenmiş ayar nesnesi</returns>
        public static void Load<T>(T instance, StartupConfiguration startupConfiguration)
        {
            Instance.LoadSettings(instance, startupConfiguration);
        }

        /// <summary>
        /// Belirlenmiş konfigürasyonlara göre hedef sistemden ayar nesnesini yükler.
        /// <para></para>
        /// <c>ConfigurationManager.Load&lt;T&gt;()</c> metodundan farklı olarak tipi bilinmeyen nesneler için kullanılmalıdır. Eğer <c>ConfigurationManager.Load&lt;object&gt;()</c> olarak kullanılırsa, verilen sınıf <c>object</c> gibi okunacak ve hiçbir özelliği atanmayacaktır.
        /// <para></para>
        /// <para></para>
        /// Not: Verilen ayar nesnesi tipi bilinmese de orjinal ayar nesnesi olmalıdır. Ayarları yüklemek için nesnenin tipi <c>Object.GetType()</c> metodu ile yüklenir.
        /// </summary>
        /// <param name="instance">Ayarların yükleneceği nesne</param>
        /// <param name="key">Konfigürasyon içinden özel bir anahtara göre işlem yapmak için kullanılabilir. Boş geçilmesi durumunda Init olmuş anahtara göre işlem yapar (<c>bkz. ConfigurationManager.Init(Config)</c>).</param>
        public static void LoadObject(object instance, string key = null)
        {
            Instance.LoadObjectSettings(instance, key);
        }
        /// <summary>
        /// Belirlenmiş konfigürasyonlara göre hedef sistemden ayar nesnesini yükler.
        /// <para></para>
        /// <c>ConfigurationManager.Load&lt;T&gt;()</c> metodundan farklı olarak tipi bilinmeyen nesneler için kullanılmalıdır. Eğer <c>ConfigurationManager.Load&lt;object&gt;()</c> olarak kullanılırsa, verilen sınıf <c>object</c> gibi okunacak ve hiçbir özelliği atanmayacaktır.
        /// <para></para>
        /// <para></para>
        /// Not: Verilen ayar nesnesi tipi bilinmese de orjinal ayar nesnesi olmalıdır. Ayarları yüklemek için nesnenin tipi <c>Object.GetType()</c> metodu ile yüklenir.
        /// </summary>
        /// <param name="instance">Ayarların yükleneceği nesne</param>
        /// <param name="startupConfiguration">Hedef sisteme ait konfigürasyon. Boş geçilmesi durumunda Init olmuş anahtara göre yüklenen konfigürasyon üzerinden işlem yapılır.</param>
        public static void LoadObject(object instance, StartupConfiguration startupConfiguration)
        {
            Instance.LoadObjectSettings(instance, startupConfiguration);
        }

        #endregion - STATIC -
    }
}
