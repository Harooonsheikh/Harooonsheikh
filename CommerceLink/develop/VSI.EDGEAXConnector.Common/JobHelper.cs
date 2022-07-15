using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VSI.EDGEAXConnector.Common
{
   public class JobHelper
    {
        public static XmlDocument convertStringToXML(string content)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content);
            return xmlDocument;
        }
    }
}
