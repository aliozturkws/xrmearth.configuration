using Microsoft.Xrm.Sdk;
using System;
using System.Text;
using XrmEarth.Configuration.Data;
using XrmEarth.Configuration.Policies;
using XrmEarth.Configuration.Target;
using ConfigurationManager = XrmEarth.Configuration.ConfigurationManager;

namespace XrmEarth.Samples
{
    public class AppSettings
    {
        public CustomApi CustomApi { get; set; }
        public ReportServer ReportServer { get; set; }

        private static AppSettings _defaultSettings;
        private static DateTime _settingsValidUntil = DateTime.MinValue;
        public static AppSettings Default(IOrganizationService service, bool sandbox = true)
        {
            if (DateTime.UtcNow > _settingsValidUntil)
            {
                _defaultSettings = null;
            }

            if (_defaultSettings == null)
            {
                _defaultSettings = LoadSettings(service);
                _settingsValidUntil = DateTime.UtcNow.AddMinutes(5);
            }

            return _defaultSettings;
        }

        public static StartupConfiguration CreateConfig(IOrganizationService service)
        {
            return new StartupConfiguration
            {
                Targets = new TargetCollection
                {
                    new CrmStorageTarget(service)
                    {
                        Policy = new WebResourceStoragePolicy
                        {
                             Prefix = "new",
                             Name = "configuration",
                             Encoding = Encoding.UTF8
                        }
                    }
                }
            };
        }
        public static AppSettings LoadSettings(IOrganizationService service)
        {
            return ConfigurationManager.Load<AppSettings>(CreateConfig(service));
        }
    }

    public class CustomApi
    {
        public string BaseUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class ReportServer
    {
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
