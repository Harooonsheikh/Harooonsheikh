using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 8.1 Application change start
    public class ErpCustomerSearchByFieldCriteria
    {
        public ErpCustomerSearchByFieldCriteria()
        {
        }
        public int DataLevelValue { get; set; }
        public System.Collections.Generic.ICollection<ErpCustomerSearchByFieldCriterion> SearchField { get; set; }
       
    }
    //NS: D365 Update 8.1 Application change end
}
