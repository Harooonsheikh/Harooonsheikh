IF (NOT EXISTS(	SELECT * FROM LogMapper WHERE MethodName = 'UpdateCustomerPortalLink'))
BEGIN
	Insert Into LogMapper (MethodName,IdentifierKey,IdentifierPath)
	VALUES ('UpdateCustomerPortalLink','SalesId','$.SalesId')
END


