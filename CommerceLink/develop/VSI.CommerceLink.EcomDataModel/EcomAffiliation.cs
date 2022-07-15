using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.CommerceLink.EcomDataModel.Enum;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomAffiliation
    {
        public EcomAffiliation()
        {
        }
        public long AffiliationId { get; set; }//;
        public long LoyaltyTierId { get; set; }//;
        public EcomRetailAffiliationType AffiliationType { get; set; }//;
        // To be implemented in required in future
        // public System.Collections.ObjectModel.Collection<ErpReasonCodeLine> ReasonCodeLines { get; set; }//;
        public string CustomerId { get; set; }//;
        public string EntityName { get; set; }//;
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }//;
        public System.Collections.Generic.ICollection<EcomCommerceProperty> ExtensionProperties { get; set; }//;
        public object Item { get; set; }//;
    }
}
