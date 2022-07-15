using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.Data
{
    public class StoreCodesDAL : BaseClass
    {
        /// <summary>
        /// 
        /// </summary>
        public StoreCodesDAL(string storeKey) : base(storeKey)
        {

        }

        /// <summary>
        /// Returns Ecom Store Code for Erp Store Code.
        /// </summary>
        /// <param name="strErpStoreCode"></param>
        /// <returns></returns>
        public string GetEcomStoreCode(String strErpStoreCode)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var configurableObject = db.ConfigurableObject.FirstOrDefault(k => k.ErpValue == strErpStoreCode && k.EntityType == (int)ConfigObjecsType.StoreCode && k.StoreId == StoreId);
                return configurableObject?.ComValue ?? String.Empty;
            }
        }

        public string GetEcomStoreCodeForPaymentLink(String strErpStoreCode)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var configurableObject = db.ConfigurableObject.FirstOrDefault(k => k.ErpValue == strErpStoreCode && k.EntityType == (int)ConfigObjecsType.StoreCode && k.StoreId == StoreId);

                //if store code not found for store, then pick any first store code
                if (configurableObject == null)
                {
                    configurableObject = db.ConfigurableObject.FirstOrDefault(k => k.EntityType == (int)ConfigObjecsType.StoreCode && k.StoreId == StoreId);
                }

                return configurableObject?.ComValue ?? String.Empty;
            }
        }

        /// <summary>
        /// Returns All relevant distinct Ecom Store Code for Erp Store Code against active stores
        /// </summary>
        /// <param name="strErpStoreCode"></param>
        /// <returns></returns>
        public List<String> GetAllRelevantEcomStoreCode(String strErpStoreCode)
        {
            List<string> storeCodesList = null;

            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                storeCodesList = db.ConfigurableObject.Where(k => (k.ErpValue == strErpStoreCode && k.EntityType == (int)ConfigObjecsType.StoreCode && k.Store.IsActive == true)).Select(m => m.ComValue).Distinct().ToList();
            }

            return storeCodesList;
        }

        /// <summary>
        /// Returns Erp Store Code for Ecom Store Code.
        /// </summary>
        /// <param name="strEcomStoreCode"></param>
        /// <returns></returns>
        public string GetErpStoreCode(String strEcomStoreCode)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var row = db.ConfigurableObject.FirstOrDefault(k => k.ComValue == strEcomStoreCode && k.EntityType == (int)ConfigObjecsType.StoreCode && k.StoreId == StoreId);
                return row != null ? row.ErpValue : String.Empty;
            }
        }

        public static List<IGrouping<string,ConfigurableObject>> GetRegionWiseStoreCodes()
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var regions = db.ConfigurableObject.Where(x => x.EntityType == (int)ConfigObjecsType.StoreCode).GroupBy(x => x.Description).ToList();
                return regions;
            }
        }
    }
}
