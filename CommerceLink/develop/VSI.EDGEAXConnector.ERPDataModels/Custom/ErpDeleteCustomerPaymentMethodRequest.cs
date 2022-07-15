using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpDeleteCustomerPaymentMethodRequest
    {
        public long CardRecId { get; set; }
        public long BankAccountRecId { get; set; }
    } 
}
