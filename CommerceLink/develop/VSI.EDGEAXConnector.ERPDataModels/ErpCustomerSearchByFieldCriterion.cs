using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels.Enums;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 8.1 Application change start
    public class ErpCustomerSearchByFieldCriterion
    {
        public ErpCustomerSearchByFieldCriterion()
        {
        }
        public string SearchTerm { get; set; }
        public ErpCustomerSearchFieldType SearchField { get; set; }
        public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
    //NS: D365 Update 8.1 Application change end
}
