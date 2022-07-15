using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public partial class ErpProductDiscount
    {
        // Required for Magento CSV
        public string ColorId { get; set; }
        // Required for Magento CSV
        public string SizeId { get; set; }
        public string StyleId { get; set; }
        // Required for Magento CSV for Date in MM/dd/yyyy format
        public string ValidationFrom { get; set; }
        // Required for Magento CSV for Date in MM/dd/yyyy format
        public string ValidationTo { get; set; }

        public string TMV_ProductType { get; set; }

        public int DiscountLineTypeValue { get; set; }
        public string ManualDiscountTypeValue { get; set; }
        public string CustomerDiscountTypeValue { get; set; }
        public int PeriodicDiscountTypeValue { get; set; }
        public ErpRetailDiscountOfferLineDiscMethodBase DiscountMethod { get; set; }

    }
}
