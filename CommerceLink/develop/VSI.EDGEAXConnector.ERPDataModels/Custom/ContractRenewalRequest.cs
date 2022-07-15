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
    [XmlType(TypeName = "ProcessContractRenewal")]
    public class ContractRenewalRequest
    {
        [Required]
        public string RequestNumber { get; set; }

        [Required]
        public string PACLicense { get; set; }
        
        [Required]
        public string Distributor { get; set; }

        [Required]
        public string SalesOrigin { get; set; }

        [Required]
        public string CustomerReference { get; set; }

        [Required]
        public List<LineDetail> SalesLines { get; set; }
       

    }

    [XmlType(TypeName = "LineDetail")]
    public class LineDetail
    {
        [Required]
        public string SKU { get; set; }
        
        public string ItemId { get; set; }
        
        public string VariantId { get; set; }
        
        [Required]
        public decimal Quantity { get; set; }
        
        [Required]
        public string CustomerReference { get; set; }

        public decimal TargetAmount { get; set; }
    }
}
