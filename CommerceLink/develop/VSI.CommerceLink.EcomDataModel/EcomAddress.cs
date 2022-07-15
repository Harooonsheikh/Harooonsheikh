using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomAddress
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string ThreeLetterISORegionName { get; set; }
        public string Phone { get; set; }
        public string TaxGroup { get; set; }
        public EcomAddressType AddressType { get; set; }
        public string IsPrimary { get; set; }

        // [MB] - TV - BR 3.0 - 12539 - Start
        public string BuildingCompliment { get; set; }
        public string Street2 { get; set; }
        // [MB] - TV - BR 3.0 - 12539 - End
    }
}
