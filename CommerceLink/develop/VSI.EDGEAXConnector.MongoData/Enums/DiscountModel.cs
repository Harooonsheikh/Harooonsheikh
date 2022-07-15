using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.MongoData.Enums
{
    public enum DiscountModel
    {
        [Description("header")]
        Header = 1,
        [Description("price-table")]
        PriceTable,
        [Description("discount-table")]
        DiscountTable
    }
}
