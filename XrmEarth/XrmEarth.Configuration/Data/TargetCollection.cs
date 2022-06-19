using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using XrmEarth.Configuration.Target;

namespace XrmEarth.Configuration.Data
{
    public class TargetCollection : List<StorageTarget>, IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return null;
        }

        public new StorageTarget this[int index] => base[index];

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement("Targets");
            while (reader.IsStartElement("StorageTarget"))
            {
                var targetTypeName = reader.GetAttribute("AssemblyName");
                if(string.IsNullOrWhiteSpace(targetTypeName))
                    throw new Exception("The type of the specified target could not be detected.");
                var type = Utils.GetType(targetTypeName);
                if(type == null)
                    throw new Exception(string.Format("'{0}' type not found. existing assembly '{1}'", targetTypeName, typeof(TargetCollection).Assembly.FullName));

                var serial = new XmlSerializer(type);

                reader.ReadStartElement("StorageTarget");
                var storageObject = serial.Deserialize(reader);
                Add((StorageTarget)storageObject);
                reader.ReadEndElement();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var dispatcher in this)
            {
                writer.WriteStartElement("StorageTarget");
                writer.WriteAttributeString("AssemblyName", dispatcher.GetType().AssemblyQualifiedName);
                var xmlSerializer = new XmlSerializer(dispatcher.GetType());
                xmlSerializer.Serialize(writer, dispatcher);
                writer.WriteEndElement();
            }
        }
    }
}
