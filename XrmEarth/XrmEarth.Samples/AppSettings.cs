using Microsoft.Xrm.Sdk;
using System;
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

        #region | Static Members |
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
                _settingsValidUntil = DateTime.UtcNow.AddMinutes(60);
            }

            return _defaultSettings;
        }
        #endregion | Static Members |

        #region | Functions |
        public static StartupConfiguration CreateConfig(IOrganizationService service)
        {
            return new StartupConfiguration
            {
                Targets = new TargetCollection
                {
                    new CrmStorageTarget(service)
                    {
                        Policy = new EntityStoragePolicy
                        {
                            Prefix = "new",
                            LogicalName = "configuration",
                            KeyAttributeLogicalName = "name",
                            ValueAttributeLogicalName = "value",
                        }
                    }
                }
            };
        }
        public static AppSettings LoadSettings(IOrganizationService service)
        {
            return ConfigurationManager.Load<AppSettings>(CreateConfig(service));
        }
        public static void SaveSettings(AppSettings settings, IOrganizationService service)
        {
            ConfigurationManager.Save(settings, CreateConfig(service));
        }
        #endregion | Functions |
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
