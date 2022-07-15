using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.CommerceLink.EcomDataModel.Enum;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class EcomAddUpdateCartLinesRequest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EcomAddUpdateCartLinesRequest()
        {
            this.CartLines = new List<EcomAddUpdateCartLine>();
        }

        /// <summary>
        /// cartId of cart
        /// </summary>
        [Required]
        public string CartId { get; set; }

        /// <summary>
        /// CalculationModes of cart
        /// </summary>
        public EcomCalculationModes CalculationModes { get; set; }

        /// <summary>
        /// Lines of cart
        /// </summary>
        public List<EcomAddUpdateCartLine> CartLines { get; set; }


        /// <summary>
        /// cartVersion of cart
        /// </summary>
        public long cartVersion { get; set; }

        /// <summary>
        /// cartVersion of cart
        /// </summary>
        public List<string> RemoveLineIds { get; set; }
    }
}
