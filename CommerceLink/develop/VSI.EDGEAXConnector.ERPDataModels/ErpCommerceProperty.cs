namespace VSI.EDGEAXConnector.ERPDataModels
{

    public class ErpCommerceProperty
	{
		public ErpCommerceProperty()
		{
		}

        public ErpCommerceProperty(string key, object value)
        {
            this.Key = key;
            this.Value = value as ErpCommercePropertyValue;
        }
		public string Key	{ get; set; }//;
        public ErpCommercePropertyValue Value { get; set; }//;
	}
}
