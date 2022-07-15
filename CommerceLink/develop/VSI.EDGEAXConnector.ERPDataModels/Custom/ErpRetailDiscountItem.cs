using System;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpRetailDiscountItem
    {
        #region "Properties"

        public ErpRetailDiscountConcurrency ConcurrencyMode { get; set; }
        public string CurrencyCode { get; set; }
        public decimal DiscAmount { get; set; }
        public ErpRetailDiscountOfferLineDiscMethodBase DiscountMethod { get; set; }
        public decimal DiscPct { get; set; }
        public string InventDimId { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string OfferId { get; set; }
        public decimal OfferPrice { get; set; }
        public long Product { get; set; }
        public long RowNumber { get; set; }
        public DateTimeOffset ValidFrom { get; set; }
        public DateTimeOffset ValidTo { get; set; }
        public string AffiliationName { get; set; }
        public long? AffiliationRecId { get; set; }
        public int PeriodicDiscountType { get; set; }
        public int DiscountType { get; set; }
        public int LineType { get; set; }

        #endregion
    }
}
