using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpCustomerPaymentInfo
    {

        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ErpCreditCardCust> CreditCards { get; set; }
        public List<ErpBankAccountCust> BankAccounts { get; set; }

        public ErpCustomerPaymentInfo(bool success, string message, List<ErpCreditCardCust> creditCards, List<ErpBankAccountCust> bankAccounts)
        {
            this.Success = success;
            this.Message = message;
            this.CreditCards = creditCards;
            this.BankAccounts = bankAccounts;
        }
    }
}
