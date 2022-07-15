using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpCustomerAccountDepositLine
    {
        public ErpCustomerAccountDepositLine()
        {

        }

        public decimal? Amount { get; set; }
        public string Comment { get; set; }
        public string CustomerAccount { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
        public string Shift { get; set; }
        public string StoreNumber { get; set; }
        public string Terminal { get; set; }
        public int? TransactionStatusValue { get; set; }
    }
}
