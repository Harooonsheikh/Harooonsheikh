using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomContractLine
    {
        public int LineNumber { get; set; }
        public decimal TargetPrice { get; set; }
        public decimal TaxAmount { get; set; }
        public string ProductId { get; set; }
        public string ItemId { get; set; }
        public string VariantId { get; set; }
        public decimal Quantity { get; set; }
        public string SalesLineAction { get; set; }
        public string OldLinePacLicense { get; set; }
        public int ParentLineNumber { get; set; }
    }
}
