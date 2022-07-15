using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    
    public class PriceResponse
    {
        public PriceResponse(bool status, string message, ProductInformation result)
        {
            this.Status = status;
            this.Message = message;
            this.Result = result;
        }

        public bool Status { get; set; }
        public string Message { get; set; }

        public ProductInformation Result { get; set; }
    }
    [XmlRoot(ElementName = "TMVPriceApiResponseContract")]
    public class ProductInformation
    {
        [XmlElement(ElementName = "NetAmountWithNoTax")]
        public decimal NetAmountWithNoTax { get; set; }
        [XmlElement(ElementName = "TaxAmount")]
        public decimal TaxAmount { get; set; }
        [XmlElement(ElementName = "NetAmountWithTax")]
        public decimal NetAmountWithTax { get; set; }
        [XmlElement(ElementName = "TotalAmount")]
        public decimal TotalAmount { get; set; }
        [XmlElement(ElementName= "Result")]
        public SaleLines SaleLines { get; set; }
    }

    [XmlRoot(ElementName = "Result")]
    public class SaleLines
    {
        [XmlElement(ElementName = "TMVPriceApiLinesResponseContract")]
        public List<ItemInformation> Items { get; set; }
    }
    [XmlRoot(ElementName = "TMVPriceApiLinesResponseContract")]
    public class ItemInformation
    {
        [XmlElement(ElementName = "LineNumber")]
        public string LineNumber { get; set; }
        [XmlElement(ElementName = "BasePrice")]
        public decimal BasePrice { get; set; }
        [XmlElement(ElementName = "NetAmount")]
        public decimal NetAmount { get; set; }
        [XmlElement(ElementName = "TaxRatePercentage")]
        public decimal TaxRatePercent { get; set; }
        [XmlElement(ElementName = "TaxAmount")]
        public decimal TaxAmount { get; set; }
        [XmlElement(ElementName = "UnitOfMeasureSymbol")]
        public string UnitOfMeasureSymbol { get; set; }
        public string ItemId { get; set; }
        [XmlElement(ElementName = "VarientId")]
        public string VariantId { get; set; }
        [XmlElement(ElementName = "TotalAmount")]
        public decimal TotalAmount { get; set; }
        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }
    }
}
