using Microsoft.Xrm.Sdk;
using System;

namespace XrmEarth.Configuration.Plugins
{
    public abstract class BasePlugin : IPlugin
    {
        public abstract void OnExecute(IServiceProvider serviceProvider);

        public AppSettings AppSettings { get; set; }
        public ITracingService tracingService { get; set; }

        public void Execute(IServiceProvider serviceProvider)
        {
            #region | Context and Service |
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            #endregion | Context and Service |

            try
            {
                tracingService.Trace("started..");

                AppSettings = AppSettings.Default(service);

                OnExecute(serviceProvider);

                tracingService.Trace("ended..");
            }
            catch (Exception ex)
            {
                tracingService.Trace(string.Concat("Error : ", ex.ToString()));
                throw;
            }
        }
    }
}
