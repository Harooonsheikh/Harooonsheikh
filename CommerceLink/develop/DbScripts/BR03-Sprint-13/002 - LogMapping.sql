IF (NOT EXISTS(	SELECT * FROM LogMapper WHERE MethodName = 'CreatePaymentJournal'))
BEGIN
	insert into LogMapper values ('CreatePaymentJournal','InvoiceId','$.InvoiceId')
END

IF (NOT EXISTS(	SELECT * FROM LogMapper WHERE MethodName = 'TransferPartnerContract'))
BEGIN
	insert into LogMapper values ('TransferPartnerContract','SalesOrderId','$.SalesOrderId')
END