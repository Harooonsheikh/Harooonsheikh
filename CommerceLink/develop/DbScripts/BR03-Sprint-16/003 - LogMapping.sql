IF (NOT EXISTS(	SELECT * FROM LogMapper WHERE MethodName = 'ContractRenewal'))
BEGIN
	insert into LogMapper values ('ContractRenewal','PACLicense','$.PACLicense')
END
