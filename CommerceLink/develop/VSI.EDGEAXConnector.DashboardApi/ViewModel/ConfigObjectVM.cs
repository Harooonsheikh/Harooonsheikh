using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.ViewModel
{
    public class ConfigObjectVM
    {
        public int Id = 0;
        public string ComValue = null;
        public string ErpValue  = null;
        public int StoreId_FK = 0;
        public int EntityType = 0;
        public int? ConnectorKey = 0;
        public ConfigObjectVM()
        {

        }
    }
}