using System;

namespace XrmEarth.Configuration.Data
{
    [Serializable]
    public class StartupConfiguration
    {
        public string Key { get; set; }
        public TargetCollection Targets { get; set; }
    }
}
