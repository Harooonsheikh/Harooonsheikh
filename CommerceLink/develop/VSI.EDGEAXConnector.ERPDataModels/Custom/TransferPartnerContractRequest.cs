using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    //// <summary>
    //// Transfer Contract Request
    //// </summary>
    [XmlRoot(ElementName = "TransferContractRequest")]
    public class TransferPartnerContractRequest
    {
        /// <summary>
        /// Sales Order Id
        /// </summary>
        [Required]
        public string SalesOrderId { get; set; }
        /// <summary>
        /// Customer Account
        /// </summary>
        [Required]
        public string CustomerAccount { get; set; }
        /// <summary>
        /// Distributor Account
        /// </summary>
        [Required]
        public string DistributorAccount { get; set; }
        /// <summary>
        /// Reseller Account
        /// </summary>
        [Required]
        public string ResellerAccount { get; set; }
        /// <summary>
        /// Indirect Customer Account
        /// </summary>
        [Required]
        public string IndirectCustomerAccount { get; set; }

        /// <summary>
        /// Sales Origin
        /// </summary>
        [Required]
        public string SalesOrigin { get; set; }
    }

}
