using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpCreateContractNewPaymentMethodResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErpTenderLine CreditCard { get; set; }
        public ErpBankAccount BankAccount { get; set; }
        public string ErrorCode { get; set; }
        public ErpCreateContractNewPaymentMethodResponse(bool success, string message, ErpTenderLine creditCard, ErpBankAccount bankAccount,string errorcode)
        {
            this.Success = success;
            this.Message = message;
            this.CreditCard = creditCard;
            this.BankAccount = bankAccount;
            this.ErrorCode = errorcode;
        }
    }
}
