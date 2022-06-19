using System.Reflection;

namespace XrmEarth.Configuration.Data.Core
{
    public class LazyValueLoader
    {
        public object Instance { get; set; }
        public PropertyInfo Property { get; set; }

        public object Value
        {
            get
            {
                if (Property == null)
                    return "{Property is null}";

                return Property.GetValue(Instance);
            }
        }
    }
}