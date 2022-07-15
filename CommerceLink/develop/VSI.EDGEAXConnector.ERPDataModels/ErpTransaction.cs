using System;
namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTransaction
	{
		public ErpTransaction()
		{
            this.GiftCardActiveFrom = DateTime.MinValue;
            this.GiftCardExpireDate = DateTime.MinValue;
        }
		public string Id	{ get; set; }//;
		public string StoreId	{ get; set; }//;
		public string TerminalId	{ get; set; }//;
		public string StaffId	{ get; set; }//;
		public string ShiftId	{ get; set; }//;
		public string ShiftTerminalId	{ get; set; }//;
		public string Description	{ get; set; }//;
		public decimal ExchangeRate	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
        //HK: D365 Update 10.0 Application change start
        public DateTime? GiftCardActiveFrom { get; set; }
        public decimal GiftCardBalance { get; set; }
        public DateTime? GiftCardExpireDate { get; set; }

        public string GiftCardHistoryDetails { get; set; }
        public decimal GiftCardIssueAmount { get; set; }
        public string GiftCardIdMasked { get; set; }
        public string FromSafe { get; set; }
        public string ToSafe { get; set; }
        public string FromShiftTerminalId { get; set; }
        public string ToShiftTerminalId { get; set; }
        public string FromShiftId { get; set; }
        public string ToShiftId { get; set; }
        //HK: D365 Update 10.0 Application change end
	}
}
