namespace XrmEarth.Core.Arguments
{
    public class CommandLineParameter
    {
        public bool HasValue { get; protected internal set; }
        public string Name { get; protected internal set; }
        public object Value { get; protected internal set; }
    }
}
