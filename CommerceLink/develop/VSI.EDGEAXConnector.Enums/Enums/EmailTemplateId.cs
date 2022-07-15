using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Enums
{
    [ObsoleteAttribute("This enum is obsolete. Use VSI.EDGEAXConnector.Common.Enums.EmailTemplateId instead.", true)]
    public enum EmailTemplateIdOld : short
    {

        Product = 2,
        Customer = 3,
        Store = 4,
        SaleOrder = 5,
        CustomerAddress = 7,
        Inventory = 6,
        SalesOrderStatus = 8,
        Discount = 10,
        Price = 11,
        SimpleNotification = 22,
        ServiceStartNotification = 23,
        ServiceStopNotification = 24,
        ThirdPartySalesOrder = 25
    }
}
