
using System.Collections.Generic;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public partial class ErpSalesLine
    {
        // Custom for MF Start
        public string ShipmentId { get; set; }
        public List<ErpSalesLine> Options { get; set; }
        // Custom for MF End
        public string MonogramFont { get; set; }
        public string MonogramInitials { get; set; }
        public string MonogramThread { get; set; }
        public decimal Monogram_Price { get; set; }
        public decimal Monogram_Tax { get; set; }
        public int IsFinalSales { get; set; }

        public string ItemDiscountCode { get; set; }

        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }

        //TODO: for mapping add this we need to handle this using salesLine.Variant.VariantId
        public string VariantId { get; set; }

        //NS: Re-Architect
        //Used for AX7
        public string FulfillmentStoreId { get; set; }
        public decimal NetAmountWithoutTax { get; set; }
        public string OriginalItemTaxGroupId { get; set; }
        public string OriginalSalesTaxGroupId { get; set; }

        //++ For TV customizations
        public string TMVAutoProlongation { get; set; }
        public string TMVBillingPeriod { get; set; }
        public string TMVContractCalculateFrom { get; set; }
        public string TMVContractCalculateTo { get; set; }
        public string TMVContractCancelDate { get; set; }
        public string TMVContractPossCancelDate { get; set; }
        public string TMVContractStatusLine { get; set; }
        public string TMVContractTermDate { get; set; }
        public string TMVContractTermDateEffective { get; set; }
        public string TMVContractValidFrom { get; set; }
        public string TMVContractValidTo { get; set; }
        public string TMVCustomerRef { get; set; }
        public string TMVEULAVersion { get; set; }
        public string TMVLineModified { get; set; }
        public string TMVOriginalLineAmount { get; set; }
        public string TMVPurchOrderFormNum { get; set; }
        public string TMVReversedLine { get; set; }
        public string TMVContractPossTermDate { get; set; }
        public string PACLicense { get; set; }
        public long TMVMigratedSalesLineNumber { get; set; }
        public long TMVOldSalesLineNumber { get; set; }
        public string TMVOldSalesLineAction { get; set; }
        public string TMVIsSwitch { get; set; }
        public string TMVIsMigrated { get; set; }
        public string TMVParent { get; set; }
        public string LineAmount { get; set; }
        public string TMVTimeQuantity {get; set;}
        public string TMVCalculateLineAmount { get; set; }
        public string TMVCustomerLineNum { get; set; }
        public string TMVAdjustmentAmount { get; set; }
        public bool TMVDisablePACLicense { get; set; }
        public decimal OldQuantity { get; set; }

    }
}
