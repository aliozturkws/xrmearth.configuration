using Newtonsoft.Json;
using System.Collections.Generic;

namespace XrmEarth.Configuration.Data.Core
{
    public class StartupConfigReadSettings
    {
        public List<JsonConverter> Converters { get; set; }
    }
}