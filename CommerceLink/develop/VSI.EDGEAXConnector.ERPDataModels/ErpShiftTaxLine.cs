using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    
    public class ErpShiftTaxLine
    {
        public ErpShiftTaxLine()
        { }
        public string TaxCode { get; set; }
        public decimal TaxAmount { get; set; }

        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }


        //NS: D365 Update 8.1 Application change start
        public decimal TaxRate { get; set; }
        public decimal NetAmount { get; set; }

        //NS: D365 Update 8.1 Application change end
    }
}