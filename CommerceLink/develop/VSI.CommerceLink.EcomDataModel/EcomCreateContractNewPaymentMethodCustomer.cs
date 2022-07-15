namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomCreateContractNewPaymentMethodCustomer
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public EcomAddress BillingAddress { get; set; }
    }
}
