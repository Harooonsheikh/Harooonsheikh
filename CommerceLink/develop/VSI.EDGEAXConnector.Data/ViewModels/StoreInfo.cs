using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data.ViewModels
{
    public class StoreInfo
    {
        public int Id { get; set; }
        public int EcomTypeId_FK { get; set; }
        public int OrganizationId_FK { get; set; }
        public string Name { get; set; }
        public string EcomName { get; set; }
        public string OrganizationName { get; set; }
        public bool? IsActive { get; set; }
    }
}
