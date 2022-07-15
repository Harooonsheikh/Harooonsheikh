using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpCreditCardCust
    {
        public string CardNumber { get; set; } //
        public string CreditCardProcessors { get; set; } //
        public string CreditCardTypeName { get; set; } //
        public string CustAccount { get; set; } //
        public string ExpiryDate { get; set; } //
        public string Name { get; set; } //
        public string Notes { get; set; }
        public string UniqueCardId { get; set; }
        public string RecId { get; set; } //
        public string TMVCreditCardTokenIdOld { get; set; } //
        public string CardToken { get; set; } //
        public string EmailAddress { get; set; }
        public string ParentTransactionId { get; set; }
        public string PayerId { get; set; }
        public string Authorization { get; set; }
        public string Note{ get; set; }
        public string BankIdentificationNumberStart{ get; set; }
        public string ProcessorId { get; set; }
        public string IssuerCountry { get; set; }
        public string AccountId { get; set; }
        public string BankIBAN { get; set; }
        public string ContactPerson { get; set; }
        public string MandateRecId { get; set; }
        public string SwiftCode { get; set; }

    }
}
