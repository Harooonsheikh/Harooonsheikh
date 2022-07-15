namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpButtonGridButton
	{
		public ErpButtonGridButton()
		{
		}
		public int Action	{ get; set; }//;
		public string ActionProperty	{ get; set; }//;
		public ErpARGBColor BackColorAsARGB	{ get; set; }//;
		public ErpARGBColor BorderColorAsARGB	{ get; set; }//;
		public int Column	{ get; set; }//;
		public int ColumnSpan	{ get; set; }//;
		public string DisplayText	{ get; set; }//;
		public ErpARGBColor FontColorAsARGB	{ get; set; }//;
		public int ButtonId	{ get; set; }//;
		public string ButtonGridId	{ get; set; }//;
		public int Row	{ get; set; }//;
		public int RowSpan	{ get; set; }//;
		public bool UseCustomLookAndFeel	{ get; set; }//;
		public string PictureAsBase64	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 8.1 Application change start
        
        public string Tooltip { get; set; }//;
        public bool EnableLiveContent { get; set; }//;
        public int NotificationContentAlignment { get; set; }//;


        //NS: D365 Update 8.1 Application change end
    }
}