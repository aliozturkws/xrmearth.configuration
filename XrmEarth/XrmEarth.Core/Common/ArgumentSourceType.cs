using System;

namespace XrmEarth.Core.Common
{
    [Flags]
    public enum ArgumentSourceType
    {
        StartupArgs = 1,
        ConfigFile = 2,

        AllPlatform = StartupArgs | ConfigFile
    }
}
