----------------- Add Columns if they don't exist in PaymentConnector table -------------------

IF NOT EXISTS(
  SELECT *
  FROM INFORMATION_SCHEMA.COLUMNS
  WHERE 
    TABLE_NAME = 'PaymentConnector'
    AND COLUMN_NAME = 'ERPCreditCardProcessorName')
BEGIN
  ALTER TABLE PaymentConnector
    ADD [ERPCreditCardProcessorName] NVARCHAR(MAX) NULL
END;

IF NOT EXISTS(
  SELECT *
  FROM INFORMATION_SCHEMA.COLUMNS
  WHERE 
    TABLE_NAME = 'PaymentConnector'
    AND COLUMN_NAME = 'EComCreditCardProcessorName')
BEGIN
  ALTER TABLE PaymentConnector
    ADD [EComCreditCardProcessorName] NVARCHAR(MAX) NULL
END;
GO
-------------------------------------------------------

------------- Update and insert values for [ERPCreditCardProcessorName] and [EComCreditCardProcessorName] in Payment Connector table -------------

update
	PaymentConnector
SET
	[ERPCreditCardProcessorName] = 'Paypal',
	[EComCreditCardProcessorName] = 'PAYPAL_EXPRESS'
where
	PaymentConnectorName = 'PayPalPaymentProcessor';

update
	PaymentConnector
SET 
	[ERPCreditCardProcessorName] = 'Wiredcard',
	[EComCreditCardProcessorName] = 'BASIC_CREDIT'
where
	PaymentConnectorName = 'WirecardPaymentProcessor' ;

IF NOT EXISTS(	SELECT	*
				FROM	PaymentConnector
				WHERE	PaymentConnectorName = 'AllPagoPaymentProcessor'
				)
BEGIN
	INSERT INTO
		PaymentConnector
			(PaymentConnectorId, PaymentConnectorName, IsActive, [ERPCreditCardProcessorName], [EComCreditCardProcessorName])
		VALUES
			(6, 'AllPagoPaymentProcessor', 1, 'Adyen', 'ADYEN_CC');
END

IF NOT EXISTS(	SELECT	*
				FROM	PaymentConnector
				WHERE	PaymentConnectorName = 'AdyenPaymentProcessor'
				)
BEGIN
	INSERT INTO
		PaymentConnector
			(PaymentConnectorId, PaymentConnectorName, IsActive, [ERPCreditCardProcessorName], [EComCreditCardProcessorName])
		VALUES
			(7, 'AdyenPaymentProcessor', 1, NULL, NULL);
END
---------------------------------------------------------------------

-------------------------- Insert values in PaymentMethod table for Finland and Swededn -------------------------

DECLARE @StoreId INT
DECLARE @PaymentMethodId INT
DECLARE @ServiceAccountId NVARCHAR(MAX)


SELECT @StoreId = (SELECT StoreId from Store where Name = 'Finland')
SELECT @ServiceAccountId = N'ad05f548-21db-4978-86f0-cb380bc2f238' ----------------------- Change it depending upon the ServiceAccountId of Finland Channel at respective enviornment

INSERT [dbo].[PaymentMethod] ([StoreId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId], [PaymentConnectorId], [CreatedOn]) 
VALUES (@StoreId, NULL, N'ADYEN_CC', N'500', 1, N'CreditCard', 0, 1, 1, @ServiceAccountId, 7, GETUTCDATE())
Set  @PaymentMethodId = SCOPE_IDENTITY();
INSERT [dbo].[PaymentMethod] ([StoreId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId], [PaymentConnectorId], [CreatedOn]) 
VALUES (@StoreId, @PaymentMethodId, N'AMEX', N'500', 0, N'AMEX', 0, 1, 1, @ServiceAccountId, NULL, GETUTCDATE())
INSERT [dbo].[PaymentMethod] ([StoreId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId], [PaymentConnectorId], [CreatedOn]) 
VALUES (@StoreId, @PaymentMethodId, N'VISA', N'500', 0, N'Visa', 0, 1, 1, @ServiceAccountId, NULL, GETUTCDATE())
INSERT [dbo].[PaymentMethod] ([StoreId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId], [PaymentConnectorId], [CreatedOn]) 
VALUES (@StoreId, @PaymentMethodId, N'MASTERCARD', N'500', 0, N'MasterCard', 0, 1, 1,@ServiceAccountId, NULL, GETUTCDATE())
INSERT [dbo].[PaymentMethod] ([StoreId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId], [PaymentConnectorId], [CreatedOn]) 
VALUES (@StoreId, @PaymentMethodId, N'DISCOVER', N'500', 0, N'Discover', 0, 1, 1, @ServiceAccountId, NULL, GETUTCDATE())

SELECT @StoreId = (SELECT StoreId from Store where Name = 'Sweden')
SELECT @ServiceAccountId =  N'481f81ae-59e4-410e-8960-1689675c90a0' ----------------------- Change it depending upon the ServiceAccountId of Sweden Channel at respective enviornment

INSERT [dbo].[PaymentMethod] ([StoreId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId], [PaymentConnectorId], [CreatedOn]) 
VALUES (@StoreId, NULL, N'ADYEN_CC', N'500', 1, N'CreditCard', 0, 1, 1, @ServiceAccountId, 7, GETUTCDATE())
Set  @PaymentMethodId = SCOPE_IDENTITY();
INSERT [dbo].[PaymentMethod] ([StoreId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId], [PaymentConnectorId], [CreatedOn]) 
VALUES (@StoreId, @PaymentMethodId, N'AMEX', N'500', 0, N'AMEX', 0, 1, 1, @ServiceAccountId, NULL, GETUTCDATE())
INSERT [dbo].[PaymentMethod] ([StoreId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId], [PaymentConnectorId], [CreatedOn]) 
VALUES (@StoreId, @PaymentMethodId, N'VISA', N'500', 0, N'Visa', 0, 1, 1, @ServiceAccountId, NULL, GETUTCDATE())
INSERT [dbo].[PaymentMethod] ([StoreId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId], [PaymentConnectorId], [CreatedOn]) 
VALUES (@StoreId, @PaymentMethodId, N'MASTERCARD', N'500', 0, N'MasterCard', 0, 1, 1, @ServiceAccountId, NULL, GETUTCDATE())
INSERT [dbo].[PaymentMethod] ([StoreId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId], [PaymentConnectorId], [CreatedOn]) 
VALUES (@StoreId, @PaymentMethodId, N'DISCOVER', N'500', 0, N'Discover', 0, 1, 1, @ServiceAccountId, NULL, GETUTCDATE())