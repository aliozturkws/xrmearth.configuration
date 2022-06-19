using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace XrmEarth.Core.Utility
{
    public class BinarySerializerUtil : ISerializerUtil<byte[]>
    {
        public byte[] Serialize<T>(T input)
        {
            using (var stream = new MemoryStream())
            {
                var bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, input);

                return stream.ToArray();
            }
        }

        public T Deserialize<T>(byte[] output)
        {
            using (var mStream = new MemoryStream(output))
            {
                var bformatter = new BinaryFormatter();
                return (T)bformatter.Deserialize(mStream);
            }
        }

        public void SerializeFile<T>(T input, string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Create))
            {
                var bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, input);
            }
        }

        public T DeserializeFile<T>(string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                var bformatter = new BinaryFormatter();
                return (T)bformatter.Deserialize(stream);
            }
        }
    }
}
