using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class EcomBaseRequest
    {
        [Required]
        public string EcomTransactionId { get; set; }
    }
}
