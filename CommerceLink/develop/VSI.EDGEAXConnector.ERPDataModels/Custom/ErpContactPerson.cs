using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpContactPerson
    {
        public ErpContactPerson()
        {
            this.Addresses = new List<ErpAddress>();
            this.TMVContactIDGUID = string.Empty;
        }

        public long DirPartyRecordId { get; set; }
        public string ContactPersonId { get; set; }
        public long ContactForParty { get; set; }
        public string CustAccount { get; set; }
        public bool InActive { get; set; }
        public string IsPrimary { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public long EmailRecordId { get; set; }
        public string Email { get; set; }
        public long PhoneRecordId { get; set; }
        public string Phone { get; set; }
        public string Language { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
        public bool DirectMail { get; set; }
        public bool IsExternallyMaintained { get; set; }
        public string PrimaryURL { get; set; }
        public string Profession { get; set; }
        public string PrimaryFax { get; set; }
        public long FaxRecID { get; set; }
        public long URLRecID { get; set; }
        public List<ErpAddress> Addresses { get; set; }
        public string TMVSourceSystem { get; set; }
        public string TMVContactIDGUID { get; set; }
    }
}
