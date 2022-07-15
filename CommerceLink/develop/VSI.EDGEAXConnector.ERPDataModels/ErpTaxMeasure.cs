using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 8.1 Application change start
    public class ErpTaxMeasure
    {
        public ErpTaxMeasure()
        { }
        public string Path { get; set; }
        public decimal Value { get; set; }
        public string DataAreaId { get; set; }
        public decimal SaleLineNumber { get; set; }
        public string StoreId { get; set; }
        public string TerminalId { get; set; }
        public string TransactionId { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
    //NS: D365 Update 8.1 Application change end
}