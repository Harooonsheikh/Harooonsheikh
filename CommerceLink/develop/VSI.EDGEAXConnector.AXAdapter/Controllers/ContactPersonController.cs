//using Autofac;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class ContactPersonController : BaseController, IContactPersonController
    {
        public ContactPersonController(string storeKey) : base(storeKey)
        {

        }

        public ERPContactPersonResponse GetContactPerson(string customerAccount, string contactPersonId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var contactPersonManager = new ContactPersonCRTManager();
            return contactPersonManager.GetContactPerson(customerAccount, currentStore.StoreKey, contactPersonId);
        }

        public ERPContactAllPersonResponse GetAllContactPersons(string customerAccount)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var contactPersonManager = new ContactPersonCRTManager();
            return contactPersonManager.GetAllContactPersons(customerAccount, currentStore.StoreKey);
        }

        public ERPContactPersonResponse UpdateContactPerson(ErpContactPerson contactPerson)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var contactPersonManager = new ContactPersonCRTManager();
            return contactPersonManager.UpdateContactPerson(contactPerson, currentStore.StoreKey);
        }

        public ERPContactPersonResponse CreateContactPerson(ErpContactPerson contactPerson, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var contactPersonManager = new ContactPersonCRTManager();

            return contactPersonManager.CreateContactPerson(contactPerson, currentStore.StoreKey, requestId);
        }

        /// <summary>
        /// GetContactPersons returns list of Contact Persons for provided list of CustomerIds.
        /// </summary>
        /// <param name="customerAccounts"></param>
        /// <returns></returns>
        public List<ErpContactPerson> GetContactPersons(List<string> customerAccounts)
        {

            var contactPersonManager = new ContactPersonCRTManager();
            var responses = contactPersonManager.GetContactPersons(customerAccounts, currentStore.StoreKey);
            List<ErpContactPerson> erpContactPersons;

            if (responses != null)
            {
                erpContactPersons = responses.Select(r => r.ContactPerson).ToList();
            }
            else
            {
                erpContactPersons = new List<ErpContactPerson>();
            }

            return erpContactPersons;
        }

        public ERPSaveContactPersonResponse SaveContactPerson(ErpContactPersonNAL contactPerson, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var contactPersonManager = new ContactPersonCRTManager();

            return contactPersonManager.SaveContactPerson(contactPerson, currentStore.StoreKey, requestId);
        }
    }
}
