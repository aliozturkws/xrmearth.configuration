using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;
using XrmEarth.Core.Configuration.Data.Exceptions;

namespace XrmEarth.Core.Configuration.Target.Mssql.Base
{
    /// <summary>
    /// Veri saklama politikası. Genel olarak verilerin saklanma ve okunma yöntemlerini içerir.
    /// <para></para>
    /// <code>Not: XML serialize destekler.</code>
    /// </summary>
    [Serializable]
    public class MssqlStoragePolicy : IXmlSerializable
    {
        public MssqlStoragePolicy()
        {
            XmlRootName = "Policy";
        }

        /// <summary>
        /// Anahtar kolon adı.
        /// </summary>
        public const string KeyColumnName = "KeyName";
        /// <summary>
        /// Veri kolonu adı.
        /// </summary>
        public const string ValueColumnName = "Value";

        [XmlIgnore]
        [JsonIgnore]
        public string XmlRootName { get; set; }

        /// <summary>
        /// Tablo kütüphaneye ait mi?
        /// </summary>
        [Obsolete("Kullanımına gerek kalmadı.")]
        public bool OwnTable { get; set; }

        /// <summary>
        /// Okuma politikası.
        /// </summary>
        public MssqlRetrievePolicy RetrievePolicy { get; set; }
        /// <summary>
        /// Yazma (saklama) politikası.
        /// </summary>
        public MssqlDeliveryPolicy DeliveryPolicy { get; set; }

        /// <summary>
        /// Kütüphanenin üzerinde çalışacağı tablo sorgusunu oluşturur.
        /// </summary>
        /// <param name="tableName">Tablo adı.</param>
        /// <param name="schemaName">Şema, varsayılan değer 'dbo'.</param>
        /// <returns>Tablo oluşturma sorgusu</returns>
        public static string CreateTableScript(string tableName, string schemaName = null)
        {
            return string.Format(
            "CREATE TABLE {0}.{1}(\r\n\t[ID] [int] IDENTITY(1,1) NOT NULL,\r\n\t[KeyName] [nvarchar](150) NOT NULL,\r\n\t[Value] [nvarchar](4000) NULL\r\n) ON [PRIMARY]\r\n",
            string.IsNullOrEmpty(schemaName) ? "[dbo]" : schemaName,
            tableName);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            OwnTable = Convert.ToBoolean(reader.GetAttribute("OwnTable"));
            reader.ReadStartElement(XmlRootName);

            while (reader.IsStartElement("RetrievePolicy"))
            {
                var targetTypeName = reader.GetAttribute("AssemblyName");
                if (string.IsNullOrWhiteSpace(targetTypeName))
                    throw new InvalidTypeException("Belirtilmiş hedefe ait tip tespit edilemedi.");
                var type = Utils.GetType(targetTypeName);
                if (type == null)
                    throw new InvalidTypeException(string.Format("'{0}' tipi bulunamadı. Mevcut kütüphane '{1}'", targetTypeName, typeof(MssqlStoragePolicy).Assembly.FullName));
                var serial = new XmlSerializer(type);

                reader.ReadStartElement("RetrievePolicy");
                RetrievePolicy = (MssqlRetrievePolicy)serial.Deserialize(reader);
                reader.ReadEndElement();
            }
            while (reader.IsStartElement("DeliveryPolicy"))
            {
                var targetTypeName = reader.GetAttribute("AssemblyName");
                if (string.IsNullOrWhiteSpace(targetTypeName))
                    throw new InvalidTypeException("Belirtilmiş hedefe ait tip tespit edilemedi.");
                var type = Utils.GetType(targetTypeName);
                if (type == null)
                    throw new InvalidTypeException(string.Format("'{0}' tipi bulunamadı. Mevcut kütüphane '{1}'", targetTypeName, typeof(MssqlStoragePolicy).Assembly.FullName));
                var serial = new XmlSerializer(type);

                reader.ReadStartElement("DeliveryPolicy");
                DeliveryPolicy = (MssqlDeliveryPolicy)serial.Deserialize(reader);
                reader.ReadEndElement();
            }

            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("OwnTable", OwnTable.ToString());

            var retType = RetrievePolicy.GetType();
            var delType = DeliveryPolicy.GetType();

            writer.WriteStartElement("RetrievePolicy");
            writer.WriteAttributeString("AssemblyName", retType.AssemblyQualifiedName);
            var retSerializer = new XmlSerializer(retType);
            retSerializer.Serialize(writer, RetrievePolicy);
            writer.WriteEndElement();

            writer.WriteStartElement("DeliveryPolicy");
            writer.WriteAttributeString("AssemblyName", delType.AssemblyQualifiedName);
            var delSerializer = new XmlSerializer(delType);
            delSerializer.Serialize(writer, DeliveryPolicy);
            writer.WriteEndElement();
        }
    }
}
