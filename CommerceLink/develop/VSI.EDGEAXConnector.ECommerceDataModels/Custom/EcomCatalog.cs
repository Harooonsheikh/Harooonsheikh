using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ECommerceDataModels
{
    public partial class EcomCatalog
    {
        public EcomCatalog()
        {
            Categories = new List<EcomcatalogCategoryEntityCreate>();
            Products = new List<EcomcatalogProductCreateEntity>();
            CategoryAssignments = new List<EcomCategoryAssignment>();
        }
        public List<EcomcatalogCategoryEntityCreate> Categories { get; set; }
        public List<EcomcatalogProductCreateEntity> Products { get; set; }
        public List<EcomCategoryAssignment> CategoryAssignments { get; set; }
    }
}
