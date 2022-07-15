using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class EcomProcessContractReactivateRequest
    {
        [Required]
        public string SalesOrderId { get; set; }
        public bool IsLineOperation { get; set; }
        public IList<SalesLineRecIds> SalesLineRecIds { get; set; }        
    }
    public class SalesLineRecIds
    {
        public long RecId { get; set; }
    }
}
