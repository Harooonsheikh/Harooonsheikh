using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpProductQuantityDiscount
    {
        public string OfferId { get; set; }
        public string Name { get; set; }
        public ErpRetailDiscountConcurrency ConcurrencyMode { get; set; }
        public string CurrencyCode { get; set; }
        public DateTimeOffset ValidFrom { get; set; }
        public DateTimeOffset ValidTo { get; set; }
        public ErpMultiBuyDiscountType MultiBuyDiscountType { get; set; }
        public List<ErpQuantityDiscountConfiguration> QuantityDiscountConfiguration { get; set; }
        public List<long> Categories { get; set; }
        public List<long> Products { get; set; }
        public List<long> Variants { get; set; }
        public Dictionary<long, List<long>> CategoryProduct {get; set;}  
        public Dictionary<long, Dictionary<long, List<long>>> CategoryProductVariant { get; set; }
        public List<string> SKUs { get; set; }
        public int PeriodicDiscountType { get; set; }
    }
}
