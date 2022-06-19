using System.Reflection;
using XrmEarth.Core.Attributes;

namespace XrmEarth.Core.Arguments
{
    public class BindArgumentContainer
    {
        public PropertyInfo Property { get; set; }
        public ArgumentAttribute Attribute { get; set; }
    }
}
