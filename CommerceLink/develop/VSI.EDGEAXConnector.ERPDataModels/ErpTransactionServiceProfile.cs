namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTransactionServiceProfile
	{
		public ErpTransactionServiceProfile()
		{
		}
		public string StaffPasswordHash	{ get; set; }//;
		public string PasswordEncryptionType	{ get; set; }//;
		public string DeviceTokenAlgorithm	{ get; set; }//;
		public string ProfileId	{ get; set; }//;
		public string LanguageId	{ get; set; }//;
		public ErpLogOnConfiguration StaffLogOnConfiguration	{ get; set; }//;
		public string Host	{ get; set; }//;
		public int Port	{ get; set; }//;
		public string EncryptedPassword	{ get; set; }//;
		public string ServerCertificateDns	{ get; set; }//;
		public string ServiceName	{ get; set; }//;
		public int Protocol	{ get; set; }//;
		public bool IsSecurityDisabled	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
