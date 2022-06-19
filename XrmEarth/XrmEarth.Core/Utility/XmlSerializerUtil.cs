using System;
using System.IO;
using System.Xml.Serialization;

namespace XrmEarth.Core.Utility
{
    public class XmlSerializerUtil
    {
        public static string Serialize<T>(T input)
        {
            return Serialize<T>(input, null);
        }

        public static T Deserialize<T>(string output)
        {
            return Deserialize<T>(output, null);
        }

        public static void SerializeFile<T>(T input, string filePath)
        {
            SerializeFile(input, filePath, null);
        }

        public static T DeserializeFile<T>(string filePath)
        {
            return DeserializeFile<T>(filePath, null);
        }

        public static string Serialize<T>(T input, Type[] extraTypes = null)
        {
            using (var sWriter = new StringWriter())
            {
                var xml = new XmlSerializer(typeof(T), extraTypes);
                xml.Serialize(sWriter, input);

                return sWriter.ToString();
            }
        }

        public static T Deserialize<T>(string input, Type[] extraTypes = null)
        {
            using (var sReader = new StringReader(input))
            {
                var xml = new XmlSerializer(typeof(T), extraTypes);
                return (T)xml.Deserialize(sReader);
            }
        }

        public static void SerializeFile<T>(T input, string filePath, Type[] extraTypes = null)
        {
            using (var stream = File.Open(filePath, FileMode.Create))
            {
                var xml = new XmlSerializer(typeof(T), extraTypes);
                xml.Serialize(stream, input);
            }
        }

        public static T DeserializeFile<T>(string filePath, Type[] extraTypes)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var reader = new StreamReader(stream);
                var xmlContent = reader.ReadToEnd();
                return Deserialize<T>(xmlContent, extraTypes);
            }
        }
    }
}
