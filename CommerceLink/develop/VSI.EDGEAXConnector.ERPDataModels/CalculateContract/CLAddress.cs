using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.CalculateContract
{
    public class CLAddress
    {
        public CLAddress()
        {
        }
        public string Name { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string Street2 { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ThreeLetterISORegionName { get; set; }
        public string Phone { get; set; }
        public string TaxGroup { get; set; }
        public ErpAddressType AddressType { get; set; }
        public int AddressTypeValue { get; set; }
        public bool IsPrimary { get; set; }
    }
}
