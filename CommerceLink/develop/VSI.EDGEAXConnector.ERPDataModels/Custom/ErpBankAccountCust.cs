using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpBankAccountCust
    {
        public string AccountId { get; set; }
        public string Name { get; set; }
        public string BankIBAN { get; set; }
        public string ContactPerson { get; set; }
        public string SWIFTNo { get; set; }
        public string CustAccount { get; set; }
        public string RecId { get; set; }
        public string MandateRecId { get; set; }
    }
}
