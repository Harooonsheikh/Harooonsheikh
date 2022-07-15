using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VSI.CommerceLink.EcomDataModel.Enum;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class EcomCartLinesRequest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EcomCartLinesRequest()
        {
            this.CartLines = new List<EcomCartLine>();
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
        public List<EcomCartLine> CartLines { get; set; }


        /// <summary>
        /// cartVersion of cart
        /// </summary>
        public long cartVersion { get; set; }

    }
}
