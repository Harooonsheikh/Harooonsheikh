using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels.Enums;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class EcomCloseExistingOrderRequest
    {
        [Required]
        public string SalesId { get; set; }
        [Required]
        public string PACLicense { get; set; }
        [Required]
        public string DisablePacLicenseOfSalesLines { get; set; }
    }
}
