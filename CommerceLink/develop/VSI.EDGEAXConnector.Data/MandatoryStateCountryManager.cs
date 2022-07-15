using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.Data
{
    public static class MandatoryStateCountryManager
    {
        private static List<MandatoryStateCountry> _lstCountries;

        public static List<MandatoryStateCountry> AllCountries
        {
            get
            {
                if (_lstCountries == null)
                {
                    using (var db = new IntegrationDBEntities())
                    {
                        _lstCountries = db.MandatoryStateCountry.ToList();
                    }
                }
                return _lstCountries;
            }
        }

        public static bool IsStateMandatoryForCountry(string threeLetterCountryCode)
        {
            return AllCountries.Any(a => a.ThreeLetterISORegionName.Equals(threeLetterCountryCode));
        }
    }
}
