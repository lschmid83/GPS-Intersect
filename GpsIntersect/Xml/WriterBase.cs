using System;
using System.Xml;
using System.Xml.Serialization;

namespace GpsIntersect.Xml
{
    public class WriterBase
    {
        /// <summary>
        /// Serializes an XML file.
        /// </summary>
        protected void SerializeFile<T>(string filename, T file, XmlAttributeOverrides attributeOverrides = null)
        {
            // Serialize file.
            XmlSerializer ser; 
            if(attributeOverrides == null)
                ser = new XmlSerializer(typeof(T));
            else
                ser = new XmlSerializer(typeof(T), attributeOverrides); // Override attribute.

            // Write to file.
            var xmlWriterSettings = new XmlWriterSettings { Indent = true };
            using (XmlWriter writer = XmlWriter.Create(filename, xmlWriterSettings))
                ser.Serialize(writer, file);
        }

        /// <summary>
        /// Converts DateTime to a timestamp.
        /// </summary>
        protected string ConvertDateTimeToTimestamp(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
       }
    }
}
