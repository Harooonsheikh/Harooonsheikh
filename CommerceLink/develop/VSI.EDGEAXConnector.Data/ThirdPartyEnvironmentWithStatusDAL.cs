using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data
{
    public class ThirdPartyEnvironmentWithStatusDAL
    {

        public List<ThirdPartyEnvironmentWithStatus> GetActiveEnvironments()
        {
            using (var db = new IntegrationDBEntities())
            {
                var listOfThirdPartyEnvironmentWithStatus = db.ThirdPartyEnvironmentWithStatus.Where(x => x.IsActive == true).ToList();
                return listOfThirdPartyEnvironmentWithStatus;
            }
        }
    }
}
