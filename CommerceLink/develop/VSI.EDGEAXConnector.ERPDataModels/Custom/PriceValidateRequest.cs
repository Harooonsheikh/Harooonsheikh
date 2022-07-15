using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class PriceRequest
    {
        [Required]
        public string RequestDate { get; set; }
        public long ChannelRecId { get; set; }
        [Required]
        public string CustomerAccount { get; set; }
        [Required]
        public string ResellerAccount { get; set; }
        [Required]
        public string IndirectCustomerAccount { get; set; }
        [Required]
        public string Currency { get; set; }
        public bool IsValidateRequest { get; set; }
        public List<PriceContractLine> ContractLines { get; set; }

        public PriceRequest()
        {
            this.ContractLines = new List<PriceContractLine>();
        }
        
    }
    [XmlType(TypeName = "ContractLine")]
    public class PriceContractLine
    {
        public decimal LineNumber { get; set; }
        public string ProductId { get; set; }
        public string ItemId { get; set; }
        public string VariantId { get; set; }
        public decimal Quantity { get; set; }
        public decimal TargetPrice { get; set; }

        #region Sales Origins
        private static readonly string WEB = "WEB";
        private static readonly string WEBSHOPMIGRATION = "WEBSHOP MIGRATION";
        private static readonly string WEBSHOPNEW = "WEBSHOP NEW";
        private static readonly string WEBQUOTE = "WEB QUOTE";
        private static readonly string WEBSHOPCONTRACTUPDATE = "WEBSHOP CONTRACT UPDATE";
        private static readonly string INGRAM = "INGRAM";
        #endregion

        public static bool PriceValidationSalesOrigin(string salesOrigin)
        {
            if (!string.IsNullOrWhiteSpace(salesOrigin))
            {
                if (salesOrigin.ToUpper() == WEB)
                {
                    return false;
                }
                else if (salesOrigin.ToUpper() == WEBSHOPMIGRATION)
                {
                    return false;
                }
                else if (salesOrigin.ToUpper() == WEBSHOPNEW)
                {
                    return false;
                }
                else if (salesOrigin.ToUpper() == WEBQUOTE)
                {
                    return false;
                }
                else if (salesOrigin.ToUpper() == WEBSHOPCONTRACTUPDATE)
                {
                    return false;
                }
                else if (salesOrigin.ToUpper() == INGRAM)
                {
                    return false;
                }
                return true; // Partner case
            }
            return false;
        }
    }
}
