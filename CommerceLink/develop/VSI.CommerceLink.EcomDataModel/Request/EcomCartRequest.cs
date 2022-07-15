using VSI.CommerceLink.EcomDataModel.Enum;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class EcomCartRequest
    {
        /// <summary>
        /// CalculationModes of cart
        /// </summary>
        public EcomCalculationModes CalculationModes { get; set; }

        /// <summary>
        /// IsUpdated of cart
        /// </summary>
        public bool IsUpdate { get; set; }

        /// <summary>
        /// Cart
        /// </summary>
        public EcomCart Cart { get; set; }
    }
}
