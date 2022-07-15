
using VSI.EDGEAXConnector.ERPDataModels;

public class ErpSalesLineDeliveryOption
{
	public ErpSalesLineDeliveryOption()
	{
	}
	public string SalesLineId	{ get; set; }//;
	public System.Collections.ObjectModel.ReadOnlyCollection<ErpDeliveryOption> DeliveryOptions	{ get; set; }//;
}
