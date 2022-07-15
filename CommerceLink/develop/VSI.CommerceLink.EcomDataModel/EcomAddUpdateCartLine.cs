using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomAddUpdateCartLine
    {
        public EcomAddUpdateCartLine()
        {
        }

        public long? CatalogId { get; set; }
        public string CommissionSalesGroup { get; set; }
        public string Description { get; set; }
        public int? EntryMethodTypeValue { get; set; }
        public string ItemId { get; set; }
        public string LineId { get; set; }
        public decimal? Quantity { get; set; }
        public string UnitOfMeasureSymbol { get; set; }
        public bool IsUpdate { get; set; }
    }
}
