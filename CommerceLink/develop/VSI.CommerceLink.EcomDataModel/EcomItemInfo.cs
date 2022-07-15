using System.Collections.Generic;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomItemInfo
    {
        public string AddressRecordId { get; set; }
        public decimal LineNumber { get; set; }
        public string ItemId { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public string RequestedDeliveryDateString { get; set; }
        public string SourceId { get; set; }
        public string TMVCONTRACTVALIDFROM { get; set; }
        public string TMVCONTRACTCALCULATEFROM { get; set; }
        public string TMVORIGINALLINEAMOUNT { get; set; }
        public string UNIT { get; set; }

        public List<EcomTaxInfo> Taxes { get; set; }
        public List<EcomDiscountInfo> Discounts { get; set; }
        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }
    }
}
