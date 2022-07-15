using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.ViewModel
{
    public class DimensionSetVM
    {
        public int Id = 0;
        public string ErpValue = null;
        public string ComValue = null;
        public bool IsActive = false;
        public string AdditionalErpValue = null;
        public int StoreId_FK = 0;
        public DimensionSetVM()
        {

        }
    
    }
}