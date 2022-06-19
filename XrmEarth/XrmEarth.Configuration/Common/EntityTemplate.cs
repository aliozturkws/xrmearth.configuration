namespace XrmEarth.Configuration.Common
{
    public class EntityTemplate
    {
        public EntityTemplate(string name, string keyName, string valueName)
        {
            Name = name;
            KeyName = keyName;
            ValueName = valueName;
        }

        public string Name { get; set; }
        public string KeyName { get; set; }
        public string ValueName { get; set; }
    }
}