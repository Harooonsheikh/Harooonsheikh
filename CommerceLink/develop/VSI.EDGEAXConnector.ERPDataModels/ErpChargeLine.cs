using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpChargeLine
	{
		public ErpChargeLine()
		{
		}
		public string ChargeCode	{ get; set; }//;
		public string CurrencyCode	{ get; set; }//;
		public ErpChargeModule ModuleType	{ get; set; }//;
		public int ModuleTypeValue	{ get; set; }//;
		public ErpChargeType ChargeType	{ get; set; }//;
		public int ChargeTypeValue	{ get; set; }//;
		public ErpChargeMethod ChargeMethod	{ get; set; }//;
		public int ChargeMethodValue	{ get; set; }//;
		public decimal CalculatedAmount	{ get; set; }//;
		public decimal Value	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string TransactionId	{ get; set; }//;
		public decimal SaleLineNumber	{ get; set; }//;
		public decimal FromAmount	{ get; set; }//;
		public decimal ToAmount	{ get; set; }//;
		public int Keep	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public decimal Quantity	{ get; set; }//;
		public decimal Price	{ get; set; }//;
		public string ItemTaxGroupId	{ get; set; }//;
		public string SalesTaxGroupId	{ get; set; }//;
		public decimal TaxAmount	{ get; set; }//;
		public string SalesOrderUnitOfMeasure	{ get; set; }//;
		public decimal NetAmount	{ get; set; }//;
		public decimal NetAmountPerUnit	{ get; set; }//;
		public decimal GrossAmount	{ get; set; }//;
		public System.Collections.Generic.IList<ErpTaxLine> TaxLines	{ get; set; }//;
		public decimal TaxAmountExemptInclusive	{ get; set; }//;
		public decimal TaxAmountInclusive	{ get; set; }//;
		public decimal TaxAmountExclusive	{ get; set; }//;
		public decimal NetAmountWithAllInclusiveTax	{ get; set; }//;
		public decimal NetAmountWithAllInclusiveTaxPerUnit	{ get; set; }//;
		public System.DateTimeOffset BeginDateTime	{ get; set; }//;
		public System.DateTimeOffset EndDateTime	{ get; set; }//;
		public decimal TaxRatePercent	{ get; set; }//;
		public bool IsReturnByReceipt	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 12 Platform change start
        public ObservableCollection<ErpTaxLine> ReturnTaxLines { get; set; }
        //NS: D365 Update 12 Platform change end

        //NS: D365 Update 8.1 Application change start
        public ErpTaxMeasure TaxMeasure { get; set; }

        //NS: D365 Update 8.1 Application change end

        //HK: D365 Update 10.0 Application change start
        public string ChargeLineId { get; set; }
        public int MarkupAutoTableRecId { get; set; }
        public decimal AmountRefunded { get; set; }
        public bool IsRefundable { get; set; }
        public bool IsShipping { get; set; }
        public ObservableCollection<ErpChargeLineOverride> ChargeLineOverrides { get; set; }
        public ObservableCollection<ErpReasonCodeLine> ReasonCodeLines { get; set; }
        //HK: D365 Update 10.0 Application change end

    }
}
