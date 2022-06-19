using XrmEarth.Core.Common;

namespace XrmEarth.Core.Arguments
{
    public class ArgumentContainer
    {
        public ArgumentSourceType Source { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }

        public object ActualValue
        {
            get
            {
                if (Value == null)
                    return Name;
                return Value;
            }
        }
    }
}
