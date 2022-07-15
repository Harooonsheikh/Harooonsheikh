IF (NOT EXISTS(	SELECT * FROM LogMapper WHERE MethodName = 'GetOrValidatePrice'))
BEGIN
	insert into LogMapper values ('GetOrValidatePrice','IndirectCustomerAccount','$.IndirectCustomerAccount')
END