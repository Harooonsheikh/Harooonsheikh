using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpQuantityDiscount
    {
        public List<ErpProductQuantityDiscount> QuantityDiscounts { get; set; }
    }
}
