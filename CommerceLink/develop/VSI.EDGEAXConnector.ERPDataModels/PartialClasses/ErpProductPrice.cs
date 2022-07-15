using System;
namespace VSI.EDGEAXConnector.ERPDataModels
{	
	public partial class ErpProductPrice
	{
		public string SKU	{ get; set; }
        public string RetailVariantId { get; set; }
        public DateTime ValidTo { get; set; }
        public string EcomProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal POSCost { get; set; }
        public string ValidFromString { get; set; }
        public string ValidToString { get; set; }


        //NS: Add because exist in CRT object
        public decimal DiscountAmount { get; set; }
        public bool IsVariantPrice { get; }

        // This is required for Magento CSV
        public string ColorId { get; set; }
        // This is required for Magento CSV
        public string SizeId { get; set; }
        public string TMV_ProductType { get; set; }

        public string OfferId { get; set; }
        public string OfferName { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int DiscountLineTypeValue { get; set; }
        public string ManualDiscountTypeValue { get; set; }
        public string CustomerDiscountTypeValue { get; set; }
        public int PeriodicDiscountTypeValue { get; set; }
        public decimal OfferPrice { get; set; }
        public ErpRetailDiscountOfferLineDiscMethodBase DiscountMethod { get; set; }

    }

}
