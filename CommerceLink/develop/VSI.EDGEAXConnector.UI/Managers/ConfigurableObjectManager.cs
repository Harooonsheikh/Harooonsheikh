using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.UI.Managers
{
    public class ConfigurableObjectManager
    {
        private static ConfigurableObjectDAL configObjDAL = new ConfigurableObjectDAL(StoreService.StoreLkey);
        public static List<ConfigurableObject> GetAllConfigurableObjects()
        {
            List<ConfigurableObject> lstConfigObjects = new List<ConfigurableObject>();
            lstConfigObjects = configObjDAL.GetAllConfigurableObjects();
            return lstConfigObjects;
        }
        public static bool UpdateConfigurableObjectById(ConfigurableObject conObj)
        {
            return configObjDAL.UpdateConfigurableObjectById(conObj);
        }
    }
}
