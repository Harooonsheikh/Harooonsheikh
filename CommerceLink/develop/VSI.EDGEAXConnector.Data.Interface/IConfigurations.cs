using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data.Interface
{
    interface IConfigurations
    {
         List<ApplicationSetting> GetAllApplicationSettings();
         List<ApplicationSetting> GetApplicationSettingsByScreenName(string screenName);
         bool UpdateApplicationSettings(List<ApplicationSetting> lstApps);
        
    }
}
