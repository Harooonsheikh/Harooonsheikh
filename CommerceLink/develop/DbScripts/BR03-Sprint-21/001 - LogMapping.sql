IF (NOT EXISTS(	SELECT * FROM LogMapper WHERE MethodName = 'GetBoletoUrl'))
BEGIN
	INSERT INTO LogMapper (MethodName,IdentifierKey,IdentifierPath)
	VALUES ('GetBoletoUrl','InvoiceId','$.InvoiceId')
END
