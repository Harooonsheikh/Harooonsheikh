DECLARE @StoreId BIGINT
DECLARE @ParentPaymentMethodId INT
SELECT @StoreId = StoreId FROM Store WHERE Name LIKE 'China'
IF @StoreId IS NOT NULL AND
	NOT EXISTS (select * from PaymentMethod WHERE StoreId = @StoreId AND ErpValue = 601)
BEGIN
	INSERT INTO PaymentMethod (ParentPaymentMethodId,ECommerceValue,ErpValue,HasSubMethod,ErpCode,IsPrepayment,StoreId,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsCreditCard,UsePaymentConnector,ServiceAccountId,PaymentConnectorId)
	VALUES (NULL,'ADYEN_HPP',601,1,'CreditCard',0,@StoreId,GETUTCDATE(),'System', GETUTCDATE(), 'System', 1, 1, 'caf9e427-cc64-454a-902e-be29186c4470', NULL)
	
	SELECT @ParentPaymentMethodId = PaymentMethodId FROM PaymentMethod WHERE STOREID = @StoreId AND ErpValue = 601
	
	INSERT INTO PaymentMethod (ParentPaymentMethodId,ECommerceValue,ErpValue,HasSubMethod,ErpCode,IsPrepayment,StoreId,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsCreditCard,UsePaymentConnector,ServiceAccountId,PaymentConnectorId)
	VALUES (@ParentPaymentMethodId,'ALIPAY',601,0,'Alipay',0,@StoreId,GETUTCDATE(),'System', GETUTCDATE(), 'System', 1, 1, 'caf9e427-cc64-454a-902e-be29186c4470', NULL)
END
GO

DECLARE @StoreId BIGINT
DECLARE @ParentPaymentMethodId INT
SELECT @StoreId = StoreId FROM Store WHERE Name LIKE 'Hong Kong'
IF @StoreId IS NOT NULL AND
	NOT EXISTS (select * from PaymentMethod WHERE StoreId = @StoreId AND ErpValue = 601)
BEGIN
	INSERT INTO PaymentMethod (ParentPaymentMethodId,ECommerceValue,ErpValue,HasSubMethod,ErpCode,IsPrepayment,StoreId,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsCreditCard,UsePaymentConnector,ServiceAccountId,PaymentConnectorId)
	VALUES (NULL,'ADYEN_HPP',601,1,'CreditCard',0,@StoreId,GETUTCDATE(),'System', GETUTCDATE(), 'System', 1, 1, 'caf9e427-cc64-454a-902e-be29186c4470', NULL)
	
	SELECT @ParentPaymentMethodId = PaymentMethodId FROM PaymentMethod WHERE STOREID = @StoreId AND ErpValue = 601
	
	INSERT INTO PaymentMethod (ParentPaymentMethodId,ECommerceValue,ErpValue,HasSubMethod,ErpCode,IsPrepayment,StoreId,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsCreditCard,UsePaymentConnector,ServiceAccountId,PaymentConnectorId)
	VALUES (@ParentPaymentMethodId,'ALIPAY_HK',601,0,'Alipay',0,@StoreId,GETUTCDATE(),'System', GETUTCDATE(), 'System', 1, 1, 'caf9e427-cc64-454a-902e-be29186c4470', NULL)
END
GO

DECLARE @StoreId BIGINT
DECLARE @ParentPaymentMethodId INT
SELECT @StoreId = StoreId FROM Store WHERE Name LIKE 'CNY'
IF @StoreId IS NOT NULL AND
	NOT EXISTS (select * from PaymentMethod WHERE StoreId = @StoreId AND ErpValue = 601)
BEGIN
	INSERT INTO PaymentMethod (ParentPaymentMethodId,ECommerceValue,ErpValue,HasSubMethod,ErpCode,IsPrepayment,StoreId,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsCreditCard,UsePaymentConnector,ServiceAccountId,PaymentConnectorId)
	VALUES (NULL,'ADYEN_HPP',601,1,'CreditCard',0,@StoreId,GETUTCDATE(),'System', GETUTCDATE(), 'System', 1, 1, 'caf9e427-cc64-454a-902e-be29186c4470', NULL)
	
	SELECT @ParentPaymentMethodId = PaymentMethodId FROM PaymentMethod WHERE STOREID = @StoreId AND ErpValue = 601
	
	INSERT INTO PaymentMethod (ParentPaymentMethodId,ECommerceValue,ErpValue,HasSubMethod,ErpCode,IsPrepayment,StoreId,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsCreditCard,UsePaymentConnector,ServiceAccountId,PaymentConnectorId)
	VALUES (@ParentPaymentMethodId,'ALIPAY',601,0,'Alipay',0,@StoreId,GETUTCDATE(),'System', GETUTCDATE(), 'System', 1, 1, 'caf9e427-cc64-454a-902e-be29186c4470', NULL)
END
GO
