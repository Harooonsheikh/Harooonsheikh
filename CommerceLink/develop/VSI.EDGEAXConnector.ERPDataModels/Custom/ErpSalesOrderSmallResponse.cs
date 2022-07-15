using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpSalesOrderSmallResponse
    {
        public ErpSalesOrderSmallResponse()
        {
        }

        public string ChannelReferenceId { get; set; }//;
        public string Id { get; set; }//;
        public System.Collections.Generic.IList<ErpSalesLineSmallResponse> SalesLines { get; set; }//;
        public string TMVMainOfferType { get; set; }
        public string Language { get; set; }
        public string SiteCode { get; set; }
        public string ThreeLetterISORegionName { get; set; }
        public string TMVContractStartDate { get; set; }
        public bool TMVInvoicePosted { get; set; }
        public string TMVContractEndDate { get; set; }
        public ErpCreditCardCustSmallResponse PaymentInfo { get; set; }
        public string TMVSubscriptionName { get; set; }
        public string TMVSubscriptionWeight { get; set; }
    }

}
