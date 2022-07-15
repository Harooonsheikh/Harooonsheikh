using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 8.1 Application change start
    public class ErpAuditEventFiscalTransaction
    {
        public ErpAuditEventFiscalTransaction()
        { }

        public long EventId { get; set; }
        public decimal LineNumber { get; set; }
        public string RegisterResponse { get; set; }
        public string Store { get; set; }
        public DateTimeOffset TransDateTime { get; set; }
        public string Terminal { get; set; }
        public ErpAuditEventUploadType UploadType { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
    //NS: D365 Update 8.1 Application change end
}