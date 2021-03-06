UPDATE	MappingTemplate
SET		[XML] = '<ObjectToCSVMapping>
  <ObjectInfo fullyQualifiedName="VSI.EDGEAXConnector.ERPDataModels.ErpProductItemQuantityDiscountWithAffiliation" objectCsvMappingName="ErpProductItemQuantityDiscountWithAffiliation_To_Discount_Csv">
    <CsvHeader>
      <HeaderItem orderid="0" name="sku" />
      <HeaderItem orderid="1" name="affiliation_id" />
      <HeaderItem orderid="2" name="affiliation_name" />
      <HeaderItem orderid="3" name="tier_price_qty" />
      <HeaderItem orderid="4" name="tier_price" />
      <HeaderItem orderid="5" name="tier_price_value_type" />
      <HeaderItem orderid="6" name="special_price_from_date" />
      <HeaderItem orderid="7" name="special_price_to_date" />
      <HeaderItem orderid="8" name="attribute_set_code" />
      <HeaderItem orderid="9" name="product_type" />
      <HeaderItem orderid="10" name="offer_id" />
      <HeaderItem orderid="11" name="discount_name" />
      <HeaderItem orderid="12" name="discount_percentage" />
      <HeaderItem orderid="13" name="discount_line_type_value" />
      <HeaderItem orderid="14" name="discount_type" />
      <HeaderItem orderid="15" name="periodic_discount_type" />
      <HeaderItem orderid="16" name="manual_discount_type_value" />
      <HeaderItem orderid="17" name="customer_discount_type_value" />
    </CsvHeader>
    <DataMember headerItemOrder="0" className="string" instanceName="SKU" isPrimitveType="true" />
    <DataMember headerItemOrder="1" className="string" instanceName="AffiliationId" isPrimitveType="true" />
    <DataMember headerItemOrder="2" className="string" instanceName="AffiliationName" isPrimitveType="true" />
    <DataMember headerItemOrder="3" className="decimal" instanceName="TierPriceQuantity" isPrimitveType="true" />
    <DataMember headerItemOrder="4" className="decimal" instanceName="TierPrice" isPrimitveType="true" />
    <DataMember headerItemOrder="5" className="string" instanceName="TierPriceValueTypeUpdatedValue" isPrimitveType="true" />
    <DataMember headerItemOrder="6" className="string" instanceName="ValidationFrom" isPrimitveType="true" />
    <DataMember headerItemOrder="7" className="string" instanceName="ValidationTo" isPrimitveType="true" />
    <DataMember headerItemOrder="8" className="string" instanceName="" isPrimitveType="true" defaultValue="TeamViewer" />
    <DataMember headerItemOrder="9" className="string" instanceName="" isPrimitveType="true" defaultValue="Virtual" />
    <DataMember headerItemOrder="10" className="string" instanceName="OfferId" isPrimitveType="true" />
    <DataMember headerItemOrder="11" className="string" instanceName="DiscountName" isPrimitveType="true" />
    <DataMember headerItemOrder="12" className="string" instanceName="DiscountPercentageUpdatedValue" isPrimitveType="true" />
    <DataMember headerItemOrder="13" className="string" instanceName="TierPriceValueType" isPrimitveType="true" />
    <DataMember headerItemOrder="14" className="string" instanceName="DiscountType" isPrimitveType="true" />
    <DataMember headerItemOrder="15" className="string" instanceName="PeriodicDiscountType" isPrimitveType="true" />
    <DataMember headerItemOrder="16" className="string" instanceName="" isPrimitveType="true" defaultValue="0" />
    <DataMember headerItemOrder="17" className="string" instanceName="" isPrimitveType="true" defaultValue="0" />
  </ObjectInfo>
</ObjectToCSVMapping>'
WHERE	[Name] = 'CREATE.ErpItemQuantityDiscountWithAffiliation'
AND		MappingTemplateTypeId = 2;