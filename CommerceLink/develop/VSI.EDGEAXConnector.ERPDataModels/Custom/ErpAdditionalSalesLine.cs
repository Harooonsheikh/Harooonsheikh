using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpAdditionalSalesLine
    {
        /// <summary>
        /// ItemId to Switch/Migrate/AddOn
        /// </summary>
        public string ItemId { get; set; }
        /// <summary>
        /// InventDimensionId to Switch/Migrate/AddOn
        /// </summary>
        public string InventDimensionId { get; set; }
        /// <summary>
        /// Quantity to Switch/Migrate/AddOn
        /// </summary>
        public int Quantity { get; set; }
    }
}
