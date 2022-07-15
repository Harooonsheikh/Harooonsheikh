using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpBankAccount
    {
        public long BankAccountRecId { get; set; }
        public long MandateRecId { get; set; }

        public ErpBankAccount(long bankAccountRecId, long mandateRecId)
        {
            BankAccountRecId = bankAccountRecId;
            MandateRecId = mandateRecId;
        }
    }
}
