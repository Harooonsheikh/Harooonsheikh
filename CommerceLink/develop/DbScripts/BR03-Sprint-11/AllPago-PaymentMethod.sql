
DECLARE @StoreId INT
DECLARE @PaymentMethodId INT
DECLARE @ServiceId NVARCHAR(200)

SELECT @StoreId=StoreId FROM dbo.Store WHERE RetailChannelId = '000177'

SELECT @ServiceId = '2fb97b3f-feeb-438d-bd11-47f113874355'


IF NOT EXISTS (SELECT * FROM PaymentMethod WHERE ECommerceValue = 'ALLPAGO_CC' and StoreId = @StoreId)
BEGIN
Insert Into [PaymentMethod] ([ECommerceValue],[ErpValue],[HasSubMethod],[ErpCode],[IsPrepayment],[StoreId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy] ,[IsCreditCard],[UsePaymentConnector],[ServiceAccountId],[PaymentConnectorId]) values (
'ALLPAGO_CC',502,1,'CreditCard',0,@StoreId,GETDATE(),1,null,1, 1,1,@ServiceId,6)


SELECT @PaymentMethodId = ( SELECT TOP 1 [PaymentMethodId] FROM [PaymentMethod] WHERE ErpValue = 502 and StoreId = @StoreId)

Insert Into [PaymentMethod] ([ParentPaymentMethodId],[ECommerceValue],[ErpValue],[HasSubMethod],[ErpCode],[IsPrepayment],[StoreId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy] ,[IsCreditCard],[UsePaymentConnector],[ServiceAccountId],[PaymentConnectorId]) values (
@PaymentMethodId,'AMEX',502,0,'Amex',0,@StoreId,GETDATE(),1,null,1, 1,1,@ServiceId,null)

Insert Into [PaymentMethod] ([ParentPaymentMethodId],[ECommerceValue],[ErpValue],[HasSubMethod],[ErpCode],[IsPrepayment],[StoreId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy] ,[IsCreditCard],[UsePaymentConnector],[ServiceAccountId],[PaymentConnectorId]) values (
@PaymentMethodId,'DINERS',502,0,'Diners',0,@StoreId,GETDATE(),1,null,1, 1,1,@ServiceId,null)

Insert Into [PaymentMethod] ([ParentPaymentMethodId],[ECommerceValue],[ErpValue],[HasSubMethod],[ErpCode],[IsPrepayment],[StoreId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy] ,[IsCreditCard],[UsePaymentConnector],[ServiceAccountId],[PaymentConnectorId]) values (
@PaymentMethodId,'DISCOVER',502,0,'Discover',0,@StoreId,GETDATE(),1,null,1, 1,1,@ServiceId,null)

Insert Into [PaymentMethod] ([ParentPaymentMethodId],[ECommerceValue],[ErpValue],[HasSubMethod],[ErpCode],[IsPrepayment],[StoreId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy] ,[IsCreditCard],[UsePaymentConnector],[ServiceAccountId],[PaymentConnectorId]) values (
@PaymentMethodId,'ELO',502,0,'Elo',0,@StoreId,GETDATE(),1,null,1, 1,1,@ServiceId,null)

Insert Into [PaymentMethod] ([ParentPaymentMethodId],[ECommerceValue],[ErpValue],[HasSubMethod],[ErpCode],[IsPrepayment],[StoreId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy] ,[IsCreditCard],[UsePaymentConnector],[ServiceAccountId],[PaymentConnectorId]) values (
@PaymentMethodId,'HIPERCARD',502,0,'Hipercard',0,@StoreId,GETDATE(),1,null,1, 1,1,@ServiceId,null)

Insert Into [PaymentMethod] ([ParentPaymentMethodId],[ECommerceValue],[ErpValue],[HasSubMethod],[ErpCode],[IsPrepayment],[StoreId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy] ,[IsCreditCard],[UsePaymentConnector],[ServiceAccountId],[PaymentConnectorId]) values (
@PaymentMethodId,'JCB',502,0,'Jcb',0,@StoreId,GETDATE(),1,null,1, 1,1,@ServiceId,null)

Insert Into [PaymentMethod] ([ParentPaymentMethodId],[ECommerceValue],[ErpValue],[HasSubMethod],[ErpCode],[IsPrepayment],[StoreId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy] ,[IsCreditCard],[UsePaymentConnector],[ServiceAccountId],[PaymentConnectorId]) values (
@PaymentMethodId,'MASTER',502,0,'Master',0,@StoreId,GETDATE(),1,null,1, 1,1,@ServiceId,null)

Insert Into [PaymentMethod] ([ParentPaymentMethodId],[ECommerceValue],[ErpValue],[HasSubMethod],[ErpCode],[IsPrepayment],[StoreId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy] ,[IsCreditCard],[UsePaymentConnector],[ServiceAccountId],[PaymentConnectorId]) values (
@PaymentMethodId,'VISA',502,0,'Visa',0,@StoreId,GETDATE(),1,null,1, 1,1,@ServiceId,null)
END
