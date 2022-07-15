IF (NOT EXISTS(	SELECT * FROM LogMapper WHERE MethodName = 'PromiseToPay'))
BEGIN
	insert into LogMapper values ('PromiseToPay','InvoiceId','$.InvoiceId')
END
