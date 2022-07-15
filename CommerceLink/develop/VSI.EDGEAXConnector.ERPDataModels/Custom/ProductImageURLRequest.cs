using System.Collections.Generic;
using System.Xml.Serialization;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    [XmlRoot(ElementName = "products")]
    public class ProductImageUrlRequest
    {
        [XmlElement(ElementName = "product")]
        public List<string> Products { get; set; }
    }
}