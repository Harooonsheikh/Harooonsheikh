using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 8.1 Application change start
    public class ErpFiscalTransaction
    {
		//HK: D365 Update 10.0 Application change start
        public ObservableCollection<ErpFiscalTransactionSalesLineAdjustment> SalesLineAdjustments { get; set; }
        public ObservableCollection<ErpFiscalTransactionTenderLineAdjustment> TenderLineAdjustments { get; set; }
        public string RegisterInfo { get; set; }
        public string ConnectorFunctionalityProfileId { get; set; }
        public string ConnectorName { get; set; }
        public string ConnectorGroup { get; set; }
        public string RegistrationProcessId { get; set; }
        public int? RegistrationStatusValue { get; set; }
        public string StaffId { get; set; }
        public ObservableCollection<ErpReasonCodeLine> ReasonCodeLines { get; set; }
        public string RegisterTerminalId { get; set; }
        public Guid? RecordGUID { get; set; }
        public bool? ReceiptCopy { get; set; }
        public string RegisterResponse { get; set; }
        public decimal? LineNumber { get; set; }
        public DateTimeOffset? TransDateTime { get; set; }
        public string TransactionId { get; set; }
        public string TerminalId { get; set; }
        public string StoreId { get; set; }
        public DateTimeOffset? CreatedDateTime { get; set; }
        public string RegisterStoreId { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
		//HK: D365 Update 10.0 Application change end
    }
    //NS: D365 Update 8.1 Application change end
}
