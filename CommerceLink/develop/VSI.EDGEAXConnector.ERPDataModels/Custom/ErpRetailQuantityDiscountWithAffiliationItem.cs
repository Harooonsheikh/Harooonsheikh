using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpRetailQuantityDiscountWithAffiliationItem
    {
        #region "Properties"
        public long RowNumber { get; set; }
        public string OfferId { get; set; }
        public string Name { get; set; }
        public ErpRetailDiscountConcurrency ConcurrencyMode { get; set; }
        public string CurrencyCode { get; set; }
        public DateTimeOffset ValidFrom { get; set; }
        public DateTimeOffset ValidTo { get; set; }
        public decimal LineNum { get; set; }
        public string MultiBuyDiscountType { get; set; }
        public decimal PriceDiscPct { get; set; }
        public decimal QtyLowest { get; set; }
        public long Category { get; set; }
        public long Product { get; set; }
        public long Variant { get; set; }
        public string ItemId { get; set; }
        public string InventDimId { get; set; }
        public string AffiliationName { get; set; }
        public long? AffiliationId { get; set; }

        #endregion
    }
}
