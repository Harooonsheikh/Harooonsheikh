using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.MongoData.Enums
{
    public enum CatalogModel
    {
        [Description("product")]
        Product =1 ,
        [Description("category")]
        Category ,
        [Description("category-assignment")]
        CategoryAssignment ,
        [Description("catalog")]
        Catalog,
        [Description("pricebook")]
        PriceBook,
        [Description("discount")]
        Discount,
        [Description("timestamp")]
        timestamp
    }
}
