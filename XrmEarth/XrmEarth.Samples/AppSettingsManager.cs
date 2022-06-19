using Microsoft.Xrm.Sdk;
using XrmEarth.Configuration;
using XrmEarth.Configuration.Data;
using XrmEarth.Configuration.Policies;
using XrmEarth.Configuration.Target;

namespace XrmEarth.Samples
{
    public class AppSettingsManager
    {
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

        public static void InitSandbox()
        {
            Config.Sandbox = true;

            ConfigurationManager.Init(new Config
            {
                ConfigurationPath = null,
                Key = null,
            });
        }
    }
}
