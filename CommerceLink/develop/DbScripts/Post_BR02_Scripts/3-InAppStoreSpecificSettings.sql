-- Select Apple StoreId from store table 
DECLARE @StoreId INT = (SELECT StoreId FROM Store WHERE [Name] = 'Apple App');

-- Run for Apple store only
update ApplicationSetting set Value = 'FALSE' 
WHERE [key]='SALESORDER.Get_SalesTransaction_Id_From_Ecom' and  StoreId = @StoreId;

-- Disable all jobs for Apple store only
update JobSchedule set IsActive = 0
WHERE StoreId = @StoreId;

-- Insert products in integrationkey table for Apple store

INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (2, N'5637144868', N'TVAD003_000000088', N'TVAD003:000000088', @StoreId, N'Admin', NULL, NULL);

INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (2, N'5637144869', N'TVB0001_000000089', N'TVB0001:000000089', @StoreId, N'Admin', NULL, NULL);


--NOTE: PLEASE MANUALLY UPDATE LANGUAGE CODES FOR APPLE APP STORE IN CONFIGURABLE OBJECT TABLE.



