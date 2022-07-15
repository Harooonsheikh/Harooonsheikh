using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpCustomerBankAccountResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErpBankAccount BankAccount { get; set; }

        public ErpCustomerBankAccountResponse(bool success, string message, ErpBankAccount bankAccount)
        {
            Success = success;
            Message = message;
            BankAccount = bankAccount;
        }

    }
}
