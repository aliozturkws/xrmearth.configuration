using Microsoft.Xrm.Sdk;
using System;

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
                if (sandbox)
                {
                    AppSettingsManager.InitSandbox();
                }

                _defaultSettings = AppSettingsManager.LoadSettings(service);
                _settingsValidUntil = DateTime.UtcNow.AddMinutes(60);
            }

            return _defaultSettings;
        }
        #endregion | Static Members |
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
