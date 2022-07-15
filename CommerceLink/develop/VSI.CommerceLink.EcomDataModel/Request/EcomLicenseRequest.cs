using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel.Request
{
   public class EcomLicenseRequest
    {
        public string GUID { get; set; }
        public string ItemId { get; set; }
        public decimal Quantity { get; set; }
    }
}
