using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 9 Platform new class
    public class ErpAuditEvent
    {
        public ErpAuditEvent()
        { }

        public string RefTransactionId { get; set; }
        public string RefTerminal { get; set; }
        public string RefStore { get; set; }
        public long RefChannel { get; set; }
        public long ReferenceId { get; set; }
        public string Staff { get; set; }
        public string LogLevel { get; set; }
        public string EventMessage { get; set; }
        public string Source { get; set; }
        public int DurationInMilliseconds { get; set; }
        public string EventType { get; set; }
        public string UploadType { get; set; }
        public string Terminal { get; set; }
        public string Store { get; set; }
        public long Channel { get; set; }
        public long EventId { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }

        //NS: D365 Update 12 Platform change start
        public long ShiftId { get; set; }
        public int AuditEventTypeValue { get; set; }
        //NS: D365 Update 12 Platform change end

        //NS: D365 Update 8.1 Application change start

        public System.Collections.Generic.ICollection<ErpAuditEventFiscalTransaction> FiscalTransactions { get; set; }

        //NS: D365 Update 8.1 Application change end
    }
}
