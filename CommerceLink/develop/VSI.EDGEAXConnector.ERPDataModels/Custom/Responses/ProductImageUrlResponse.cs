using System.Collections.Generic;
using System.Xml.Serialization;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{

    [XmlRoot(ElementName = "Products")]
    public class ProductImageUrlResponse
    {
        public ProductImageUrlResponse()
        {
            this.Status = true;
            this.Message = "";
            this.Products = new List<ProductImage>();
        }

        public ProductImageUrlResponse(bool status, string message)
        {
            this.Status = status;
            this.Message = message;
            this.Products = new List<ProductImage>();
        }

        public bool Status { get; set; }
        public string Message { get; set; }

        [XmlElement(ElementName = "Product")]
        public List<ProductImage> Products { get; set; }
    }

    [XmlRoot(ElementName = "Product")]
    public class ProductImage
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "ImageURL")]
        public string ImageUrl { get; set; }
    }

    
}