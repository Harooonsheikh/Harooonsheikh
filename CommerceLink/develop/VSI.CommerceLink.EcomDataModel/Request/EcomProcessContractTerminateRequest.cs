using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class EcomProcessContractTerminateRequest
    {
        [Required]
        public string SalesOrderId { get; set; }
        public bool IsLineOperation { get; set; }
        public IList<SalesLineRecId> SalesLineRecIds { get; set; }
        [Required]
        public bool CreateOpportunityInCRM { get; set; }
        [Required]
        public bool RequestTermination { get; set; }

        [Required]
        public string ReasonId { get; set; }
        [Required]
        public string ReasonCode { get; set; }
        [Required]
        public string Comments { get; set; }

        public int Interval { get; set; }
        public bool IsFutureTermination { get; set; }

    }
    public class SalesLineRecId
    {
        public long RecId { get; set; }
    }
}