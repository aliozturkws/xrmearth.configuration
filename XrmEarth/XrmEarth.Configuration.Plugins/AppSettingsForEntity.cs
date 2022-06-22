using Microsoft.Xrm.Sdk;
using System;
using XrmEarth.Configuration.Data;
using XrmEarth.Configuration.Policies;
using XrmEarth.Configuration.Target;

namespace XrmEarth.Configuration.Plugins
{
    public class AppSettings
    {
        public D365 D365 { get; set; }

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
    }

    public class D365
    {
        public string AdminUserName { get; set; }
        public Guid? PriceLevelId { get; set; }
    }
}
