using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.UI.Managers
{
    public static class DimensionSetManager
    {
        private static DimensionSetDAL dimObjDAL = new DimensionSetDAL(StoreService.StoreLkey);

        public static List<DimensionSet> GetAllDimensionSets()
        {
            List<DimensionSet> lstDimObj = new List<DimensionSet>();

            lstDimObj = dimObjDAL.GetAllDimensionSets();

            return lstDimObj;
        }

        public static bool UpdateDimensionSetById(DimensionSet dimObj)
        {
            return dimObjDAL.UpdateDimensionSetById(dimObj);
        }
    }
}
