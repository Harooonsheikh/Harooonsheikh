using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel
{
   public class EcomContactPerson

    {
        public EcomContactPerson()
        {
            this.SwapLanguage = false;
            this.Addresses = new List<EcomAddress>();
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
        public bool SwapLanguage { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
        public bool DirectMail { get; set; }
        public bool IsExternallyMaintained { get; set; }
        public string PrimaryURL { get; set; }
        public string Profession { get; set; }
        public string PrimaryFax { get; set; }
        public long FaxRecID { get; set; }
        public long URLRecID { get; set; }
        public List<EcomAddress> Addresses { get; set; }
        public string TMVSourceSystem { get; set; }
        public string TMVContactIDGUID { get; set; }
    }

    public class eComAddress
    {
        public long RecordId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string ThreeLetterISORegionName { get; set; }
        public string Phone { get; set; }
    }

    /// <summary>
    /// Represents contact person's address
    /// </summary>
    public class Address
    {
        /// <summary>
        /// First name of the contact person address
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// City of the contact person address
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Zip code of the contact person address
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// State of the contact person address
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Country of the contact person address
        /// </summary>
        public string ThreeLetterISORegionName { get; set; }
        /// <summary>
        /// Phone of the contact person address
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Address type of the contact person address
        /// </summary>
        public int AddressTypeValue { get; set; }
    }
}
