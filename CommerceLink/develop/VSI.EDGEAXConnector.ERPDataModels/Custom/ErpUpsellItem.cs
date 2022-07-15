namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpUpsellItem
    {
        #region "Properties"
        public string ItemId { get; set; }
        public string LinkedItemId { get; set; }
        public string LinkedProductSKU { get; set; }
        public int Priority { get; set; }
        public long RecId { get; set; }
        public ErpUpsellType UpsellTypeId { get; set; }
        public ErpTMVCrosssellType TMVCrosssellType { get; set; }
        public int TMVCrosssellBundleQuantity { get; set; }
        #endregion
    }
}
