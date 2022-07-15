using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpQuoteOpportunityUpdateRequest
    {
        public string QuoteId { get; set; }
        public string OpportunityId { get; set; }
        public string OpportunityGuid { get; set; }
    }
}
