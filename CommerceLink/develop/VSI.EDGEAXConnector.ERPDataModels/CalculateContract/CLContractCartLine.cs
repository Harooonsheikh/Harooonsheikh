using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.CalculateContract
{
    public class CLContractCartLine
    {
        public CLContractCartLine()
        {
        }

        public long? CatalogId { get; set; }
        public string CommissionSalesGroup { get; set; }
        public string Description { get; set; }
        public int? EntryMethodTypeValue { get; set; }
        public string EcomItemId { get; set; }
        public string ItemId { get; set; }
        public long? ProductId { get; set; }
        public string LineId { get; set; }
        public decimal? Quantity { get; set; }
        public string UnitOfMeasureSymbol { get; set; }

        public DateTimeOffset TMVContractCalculateFrom { get; set; }
        public DateTimeOffset TMVContractCalculateTo { get; set; }
        public decimal TMVOldLineInvoiceAmountAndAdjustment { get; set; }
        public CLContractOperation CLSalesLineAction { get; set; }
        public string CLSwitchFromLineId { get; set; }
        public string CLParentLineNumber { get; set; }
    }
}
