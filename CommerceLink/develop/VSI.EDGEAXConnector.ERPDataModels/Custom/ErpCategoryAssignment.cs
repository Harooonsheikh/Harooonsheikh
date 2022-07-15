namespace VSI.EDGEAXConnector.ERPDataModels
{
    public partial class ErpCategoryAssignment
    {
        public string ProductId { get; set; }
        public string CategoryId { get; set; }
        public bool PrimaryFlag { get; set; }
        public ErpChangeMode Mode { get; set; }
    }
}
