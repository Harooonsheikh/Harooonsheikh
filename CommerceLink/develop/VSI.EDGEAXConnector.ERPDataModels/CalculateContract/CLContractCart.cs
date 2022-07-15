using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.CalculateContract
{
    public class CLContractCart
    {
        [Required]
        public string SalesOrderId { get; set; }
        public DateTimeOffset TMVContractStartDate { get; set; }
        public DateTimeOffset TMVContractEndDate { get; set; }
        [Required]
        public string TMVSubscriptionWeight { get; set; }
        public DateTimeOffset RequestDate { get; set; }
        public bool UseOldContractDates { get; set; }
        public long AffiliationId { get; set; }
        public List<CLContractCartLine> CartLines { get; set; }
        public CLDeliverySpecification DeliverySpecification { get; set; }
        public ObservableCollection<CLCoupon> CouponCodes { get; set; }
    }
}
