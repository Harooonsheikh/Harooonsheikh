namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpValidationPeriod
	{
		public ErpValidationPeriod()
		{
		}
		public long RecordId	{ get; set; }//;
		public string PeriodId	{ get; set; }//;
		public string Description	{ get; set; }//;
		public System.DateTimeOffset ValidFrom	{ get; set; }//;
		public System.DateTimeOffset ValidTo	{ get; set; }//;
		public int StartingTime	{ get; set; }//;
		public int EndingTime	{ get; set; }//;
		public int IsTimeBounded	{ get; set; }//;
		public int MondayStartingTime	{ get; set; }//;
		public int MondayEndingTime	{ get; set; }//;
		public int IsMondayTimeBounded	{ get; set; }//;
		public int TuesdayStartingTime	{ get; set; }//;
		public int TuesdayEndingTime	{ get; set; }//;
		public int IsTuesdayTimeBounded	{ get; set; }//;
		public int WednesdayStartingTime	{ get; set; }//;
		public int WednesdayEndingTime	{ get; set; }//;
		public int IsWednesdayTimeBounded	{ get; set; }//;
		public int ThursdayStartingTime	{ get; set; }//;
		public int ThursdayEndingTime	{ get; set; }//;
		public int IsThursdayTimeBounded	{ get; set; }//;
		public int FridayStartingTime	{ get; set; }//;
		public int FridayEndingTime	{ get; set; }//;
		public int IsFridayTimeBounded	{ get; set; }//;
		public int SaturdayStartingTime	{ get; set; }//;
		public int SaturdayEndingTime	{ get; set; }//;
		public int IsSaturdayTimeBounded	{ get; set; }//;
		public int SundayStartingTime	{ get; set; }//;
		public int SundayEndingTime	{ get; set; }//;
		public int IsSundayTimeBounded	{ get; set; }//;
		public int IsEndTimeAfterMidnight	{ get; set; }//;
		public int IsMondayEndTimeAfterMidnight	{ get; set; }//;
		public int IsTuesdayEndTimeAfterMidnight	{ get; set; }//;
		public int IsWednesdayEndTimeAfterMidnight	{ get; set; }//;
		public int IsThursdayEndTimeAfterMidnight	{ get; set; }//;
		public int IsFridayEndTimeAfterMidnight	{ get; set; }//;
		public int IsSaturdayEndTimeAfterMidnight	{ get; set; }//;
		public int IsSundayEndTimeAfterMidnight	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
