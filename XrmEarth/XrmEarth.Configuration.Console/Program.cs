using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XrmEarth.Configuration.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            CrmServiceClient adminClientService = XrmConnection.AdminCrmClient;

            IOrganizationService orgService = adminClientService.GetOrganizationService();

            var settings = AppSettings.Default(orgService);

            string jsonFormatted = JValue.Parse(JsonConvert.SerializeObject(settings)).ToString(Formatting.Indented);

            System.Console.WriteLine(jsonFormatted);

            System.Console.ReadKey();
        }
    }
}
