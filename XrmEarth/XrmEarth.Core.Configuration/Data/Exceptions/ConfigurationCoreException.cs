using System;

namespace XrmEarth.Core.Configuration.Data.Exceptions
{
    [Serializable]
    public class ConfigurationCoreException : Exception
    {
        internal const string DefaultKeyMessage = "Varsayılan yükleme işlemi kullanılırken (ConfigurationManager.Load<T>()) ayar dosyasında (app.config veya web.config) veya uygulama başlatılırken parametre olarak belirtilmesi gerekmektedir. Parametre ve yapılandırma ayarları için uygulamayı '-ckhelp' ile başlatın.";

        internal const string InitializeStartupConfigurationMessage = "Ayar dosyası oluşturulurken bir hata meydana geldi. Dosya yolu : {0}";

        internal const string StartupArgumentException = "Uygulama başlangıç parametrelerine '{0}' değerini eklemeniz gerekiyor.";
        internal const string InitializeArgumentMissingException = "Uygulama '{0}' kaynağından, Anahtar : '{1}' İsim : '{2}' değerlerine erişemedi. Ayarlarınızı kontrol ediniz.";

        internal const string InitializerMssqlNotChangeException = "Veritabanında herhangi bir değişim meydana gelmedi.";


        public ConfigurationCoreException()
        {
            
        }

        public ConfigurationCoreException(string message) : base(message)
        {
            
        }

        public ConfigurationCoreException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }

    public class ArgumentMissingException : ConfigurationCoreException
    {
        public ArgumentMissingException()
        {

        }

        public ArgumentMissingException(string message)
            : base(message)
        {

        }

        public ArgumentMissingException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }

    public class MssqlStorageException : ConfigurationCoreException
    {
        public MssqlStorageException()
        {

        }

        public MssqlStorageException(string message)
            : base(message)
        {

        }

        public MssqlStorageException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
