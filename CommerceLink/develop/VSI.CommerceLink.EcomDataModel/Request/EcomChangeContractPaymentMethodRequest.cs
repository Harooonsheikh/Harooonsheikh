using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class EcomChangeContractPaymentMethodRequest
    {
        [Required]
        public string SalesId { get; set; }
        public long NewPaymentMethodRecId { get; set; }
        [Required]
        public string TenderTypeId { get; set; }
        [Required]
        public long BankAccountRecId { get; set; }
    }
}
