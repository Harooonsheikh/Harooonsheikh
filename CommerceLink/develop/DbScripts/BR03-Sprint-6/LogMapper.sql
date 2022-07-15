
Create table LogMapper (
Id int identity(1,1) primary key,
MethodName nvarchar(max) NOT NULL,
IdentifierKey nvarchar(max)  NULL,
IdentifierPath nvarchar(max)  NULL
)
insert into LogMapper values ('CreateSalesOrderTransaction','original-order-no','$.order.original-order-no')
insert into LogMapper values ('CreateMergeSalesOrderTransaction','original-order-no','$.order.original-order-no')
insert into LogMapper values ('GetContractSalesOrders','SalesOrderId','$.SalesOrderId')
insert into LogMapper values ('CalculateLineAmount','CalculateDateFrom','$.CalculateDateFrom')
insert into LogMapper values ('CalculateTimeQuantity','CalculateDateFrom','$.CalculateDateFrom')
insert into LogMapper values ('GetContractInvoices','SalesOrderId','$.SalesOrderId')
insert into LogMapper values ('ValidateVATNumber','VATNumber','$.VATNumber')
insert into LogMapper values ('GetRetailAffiliations',null,null)
insert into LogMapper values ('CreateProductLicense','GUID','$.Products[0].GUID')
insert into LogMapper values ('CloseExistingOrder','SalesId','$.SalesId')
insert into LogMapper values ('ChangeContractPaymentMethod','SalesId','$.SalesId')
insert into LogMapper values ('CreateMergeSalesOrderTransaction','original-order-no','$.order.original-order-no')
--Cart--
insert into LogMapper values ('ApplyCouponsWithNewCart','CartId','$.CartId')
insert into LogMapper values ('GetCart','cartId',null)
insert into LogMapper values ('CreateOrUpdateCart','Id','$.Cart.Id')
insert into LogMapper values ('CreateMergedCart','CartId','$.CartId')
insert into LogMapper values ('MergeAddUpdateCartLines','CartId','$.CartId')
insert into LogMapper values ('AddCartLines','CartId','$.CartId')
insert into LogMapper values ('VoidCartLines','CartId','$.CartId')
insert into LogMapper values ('RemoveCartLines','CartId','$.CartId')
insert into LogMapper values ('UpdateDeliverySpecification','CartId','$.CartId')
insert into LogMapper values ('UpdateCartLines','CartId','$.CartId')
insert into LogMapper values ('AddCouponsToCart','CartId','$.CartId')
insert into LogMapper values ('RemoveCouponsFromCart','CartId','$.CartId')
insert into LogMapper values ('AddTenderLine','CartId','$.CartId')
insert into LogMapper values ('AddPreprocessedTenderLine','CartId','$.CartId')
insert into LogMapper values ('MergeRemoveUpdateCartLines','CartId','$.CartId')
--ContactPerson
insert into LogMapper values ('GetContactPerson','customerAccount',null)
insert into LogMapper values ('UpdateContactPerson','ContactPersonId','$.ContactPersonId')
insert into LogMapper values ('GetAllContactPersons','customerAccount',null)
insert into LogMapper values ('SaveContactPerson','CustAccount','$.ContactPerson.CustAccount')
-- Customer
insert into LogMapper values ('CreateCustomer','EcomCustomerId','$.Customer.EcomCustomerId')
insert into LogMapper values ('MergeCustomerReseller','EcomCustomerId','$.Customer.EcomCustomerId')
insert into LogMapper values ('MergeCreateCustomerContactPerson','EcomCustomerId','$.CustomerInfo.Customer.EcomCustomerId')
insert into LogMapper values ('MergeUpdateCustomerContactPerson','EcomCustomerId','$.Customer.EcomCustomerId')
insert into LogMapper values ('GetCustomer','CustomerId','$.CustomerId')
insert into LogMapper values ('GetCustomerByLicence','licenceNumber','$.licenceNumber[0]')
insert into LogMapper values ('UpdateCustomer','AccountNumber','$.Customer.AccountNumber')
insert into LogMapper values ('GetCustomerPaymentMethods','customerAccount','$.customerAccount')
insert into LogMapper values ('CreateCustomerPaymentMethod','CustomerNo','$.Customer.CustomerNo')
insert into LogMapper values ('UpdateCustomerPaymentMethod','CustomerNo','$.Customer.CustomerNo')
insert into LogMapper values ('GetCustomerInvoices','CustomerAccount','$.CustomerAccount')
insert into LogMapper values ('DeleteCustomerPaymentMethod','CardRecId','$.CardRecId')
-- InAppPurchase
insert into LogMapper values ('AutoRenewContract','ChannelReferenceId','$.ChannelReferenceId')
insert into LogMapper values ('CancelContract','ChannelReferenceId','$.ChannelReferenceId')
insert into LogMapper values ('ReactivateContract','ChannelReferenceId','$.ChannelReferenceId')
insert into LogMapper values ('TransferContract','ChannelReferenceId','$.ChannelReferenceId')
-- PaymentLink
insert into LogMapper values ('GetCustomerInfoByInvoiceId','InvoiceId','$.InvoiceId')
insert into LogMapper values ('GetCustomerInvoiceDetails','InvoiceId','$.InvoiceId')
insert into LogMapper values ('AddPaymentLinkForInvoice','InvoiceId','$.InvoiceId')
-- PriceDiscountController
insert into LogMapper values ('GetIndependentProductPriceDiscount','ProductIds','$.productIds[0]')
-- Quotation
insert into LogMapper values ('CreateQuotation','CustomerAccount','$.customerQuotation.CustomerAccount')
insert into LogMapper values ('GetQuotation','QuotationId','$.QuotationId')
insert into LogMapper values ('ConfirmCustomerQuotation','quotationId',null)
insert into LogMapper values ('RejectCustomerQuotation','QuotationId','$.QuotationId')
insert into LogMapper values ('ConfirmQuotation','QuotationId','$.QuotationId')
-- Store
insert into LogMapper values ('GetStoreAvailability','itemId',null)
insert into LogMapper values ('GetDiscountThreshold',null,null)
-- Payment
insert into LogMapper values ('SynchronizeServiceAccountIdsWithCL','StoreIds','$.StoreIds[0]')





























































select * from LogMapper








