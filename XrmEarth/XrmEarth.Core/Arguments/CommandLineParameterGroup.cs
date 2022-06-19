using System.Collections.Generic;

namespace XrmEarth.Core.Arguments
{
    public class CommandLineParameterGroup : List<CommandLineParameter>
    {
        public string Key { get; set; }
    }
}
