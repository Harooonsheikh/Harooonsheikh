using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.Data
{
    public static class AosUrlManager
    {
        private static List<AosUrlSetting> _lstUrls;

        public static List<AosUrlSetting> AllUrls
        {
            get
            {
                if (_lstUrls == null)
                {
                    using (var db = new IntegrationDBEntities())
                    {
                        _lstUrls = db.AosUrlSetting.Where(a => a.IsActive).ToList();
                    }
                }
                return _lstUrls;
            }
        }

        public static string GetUrl(AosMethod method)
        {
            return AllUrls.FirstOrDefault(a => a.IsActive && a.MethodName == method.ToString())?.MethodUrl;
        }
    }
}
