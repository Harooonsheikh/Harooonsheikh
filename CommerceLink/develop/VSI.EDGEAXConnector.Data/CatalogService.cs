using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data
{
   public class CatalogService : BaseClass
    {

        public CatalogService(string storeKey) : base(storeKey)
        {

        }
        public CatalogService()
        {

        }
        public void LogCatalog(List<CatalogLogs> catalogLogs)
        {
            try
            {
                using (IntegrationDBEntities db = new IntegrationDBEntities())
                {
                    db.CatalogLogs.AddRange(catalogLogs);
                    db.SaveChanges();
                }
            }
            catch (Exception exception)
            {
            }
        }
    }
}
