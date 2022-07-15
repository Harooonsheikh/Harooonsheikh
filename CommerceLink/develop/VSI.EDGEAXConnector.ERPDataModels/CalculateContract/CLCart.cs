using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.CalculateContract
{
    public class CLCart
    {
        public CLCart()
        {
        }
        public ObservableCollection<ErpAffiliationLoyaltyTier> AffiliationLines { get; set; }
        public ObservableCollection<CLCartLine> CartLines { get; set; }
        public long? ChannelId { get; set; }
        public ObservableCollection<ErpCoupon> Coupons { get; set; }
        public string DeliveryMode { get; set; }
        public decimal? DiscountAmount { get; set; }
        public ObservableCollection<string> DiscountCodes { get; set; }
        public string Id { get; set; }
        public DateTimeOffset? ModifiedDateTime { get; set; }
        public CLAddress ShippingAddress { get; set; }
        public decimal? SubtotalAmount { get; set; }
        public decimal? SubtotalAmountWithoutTax { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public long? Version { get; set; }
        public string WarehouseId { get; set; }


        public decimal? CLDiscountAmount { get; set; }
        public decimal? CLSubtotalAmount { get; set; }
        public decimal? CLSubtotalAmountWithoutTax { get; set; }
        public decimal? CLTaxAmount { get; set; }
        public decimal? CLTotalAmount { get; set; }

    }
}
