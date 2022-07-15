namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomTaxInfo
    {
        public decimal Amount { get; set; }
        public string TaxCode { get; set; }
        public bool IsIncludedInPrice { get; set; }
        public decimal TaxPercentage { get; set; }
    }
}
