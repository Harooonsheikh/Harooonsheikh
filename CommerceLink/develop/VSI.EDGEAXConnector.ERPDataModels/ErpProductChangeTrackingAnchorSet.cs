namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductChangeTrackingAnchorSet
	{
		public ErpProductChangeTrackingAnchorSet()
		{
		}
		public long ChannelId	{ get; set; }//;
		public long EcoResProduct	{ get; set; }//;
		public long EcoResProductInstanceValue	{ get; set; }//;
		public long EcoResInstanceValue	{ get; set; }//;
		public long EcoResAttribute	{ get; set; }//;
		public long EcoResAttributeValue	{ get; set; }//;
		public long EcoResValue	{ get; set; }//;
		public long EcoResBooleanValue	{ get; set; }//;
		public long EcoResDateTimeValue	{ get; set; }//;
		public long EcoResCurrencyValue	{ get; set; }//;
		public long EcoResFloatValue	{ get; set; }//;
		public long EcoResIntValue	{ get; set; }//;
		public long EcoResReferenceValue	{ get; set; }//;
		public long EcoResTextValue	{ get; set; }//;
		public long EcoResTextValueTranslation	{ get; set; }//;
		public long EcoResProductTranslation	{ get; set; }//;
		public long EcoResProductVariantColor	{ get; set; }//;
		public long EcoResProductVariantConfiguration	{ get; set; }//;
		public long EcoResProductVariantDimensionValue	{ get; set; }//;
		public long EcoResProductVariantSize	{ get; set; }//;
		public long EcoResProductVariantStyle	{ get; set; }//;
		public long EcoResColor	{ get; set; }//;
		public long EcoResConfiguration	{ get; set; }//;
		public long EcoResSize	{ get; set; }//;
		public long EcoResStyle	{ get; set; }//;
		public long EcoResAttributeGroupAttribute	{ get; set; }//;
		public long EcoResCategoryAttributeGroup	{ get; set; }//;
		public long EcoResCategoryAttributeLookup	{ get; set; }//;
		public long EcoResDistinctProductVariant	{ get; set; }//;
		public long EcoResProductCategory	{ get; set; }//;
		public long RetailCategoryContainmentLookup	{ get; set; }//;
		public long RetailPubCatalogProduct	{ get; set; }//;
		public long RetailPubCatalogProductCategory	{ get; set; }//;
		public long RetailPubCatalogProductRelation	{ get; set; }//;
		public long RetailPubCatalogProductRelationExclusion	{ get; set; }//;
		public long RetailPubInternalOrgAttributeGroup	{ get; set; }//;
		public long RetailPubIntOrgInheritanceExploded	{ get; set; }//;
		public long RetailPubProductAttributeChannelMetadata	{ get; set; }//;
		public long RetailPubProductAttributeValue	{ get; set; }//;
		public long RetailPubRetailChannelTable	{ get; set; }//;
		public long RetailSharedParameters	{ get; set; }//;
		public long RetailStandardAttribute	{ get; set; }//;
		public long UnitOfMeasure	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
