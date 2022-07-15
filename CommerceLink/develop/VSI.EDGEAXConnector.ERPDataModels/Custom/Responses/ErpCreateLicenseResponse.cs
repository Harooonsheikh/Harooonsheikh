using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpCreateLicenseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ErpProductLicenseResponse> productLicenseResponses { get; set; }
        public ErpCreateLicenseResponse(bool success, string message, List<ErpProductLicenseResponse> productLicenseResponses)
        {
            this.Success = success;
            this.Message = message;
            this.productLicenseResponses = productLicenseResponses;
        }
    }

    public class ErpProductLicenseResponse
    {
        public string GUID { get; set; }
        public string ItemId { get; set; }
        public string VariantId { get; set; }
        public long ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string ActionLink { get; set; }
        public ErpProductLicenseResponse(string guid, string itemId, string variantId, long productId, decimal quantity, string actionLink)
        {
            this.GUID = guid;
            this.ItemId = itemId;
            this.VariantId = variantId;
            this.ProductId = productId;
            this.Quantity = quantity;
            this.ActionLink = actionLink;
        }
    }
}
