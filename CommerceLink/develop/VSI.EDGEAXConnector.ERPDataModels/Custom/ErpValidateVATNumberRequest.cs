using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    /// <summary>
    /// 
    /// </summary>
    public class ErpValidateVATNumberRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string CountryId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string VATNumber { get; set; }
    }
}
