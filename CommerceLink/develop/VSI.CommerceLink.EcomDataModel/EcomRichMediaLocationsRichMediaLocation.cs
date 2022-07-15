using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel
{
    public partial class EcomRichMediaLocationsRichMediaLocation
    {
        public EcomRichMediaLocationsRichMediaLocation()
        {
        }
        public string Url { get; set; }
        public string AltText { get; set; }
        public string DimensionKey { get; set; }
        public string DimensionValue { get; set; }
    }
}
