using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.Data
{
    public class LanguageCodesDAL : BaseClass
    {
        /// <summary>
        /// 
        /// </summary>
        public LanguageCodesDAL(string storeKey) : base(storeKey)
        {

        }

        /// <summary>
        /// Returns Ecom Langaguage Code for Erp Langauage Code.
        /// </summary>
        /// <param name="strErpLanguageCode"></param>
        /// <returns></returns>
        public  string GetEcomLanguageCode(String strErpLanguageCode)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var configurableObject = db.ConfigurableObject.FirstOrDefault(k => k.ErpValue == strErpLanguageCode && k.EntityType == (int)ConfigObjecsType.LanguageCode && k.StoreId == StoreId);
                return configurableObject?.ComValue ?? String.Empty;
            }
        }

        /// <summary>
        /// Returns Ecom Langaguage Code for payment Link Erp Langauage Code.
        /// </summary>
        /// <param name="strErpLanguageCode"></param>
        /// <returns></returns>
        public string GetEcomLanguageCodeForPaymentLink(String strErpLanguageCode)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var configurableObject = db.ConfigurableObject.FirstOrDefault(k => k.ErpValue == strErpLanguageCode && k.EntityType == (int)ConfigObjecsType.LanguageCode && k.StoreId == StoreId);

                //if language not found for store, then pick any first language
                if (configurableObject == null)
                {
                    configurableObject = db.ConfigurableObject.FirstOrDefault(k => k.EntityType == (int)ConfigObjecsType.LanguageCode && k.StoreId == StoreId);
                }

                return configurableObject?.ComValue ?? String.Empty;
            }
        }
        
        /// <summary>
        /// Returns Erp Langaguage Code for Ecom Langauage Code.
        /// </summary>
        /// <param name="strErpLanguageCode"></param>
        /// <returns></returns>
        public string GetErpLanguageCode(String strEcomLanguageCode)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var row = db.ConfigurableObject.FirstOrDefault(k => k.ComValue == strEcomLanguageCode && k.EntityType == (int)ConfigObjecsType.LanguageCode && k.StoreId == StoreId);
                return row != null ? row.ErpValue : String.Empty;
            }
        }

        /// <summary>
        /// Returns Erp Langaguage Code for Ecom Langauage Code.
        /// </summary>
        /// <param name="strErpLanguageCode"></param>
        /// <returns></returns>
        public bool ValidateErpLanguageCode(String strErpLanguageCode)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var row = db.ConfigurableObject.FirstOrDefault(k => k.ErpValue == strErpLanguageCode && k.EntityType == (int)ConfigObjecsType.LanguageCode);
                return row != null;
            }
        }

    }
}
