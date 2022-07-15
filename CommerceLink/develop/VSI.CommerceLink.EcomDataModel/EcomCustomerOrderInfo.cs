using System.Collections.Generic;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomCustomerOrderInfo
    {
        public string OrderType { get; set; }
        public string CustomerAccount { get; set; }
        public string TransactionId { get; set; }
        public string RequestedDeliveryDateString { get; set; }
        public string ChannelReferenceId { get; set; }
        public string ExpiryDateString { get; set; }
        public string CurrencyCode { get; set; }
        public string AddressRecordId { get; set; }
        public bool IsEcomCustomerId { get; set; }

        public List<EcomItemInfo> Items { get; set; }
        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }
    }
}
