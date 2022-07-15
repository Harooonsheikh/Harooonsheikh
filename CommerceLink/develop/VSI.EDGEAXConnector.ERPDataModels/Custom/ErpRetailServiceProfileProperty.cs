namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpRetailServiceProfileProperty
    {
        #region Properties
        public int Active { get; set; }
        public string Name { get; set; }
        public long ServiceProfileId { get; set; }
        public long ServiceProfilePropertyId { get; set; }
        public int ServiceTypeId { get; set; }
        //public string ServiceTypeName { get; set; }
        public string ServiceUrl { get; set; }
        #endregion Properties

        #region Constructor
        public ErpRetailServiceProfileProperty()
        {

        }
        #endregion Constructor
    }
}