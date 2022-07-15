using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface IContactPersonController
    {
        ERPContactPersonResponse GetContactPerson(string customerAccount, string contactPersonId);
        ERPContactPersonResponse UpdateContactPerson(ErpContactPerson contactPerson);

        /// <summary>
        /// CreateContactPerson creates Contact Person.
        /// </summary>
        /// <param name="contactPerson"></param>
        /// <returns></returns>
        ERPContactPersonResponse CreateContactPerson(ErpContactPerson contactPerson, string requestId);

        /// <summary>
        /// GetContactPersons returns list of Contact Persons for provided list of CustomerIds.
        /// </summary>
        /// <param name="customerAccounts"></param>
        /// <returns></returns>
        List<ERPContactPersonResponse> GetContactPersons(List<string> customerAccounts);
        ERPContactAllPersonResponse GetAllContactPersons(string customerAccount);
        ERPSaveContactPersonResponse SaveContactPerson(ErpContactPersonNAL contactPerson, string requestId);
    }
}
