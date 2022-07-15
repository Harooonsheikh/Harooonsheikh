using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface IContactPersonController
    {
        ERPContactPersonResponse GetContactPerson(string customerAccount, string contactPersonId);
        ERPContactAllPersonResponse GetAllContactPersons (string customerAccount);
        ERPContactPersonResponse UpdateContactPerson(ErpContactPerson customerAccount);
        ERPContactPersonResponse CreateContactPerson(ErpContactPerson customerAccount, string requestId);
        
        List<ErpContactPerson> GetContactPersons(List<string> customerAccounts);
        ERPSaveContactPersonResponse SaveContactPerson(ErpContactPersonNAL contactPerson, string requestId);
    }
}
