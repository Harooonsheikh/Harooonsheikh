using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpContractInvoicesResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ErpCustInvoiceJour> ErpCustInvoiceJourList { get; set; }

        public ErpContractInvoicesResponse(bool success, string message, List<ErpCustInvoiceJour> erpCustInvoiceJourList)
        {
            this.Success = success;
            this.Message = message;
            this.ErpCustInvoiceJourList = erpCustInvoiceJourList;
        }
    }
}
