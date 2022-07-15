using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ContractSalesorderRequest
    {
        [Required]
        public string CustomerAccount { get; set; }
        public string EcomCustomerId { get; set; }
        public string OfferType { get; set; }
        public string SalesOrderId { get; set; }
        public string ChannelReferenceId { get; set; }
        public bool isActive { get; set; }
        public string LicenseNumber { get; set; }
        public List<string> Status { get; set; }
        public bool SmallResponse { get; set; }

    }
}
