namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpConfiguration
    {
        #region Properties
        public ErpRetailChannelProfile ChannelProfile { get; set; }
        public ErpRetailServiceProfile ServiceProfile { get; set; }
        public ErpChannel Channel { get; set; }
        #endregion Properties

        #region Constants
        public ErpConfiguration()
        {
            Channel = new ErpChannel();
            ChannelProfile = new ErpRetailChannelProfile();
            ServiceProfile = new ErpRetailServiceProfile();
        }
        #endregion Constants
    }
}
