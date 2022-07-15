using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpCoupon
    {
        public ErpCoupon()
        {

        }

        public string Code { get; set; }
        public string CodeId { get; set; }
        public string DiscountOfferId { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
}
