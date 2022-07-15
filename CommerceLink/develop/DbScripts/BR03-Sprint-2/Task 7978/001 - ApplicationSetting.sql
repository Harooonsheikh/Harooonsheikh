SELECT	StoreId
INTO	#ControlTable 
FROM	dbo.Store

DECLARE @StoreId INT

WHILE EXISTS (SELECT * FROM #ControlTable)
BEGIN

    SELECT	@StoreId = (	SELECT		TOP 1 StoreId
							FROM		#ControlTable
							ORDER BY	StoreId ASC)

-- Run for every store
-------------------------------------------------------------

INSERT [dbo].[ApplicationSetting]
	([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [FieldTypeId]) 
VALUES
	( N'ECOM.Discount_With_Affiliation_Output_Type', 'CSV', N'Discount With Affiliation Output Type', N'ECOM Settings', 1, 1, @StoreId, 1),
	( N'DISCOUNTWITHAFFILIATION.Filename_Prefix', 'DiscountWithAffiliation-', N'Discount with Affiliation File Name Prefix', N'Discount with Affiliation', 1, 1, @StoreId, 1),
	( N'DISCOUNTWITHAFFILIATION.Local_Output_Path', '{APPLICATION.Local_Base_Path}\DataFiles\DiscountWithAffiliation', N'Discount with Affiliation Local Output Path', N'Discount with Affiliation', 1, 1, @StoreId, 1),
	( N'DISCOUNTWITHAFFILIATION.CSV_Map_Path', '{APPLICATION.Local_Base_Path}\Templates\MagentoXMLMappingFiles\CREATE.ErpDiscountWithAffiliation.xml', N'Discount with Affiliation CSV Map Path', N'Discount with Affiliation', 1, 1, @StoreId, 1),
	( N'DISCOUNTWITHAFFILIATION.Remote_Path', '{APPLICATION.Remote_Base_Path}/discountwithaffiliations', N'Discount with Affiliation Remote Path', N'Discount with Affiliation', 1, 1, @StoreId, 1),

	( N'ECOM.Quantity_Discount_With_Affiliation_Output_Type', 'CSV', N'Quantity Discount With Affiliation Output Type', N'ECOM Settings', 1, 1, @StoreId, 1),
	( N'QUANTITYDISCOUNTWITHAFFILIATION.Filename_Prefix', 'QuantityDiscountWithAffiliation-', N'Quatity Discount with Affiliation File Name Prefix', N'Quantity Discount with Affiliation', 1, 1, @StoreId, 1),
	( N'QUANTITYDISCOUNTWITHAFFILIATION.Local_Output_Path', '{APPLICATION.Local_Base_Path}\DataFiles\QuantityDiscountWithAffiliation', N'Qty. Disc. with Affiliation Local Output Path', N'Quantity Discount with Affiliation', 1, 1, @StoreId, 1),
	( N'QUANTITYDISCOUNTWITHAFFILIATION.Remote_Path', '{APPLICATION.Remote_Base_Path}/quantitydiscountwithaffiliations', N'Quantity Discount with Affiliation Remote Path', N'Quantity Discount with Affiliation', 1, 1, @StoreId, 1),
	( N'QUANTITYDISCOUNTWITHAFFILIATION.CSV_Map_Path', '{APPLICATION.Local_Base_Path}\Templates\MagentoXMLMappingFiles\CREATE.ErpProductItemQuantityDiscountWithAffiliation.xml', N'Quantity Discount with Affiliation Template Path', N'Quantity Discount with Affiliation', 1, 1, @StoreId, 1)
--------------------------------

    DELETE	#ControlTable
    WHERE	StoreId = @StoreId

END

DROP TABLE #ControlTable

-------------------------------------------------------------