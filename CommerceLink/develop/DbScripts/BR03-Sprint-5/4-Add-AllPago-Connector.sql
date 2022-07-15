 --Get PaymentConnectorId
 declare @PaymentConnectorId int;
 set @PaymentConnectorId = (select max(PaymentConnectorId) from PaymentConnector) +1;
 --Add PaymentConnector
 insert into PaymentConnector (PaymentConnectorId, PaymentConnectorName, IsActive)
  values (@PaymentConnectorId, 'AllPagoPaymentProcessor', 1)

  --IMPORTANT NOTE: Please Chanege Store Id manually according to requirments.
declare @storeId int; 
set @storeId = (select top 1 StoreId from Store where name = 'Brazil');


insert PaymentMethod ( ParentPaymentMethodId, ECommerceValue, ErpValue, HasSubMethod, ErpCode, IsPrepayment, StoreId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, IsCreditCard, UsePaymentConnector, ServiceAccountId, PaymentConnectorId)
  values ( null, 'ALLPAGO_CC', 502, 1, 'CreditCard', 0, @storeId, '2019-04-08 14:31:24.0230000', 1, null, null,1,1, '', @PaymentConnectorId)

--Get Parent PaymentMethod Id
  declare @ParentPaymentMethodId int;
 set @PaymentConnectorId =  SCOPE_IDENTITY();

insert PaymentMethod ( ParentPaymentMethodId, ECommerceValue, ErpValue, HasSubMethod, ErpCode, IsPrepayment, StoreId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, IsCreditCard, UsePaymentConnector, ServiceAccountId, PaymentConnectorId)
  values ( @PaymentConnectorId, 'AMEX', 502, 0, 'AMEX', 0, @storeId, '2019-04-08 14:31:24.0230000', 1, null, null,1,1, '', null)

insert PaymentMethod ( ParentPaymentMethodId, ECommerceValue, ErpValue, HasSubMethod, ErpCode, IsPrepayment, StoreId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, IsCreditCard, UsePaymentConnector, ServiceAccountId, PaymentConnectorId)
  values ( @PaymentConnectorId, 'VISA', 502, 0, 'Visa', 0, @storeId, '2019-04-08 14:31:24.0230000', 1, null, null,1,1, '', null)

insert PaymentMethod ( ParentPaymentMethodId, ECommerceValue, ErpValue, HasSubMethod, ErpCode, IsPrepayment, StoreId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, IsCreditCard, UsePaymentConnector, ServiceAccountId, PaymentConnectorId)
  values ( @PaymentConnectorId, 'MASTERCARD', 502, 0, 'MasterCard', 0, @storeId, '2019-04-08 14:31:24.0230000', 1, null, null,1,1, '', null)

insert PaymentMethod ( ParentPaymentMethodId, ECommerceValue, ErpValue, HasSubMethod, ErpCode, IsPrepayment, StoreId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, IsCreditCard, UsePaymentConnector, ServiceAccountId, PaymentConnectorId)
  values ( @PaymentConnectorId, 'DISCOVER', 502, 0, 'Discover', 0, @storeId, '2019-04-08 14:31:24.0230000', 1, null, null,1,1, '', null)


  





