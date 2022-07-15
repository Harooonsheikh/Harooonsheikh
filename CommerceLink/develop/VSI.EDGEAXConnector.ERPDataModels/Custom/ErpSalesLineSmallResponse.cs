using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpSalesLineSmallResponse
    {
        public ErpSalesLineSmallResponse()
        {
        }

        public decimal DiscountAmount { get; set; }
        // public ErpAddress ShippingAddress { get; set; } // Removed after discussion with Aneel Kumar as we are populating ThreeLetterISORegionName at header level
        public long RecordId { get; set; }
        public decimal LinePercentageDiscount { get; set; }
        public System.Collections.Generic.IList<ErpDiscountLine> DiscountLines { get; set; }
        public string ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TaxRatePercent { get; set; }
        public string TMVAutoProlongation { get; set; }
        public string TMVBillingPeriod { get; set; }
        public string TMVContractValidFrom { get; set; }
        public string TMVContractValidTo { get; set; }
        public string PACLicense { get; set; }
        public string TMVIsSwitch { get; set; }
        public string TMVIsMigrated { get; set; }
        public string TMVParent { get; set; }
        public string TMVTimeQuantity { get; set; }
        public string TMVCalculateLineAmount { get; set; }
        public string TMVOriginalLineAmount { get; set; }
        public string TMVAdjustmentAmount { get; set; }
    }
}
