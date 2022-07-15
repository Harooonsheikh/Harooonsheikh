using System;
using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 9 Platform new class
    public class ErpProductBehavior
    {
        public ErpProductBehavior()
        {
        }

        public DateTimeOffset ValidToDateForSaleAtPhysicalStores { get; set; }
        public DateTimeOffset ValidFromDateForSaleAtPhysicalStores { get; set; }
        public bool MustWeighProductAtSale { get; set; }
        public bool MustPromptForSerialNumberOnlyAtSale { get; set; }
        public bool MustPrintIndividualShelfLabelsForVariants { get; set; }
        public bool MustKeyInComment { get; set; }
        public int KeyInQuantityValue { get; set; }
        public bool IsBlankSerialNumberAllowed { get; set; }
        public int KeyInPriceValue { get; set; }
        public bool IsSaleAtPhysicalStoresAllowed { get; set; }
        public bool IsReturnAllowed { get; set; }
        public bool IsNegativeQuantityAllowed { get; set; }
        public bool IsKitDisassemblyAllowed { get; set; }
        public bool IsDiscountAllowed { get; set; }
        public bool HasSerialNumber { get; set; }
        public bool IsZeroSalePriceAllowed { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }

        //NS: D365 Update 12 Platform change start
        public bool IsManualDiscountAllowed { get; set; }
        //NS: D365 Update 12 Platform change end
    }
}
