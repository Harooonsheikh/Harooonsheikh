namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpCreateContractNewPaymentMethodCustomer
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ErpAddress BillingAddress { get; set; }
    }
}
