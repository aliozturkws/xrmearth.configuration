namespace XrmEarth.Core.Utility
{
    public interface ISerializerUtil<R>
    {
        R Serialize<T>(T input);

        T Deserialize<T>(R output);

        void SerializeFile<T>(T input, string filePath);

        T DeserializeFile<T>(string filePath);
    }
}
