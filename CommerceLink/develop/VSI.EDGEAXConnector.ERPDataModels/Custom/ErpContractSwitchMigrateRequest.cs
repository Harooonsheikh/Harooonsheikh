using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels.BoletoPayment;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ProcessContractOperationRequest
    {
        [Required]
        public string RequestNumber { get; set; }

        [Required]
        public string RequestDate { get; set; }

        [Required]
        public string ContractAction { get; set; }

        [Required]
        public string UseOldContractDates { get; set; }

        [Required]
        public string CurrencyCode { get; set; }

        [Required]
        public string PrimaryPacLicense { get; set; }

        [Required] public string SalesOrigin { get; set; } = "";

        public List<ContractLine> ContractLines { get; set; }

        public ProcessContractOperationRequest()
        {
            this.ContractLines = new List<ContractLine>();
            this.UseOldContractDates = "false";
        }
        public List<TenderLineInformation> TenderLine { get; set; }
        public CustomerInformation CustomerInformation { get; set; }
    }
    public class ContractLine
    {
        public string LineNumber { get; set; }
        public string TargetPrice { get; set; }
        public string ProductId { get; set; }
        public string ItemId { get; set; }
        public string VariantId { get; set; }
        public string Quantity { get; set; }
        public string SalesLineAction { get; set; }
        public string OldLinePacLicense { get; set; }
        public string ParentLineNumber { get; set; }
        public string CustomerRef { get; set; } = "";
    }

    public class CustomerInformation
    {
        public string CustomerEmail { get; set; }
        public string Phone { get; set; }
	    public string VATNumber { get; set; }
        public ErpAddress BillingAddress { get; set; }
    }

    public class TenderLineInformation {
        public string CardOrAccount { get; set; }
        public string ThreeDSecure { get; set; }
        public string CardToken { get; set; }
        public string AuthorizationToken { get; set; }
        public string TenderTypeId { get; set; }
        public decimal? AuthorizationAmount { get; set; }
        public string UniqueCardId { get; set; }
        public string IBAN { get; set; }
        public string BankName { get; set; }
        public string SwiftCode { get; set; }
        public Boleto Boleto { get; set; }
    }
}
