using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public partial class ErpCustomerOrderInfo
    {
        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }

        //++ 
        public List<ErpAddress> Addresses { get; set; }
        public string TMVMAINOFFERTYPE { get; set; }
        public string TMVPRODUCTFAMILY { get; set; }
        public string TMVSALESORDERSUBTYPE { get; set; }
        public string TMVOFFERTYPE { get; set; }
        public string TMVBILLINGINTERVAL { get; set; }
        public string TMVCONTRACTENDDATE { get; set; }
        public string SHIPPINGDATEREQUESTED { get; set; }
        public string PURCHORDERFORMNUM { get; set; }
        public string CONTACTPERSONID { get; set; }
        public string SALESID { get; set; }
        public string SiteCode { get; set; }
        public string Language { get; set; }
        public string RetailChannel { get; set; }
        public string ThreeLetterISORegionName { get; set; }

        // VSTS 33525 Begin
        /// <summary>
        /// Reseller Account
        /// </summary>
        public string TMVResellerAccount{ get; set; }

        /// <summary>
        /// Distributor Account
        /// </summary>
        public string TMVDistributorAccount{ get; set; }

        /// <summary>
        /// Indirect Customer
        /// </summary>
        public string TMVIndirectCustomer{ get; set; }

        /// <summary>
        /// Comment for Quotation
        /// </summary>
        public string TMVCommentForQuote{ get; set; }

        /// <summary>
        /// Customer Reference
        /// </summary>
        public string TMVCustomerReference{ get; set; }
        // VSTS 33525 End

        // VSTS 55353 Begin
        public string TMVLoginEmail { get; set; }
        // VSTS 55353 End
    }
}
