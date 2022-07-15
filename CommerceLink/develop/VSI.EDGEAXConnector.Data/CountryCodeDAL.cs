using System.Linq;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.Data
{
    public class CountryCodeDAL : BaseClass
    {
        /// <summary>
        /// CountryCodeDAL constructor
        /// </summary>
        public CountryCodeDAL(string storeKey) : base(storeKey)
        {
        }

        /// <summary>
        /// Returns Country Code by two letter iso code.
        /// </summary>
        /// <param name="twoLetterCode"></param>
        /// <returns>CountryCode</returns>
        public CountryCode GetCountryByTwoLetterCode(string twoLetterCode)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var counrtyCode = db.CountryCode.FirstOrDefault(c => c.TwoLetterCode == twoLetterCode);

                if (counrtyCode == null)
                    throw new CommerceLinkError(string.Format("Invalid or not found two letter country code = {0}", twoLetterCode));

                return counrtyCode;
            }
        }

    }
}
