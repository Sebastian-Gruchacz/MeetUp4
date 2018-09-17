namespace OrderService
{
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Xml;
    using System.Xml.Xsl;

    using MeetUp.Enumerations;

    public class XSLTransformHelper
    {
        public string CreateHTML(string inputXml, LanguageCode LanguageCode)
        {
            XslCompiledTransform transform = new XslCompiledTransform();
            string pathOfFile = string.Empty;
            if (LanguageCode.Equals(LanguageCode.English))
            {
                pathOfFile = "XSLTOrderFile";
            }
            else
            {
                pathOfFile = "XSLTOrderFileDanish";
            }
            WebRequest request = HttpWebRequest.Create(ConfigurationManager.AppSettings[pathOfFile]);
            using (WebResponse response = request.GetResponse())

                using (Stream stream = response.GetResponseStream())
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(stream);

                    transform.Load(doc);
                }

            StringWriter results = new StringWriter();
            using (XmlReader reader = XmlReader.Create(new StringReader(inputXml)))
            {
                transform.Transform(reader, null, results);
            }
            return results.ToString();
        }
    }
}