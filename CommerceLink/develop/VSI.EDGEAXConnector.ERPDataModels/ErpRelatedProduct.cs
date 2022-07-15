namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpRelatedProduct
	{
		public ErpRelatedProduct()
		{
		}
		public long ProductRecordId	{ get; set; }//;
		public long RelatedProductRecordId	{ get; set; }//;
		public long CatalogId	{ get; set; }//;
		public string RelationName	{ get; set; }//;
		public bool IsExcludedRelation	{ get; set; }//;
		public bool IsDirectRelation	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //public override bool Equals(object obj)
        //{
        //    ErpRelatedProduct q = obj as ErpRelatedProduct;
        //    return q != null && q.RelatedProductRecordId == this.RelatedProductRecordId;
        //}
        //public override int GetHashCode()
        //{
        //    return this.RelatedProductRecordId.GetHashCode() ^ this.RelatedProductRecordId.GetHashCode();
        //}

    }
}
