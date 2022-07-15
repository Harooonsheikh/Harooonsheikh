--Email Template inserts
GO
SET IDENTITY_INSERT [dbo].[EmailTemplate] ON 

if exists (select * from EmailTemplate where Id = 2)
begin
   update EmailTemplate set Body=N'<p>Hi,</p>
<h3>Exception:</h3>
<p>{1}</p>
<h3>{2}</h3>' , Subject = N'Product Exception Occured on VW {0}'
   where Id = 2
end
else
begin
	INSERT [dbo].[EmailTemplate] ([Id], [Name], [Subject], [Body], [Footer], [IsActive]) VALUES (2, N'Product', N'Product : {0} Failed on Commerce Link', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>

', N'', 1) 
end


if exists (select * from EmailTemplate where Id = 3)
begin
   update EmailTemplate set Body=N'<p>Hi,</p>
<h3>Exception:</h3>
<p>{1}</p>
<h3>{2}</h3>' , Subject = N'Customer Exception Occured on VW {0}'
   where Id =3 
end
else
begin
	INSERT [dbo].[EmailTemplate] ([Id], [Name], [Subject], [Body], [Footer], [IsActive]) VALUES (3, N'Customer', N'Customer: {0} Failed on Commerce Link QA', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>

', N'Take Care.', 1)
end


if exists (select * from EmailTemplate where Id = 4)
begin
   update EmailTemplate set Body=N'<p>Hi,</p>
<h3>Exception:</h3>
<p>{1}</p>
<h3>{2}</h3>' , Subject = N'Store Exception Occured on VW {0}'
   where Id =4 
end
else
begin
	INSERT [dbo].[EmailTemplate] ([Id], [Name], [Subject], [Body], [Footer], [IsActive]) VALUES (4, N'Store', N'Store: {0} Commerce Link QA', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>

', N'Take Care.', 1)
end


if exists (select * from EmailTemplate where Id = 5)
begin
   update EmailTemplate set Body=N'<p>Hi,</p>
<h3>Exception:</h3>
<p>{1}</p>
<h3>{2}</h3>' , Subject = N'SalesOrder Exception Occured on VW {0}'
   where Id =5 
end
else
begin
	INSERT [dbo].[EmailTemplate] ([Id], [Name], [Subject], [Body], [Footer], [IsActive]) VALUES (5, N'SalesOrder', N'Sales Order : {0} Commerce Link QA', N'<p>Hi,</p>
<h3>Exception:</h3>
<p>{1}</p>
<h3>{2}</h3>', N'Take Care.', 1)
end


if exists (select * from EmailTemplate where Id = 6)
begin
   update EmailTemplate set Body=N'<p>Hi,</p>
<h3>Exception:</h3>
<p>{1}</p>
<h3>{2}</h3>' , Subject = N'Inventory Exception Occured on VW {0}'
   where Id =6 
end
else
begin
	INSERT [dbo].[EmailTemplate] ([Id], [Name], [Subject], [Body], [Footer], [IsActive]) VALUES (6, N'Inventory', N'Inventory : {0} Failed on Commerce Link QA', N'<p>Hi,</p>
<h3>Exception:</h3>
<p>{1}</p>
<h3>{2}</h3>', N'Take Care.', 1)
end

if exists (select * from EmailTemplate where Id = 7)
begin
   update EmailTemplate set Body=N'<p>Hi,</p>
<h3>Exception:</h3>
<p>{1}</p>
<h3>{2}</h3>' , Subject = N'Price Exception Occured on VW {0}'
   where Id = 7
end
else
begin
	INSERT [dbo].[EmailTemplate] ([Id], [Name], [Subject], [Body], [Footer], [IsActive]) VALUES (7, N'Price', N'Price Exception Occured on VW {0}', N'<p>Hi,</p>
<h3>Exception:</h3>
<p>{1}</p>
<h3>{2}</h3>', N'', 1) 
end

if exists (select * from EmailTemplate where Id = 8)
begin
   update EmailTemplate set Body=N'<p>Hi,</p>
<h3>Exception:</h3>
<p>{1}</p>
<h3>{2}</h3>' , Subject = N'Discount Exception Occured on VW {0}'
   where Id = 8
end
else
begin
	INSERT [dbo].[EmailTemplate] ([Id],[Name], [Subject], [Body], [Footer], [IsActive]) VALUES (8, N'Discount', N'Discount : {0} Failed on Commerce Link', N'<p>Hi,</p>
<h3>Exception:</h3>
<p>{1}</p>
<h3>{2}</h3>' , N'', 1) 
end

if exists (select * from EmailTemplate where Id = 22)
begin
   update EmailTemplate set Body= N'<p>Hi,</p>
<p>Service has been Stopped</p>
', Subject = N'Service Stopped on VW {0}'
   where Id =22 
end
else
begin
	INSERT [dbo].[EmailTemplate] ([Id], [Name], [Subject], [Body], [Footer], [IsActive]) VALUES (22, N'SimpleNotification', N'Service Stopped on Commerce Link {0}', N'Hi,  Service Stopped  {0}', N'Exception :  {1}', 1)
end


SET IDENTITY_INSERT [dbo].[EmailTemplate] OFF


-- Configurable objects Inserts
TRUNCATE TABLE ConfigurableObjects

INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'Ground', N'', 1, 1)
INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'2 Day', N'', 1, 1)
INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'Next Day', N'', 1, 1)
INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'Store Pickup', N'', 1, 2)
INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'SalesTaxGroup', N'', 3, 1)
INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'DiscountCharges', N'', 4, 1)
INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'ShippingCharges', N'Standard', 4, 2)
INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'ItemTaxGroup', N'', 3, 2)
INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'giftcard', N'GIFTCARD', 5, 4)
--NS:
INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'001', N'Standard', 1, 1)
INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'002', N'USPS Posta', 1, 1)
INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'003', N'Expedited', 1, 1)
INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'004', N'Express', 1, 1)
INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'005', N'Internatio', 1, 1)
--NS:TFS1989
--INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'ShippingChargesDiscount', N'A69', 4, 4)

--Dimension set inserts
TRUNCATE TABLE DimensionSet

INSERT [dbo].[DimensionSet] ([ErpValue], [ComValue], [IsActive],[AdditionalErpValue]) VALUES (N'quantity', N'Size', 1,N'Quantity Measure')
INSERT [dbo].[DimensionSet] ([ErpValue], [ComValue], [IsActive],[AdditionalErpValue]) VALUES (N'flavor', N'Flavor', 1,Null)
INSERT [dbo].[DimensionSet] ([ErpValue], [ComValue], [IsActive],[AdditionalErpValue]) VALUES (N'ecomcolor', N'ColorVariety', 1,Null)
INSERT [dbo].[DimensionSet] ([ErpValue], [ComValue], [IsActive],[AdditionalErpValue]) VALUES (N'scent', N'Scent', 1,Null)


--Application Settings inserts
TRUNCATE TABLE ApplicationSetting

INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'INVENTORY.CSV_Map_Path', N'Maps\ProductInventoryCSVMap.xml', N'CSV_Map_Path', N'abc', 0, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'STORE.Local_Output_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\Stores', N'Store Local Output Path', N'abc', 0, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'APPLICATION.Remote_Base_Path', N'/Users/svc_commerce-ftp/CommerceLink/TV_INT/', N'Remote Directory Base Path', N'SFTPConfiguration', 1, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'APPLICATION.ERP_AX_OUN', N'VW01', N'AX Channel Org Unit Number', N'ERPAdapterGeneral', 10, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'APPLICATION.ERP_AX_RetailServerUri', N'https://vwaxdev01ret.sandbox.ax.dynamics.com/Commerce/', N'AX Retail Server URL', N'ERPAdapterGeneral', 0, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'APPLICATION.ERP_Default_Customer_Group', N'30', N'ERP Default Customer Group', N'ERPAdapterCustomer', 0, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'APPLICATION.Retail_Media_Path', N'/images/2017/', N'Retail Media Directory', N'ERPAdapterProduct', 130, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'APPLICATION.ERP_AX_InferPeriodicDiscount', N'TRUE', N'ERP AX Infer Periodic Discount', N'ERPAdapterSalesOrder', 50, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'APPLICATION.ERP_Legal_Company', N'VWO', N'ERP Legal Company', N'ERPAdapterGeneral', 30, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'PRODUCT.SKU_Prefix', N'0070', N'SKU Prefix', N'ERPAdapterProduct', 80, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'PRODUCT.Retail_Server_Paging', N'1000', N'AX Retail Server Paging', N'ERPAdapterGeneral', 20, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'PRODUCT.Attr_Flat_Hierarchy_Related', N'Child Variant', N'Product Flat Hierarchy Relation Attribute', N'ERPAdapterProduct', 110, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'PRODUCT.Attr_IsMaster', N'Is Master', N'Product Is Master Attribute', N'ERPAdapterProduct', 120, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'PRODUCT.Flat_Hierarchy_Enable', N'TRUE', N'Product Flat Hierarchy Enable', N'ERPAdapterProduct', 100, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'PRODUCT.SKU_Postfix', N'M', N'SKU Postfix', N'ERPAdapterProduct', 90, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'PRODUCT.Local_Output_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\Product', N'Product Local Output Path', N'Product/Inventory', 10, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'PRODUCT.Remote_Path', N'{APPLICATION.Remote_Base_Path}/Catalog', N'Product Remote Path', N'Product/Inventory', 20, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'PRODUCT.Filename_Prefix', N'Catalog-', N'Product File Name Prefix', N'Product/Inventory', 30, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'DISCOUNT.Remote_Path', N'{APPLICATION.Remote_Base_Path}/Discounts', N'Discount Remote Path', N'Price/Discount', 50, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'DISCOUNT.Filename_Prefix', N'Discount-', N'Discount File Name Prefix', N'Price/Discount', 60, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'DISCOUNT.Local_Output_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\Discount', N'Discount Local Output Path', N'Price/Discount', 40, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'ECOM.Root_Category_Id', N'root', N'Root Category Id', N'ERPAdapterProduct', 70, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'ECOM.Category_Assignment', N'SINGLE', N'Category Assignment', N'ERPAdapterProduct', 60, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'ECOM.SalesPerson_Id', N'', N'Sales Person Id', N'ERPAdapterGeneral', 50, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'ECOM.Remote_SFTP_Host', N'52.166.121.119', N'SFTP Host', N'SFTPConfiguration', 1, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'ECOM.Remote_SFTP_UserName', N'svc_commerce-ftp', N'SFTP User Name', N'SFTPConfiguration', 2, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'ECOM.Remote_SFTP_Password', N'glue-aZGiZU', N'SFTP Password', N'SFTPConfiguration', 3, 1, NULL, NULL, 1)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'ECOM.Remote_SFTP_Extenstions', N'csv|xml', N'SFTP Extension', N'SFTPConfiguration', 4, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'PRICE.Remote_Path', N'{APPLICATION.Remote_Base_Path}/Price', N'Price Remote Path', N'Price/Discount', 20, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'PRICE.local_Output_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\Price', N'Price Local Output Path', N'Price/Discount', 10, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'PRICE.Filename_Prefix', N'PriceBook-', N'Price File Name Prefix', N'Price/Discount', 30, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Multiplefile_Input_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\SalesOrder\MultiSalesOrderFile', N'Sales Order Multiple File Input Path', N'SalesOrder', 10, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Singlefile_Input_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\SalesOrder', N'Sales Order Single File Input Path', N'SalesOrder', 20, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Status_Remote_Path', N'{APPLICATION.Remote_Base_Path}/OrderStatusImport', N'Sales Order Status Remote Path', N'SalesOrder', 50, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Header_Discount_Reason_Code', N'HEADER_DISCOUNT', N'Header Discount Reason Code', N'ERPAdapterSalesOrder', 60, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Line_Discont_Reason_Code', N'LINE_DISCOUNT', N'Line Discont Reason Code', N'ERPAdapterSalesOrder', 70, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Update_Status_inDays', N'7', N'Update Status of Previous Days', N'ERPAdapterSalesOrder', 0, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Status_File_Name', N'orderstatus-', N'Sales Order Status File Name', N'SalesOrder', 60, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Status_local_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\SalesOrderStatus', N'Sales Order Status Local Output Path', N'SalesOrder', 40, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Remote_Input_Path', N'{APPLICATION.Remote_Base_Path}/Orders', N'Sales Order Remote Input Path', N'SalesOrder', 30, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Include_ERP_Order_Number_in_Status', N'TRUE', N'Include ERP Order Number in Status', N'ERPAdapterSalesOrder', 10, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Include_Tracking_Info_in_Status', N'TRUE', N'Include Tracking Info in Status', N'ERPAdapterSalesOrder', 20, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Order_Shipping_Tax_As_Charges_Code', N'C74', N'Order Shipping Tax As Charges Code', N'ERPAdapterSalesOrder', 80, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Rebate_Reason_Code', N'REBATE', N'Rebate Reason Code', N'ERPAdapterSalesOrder', 120, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.OOB_Coupon_Reason_Code', N'COUPON', N'OOB Coupon Reason Code', N'ERPAdapterSalesOrder', 121, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Affiliate_Reason_Code', N'SOURCECODE', N'SourceCode Reason Code', N'ERPAdapterSalesOrder', 122, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Statuses_For_Sync', N'Invoiced, Canceled', N'Order Status to Sync', N'ERPAdapterSalesOrder', 30, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Statuses_For_Tracking', N'Invoiced', N'Order Status for Tracking URL', N'ERPAdapterSalesOrder', 40, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Order_Tax_As_Charges', N'TRUE', N'Order Tax As Charges', N'ERPAdapterSalesOrder', 79, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.SalesLine_Tax_As_Charges_Code', N'C73', N'Sale Line Tax As Charges Code', N'ERPAdapterSalesOrder', 100, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.SalesLine_Tax_As_Charges_Description', N'SALESLINE_TAX', N'Sale Line Tax As Charges Description', N'ERPAdapterSalesOrder', 110, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Order_Shipping_Tax_As_Charges_Description', N'SHIPPING_TAX', N'Order Shipping Tax As Charges Description', N'ERPAdapterSalesOrder', 90, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.AX_Invoice_Address_Type', N'9', N'AX Invoice Address Type', N'ERPAdapterCustomer', 10, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.AX_Delivery_Address_Type', N'2', N'AX Delivery Address Type', N'ERPAdapterCustomer', 20, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Multiple_To_Single_File', N'TRUE', N'Break Multiple SalesOrders File To Single File', N'SalesOrder', 25, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.AX_VW_AvalaraShippingTax_Category', N'7', N'VW Avalara ShippingTax Category', N'ERPAdapterSalesOrder', 91, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.AX_VW_AvalaraProductTax_Category', N'8', N'VW Avalara Sales Line Tax Category', N'ERPAdapterSalesOrder', 111, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.AX_VW_VitaminWorldShipping_Category', N'6', N'VW Shipping Category', N'ERPAdapterSalesOrder', 92, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.AX_VW_Shipping_Custom_Category', N'Standard,USPS Posta,Express,Expedited,Internatio', N'VW Shipping Custom Category', N'ERPAdapterSalesOrder', 93, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'INVENTORY.Remote_Path', N'{APPLICATION.Remote_Base_Path}/Inventory', N'Inventory Remote Path', N'Product/Inventory', 50, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'INVENTORY.Filename_Prefix', N'ProductInventory-', N'Inventory File Name Prefix', N'Product/Inventory', 60, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'INVENTORY.Local_Output_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\Inventory', N'Inventory Local Output Path', N'Product/Inventory', 40, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'INVENTORY.LocationId', N'0070', N'Invent Location Id', N'ERPAdapterGeneral', 40, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'APPLICATION.Enviornment', N'DEV', N'Enviornment', N'EmailSetting', 0, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'NOTIFICATION.Email_SMTP', N'smtp.gmail.com', N'Email SMTP', N'EmailSetting', 0, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'NOTIFICATION.Email_Source', N'emailaxconnector@gmail.com', N'Email Source', N'EmailSetting', 0, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'NOTIFICATION.Email_Port', N'587', N'Email Port', N'EmailSetting', 0, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'NOTIFICATION.Email_Username', N'emailaxconnector@gmail.com', N'Email User Name', N'EmailSetting', 0, 1, NULL, NULL, 1)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'NOTIFICATION.Email_Password', N'axconnector@1234', N'Email Password', N'EmailSetting', 0, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'NOTIFICATION.Email_SSL_Enable', N'TRUE', N'Email SSL Enable', N'EmailSetting', 0, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'APPLICATION.Windows_Service', N'EdgeAX CommerceLink Sync Service', N'Windows Service', N'Services', 1, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'APPLICATION.File_Service', N'EdgeAX CommerceLink File Service', N'File Service', N'Services', 1, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'APPLICATION.XML_Base_Path', N'{APPLICATION.Local_Base_Path}\Templates', N'Template XML Base Path', N'MapsLocation', 1, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'APPLICATION.Reset_Time_InMinutes', N'60', N'Service Reset Time in Minutes', N'Services', 1, 1, NULL, NULL, 0)
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.AX_Address_IsPrivate', N'FALSE', N'AX Address Is Private', N'ERPAdapterCustomer', 21, 0, NULL, NULL, 54, 0)

-- Mapping template inserts
TRUNCATE TABLE MappingTemplate

INSERT [dbo].[MappingTemplate] ([SourceEntity], [Name], [Type], [XML], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive]) VALUES (N'ErpCatalog', N'CREATE.ErpCatalog', N'CREATE', N'<?xml version="1.0" encoding="utf-16"?>
<catalog xmlns="http://www.demandware.com/xml/impex/catalog/2006-10-31" catalog-id="aw-catalog">
  <!-- Header -->
  <header>
    <image-settings>
      <external-location>
        <http-url>http://edgeaxcomlink-d2.cloudapp.net:8989/Products/</http-url>
      </external-location>
      <view-types>
        <view-type>large</view-type>
        <view-type>medium</view-type>
        <view-type>small</view-type>
        <view-type>swatch</view-type>
        <view-type>hi-res</view-type>
      </view-types>
      <!-- variation-attribute-id>size</variation-attribute-id -->
      <!--<variation-attribute-id>color</variation-attribute-id >-->
      <alt-pattern>
        ${productname}, ${variationvalue}, ${viewtype}
      </alt-pattern>
      <title-pattern>${productname}, ${variationvalue}</title-pattern>
    </image-settings>
  </header>
  
  <category repeat="true" data-source="ErpCatalog~Categories" category-id="ErpCategory~EcomCategoryId">
    <display-name xml:lang="x-default">ErpCategory~Name</display-name>
    <online-flag >true</online-flag>
    <parent>ErpCategory~EcomParentCategoryId</parent>
    <position>ErpCategory~Position</position>
    <custom-attributes>
      <custom-attribute constant-value="true" attribute-id="headerMenuOrientation">true</custom-attribute>
      <custom-attribute constant-value="true" attribute-id="showInMenu">true</custom-attribute>
    </custom-attributes>
  </category>
  <product repeat="true" data-source="ErpCatalog~Products" product-id="ErpProduct~EcomProductId" mode="ErpProduct~Mode">
    <min-order-quantity>1</min-order-quantity>
    <display-name xml:lang="x-default" >ErpProduct~ProductName</display-name>
    <short-description xml:lang="x-default" >ErpProduct~ProductName</short-description>
    <long-description xml:lang="x-default" >ErpProduct~Description</long-description>
    <online-flag>true</online-flag>
    <available-flag >true</available-flag>
    <searchable-flag >true</searchable-flag>
    <images show-node="(sourceObject.IsMasterProduct || sourceObject.MasterProductId == 0) ? true : false">
      <image-group view-type="medium">
 <image repeat="true" data-source="ErpProduct~ImageList" path=''{{sourceObject.Url}}'' />
      </image-group>
      <image-group view-type="small">
 <image repeat="true" data-source="ErpProduct~ImageList" path=''{{sourceObject.Url}}'' />
      </image-group>
      <image-group view-type="large">
        <image repeat="true" data-source="ErpProduct~ImageList" path=''{{sourceObject.Url}}'' />
      </image-group>
    </images>
    <tax-class-id >standard</tax-class-id>
    <brand show-node="false"  custom-attribute-value="true">Brand</brand>
    <manufacturer-sku expression="true">(sourceObject.IsMasterProduct || sourceObject.MasterProductId == 0) ? sourceObject.ItemId : sourceObject.VariantId</manufacturer-sku>
    <page-attributes>
      <page-title xml:lang="x-default" custom-attribute-value="true">ProductName</page-title>
      <page-description  xml:lang="x-default" custom-attribute-value="true">Description</page-description>
      <page-keywords xml:lang="x-default" custom-attribute-value="true">ProductName</page-keywords>
      <page-url xml:lang="x-default"  custom-attribute-value="true">ProductName</page-url>
    </page-attributes>
    <custom-attributes>
      <custom-attribute show-node="false"  attribute-id="external_id">ItemId</custom-attribute>
      <custom-attribute show-node="false" attribute-id="style">Style</custom-attribute>
      <custom-attribute show-node="false" attribute-id="color">Color</custom-attribute>
      <custom-attribute show-node="false" attribute-id="size">Size</custom-attribute> 
	  <custom-attribute show-node="false"  attribute-id="AssociateCategory">AssociateCategory</custom-attribute>
      <custom-attribute show-node="false" attribute-id="Manager">Manager</custom-attribute>
      <custom-attribute show-node="false" attribute-id="Category">Category</custom-attribute>
      <custom-attribute show-node="false" attribute-id="CategoryManager">CategoryManager</custom-attribute> 
	  <custom-attribute show-node="false"  attribute-id="Description">Description</custom-attribute>
      <custom-attribute show-node="false" attribute-id="ItemStatus">ItemStatus</custom-attribute>
      <custom-attribute show-node="false" attribute-id="ItemType(IM/PM)">ItemType(IM/PM)</custom-attribute>
      <custom-attribute show-node="false" attribute-id="MINMonthorPSDMonth">MINMonthorPSDMonth</custom-attribute> 
	  <custom-attribute show-node="false"  attribute-id="POItemText">POItemText</custom-attribute>
      <custom-attribute show-node="false" attribute-id="PotencyMeasure">PotencyMeasure</custom-attribute>
      <custom-attribute show-node="false" attribute-id="ProductForm">ProductForm</custom-attribute>
      <custom-attribute show-node="false" attribute-id="Productname">Productname</custom-attribute>   
	  <custom-attribute show-node="false"  attribute-id="Refrigerate">Refrigerate</custom-attribute>
      <custom-attribute show-node="false" attribute-id="ServingperDay">ServingperDay</custom-attribute>
      <custom-attribute show-node="false" attribute-id="ServingsperContainer">ServingsperContainer</custom-attribute>
      <custom-attribute show-node="false" attribute-id="Size(IM)">Size(IM)</custom-attribute>  
	  <custom-attribute show-node="false"  attribute-id="TaxRateCode">TaxRateCode</custom-attribute>
      <custom-attribute show-node="false" attribute-id="VendorCodeIM">VendorCodeIM</custom-attribute>
      <custom-attribute show-node="false" attribute-id="VendorItemNumber">VendorItemNumber</custom-attribute>
      <custom-attribute show-node="false" attribute-id="ItemCatalogCategory">ItemCatalogCategory</custom-attribute>   
	  <custom-attribute show-node="false"  attribute-id="ActivatedDate">ActivatedDate</custom-attribute>
      <custom-attribute show-node="false" attribute-id="AvailableInventory">AvailableInventory</custom-attribute>
      <custom-attribute show-node="false" attribute-id="BackOrderDate">BackOrderDate</custom-attribute>
      <custom-attribute show-node="false" attribute-id="BrandId">BrandId</custom-attribute>   
	  <custom-attribute show-node="false"  attribute-id="BrandName">BrandName</custom-attribute>
      <custom-attribute show-node="false" attribute-id="BulkNumber">BulkNumber</custom-attribute>
      <custom-attribute show-node="false" attribute-id="CategoryManagerName">CategoryManagerName</custom-attribute>
      <custom-attribute show-node="false" attribute-id="Cost">Cost</custom-attribute>  
	  <custom-attribute show-node="false"  attribute-id="CubeVolume">CubeVolume</custom-attribute>
      <custom-attribute show-node="false" attribute-id="EAN">EAN</custom-attribute>
      <custom-attribute show-node="false" attribute-id="FormId">FormId</custom-attribute>
      <custom-attribute show-node="false" attribute-id="HasLabelInfo">HasLabelInfo</custom-attribute> 
	  <custom-attribute show-node="false"  attribute-id="IsActive">IsActive</custom-attribute>
      <custom-attribute show-node="false" attribute-id="IsFutureDelete">IsFutureDelete</custom-attribute>
      <custom-attribute show-node="false" attribute-id="IsHazmat">IsHazmat</custom-attribute>
      <custom-attribute show-node="false" attribute-id="IsTaxable">IsTaxable</custom-attribute> 
	  <custom-attribute show-node="false"  attribute-id="IsWebOnly">IsWebOnly</custom-attribute>
      <custom-attribute show-node="false" attribute-id="ItemNumber">ItemNumber</custom-attribute>
      <custom-attribute show-node="false" attribute-id="Keywords">Keywords</custom-attribute>
      <custom-attribute show-node="false" attribute-id="LongDescription">LongDescription</custom-attribute> 	  
	  <custom-attribute show-node="false"  attribute-id="ModifiedBy">ModifiedBy</custom-attribute>
      <custom-attribute show-node="false" attribute-id="ModifiedDate">ModifiedDate</custom-attribute>
      <custom-attribute show-node="false" attribute-id="Name">Name</custom-attribute>
      <custom-attribute show-node="false" attribute-id="ShortDescription">ShortDescription</custom-attribute>  
	  <custom-attribute show-node="false"  attribute-id="TaxCode">TaxCode</custom-attribute>
      <custom-attribute show-node="false" attribute-id="UPC">UPC</custom-attribute>
      <custom-attribute show-node="false" attribute-id="Weight">Weight</custom-attribute>  
	   <custom-attribute show-node="false"  attribute-id="MasterProductID">MasterProductID</custom-attribute>
      <custom-attribute show-node="false" attribute-id="bvAverageRating">bvAverageRating</custom-attribute>
      <custom-attribute show-node="false" attribute-id="bvRatingRange">bvRatingRange</custom-attribute>
      <custom-attribute show-node="false" attribute-id="bvReviewCount">bvReviewCount</custom-attribute> 	 	  
	  <custom-attribute show-node="false"  attribute-id="Coated">Coated</custom-attribute>
      <custom-attribute show-node="false" attribute-id="containsHazmat">containsHazmat</custom-attribute>
      <custom-attribute show-node="false" attribute-id="CrueltyFree">CrueltyFree</custom-attribute>
      <custom-attribute show-node="false" attribute-id="excludeInRKGProductFeed">excludeInRKGProductFeed</custom-attribute>   
	  <custom-attribute show-node="false"  attribute-id="GEL">GEL</custom-attribute>
      <custom-attribute show-node="false" attribute-id="GlutenFree">GlutenFree</custom-attribute>
      <custom-attribute show-node="false" attribute-id="IsNewItem">IsNewItem</custom-attribute>
      <custom-attribute show-node="false" attribute-id="isProp65">isProp65</custom-attribute>  
	  <custom-attribute show-node="false"  attribute-id="isProp65Bpa">isProp65Bpa</custom-attribute>
      <custom-attribute show-node="false" attribute-id="Kosher">Kosher</custom-attribute>
      <custom-attribute show-node="false" attribute-id="labelDirections">labelDirections</custom-attribute>
      <custom-attribute show-node="false" attribute-id="labelOtherIngredients">labelOtherIngredients</custom-attribute> 
	  <custom-attribute show-node="false"  attribute-id="labelSupplement">labelSupplement</custom-attribute>
      <custom-attribute show-node="false" attribute-id="labelWarning">labelWarning</custom-attribute>
      <custom-attribute show-node="false" attribute-id="maxOrderQuantity">maxOrderQuantity</custom-attribute>
      <custom-attribute show-node="false" attribute-id="newPackagingImage">newPackagingImage</custom-attribute>	  
	  <custom-attribute show-node="false"  attribute-id="NonGmo">NonGmo</custom-attribute>
      <custom-attribute show-node="false" attribute-id="Organic">Organic</custom-attribute>
      <custom-attribute show-node="false" attribute-id="ParabenFree">ParabenFree</custom-attribute>
      <custom-attribute show-node="false" attribute-id="Potencies">Potencies</custom-attribute> 	  
	   <custom-attribute show-node="false"  attribute-id="PotencyName">PotencyName</custom-attribute>
      <custom-attribute show-node="false" attribute-id="RapidRelease">RapidRelease</custom-attribute>
      <custom-attribute show-node="false" attribute-id="ShippingLimit">ShippingLimit</custom-attribute>  
	   <custom-attribute show-node="false" attribute-id="TimeRelease">TimeRelease</custom-attribute>
      <custom-attribute show-node="false" attribute-id="Vegan">Vegan</custom-attribute>   
	   <custom-attribute show-node="false"  attribute-id="Vegetarian">Vegetarian</custom-attribute>
      <custom-attribute show-node="false" attribute-id="WeeklyWellness">WeeklyWellness</custom-attribute>
	  <custom-attribute show-node="false" attribute-id="BrnadName">BrnadName</custom-attribute>

    </custom-attributes>
    <variations show-node="(sourceObject.IsMasterProduct || sourceObject.MasterProductId == 0) ? true : false">
      <attributes>
        <variation-attribute repeat="true" data-source="ErpProduct~DimensionSets" attribute-id="ErpProductDimensionSet~DimensionKey" variation-attribute-id="ErpProductDimensionSet~DimensionKey"  show-node=''ConfigurationHelper.DimensionSets.Find(d =&gt;d.ErpValue.Equals(sourceObject.DimensionKey.ToLower())).IsActive''>
          <variation-attribute-values>
            <variation-attribute-value repeat="true" data-source="ErpProductDimensionSet~DimensionValues" value="ErpProductDimensionValueSet~DimensionValue">
              <display-value xml:lang="x-default">ErpProductDimensionValueSet~DimensionValue</display-value>
            </variation-attribute-value>
          </variation-attribute-values>
        </variation-attribute>
      </attributes>
      <variants>
        <variant repeat="true" data-source="ErpProduct~ProductVariants" product-id="ErpProduct~EcomProductId" />
      </variants>
    </variations>
  </product>
  <category-assignment repeat="true" data-source="ErpCatalog~CategoryAssignments" category-id="ErpCategoryAssignment~CategoryId" product-id="ErpCategoryAssignment~ProductId" mode="ErpCategoryAssignment~Mode">
    <primary-flag to-lower="true">ErpCategoryAssignment~PrimaryFlag</primary-flag>
  </category-assignment>
</catalog>', CAST(N'2016-12-29T17:32:15.207' AS DateTime), N'admin', NULL, NULL, 0)

INSERT [dbo].[MappingTemplate] ([SourceEntity], [Name], [Type], [XML], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive]) VALUES (N'ErpDiscount', N'CREATE.ErpDiscount', N'CREATE', N'<?xml version="1.0" encoding="utf-16"?>
<pricebooks xmlns="http://www.demandware.com/xml/impex/pricebook/2006-10-31">
  <pricebook>
    <header pricebook-id="ErpDiscount~OfferId">
      <currency>ErpDiscount~Currency</currency>
      <display-name xml:lang="x-default">ErpDiscount~Name</display-name>
      <online-flag to-lower="true">ErpDiscount~Online</online-flag>
      <online-from>ErpDiscount~ValidFrom</online-from>
      <online-to>ErpDiscount~ValidTo</online-to>
      <parent>mfi-list-prices</parent>
    </header>
    <price-tables>
      <price-table repeat="true" data-source="ErpDiscount~Discounts" product-id="ErpProductDiscount~SKU">
        <amount quantity="ErpProductDiscount~Quantity">ErpProductDiscount~OfferPrice</amount>
      </price-table>
    </price-tables>
  </pricebook>
</pricebooks>', CAST(N'2016-12-29T17:37:52.093' AS DateTime), N'admin', NULL, NULL, 0)

INSERT [dbo].[MappingTemplate] ([SourceEntity], [Name], [Type], [XML], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive]) VALUES (N'ErpInventoryProducts', N'CREATE.ErpInventoryProducts', N'CREATE', N'<?xml version="1.0" encoding="utf-16"?>
<inventory xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://www.demandware.com/xml/impex/inventory/2007-05-31">
  <inventory-list>
    <header list-id="fabrikam_inventory_list">
      <default-instock>false</default-instock>
      <description>Fabrikam Inventory List</description>
      <use-bundle-inventory-only>false</use-bundle-inventory-only>
    </header>
    <records>
      <record repeat="true" data-source="ErpInventoryProducts~Products" product-id="ErpProduct~SKU">
        <allocation>ErpProduct~AvailableQuantity</allocation>
        <allocation-timestamp>2016-12-02T19:58:14.109596+05:00</allocation-timestamp>
        <perpetual>false</perpetual>
        <preorder-backorder-handling>none</preorder-backorder-handling>
        <ats>0</ats>
        <on-order>0</on-order>
        <turnover>0</turnover>
      </record>
    </records>
  </inventory-list>
</inventory>', CAST(N'2016-12-29T17:41:24.010' AS DateTime), N'admin', CAST(N'2016-12-29T17:42:53.403' AS DateTime), N'admin', 0)

INSERT [dbo].[MappingTemplate] ([SourceEntity], [Name], [Type], [XML], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive]) VALUES (N'ErpOrderStatus', N'CREATE.ErpOrderStatus', N'CREATE', N'<?xml version="1.0" encoding="utf-16"?>
<orders xmlns="http://www.demandware.com/xml/impex/order/2006-10-31">
  <order repeat="true" data-source="ErpOrderStatus~ordersStatus" order-no="ErpSalesOrderStatus~orderNo">
    <tracking-number>ErpSalesOrderStatus~TrackingNumber</tracking-number>
    <tracking-url>ErpSalesOrderStatus~TrackingURL</tracking-url>
    <status>
      <order-status>ErpSalesOrderStatus~status</order-status>
      <shipping-status>ErpSalesOrderStatus~shippingStatus</shipping-status>
    </status>
  </order>
</orders>', CAST(N'2016-12-29T20:37:35.283' AS DateTime), N'admin', CAST(N'2017-01-11T13:24:48.517' AS DateTime), N'admin', 1)

INSERT [dbo].[MappingTemplate] ([SourceEntity], [Name], [Type], [XML], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive]) VALUES (N'ErpPrice', N'CREATE.ErpPrice', N'CREATE', N'<?xml version="1.0" encoding="utf-16"?>
<pricebooks xmlns="http://www.demandware.com/xml/impex/pricebook/2006-10-31">
  <pricebook>
    <header pricebook-id="aw-list-prices">
      <currency>ErpPrice~Currency</currency>
      <display-name xml:lang="x-default">List Prices</display-name>
      <online-flag to-lower="true">ErpPrice~Online</online-flag>
    </header>
    <price-tables>
      <price-table repeat="true" data-source="ErpPrice~Prices" product-id="ErpProductPrice~SKU">
        <amount quantity="ErpProductPrice~Quantity">ErpProductPrice~BasePrice</amount>
      </price-table>
    </price-tables>
  </pricebook>
</pricebooks>', CAST(N'2016-12-29T17:56:19.467' AS DateTime), N'admin', NULL, NULL, 0)

INSERT [dbo].[MappingTemplate] ([SourceEntity], [Name], [Type], [XML], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive]) VALUES (N'ErpSalesOrder', N'READ.ErpSalesOrder', N'READ', N'<?xml version="1.0" encoding="utf-16"?>
<Targets>
  <!--ErpSalesOrder Properties-->
  <Target property="ErpSalesOrder~Id" source-path="//orders/order[@order-no]" attribute-name="order-no"/>
  <Target property="ErpSalesOrder~OrderPlacedDate" source-path="//orders/order/order-date"/>
  <!--<Target property="ErpSalesOrder~InventoryLocationId" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''InventLocationId'']"/>-->
  <!--<Target property="ErpSalesOrder~InventoryLocationId" constant-value="095097"/>-->
  <!--It should be used for store pick up-->
  <!--<Target property="ErpSalesOrder~StoreId" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''InventLocationId'']"/>-->
  <!--<Target property="ErpSalesOrder~StoreId" constant-value="095097"/>-->
  <Target property="ErpSalesOrder~SourceCode" source-path="//orders/order/source-code/code"/>

  <Target property="ErpSalesOrder~ChannelReferenceId" source-path="//orders/order/original-order-no"/>
  <Target property="ErpSalesOrder~CurrencyCode" source-path="//orders/order/currency"/>
  <Target property="ErpSalesOrder~CustomerId" source-path="//orders/order/customer/customer-no"/>
  <Target property="ErpSalesOrder~CustomerName" source-path="//orders/order/customer/customer-name"/>
  <Target property="ErpSalesOrder~CustomerEmail" source-path="//orders/order/customer/customer-email"/>
  <Target property="ErpSalesOrder~ReceiptEmail" source-path="//orders/order/customer/customer-email"/>
  <!--<Target property="ErpSalesOrder~Status" source-path="//orders/order/status/order-status"/>-->
  <Target property="ErpSalesOrder~Status" constant-value="Created"/>
  <Target property="ErpSalesOrder~ChannelReferenceId" source-path="//orders/order/current-order-no"/>

  <Target property="ErpSalesOrder~TotalAmount" source-path="//orders/order/totals/order-total/gross-price"/>
  <Target property="ErpSalesOrder~TaxAmount" source-path="//orders/order/totals/order-total/tax"/>
  <Target property="ErpSalesOrder~NetAmountWithNoTax" source-path="//orders/order/totals/order-total/net-price"/>
  <Target property="ErpSalesOrder~NetAmountWithTax" source-path="//orders/order/totals/order-total/gross-price"/>
  <!--Order Discount Old-->
  <!--<Target property="ErpSalesOrder~DiscountAmount" source-path="//orders/order/totals/merchandize-total/price-adjustments/price-adjustment/gross-price"/>
  <Target property="ErpSalesOrder~DiscountCode" source-path="//orders/order/totals/merchandize-total/price-adjustments/price-adjustment/promotion-id"/>-->

  <!--Reading discount of Order-->
  <Target property="ErpSalesOrder~OrderDiscounts" source-path="//orders/order/totals/merchandize-total/price-adjustments/price-adjustment" target-source="ErpDiscountLine" repeat="true" >
    <Properties>
      <Target property="ErpDiscountLine~EffectiveAmount" source-path="net-price"/>
      <Target property="ErpDiscountLine~Tax" source-path="tax"/>
      <Target property="ErpDiscountLine~DiscountCode" source-path="promotion-id"/>
      <Target property="ErpDiscountLine~OfferId" source-path="campaign-id"/>
      <Target property="ErpDiscountLine~CouponId" source-path="coupon-id"/>
      <Target property="ErpDiscountLine~RebateCode" source-path="custom-attributes/custom-attribute[@attribute-id=''rebate-code'']"/>
      <Target property="ErpDiscountLine~Amount" source-path="custom-attributes/custom-attribute[@attribute-id=''discount'']"/>
      <Target property="ErpDiscountLine~SppNumber" source-path="custom-attributes/custom-attribute[@attribute-id=''spp-no'']"/>
      <Target property="ErpDiscountLine~OfferName" source-path="custom-attributes/custom-attribute[@attribute-id=''reason-code'']"/>
    </Properties>
  </Target>

  <!--ErpSalesOrder Products-->
  <Target property="ErpSalesOrder~SalesLines" source-path="//orders/order/product-lineitems/product-lineitem" target-source="ErpSalesLine" repeat="true" >
    <Properties>
      <Target property="ErpSalesLine~NetAmount" source-path="net-price"/>
      <Target property="ErpSalesLine~TaxAmount" source-path="tax"/>
      <Target property="ErpSalesLine~TotalAmount" source-path="gross-price"/>
      <Target property="ErpSalesLine~BasePrice" source-path="base-price"/>
      <Target property="ErpSalesLine~Price" source-path="base-price"/>
      <Target property="ErpSalesLine~LineNumber" source-path="position"/>
      <Target property="ErpSalesLine~Description" source-path="product-id"/>
      <Target property="ErpSalesLine~ItemId" source-path="product-id"/>
      <Target property="ErpSalesLine~Quantity" source-path="quantity"/>
      <Target property="ErpSalesLine~QuantityOrdered" source-path="quantity"/>
      <Target property="ErpSalesLine~TaxRatePercent" source-path="tax-rate"/>
      <Target property="ErpSalesLine~IsGiftCardLine" source-path="gift"/>
      <Target property="ErpSalesLine~ShipmentId" source-path="shipment-id"/>

      <Target property="ErpSalesLine~InventoryLocationId" source-path="custom-attributes/custom-attribute[@attribute-id=''fromStoreId'']"/>
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''SKU'']" is-custom-attribute="true" attribute-id="SKU"/>
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''bogoPromotionId'']" is-custom-attribute="true" attribute-id="bogoPromotionId"/>
      <Target property="ErpSalesLine~SalesTaxGroupId" source-path="custom-attributes/custom-attribute[@attribute-id=''taxJurisdictionID'']"/>


      <!--TVW FDD-007-->
      <!--Set in the "Synchronize orders" batch flow based Shipping Date Requested field on the SalesLin-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDFROM'']" is-custom-attribute="true" attribute-id="TMVCONTRACTVALIDFROM"/>
      <!--Set in the "Synchronize orders" batch flow and should be equal to TMVContractValidFrom in the happy flow.-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTCALCULATEFROM'']" default-value="" is-custom-attribute="true" attribute-id="TMVCONTRACTCALCULATEFROM"/>
      <!--Set in the "Synchronize orders" batch flow and should be equal to TMVContractValidFrom + the length of the offer type from the deimension value - could also be empty if perpetual i.e. same logic implemented in Basic Contract FDD should be automatically triggerred. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDTO'']" default-value="" is-custom-attribute="true" attribute-id="TMVCONTRACTVALIDTO"/>
      <!--No logic implemented at time of writing - leave default empty value-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTPOSSTERMDATE'']"  is-custom-attribute="true" attribute-id="TMVCONTRACTPOSSTERMDATE"/>
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTCANCELDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTCANCELDATE"/>
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTPOSSCANCELDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTPOSSCANCELDATE"/>
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTTERMDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTTERMDATE"/>
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTTERMDATEEFFECTIVE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTTERMDATEEFFECTIVE"/>
      <!--Ensure that is set to false (i.e. the default value). This is only used in more advanced scenarios and not the happy flow. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVAUTOPROLONGATION'']" default-value="0" is-custom-attribute="true" attribute-id="TMVAUTOPROLONGATION"/>
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVPURCHORDERFORMNUM'']" is-custom-attribute="true" attribute-id="TMVPURCHORDERFORMNUM"/>
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCUSTOMERREF'']" is-custom-attribute="true" attribute-id="TMVCUSTOMERREF"/>
      <!--Set in the "Synchronize orders" batch flow to value of "created". -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTSTATUSLINE'']" default-value="10" is-custom-attribute="true" attribute-id="TMVCONTRACTSTATUSLINE"/>
      <!--This field is required in order to post the confirmation but no logic implemented at time of writing - fill in with dummy value. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVEULAVERSION'']" default-value="v1" is-custom-attribute="true" attribute-id="TMVEULAVERSION"/>
      <!--Set in the "Synchronize orders" batch flow to value of "year". Note there is no logic tied to this field at the moment. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVBILLINGPERIOD'']" default-value="1" is-custom-attribute="true" attribute-id="TMVBILLINGPERIOD"/>
      <!--Random GUID passed from Magento/Postman but is inserted in a related table to the SalesLine hence the field is an Int64 RefRecId field. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''PACLICENSE'']" is-custom-attribute="true" attribute-id="PACLICENSE"/>
      <!--Set in the "Synchronize orders" batch flow. Copy value from SalesLine.LineAmount-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="gross-price" is-custom-attribute="true" attribute-id="TMVORIGINALLINEAMOUNT"/>
      <!--Set in the "Synchronize orders" batch flow to value of "yes"-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVLINEMODIFIED'']" default-value="1" is-custom-attribute="true" attribute-id="TMVLINEMODIFIED"/>
      <!--Set in the "Synchronize orders" batch flow to value of "none". -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVREVERSEDLINE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVREVERSEDLINE"/>


      <!--Reading options of product-->
      <Target property="ErpSalesOrder~Options" source-path="option-lineitems/option-lineitem" target-source="ErpSalesLine" repeat="true" >
        <Properties>
          <Target property="ErpSalesLine~NetAmount" source-path="net-price"/>
          <Target property="ErpSalesLine~TaxAmount" source-path="tax"/>
          <Target property="ErpSalesLine~TotalAmount" source-path="gross-price"/>
          <Target property="ErpSalesLine~BasePrice" source-path="base-price"/>
          <Target property="ErpSalesLine~Price" source-path="base-price"/>
          <!--<Target property="ErpSalesLine~VariantId" source-path="value-id"/>-->
          <Target property="ErpSalesLine~ItemId" source-path="product-id"/>
          <Target property="ErpSalesLine~Quantity" constant-value="1" />
          <Target property="ErpSalesLine~Comment" constant-value="OptionItem"/>

          <Target property="ErpSalesLine~SalesTaxGroupId" source-path="custom-attributes/custom-attribute[@attribute-id=''taxJurisdictionID'']"/>

          <!--Reading discount of option-->
          <Target property="ErpSalesLine~DiscountLines" source-path="price-adjustments/price-adjustment" target-source="ErpDiscountLine" repeat="true" >
            <Properties>
              <Target property="ErpDiscountLine~Amount" source-path="net-price"/>
              <Target property="ErpDiscountLine~Tax" source-path="tax"/>
              <Target property="ErpDiscountLine~DiscountCode" source-path="promotion-id"/>
              <Target property="ErpDiscountLine~OfferId" source-path="campaign-id"/>
              <Target property="ErpDiscountLine~CouponId" source-path="coupon-id"/>
            </Properties>
          </Target>

          <Target property="ErpSalesLine~InventoryLocationId" source-path="custom-attributes/custom-attribute[@attribute-id=''fromStoreId'']"/>

          <!--TVW FDD-007-->
          <!--Set in the "Synchronize orders" batch flow based Shipping Date Requested field on the SalesLin-->
          <Target property="ErpSalesLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDFROM'']" is-custom-attribute="true" attribute-id="TMVCONTRACTVALIDFROM"/>
          <!--Set in the "Synchronize orders" batch flow and should be equal to TMVContractValidFrom in the happy flow.-->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTCALCULATEFROM'']" is-custom-attribute="true" attribute-id="TMVCONTRACTCALCULATEFROM"/>
          <!--Set in the "Synchronize orders" batch flow and should be equal to TMVContractValidFrom + the length of the offer type from the deimension value - could also be empty if perpetual i.e. same logic implemented in Basic Contract FDD should be automatically triggerred. -->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDTO'']" is-custom-attribute="true" attribute-id="TMVCONTRACTVALIDTO"/>
          <!--No logic implemented at time of writing - leave default empty value-->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTPOSSTERMDATE'']"  is-custom-attribute="true" attribute-id="TMVCONTRACTPOSSTERMDATE"/>
          <!--No logic implemented at time of writing - leave default empty value -->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTCANCELDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTCANCELDATE"/>
          <!--No logic implemented at time of writing - leave default empty value -->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTPOSSCANCELDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTPOSSCANCELDATE"/>
          <!--No logic implemented at time of writing - leave default empty value -->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTTERMDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTTERMDATE"/>
          <!--No logic implemented at time of writing - leave default empty value -->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTTERMDATEEFFECTIVE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTTERMDATEEFFECTIVE"/>
          <!--Ensure that is set to false (i.e. the default value). This is only used in more advanced scenarios and not the happy flow. -->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVAUTOPROLONGATION'']" default-value="0" is-custom-attribute="true" attribute-id="TMVAUTOPROLONGATION"/>
          <!--No logic implemented at time of writing - leave default empty value -->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVPURCHORDERFORMNUM'']" is-custom-attribute="true" attribute-id="TMVPURCHORDERFORMNUM"/>
          <!--No logic implemented at time of writing - leave default empty value -->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCUSTOMERREF'']" is-custom-attribute="true" attribute-id="TMVCUSTOMERREF"/>
          <!--Set in the "Synchronize orders" batch flow to value of "created". -->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTSTATUSLINE'']" default-value="10" is-custom-attribute="true" attribute-id="TMVCONTRACTSTATUSLINE"/>
          <!--This field is required in order to post the confirmation but no logic implemented at time of writing - fill in with dummy value. -->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVEULAVERSION'']" default-value="v1" is-custom-attribute="true" attribute-id="TMVEULAVERSION"/>
          <!--Set in the "Synchronize orders" batch flow to value of "year". Note there is no logic tied to this field at the moment. -->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVBILLINGPERIOD'']" default-value="1" is-custom-attribute="true" attribute-id="TMVBILLINGPERIOD"/>
          <!--Random GUID passed from Magento/Postman but is inserted in a related table to the SalesLine hence the field is an Int64 RefRecId field. -->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''PACLICENSE'']" is-custom-attribute="true" attribute-id="PACLICENSE"/>
          <!--Set in the "Synchronize orders" batch flow. Copy value from SalesLine.LineAmount-->
          <Target property="ErpSalesLine~CustomAttributes" source-path="gross-price" is-custom-attribute="true" attribute-id="TMVORIGINALLINEAMOUNT"/>
          <!--Set in the "Synchronize orders" batch flow to value of "yes"-->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVLINEMODIFIED'']" default-value="1" is-custom-attribute="true" attribute-id="TMVLINEMODIFIED"/>
          <!--Set in the "Synchronize orders" batch flow to value of "none". -->
          <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVREVERSEDLINE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVREVERSEDLINE"/>

        </Properties>
      </Target>

      <!--Reading discount of product-->
      <Target property="ErpSalesLine~DiscountLines" source-path="price-adjustments/price-adjustment" target-source="ErpDiscountLine" repeat="true" >
        <Properties>
          <Target property="ErpDiscountLine~Amount" source-path="net-price"/>
          <Target property="ErpDiscountLine~Tax" source-path="tax"/>
          <Target property="ErpDiscountLine~DiscountCode" source-path="promotion-id"/>
          <Target property="ErpDiscountLine~OfferId" source-path="campaign-id"/>
          <Target property="ErpDiscountLine~CouponId" source-path="coupon-id"/>
        </Properties>
      </Target>

    </Properties>
  </Target>

  <!--Shipping-->
  <Target property="ErpSalesOrder~DeliveryMode" source-path="//orders/order/shipments/shipment/shipping-method"/>
  <Target property="ErpSalesOrder~DeliveryModeChargeAmount" source-path="//orders/order/shipping-lineitems/shipping-lineitem/gross-price"/>
  <!-- Shipping Tax is commented out because we are taking gross amount to shipping charges which includes tax-->
  <!--<Target property="ErpSalesOrder~Shipping_Tax" source-path="//orders/order/shipping-lineitems/shipping-lineitem/tax"/>-->

  <!--Reading discount of Shipping-->
  <Target property="ErpSalesOrder~ShippingDiscounts" source-path="//orders/order/shipping-lineitems/shipping-lineitem/price-adjustments/price-adjustment" target-source="ErpDiscountLine" repeat="true" >
    <Properties>
      <Target property="ErpDiscountLine~Amount" source-path="net-price"/>
      <Target property="ErpDiscountLine~Tax" source-path="tax"/>
      <Target property="ErpDiscountLine~DiscountCode" source-path="promotion-id"/>
      <Target property="ErpDiscountLine~OfferId" source-path="campaign-id"/>
      <Target property="ErpDiscountLine~CouponId" source-path="coupon-id"/>
    </Properties>
  </Target>


  <!--New mapping with orders/order/shipping-lineitems/shipping-lineitem-->
  <!--<Target property="ErpSalesOrder~SalesLines" source-path="//orders/order/shipping-lineitems/shipping-lineitem" target-source="ErpSalesLine" repeat="true" >
    <Properties>
      <Target property="ErpSalesLine~Price" source-path="base-price"/>
      <Target property="ErpSalesLine~NetAmount" source-path="net-price"/>
      <Target property="ErpSalesLine~TaxAmount" source-path="tax"/>
      <Target property="ErpSalesLine~TaxRatePercent" source-path="tax-rate"/>
      <Target property="ErpSalesLine~TotalAmount" source-path="gross-price"/>
      <Target property="ErpSalesLine~BasePrice" source-path="base-price"/>
      <Target property="ErpSalesLine~ItemId" source-path="item-id"/>
      <Target property="ErpSalesLine~ShipmentId" source-path="shipment-id"/>
      <Target property="ErpSalesLine~Quantity" constant-value="1"/>
      <Target property="ErpSalesLine~Comment" constant-value="DeliveryItem"/>
    </Properties>
  </Target>-->

  <Target property="ErpSalesOrder~BillingAddress" source-path="//orders/order/customer/billing-address" target-source="ErpAddress" repeat="false">
    <Properties>
      <Target property="ErpAddress~Name" source-path="first-name"/>
      <Target property="ErpAddress~Name" source-path="last-name" concatenate="true"/>
      <Target property="ErpAddress~Street" source-path="address1"/>
      <Target property="ErpAddress~City" source-path="city"/>
      <Target property="ErpAddress~ZipCode" source-path="postal-code"/>
      <Target property="ErpAddress~State" source-path="state-code"/>
      <Target property="ErpAddress~TwoLetterISORegionName" source-path="country-code"/>
      <Target property="ErpAddress~ThreeLetterISORegionName" source-path="country-code"/>
      <Target property="ErpAddress~Phone" source-path="phone"/>
    </Properties>
  </Target>

  <!--ErpSalesOrder Shipments-->
  <Target property="ErpSalesOrder~Shipments" source-path="//orders/order/shipments/shipment" target-source="ErpShipment" repeat="true" >
    <Properties>
      <Target property="ErpShipment~ShipmentId" source-path="" attribute-name="shipment-id"/>
      <Target property="ErpShipment~DeliveryMode" source-path="shipping-method"/>
      <Target property="ErpShipment~ShippingStatus" source-path="status/shipping-status"/>
      <Target property="ErpShipment~IsGift" source-path="gift"/>

      <Target property="ErpShipment~DeliveryAddress" source-path="shipping-address" target-source="ErpAddress" repeat="false">
        <Properties>
          <Target property="ErpAddress~Name" source-path="first-name"/>
          <Target property="ErpAddress~Name" source-path="last-name" concatenate="true"/>
          <Target property="ErpAddress~Street" source-path="address1"/>
          <Target property="ErpAddress~City" source-path="city"/>
          <Target property="ErpAddress~ZipCode" source-path="postal-code"/>
          <Target property="ErpAddress~State" source-path="state-code"/>
          <Target property="ErpAddress~TwoLetterISORegionName" source-path="country-code"/>
          <Target property="ErpAddress~ThreeLetterISORegionName" source-path="country-code"/>
          <Target property="ErpAddress~Phone" source-path="phone"/>
        </Properties>
      </Target>
    </Properties>
  </Target>

  <!--ErpSalesOrder Payments-->
  <Target property="ErpSalesOrder~TenderLines" source-path="//orders/order/payments/payment" target-source="ErpTenderLine" repeat="true" >
    <Properties>
      <!--Common for all Payment methods-->
      <Target property="ErpTenderLine~TenderTypeId" source-path="processor-id"/>
      <Target property="ErpTenderLine~Amount" source-path="amount"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="transaction-id" is-custom-attribute="true" attribute-id="transaction-id"/>
      <!--PayPal-->
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalAck'']" is-custom-attribute="true" attribute-id="paypalAck"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalAmount'']" is-custom-attribute="true" attribute-id="paypalAmount"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalCorrelationId'']" is-custom-attribute="true" attribute-id="paypalCorrelationId"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalPayerID'']" is-custom-attribute="true" attribute-id="paypalPayerID"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalPaymentStatus'']" is-custom-attribute="true" attribute-id="paypalPaymentStatus"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalToken'']" is-custom-attribute="true" attribute-id="paypalToken"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalTransactionID'']" is-custom-attribute="true" attribute-id="paypalTransactionID"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''transactionsHistory'']" is-custom-attribute="true" attribute-id="transactionsHistory"/>

      <!--CreditCard-->
      <Target property="ErpTenderLine~CardTypeId" source-path="credit-card/card-type"/>
      <Target property="ErpTenderLine~MaskedCardNumber" source-path="credit-card/card-number"/>
      <Target property="ErpTenderLine~CardOrAccount" source-path="credit-card/card-holder"/>
      <Target property="ErpTenderLine~Authorization" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authorizationCode'']"/>
      <Target property="ErpTenderLine~CardToken" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceNumber'']"/>

      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''Fraud_Status'']" is-custom-attribute="true" attribute-id="Fraud_Status"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''Realtime_auth_and_fraud_check_done'']" is-custom-attribute="true" attribute-id="Realtime_auth_and_fraud_check_done"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authStatus'']" is-custom-attribute="true" attribute-id="authStatus"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authorizationCode'']" is-custom-attribute="true" attribute-id="authorizationCode"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''paymentProfileID'']" is-custom-attribute="true" attribute-id="paymentProfileID"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''responseDateTime'']" is-custom-attribute="true" attribute-id="responseDateTime"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''tokenNumber'']" is-custom-attribute="true" attribute-id="tokenNumber"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceIndex'']" is-custom-attribute="true" attribute-id="transactionReferenceIndex"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceNumber'']" is-custom-attribute="true" attribute-id="transactionReferenceNumber"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''webOrderID'']" is-custom-attribute="true" attribute-id="webOrderID"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''paypalPaymentMethod'']" is-custom-attribute="true" attribute-id="paypalPaymentMethod"/>

    </Properties>
  </Target>

  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''Fraud_Status'']" is-custom-attribute="true" attribute-id="Fraud_Status"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''Realtime_auth_and_fraud_check_done'']" is-custom-attribute="true" attribute-id="Realtime_auth_and_fraud_check_done"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authStatus'']" is-custom-attribute="true" attribute-id="authStatus"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authorizationCode'']" is-custom-attribute="true" attribute-id="authorizationCode"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''paymentProfileID'']" is-custom-attribute="true" attribute-id="paymentProfileID"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''responseDateTime'']" is-custom-attribute="true" attribute-id="responseDateTime"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceIndex'']" is-custom-attribute="true" attribute-id="transactionReferenceIndex"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceNumber'']" is-custom-attribute="true" attribute-id="transactionReferenceNumber"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''webOrderID'']" is-custom-attribute="true" attribute-id="webOrderID"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''paypalPaymentMethod'']" is-custom-attribute="true" attribute-id="paypalPaymentMethod"/>

  <!--TVW FDD-007-->
  <Target property="ErpSalesOrder~RequestedDeliveryDate" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDFROM'']"/>
  
  <!--Should remain empty: only applicable for use in call-center orders in BR1 -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVRESELLERACCOUNT'']" is-custom-attribute="true" attribute-id="TMVRESELLERACCOUNT"/>
  <!--Should remain empty: only applicable for use in call-center orders in BR1 -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVDISTRIBUTORACCOUNT'']" is-custom-attribute="true" attribute-id="TMVDISTRIBUTORACCOUNT"/>
  <!--Should remain empty: only applicable for use in call-center orders in BR1 -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVINDIRECTCUSTOMER'']" is-custom-attribute="true" attribute-id="TMVINDIRECTCUSTOMER"/>
  <!--Set in the "Synchronize orders" batch flow based on the TMVMainOfferType of the Offer Type Group dimension line. -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVMAINOFFERTYPE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVMAINOFFERTYPE"/>
  <!--Set in the "Synchronize orders" batch flow - more information to be received about this field. Contract confirmation fails if this field has no value. -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPRODUCTFAMILY'']" is-custom-attribute="true" attribute-id="TMVPRODUCTFAMILY"/>
  <!--This should be mapped to value ‚1‘ i.e. contract in the commerce link mapper. Orders created from the RetailSynchOrdersSchedulerTask class should use the value coming from the mapper and ignore the default value set on Accounts Receivable. This is for future-compatabilty reasons. -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVSALESORDERSUBTYPE'']" default-value="1" is-custom-attribute="true" attribute-id="TMVSALESORDERSUBTYPE"/>
  <!--Ensure that is set to false (i.e. the default value). This is only used in Advanced Contract Management -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVINVOICESCHEDULECOMPLETE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVINVOICESCHEDULECOMPLETE"/>
  <!--Set in the "Synchronize orders" batch flow based on the lowest value of SalesLine.TMVContractStatusLine within all contract lines. -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTSTATUSLINE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVCONTRACTSTATUSLINE"/>
  <!--This should be passed from Magento when the order is created as part of a marketing campaing (such as email sent to customer from Marketo). Part of work package WP-S07.-->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVSMMCAMPAIGNID'']" is-custom-attribute="true" attribute-id="TMVSMMCAMPAIGNID"/>
  <!--This should be passed from Magento . Part of work package WP-S07.-->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPURCHORDERFORMNUM'']" default-value="" is-custom-attribute="true" attribute-id="TMVPURCHORDERFORMNUM" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPIT'']" default-value="" is-custom-attribute="true" attribute-id="TMVPIT" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVQUOTATIONID'']" default-value="" is-custom-attribute="true" attribute-id="TMVQUOTATIONID" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVMIGRATEDORDERNUMBER'']" default-value="" is-custom-attribute="true" attribute-id="TMVMIGRATEDORDERNUMBER" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVMIGRATEDSALESLINENUMBER'']" default-value="" is-custom-attribute="true" attribute-id="TMVMIGRATEDSALESLINENUMBER" />
</Targets>', CAST(N'2016-12-29T18:04:09.473' AS DateTime), N'admin', CAST(N'2016-12-29T18:04:21.697' AS DateTime), N'admin', 0)


INSERT [dbo].[MappingTemplate] ([SourceEntity], [Name], [Type], [XML], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive]) VALUES (N'ErpStoreInfo', N'CREATE.ErpStoreInfo', N'CREATE', N'<?xml version="1.0" encoding="utf-16"?>
<stores xmlns="http://www.demandware.com/xml/impex/store/2007-04-30">
  <store repeat="true" data-source="ErpStoreInfo~stores" store-id="ErpStore~StoreId">
    <name>ErpStore~Name</name>
    <address1>ErpStore~Address</address1>
    <city>ErpStore~City</city>
    <postal-code>ErpStore~zipcode</postal-code>
    <state-code>ErpStore~State</state-code>
    <country-code>ErpStore~Country</country-code>
    <email>ErpStore~Email</email>
    <phone>ErpStore~phone</phone>
    <store-events xml:lang="x-default"></store-events>
    <store-hours xml:lang="x-default"></store-hours>
    <latitude>ErpStore~Latitute</latitude>
    <longitude>ErpStore~Longitute</longitude>
  </store>
</stores>', CAST(N'2016-12-29T20:10:31.737' AS DateTime), N'admin', CAST(N'2016-12-29T20:12:34.793' AS DateTime), N'admin', 0)

INSERT [dbo].[MappingTemplate] ([SourceEntity], [Name], [Type], [XML], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive]) VALUES (N'ErpCustomer', N'CREATE.ErpCustomer', N'CREATE', N'<?xml version="1.0" encoding="utf-16"?>
<customer>
  <company_account_no>ErpCustomer~AccountNumber</company_account_no>
  <company_name>ErpCustomer~Name</company_name>
	<vat_tax_id>ErpCustomer~VatNumber</vat_tax_id>
	<chamber_of_commerce_number>ErpCustomer~IdentificationNumber</chamber_of_commerce_number>
	<email_address>ErpCustomer~Email</email_address>
	<telephone>ErpCustomer~Phone</telephone>
	<addresses>
		<address repeat="true" data-source="ErpCustomer~CustomerAddresses">
			<street>ErpAddress~Street</street>
			<postcode>ErpAddress~ZipCode</postcode>
			<city>ErpAddress~City</city>
			<region_id>ErpAddress~State</region_id>
			<country_id>ErpAddress~ThreeLetterISORegionName</country_id>
		</address>
	</addresses>
</customer>', CAST(N'2016-12-29T17:41:24.010' AS DateTime), N'admin', CAST(N'2016-12-29T17:42:53.403' AS DateTime), N'admin', 0)

-- Payment Methods Inserts
TRUNCATE TABLE PaymentMethod

INSERT [dbo].[PaymentMethod] ([ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES (NULL, N'BASIC_CREDIT', N'4', 1, N'CreditCard', 0)
INSERT [dbo].[PaymentMethod] ([ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES (1, N'AMEX', N'11', 0, N'AMEX', 0)
INSERT [dbo].[PaymentMethod] ([ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES (1, N'VISA', N'4', 0, N'Visa', 0)
INSERT [dbo].[PaymentMethod] ([ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES (1, N'MASTER', N'4', 0, N'MasterCard', 0)
INSERT [dbo].[PaymentMethod] ([ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES (1, N'DISCOVER', N'5', 0, N'Discover', 0)
INSERT [dbo].[PaymentMethod] ([ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES (NULL, N'PAYPAL_EXPRESS', N'8', 0, N'Visa', 1)


-- Activate Jobs
UPDATE [dbo].[Jobs] SET IsActive = 1 WHERE [JobID] = 1 OR [JobID] = 2 OR [JobID] = 3 OR [JobID] = 4 OR [JobID] = 11 
									   OR [JobID] = 12 OR [JobID] = 14 OR [JobID] = 17 OR [JobID] = 19 OR [JobID] = 20 
									   OR [JobID] = 117 OR [JobID] = 119 OR [JobID] = 122