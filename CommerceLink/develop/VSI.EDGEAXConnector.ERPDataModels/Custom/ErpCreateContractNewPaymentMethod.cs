using System;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpCreateContractNewPaymentMethod
    {
        public string EcomTransactionId { get; set; }
        public string SalesId { get; set;}
        public string CurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public ErpCreateContractNewPaymentMethodCustomer Customer { get; set; }
        public ErpTenderLine TenderLine { get; set; }
    }

    
}
