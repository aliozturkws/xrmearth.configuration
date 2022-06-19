using System.Collections.Generic;

namespace XrmEarth.Core.Data.Sql
{
    public class SPConfig
    {
        public string Name { get; set; }
        public IEnumerable<Parameter> Parameters { get; set; }
    }
}
