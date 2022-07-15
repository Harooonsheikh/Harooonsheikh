using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpGetCustomIndependentProductPriceDiscountResponse
    {
        public string Message { get; set; }
        public List<ErpProductPrice> ProductPrices { get; set; }
        public bool Success { get; set; }

        public ErpGetCustomIndependentProductPriceDiscountResponse(bool success, string message, List<ErpProductPrice> productPrices)
        {
            this.Success = success;
            this.Message = message;
            this.ProductPrices = productPrices;
        }
    }
}
