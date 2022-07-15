using System.Collections.Generic;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpCreditCardResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ErpCreditCardCust> CreditCards { get; set; }

        public ErpCreditCardResponse(bool success, string message, List<ErpCreditCardCust> creditCards)
        {
            this.Success = success;
            this.Message = message;
            this.CreditCards = creditCards;
        }
    }
}
