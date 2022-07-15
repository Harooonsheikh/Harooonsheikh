using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpStore
    {
        public string Status { get; set; }
        public string StoreId { get; set; }
        public string Name { get; set; }
        public bool isEnabled { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }

        public string Email { get; set; }
        public string Link { get; set; }
        public string Zoom { get; set; }
        public string ImageName { get; set; }
        public string TagStore { get; set; }



    }
}
