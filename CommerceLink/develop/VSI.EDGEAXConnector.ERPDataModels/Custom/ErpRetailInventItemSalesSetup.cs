namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpRetailInventItemSalesSetup
    {
        #region Properties
        public long RecId { get; set; }

        public long Product { get; set; }

        public string InventDimId { get; set; }

        public string ItemId { get; set; }

        public string InventDimDefault { get; set; }

        public int MandatoryInventLocation { get; set; }

        public int MandatoryInventSize { get; set; }

        public decimal HighestQty { get; set; }

        public int LeadTime { get; set; }

        public decimal LowestQty { get; set; }

        public decimal MultipleQty { get; set; }

        public int Over_ride { get; set; } // As override is a reserved word so using Over_ride as name

        public int OverrideSalesLeadTime { get; set; }

        public decimal StandardQty { get; set; }

        public int Stopped { get; set; }

        public long Sequence { get; set; }

        public int OverrideDefaultStorageDimensions { get; set; }

        public string DataAreadId { get; set; }

        #endregion
    }
}
