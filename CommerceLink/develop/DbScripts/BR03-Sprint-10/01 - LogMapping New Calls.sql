-- Customer Portal
IF (NOT EXISTS(	SELECT * FROM LogMapper WHERE MethodName = 'CreateContractNewPaymentMethod'))
BEGIN
	insert into LogMapper values ('CreateContractNewPaymentMethod','SalesId','$.SalesId')
END

IF (NOT EXISTS(	SELECT * FROM LogMapper WHERE MethodName = 'UpdateBillingAddress'))
BEGIN
	insert into LogMapper values ('UpdateBillingAddress','SalesOrderId','$.SalesOrderId')
END

IF (NOT EXISTS(	SELECT * FROM LogMapper WHERE MethodName = 'CreateSalesOrder'))
BEGIN
	insert into LogMapper values ('CreateSalesOrder','ChannelReferenceId','$.ChannelReferenceId')
END