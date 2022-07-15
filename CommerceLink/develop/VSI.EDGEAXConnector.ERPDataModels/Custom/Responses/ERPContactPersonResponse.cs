using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ERPContactPersonResponse
    {

        public string Message { get; set; }
        public ErpContactPerson ContactPerson { get; set; }
        public bool Success { get; set; }

        public ERPContactPersonResponse(bool success, string message, ErpContactPerson erpErpContactPerson)
        {
            this.Success = success;
            this.Message = message;
            this.ContactPerson = erpErpContactPerson;
        }

    }

    public class ERPContactAllPersonResponse
    {

        public string Message { get; set; }
        public List<ErpContactPersonNAL> ContactPersonNALList { get; set; }
        public bool Success { get; set; }

        public ERPContactAllPersonResponse(bool success, string message, List<ErpContactPersonNAL> erpContactPersonNALList)
        {
            this.Success = success;
            this.Message = message;
            this.ContactPersonNALList = erpContactPersonNALList;
        }

    }

    public class ERPSaveContactPersonResponse
    {

        public string Message { get; set; }
        public ErpContactPersonNAL ContactPerson { get; set; }
        public bool Success { get; set; }

        public ERPSaveContactPersonResponse(bool success, string message, ErpContactPersonNAL erpErpContactPerson)
        {
            this.Success = success;
            this.Message = message;
            this.ContactPerson = erpErpContactPerson;
        }

    }
}
