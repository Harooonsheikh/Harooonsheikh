using System.Collections.Generic;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class IngramTransferOrder
    {
        public string OrderPlacedDate { get; set; }

        public string PACLicense { get; set; }

        public string ChannelReferenceId { get; set; }

        public string Distributor { get; set; }

        public string Reseller { get; set; }

        public string IndirectCustomer { get; set; }

        public string SalesOrigin { get; set; }

        public string ActivationLinkEmail { get; set; }

        public List<IngramSalesLine> SalesLines { get; set; }
    }
}
