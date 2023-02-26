using System.IO;

using System.Xml.Serialization;

namespace Rss_reader.Serializers
{
    public static class XMLSerializer
    {
        public static RssModel Deserialize(string xml)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RssModel));
                TextReader reader = new StringReader(xml);
                var rssModel = (RssModel) serializer.Deserialize(reader);
                reader.Close();
                return rssModel;
            }
            catch
            {
                return null;
            }
           
        }
    }
}