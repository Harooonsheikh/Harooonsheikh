-- Update Mapping Template for new elements

update MappingTemplate set [SourceEntity] = 'ErpItemQuantityDiscount', [Name] = 'CREATE.ErpItemQuantityDiscount', [XML] =   
N'<Items>
  <item repeat="true" data-source="ErpItemQuantityDiscount~ProductItemQuantityDiscounts">
    <sku>ProductItemQuantityDiscounts~SKU</sku>
    <tier_price_website>base</tier_price_website>
    <tier_price_customer_group>ALL GROUPS</tier_price_customer_group>
    <tier_price_qty>ProductItemQuantityDiscounts~TierPriceQuantity</tier_price_qty>
    <tier_price>ProductItemQuantityDiscounts~TierPrice</tier_price>
    <tier_price_value_type expression="true">sourceObject.TierPriceValueType == ErpMultiBuyDiscountType.UnitPrice ? "Fixed" : "Discount"</tier_price_value_type>
	<offer-id>ProductItemQuantityDiscounts~OfferId</offer-id>
	<discount-name>ProductItemQuantityDiscounts~DiscountName</discount-name>
	<multi-buy-discount-type>ProductItemQuantityDiscounts~TierPriceValueType</multi-buy-discount-type>
	<discount-percentage expression="true">sourceObject.TierPriceValueType == ErpMultiBuyDiscountType.DiscountPercentage ? sourceObject.TierPrice.ToString() : "0"</discount-percentage>
	<discount-line-type-value expression="true">(int)sourceObject.TierPriceValueType</discount-line-type-value>
	<periodic-discount-type>ProductItemQuantityDiscounts~PeriodicDiscountType</periodic-discount-type>
	<manual-discount-type-value>0</manual-discount-type-value>
	<customer-discount-type-value>0</customer-discount-type-value>
  </item>
</Items>'
where [SourceEntity] = 'ErpItemQuantityDiscount'

-- delete un necessary keys

delete from ApplicationSetting where [Key] = 'QUANTITYDISCOUNT.Data_Area_Id'
