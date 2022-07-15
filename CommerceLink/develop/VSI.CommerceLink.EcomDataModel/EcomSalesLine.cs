using System.Collections.Generic;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomSalesLine
    {
        public decimal LineNumber { get; set; }
        public string ItemId { get; set; }
        public decimal Quantity { get; set; }
        public string UnitOfMeasureSymbol { get; set; }
        public decimal BasePrice { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TaxRatePercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        
        #region Checkout Process Contract Operation
        public decimal TargetPrice { get; set; }
        public string SalesLineAction { get; set; }
        public string OldLinePacLicense { get; set; } 
        #endregion

        public IList<EcomDiscountLine> DiscountLines { get; set; }
        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }

    }
}
