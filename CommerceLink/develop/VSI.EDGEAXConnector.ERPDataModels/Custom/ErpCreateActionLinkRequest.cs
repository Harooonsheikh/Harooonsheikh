using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
   public class ErpCreateActionLinkRequest
    {
        public string GUID { get; set; }
        public string ItemId { get; set; }
        public string VariantId { get; set; }
        public long ProductId { get; set; }
        public decimal Quantity { get; set; }
    }
}
