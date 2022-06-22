using System;

namespace XrmEarth.Configuration.Plugins
{
    public class Sample1 : BasePlugin
    {
        public override void OnExecute(IServiceProvider serviceProvider)
        {
            tracingService.Trace(AppSettings.D365.AdminUserName);
        }
    }
}