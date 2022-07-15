using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpDiscountWithAffiliation
    {
        #region "Properties"

        public string Currency { get; set; }
        public string Name { get; set; }
        public string OfferId { get; set; }
        public bool Online { get; set; }
        public string ValidFrom { get; set; }//;
        public string ValidTo { get; set; }//;
        public List<ErpProductDiscountWithAffiliation> Discounts { get; set; }

        #endregion

        #region "Constructor"

        public ErpDiscountWithAffiliation()
        {

        }

        #endregion
    }
}
