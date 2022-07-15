using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
//HK: D365 Update 10.0 Application change start
    public class ErpSuspendedCart
    {
            public string Id { get; set; }
            public string ReceiptId { get; set; }
            public long? ShiftId { get; set; }
            public string ShiftTerminalId { get; set; }
            public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
            public ErpCart Cart { get; set; }

    }
//HK: D365 Update 10.0 Application change end
}
