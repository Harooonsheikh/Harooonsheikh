using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.UI.Managers
{
    public class ApplicationSettingManager
    {
        private static ApplicationSettingsDAL appsDAL = new ApplicationSettingsDAL(StoreService.StoreLkey);

        public static List<ApplicationSetting> GetApplicationSettingsByScreenName(string screenName)
        {
            List<ApplicationSetting> lstApps = new List<ApplicationSetting>();

            lstApps = appsDAL.GetApplicationSettingsByScreenName(screenName);

            return lstApps;
        }

        public static bool UpdateApplicationSettings(List<ApplicationSetting> lstApps)
        {
            bool result = appsDAL.UpdateApplicationSettings(lstApps);

            return result;
        }
    }
}
