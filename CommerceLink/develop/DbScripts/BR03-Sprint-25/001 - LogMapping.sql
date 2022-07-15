
IF (NOT EXISTS(	SELECT * FROM LogMapper WHERE MethodName = 'RebuyContract'))
BEGIN
	Insert Into LogMapper (MethodName,IdentifierKey,IdentifierPath)
	VALUES ('RebuyContract','CustomerRef','$.CustomerRef')
END
