using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XrmEarth.Core.Configuration.Data
{
    [Serializable]
    [XmlRoot]
    //TODO - [DEL]
    public class StartupConfigurationCollection
    {
        public List<StartupConfiguration> StartupConfigurations { get; set; }
    }
}
