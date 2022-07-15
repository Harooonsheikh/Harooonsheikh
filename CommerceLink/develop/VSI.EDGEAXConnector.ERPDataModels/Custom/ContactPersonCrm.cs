using System.Collections.Generic;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ContactPersonCrm
    {
        public ContactPersonCrm()
        {
            SwapLanguage = true;
            this.TMVContactIDGUID = string.Empty;
        }

        public long DirPartyRecordId { get; set; }
        public string ContactPersonId { get; set; }
        public long ContactForParty { get; set; }
        public string CustAccount { get; set; }
        public bool Inactive { get; set; }
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
        
        public IList<ContactPersonAddress> Addresses { get; set; }

        public bool DirectMail { get; set; }
        public string Department { get; set; }
        public string Gender { get; set; }
        public bool IsExternallyMaintained { get; set; }
        public long URLRecordId { get; set; }
        public string PrimaryURL { get; set; }
        public long FaxRecordId { get; set; }
        public string PrimaryFax { get; set; }
        public string Profession { get; set; }
        public bool SwapLanguage { get; set; }
        public string TMVSourceSystem { get; set; }
        public string TMVContactIDGUID { get; set; }
    }

    public class ContactPersonAddress
    {
        public string AddressName { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}