using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 12 Platform new class
    public class ErpNotificationItem
    {
        public ErpNotificationItem()
        {

        }

        public int? Action { get; set; }
        public string ActionName { get; set; }
        public ObservableCollection<ErpNotificationDetail> NotificationDetails { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
}
