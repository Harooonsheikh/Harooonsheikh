/* Drop the column "CsvHeaders" from table "MappingTemplate" if it exists */
IF EXISTS(	SELECT	*
			FROM	[sys].[tables] [t]
			INNER JOIN	[sys].[columns] [c] ON [t].[object_id] = [c].[object_id]
			WHERE		[t].[name] = 'MappingTemplate'
			AND			[c].[name] = 'CsvHeaders'
			)
BEGIN
	ALTER TABLE [dbo].[MappingTemplate] DROP COLUMN [CsvHeaders]
	PRINT('Column dropped');
END
ELSE BEGIN
	PRINT('Column already dropped');
END
GO

/* Before inserting the new mapping templates first remove the old mapping template for all stores so that there are no duplicates */
DELETE FROM	[dbo].[MappingTemplate]
WHERE		[MappingTemplateTypeId] = 2
AND			[Name] IN ('CREATE.ErpDiscount', 'CREATE.ErpPrice', 'CREATE.ErpDiscountWithAffiliation', 'CREATE.ErpItemQuantityDiscountWithAffiliation');

SELECT StoreId
INTO #ControlTable 
FROM dbo.Store

DECLARE @StoreId INT

WHILE exists (SELECT * FROM #ControlTable)
BEGIN

    SELECT @StoreId = (SELECT TOP 1 StoreId
                       FROM #ControlTable
                       ORDER BY StoreId ASC)



-- Run for every store
-------------------------------------------------------------

INSERT [dbo].[MappingTemplate] ([SourceEntity], [Name], [MappingTemplateTypeId], [XML], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive], [StoreId], [ReadMode]) 
VALUES (N'ErpDiscount', N'CREATE.ErpDiscount', 2, 
N'<ObjectToCSVMapping>

	<!--For Discount-->
	<ObjectInfo fullyQualifiedName="VSI.EDGEAXConnector.ERPDataModels.ErpProductDiscount" objectCsvMappingName="ErpProducDiscount_To_Discount_Csv">
	
		<CsvHeader>
		
			<HeaderItem orderid="0" name="sku" />
			<HeaderItem orderid="1" name="special_price" />
			<HeaderItem orderid="2" name="special_price_from_date" />
			<HeaderItem orderid="3" name="special_price_to_date" />
			<HeaderItem orderid="4" name="attribute_set_code" />
			<HeaderItem orderid="5" name="product_type" />
			<HeaderItem orderid="6" name="promotion_id" />
			<HeaderItem orderid="7" name="offer_name" />
			<HeaderItem orderid="8" name="discount_line_type_value" />
			<HeaderItem orderid="9" name="manual_discount_type_value" />
			<HeaderItem orderid="10" name="customer_discount_type_value" />
			<HeaderItem orderid="11" name="periodic_discount_type_value" />
			<HeaderItem orderid="12" name="discount_method" />
			<HeaderItem orderid="13" name="discount_percentage" />
			<HeaderItem orderid="14" name="discount_amount" />
			<HeaderItem orderid="15" name="discount_price" />
			
		</CsvHeader>

		<DataMember headerItemOrder="0" className="string" instanceName="SKU" isPrimitveType="true"  />
		<DataMember headerItemOrder="1" className="decimal" instanceName="OfferPrice" isPrimitveType="true"  />
		<DataMember headerItemOrder="2" className="string" instanceName="ValidationFrom" isPrimitveType="true" />
		<DataMember headerItemOrder="3" className="string" instanceName="ValidationTo" isPrimitveType="true"  />
		<DataMember headerItemOrder="4" className="string" instanceName="" isPrimitveType="true" defaultValue="TeamViewer" />
		<DataMember headerItemOrder="5" className="string" instanceName="TMV_ProductType" isPrimitveType="true" />
		<DataMember headerItemOrder="6" className="string" instanceName="OfferId" isPrimitveType="true" />
		<DataMember headerItemOrder="7" className="string" instanceName="DiscountName" isPrimitveType="true" />
		<DataMember headerItemOrder="8" className="string" instanceName="DiscountLineTypeValue" isPrimitveType="true" />
		<DataMember headerItemOrder="9" className="string" instanceName="" isPrimitveType="true" defaultValue="0" />
		<DataMember headerItemOrder="10" className="string" instanceName="" isPrimitveType="true" defaultValue="0" />
		<DataMember headerItemOrder="11" className="string" instanceName="PeriodicDiscountTypeValue" isPrimitveType="true" />
		<DataMember headerItemOrder="12" className="string" instanceName="DiscountMethod" isPrimitveType="true"  />
		<DataMember headerItemOrder="13" className="string" instanceName="DiscPct" isPrimitveType="true" />
		<DataMember headerItemOrder="14" className="string" instanceName="DiscAmount" isPrimitveType="true" />
		<DataMember headerItemOrder="15" className="string" instanceName="DiscPrice" isPrimitveType="true" />
    
	</ObjectInfo>

</ObjectToCSVMapping>', GETUTCDATE(), N'1', NULL, NULL, 1, @StoreId, N'CREATE')

INSERT [dbo].[MappingTemplate] ([SourceEntity], [Name], [MappingTemplateTypeId], [XML], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive], [StoreId], [ReadMode]) 
VALUES (N'ErpPrice', N'CREATE.ErpPrice', 2, 
N'<ObjectToCSVMapping>
	<ObjectInfo fullyQualifiedName="VSI.EDGEAXConnector.ERPDataModels.ErpProductPrice" objectCsvMappingName="ErpProductPrice_To_Price_Csv">
		<CsvHeader>
			<HeaderItem orderid="0" name="sku" />
			<HeaderItem orderid="1" name="price" />
			<HeaderItem orderid="2" name="attribute_set_code" />
			<HeaderItem orderid="3" name="product_type" />
		</CsvHeader>
		<DataMember headerItemOrder="0" className="string" instanceName="SKU" isPrimitveType="true" />
		<DataMember headerItemOrder="1" className="decimal" instanceName="BasePrice" isPrimitveType="true" />
		<DataMember headerItemOrder="2" className="string" instanceName="" isPrimitveType="true" defaultValue="TeamViewer" />
		<DataMember headerItemOrder="3" className="string" instanceName="TMV_ProductType" isPrimitveType="true" />
	</ObjectInfo>
</ObjectToCSVMapping>', GETUTCDATE(), N'1', NULL, NULL, 1, @StoreId, N'CREATE')

INSERT [dbo].[MappingTemplate] ([SourceEntity], [Name], [MappingTemplateTypeId], [XML], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive], [StoreId], [ReadMode]) 
VALUES (N'ErpDiscountWithAffiliation', N'CREATE.ErpDiscountWithAffiliation', 2, 
N'<ObjectToCSVMapping>

	<!--For Discount with Affiliation-->
	<ObjectInfo fullyQualifiedName="VSI.EDGEAXConnector.ERPDataModels.ErpProductDiscountWithAffiliation" objectCsvMappingName="ErpProducDiscountWithAffiliation_To_Discount_Csv">

		<CsvHeader>

			<HeaderItem orderid="0" name="sku" />
			<HeaderItem orderid="1" name="affiliation_recid" />
			<HeaderItem orderid="2" name="affiliation_name" />
			<HeaderItem orderid="3" name="source_discounted_price" />
			<HeaderItem orderid="4" name="special_price_from_date" />
			<HeaderItem orderid="5" name="special_price_to_date" />
			<HeaderItem orderid="6" name="promotion_id" />
			<HeaderItem orderid="7" name="promotion_name" />
			<HeaderItem orderid="8" name="attribute_set_code" />
			<HeaderItem orderid="9" name="product_type" />
			<HeaderItem orderid="10" name="periodic_discount_type" />
			<HeaderItem orderid="11" name="discount_type" />
			<HeaderItem orderid="12" name="manual_discount_type_value" />
			<HeaderItem orderid="13" name="customer_discount_type_value" />
			<HeaderItem orderid="14" name="discount_method" />
			<HeaderItem orderid="15" name="percentage" />
			<HeaderItem orderid="15" name="discount_amount" />
			<HeaderItem orderid="15" name="discount_price" />

		</CsvHeader>

		<DataMember headerItemOrder="0" className="string" instanceName="SKU" isPrimitveType="true"  />
		<DataMember headerItemOrder="1" className="string" instanceName="AffiliationId" isPrimitveType="true"  />
		<DataMember headerItemOrder="2" className="string" instanceName="AffiliationName" isPrimitveType="true"  />
		<DataMember headerItemOrder="3" className="decimal" instanceName="OfferPrice" isPrimitveType="true"  />
		<DataMember headerItemOrder="4" className="string" instanceName="ValidationFrom" isPrimitveType="true" />
		<DataMember headerItemOrder="5" className="string" instanceName="ValidationTo" isPrimitveType="true"  />
		<DataMember headerItemOrder="6" className="string" instanceName="OfferId" isPrimitveType="true"  />
		<DataMember headerItemOrder="7" className="string" instanceName="OfferName" isPrimitveType="true" />
		<DataMember headerItemOrder="8" className="string" instanceName="" isPrimitveType="true" defaultValue="TeamViewer" />
		<DataMember headerItemOrder="9" className="string" instanceName="" isPrimitveType="true" defaultValue="virtual"  />
		<DataMember headerItemOrder="10" className="string" instanceName="PeriodicDiscountType" isPrimitveType="true"  />
		<DataMember headerItemOrder="11" className="string" instanceName="DiscountType" isPrimitveType="true"  />
		<DataMember headerItemOrder="12" className="string" instanceName="" isPrimitveType="true" defaultValue="0" />
		<DataMember headerItemOrder="13" className="string" instanceName="" isPrimitveType="true" defaultValue="0" />
		<DataMember headerItemOrder="14" className="string" instanceName="DiscountMethod" isPrimitveType="true"  />
		<DataMember headerItemOrder="15" className="decimal" instanceName="DiscPct" isPrimitveType="true"  />
		<DataMember headerItemOrder="16" className="decimal" instanceName="DiscAmount" isPrimitveType="true"  />
		<DataMember headerItemOrder="17" className="decimal" instanceName="DiscPrice" isPrimitveType="true"  />

	</ObjectInfo>

</ObjectToCSVMapping>', GETUTCDATE(), N'1', NULL, NULL, 1, @StoreId, N'CREATE')

INSERT [dbo].[MappingTemplate] ([SourceEntity], [Name], [MappingTemplateTypeId], [XML], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive], [StoreId], [ReadMode]) 
VALUES (N'ErpItemQuantityDiscountWithAffiliation', N'CREATE.ErpItemQuantityDiscountWithAffiliation', 2, 
N'<ObjectToCSVMapping>
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
			<HeaderItem orderid="13" name="unit_price" />
			<HeaderItem orderid="14" name="discount_line_type_value" />
			<HeaderItem orderid="15" name="discount_type" />
			<HeaderItem orderid="16" name="periodic_discount_type" />
			<HeaderItem orderid="17" name="manual_discount_type_value" />
			<HeaderItem orderid="18" name="customer_discount_type_value" />
		</CsvHeader>
		<DataMember headerItemOrder="0" className="string" instanceName="SKU" isPrimitveType="true"  />
		<DataMember headerItemOrder="1" className="string" instanceName="AffiliationId" isPrimitveType="true"  />
		<DataMember headerItemOrder="2" className="string" instanceName="AffiliationName" isPrimitveType="true"  />
		<DataMember headerItemOrder="3" className="decimal" instanceName="TierPriceQuantity" isPrimitveType="true"  />
		<DataMember headerItemOrder="4" className="decimal" instanceName="TierPrice" isPrimitveType="true"  />
		<DataMember headerItemOrder="5" className="string" instanceName="TierPriceValueTypeUpdatedValue" isPrimitveType="true" />
		<DataMember headerItemOrder="6" className="string" instanceName="ValidFrom" isPrimitveType="true" />
		<DataMember headerItemOrder="7" className="string" instanceName="ValidTo" isPrimitveType="true" />
		<DataMember headerItemOrder="8" className="string" instanceName="" isPrimitveType="true" defaultValue="TeamViewer" />
		<DataMember headerItemOrder="9" className="string" instanceName="" isPrimitveType="true" defaultValue="Virtual" />
		<DataMember headerItemOrder="10" className="string" instanceName="OfferId" isPrimitveType="true"  />
		<DataMember headerItemOrder="11" className="string" instanceName="DiscountName" isPrimitveType="true" />
		<DataMember headerItemOrder="12" className="string" instanceName="DiscountPercentageUpdatedValue" isPrimitveType="true"  />
		<DataMember headerItemOrder="13" className="string" instanceName="UnitPriceUpdatedValue" isPrimitveType="true"  />
		<DataMember headerItemOrder="14" className="string" instanceName="TierPriceValueType" isPrimitveType="true"  />
		<DataMember headerItemOrder="15" className="string" instanceName="DiscountType" isPrimitveType="true"  />
		<DataMember headerItemOrder="16" className="string" instanceName="PeriodicDiscountType" isPrimitveType="true" />
		<DataMember headerItemOrder="17" className="string" instanceName="" isPrimitveType="true" defaultValue="0" />
		<DataMember headerItemOrder="18" className="string" instanceName="" isPrimitveType="true" defaultValue="0" />
	</ObjectInfo>
</ObjectToCSVMapping>', GETUTCDATE(), N'1', NULL, NULL, 1, @StoreId, N'CREATE')

--------------------------------

    DELETE #ControlTable
    WHERE StoreId = @StoreId

END

DROP TABLE #ControlTable

-------------------------------------------------------------