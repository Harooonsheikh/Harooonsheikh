using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.MongoData.Enums
{
    public enum SaleOrdersModel
    {
        [Description("orders")]
        Orders = 1,
        [Description("order")]
        Order = 2,
        [Description("XMLDocument")]
        XmlOfSalesOrder = 3
    }
}
