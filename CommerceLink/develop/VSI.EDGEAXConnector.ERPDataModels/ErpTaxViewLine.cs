using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpTaxViewLine
    {
        public ErpTaxViewLine()
        {

        }

        public decimal? TaxAmount { get; set; }
        public string TaxId { get; set; }
    }
}
