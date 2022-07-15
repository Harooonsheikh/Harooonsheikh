using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.CalculateContract
{
    public class CLCartLine
    {
        public CLCartLine()
        {
        }
        public long? CatalogId { get; set; }
        public string Description { get; set; }
        public decimal? DiscountAmount { get; set; }
        public ObservableCollection<ErpDiscountLine> DiscountLines { get; set; }
        public string InventoryDimensionId { get; set; }
        public string ItemId { get; set; }
        public string ItemTaxGroupId { get; set; }
        public decimal? LineDiscount { get; set; }
        public string LineId { get; set; }
        public decimal? LineManualDiscountAmount { get; set; }
        public decimal? LineManualDiscountPercentage { get; set; }
        public decimal? LineNumber { get; set; }
        public decimal? LinePercentageDiscount { get; set; }
        public decimal? NetAmountWithoutTax { get; set; }
        public decimal? Price { get; set; }
        public long? ProductId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? TaxAmount { get; set; }
        public string TaxOverrideCode { get; set; }
        public decimal? TaxRatePercent { get; set; }
        public decimal? TotalAmount { get; set; }
        public string UnitOfMeasureSymbol { get; set; }
        public string WarehouseId { get; set; }
        public bool IsTaxOverideCodeTaxExempt { get; set; }
        public ObservableCollection<string> RelatedDiscountedLineIds { get; set; }
        public DateTimeOffset TMVContractCalculateFrom { get; set; }
        public DateTimeOffset TMVContractCalculateTo { get; set; }
        public decimal TMVOldLineInvoiceAmountAndAdjustment { get; set; }
        public string CLSalesLineAction { get; set; }
        public string CLSwitchFromLineId { get; set; }
        public string CLParentLineNumber { get; set; }
        public decimal CLTimeQantity { get; set; }
        public decimal CLAdjustmentAmount { get; set; }
        public decimal CLNetAmountWithoutTax { get; set; }
        public decimal CLDiscountAmount { get; set; }
        public decimal CLTaxAmount { get; set; }
        public decimal CLTotalAmount { get; set; }
    }
}
