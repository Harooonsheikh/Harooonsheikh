using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class ContactPersonCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public ContactPersonCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public ERPContactPersonResponse GetContactPerson(string customerAccount,string storeKey, string contactPersonId)
        {
            var contactPersonController = _crtFactory.CreateContactPersonController(storeKey);
            return contactPersonController.GetContactPerson(customerAccount, contactPersonId);
        }
        public ERPContactAllPersonResponse GetAllContactPersons(string customerAccount, string storeKey)
        {
            var contactPersonController = _crtFactory.CreateContactPersonController(storeKey);
            return contactPersonController.GetAllContactPersons(customerAccount);
        }

        public ERPContactPersonResponse UpdateContactPerson(ErpContactPerson contactPerson, string storeKey)
        {
            var contactPersonController = _crtFactory.CreateContactPersonController(storeKey);
            return contactPersonController.UpdateContactPerson(contactPerson);
        }

        public ERPContactPersonResponse CreateContactPerson(ErpContactPerson contactPerson, string storeKey, string requestId)
        {
            var contactPersonController = _crtFactory.CreateContactPersonController(storeKey);

            return contactPersonController.CreateContactPerson(contactPerson, requestId);
        }

        /// <summary>
        /// GetContactPersons returns list of Contact Persons for provided list of CustomerIds.
        /// </summary>
        /// <param name="customerAccounts"></param>
        /// <returns></returns>
        public List<ERPContactPersonResponse> GetContactPersons(List<string> customerAccounts, string storeKey)
        {
            var contactPersonController = _crtFactory.CreateContactPersonController(storeKey);
            return contactPersonController.GetContactPersons(customerAccounts);
        }

        public ERPSaveContactPersonResponse SaveContactPerson(ErpContactPersonNAL contactPerson, string storeKey, string requestId)
        {
            var contactPersonController = _crtFactory.CreateContactPersonController(storeKey);
            return contactPersonController.SaveContactPerson(contactPerson, requestId);
        }
    }
}
