
IF (NOT EXISTS(	SELECT * FROM LogMapper WHERE MethodName = 'CreateUpdateCustomer'))
BEGIN
	insert into LogMapper values ('CreateUpdateCustomer','EcomCustomerId','$.Customer.EcomCustomerId')
END

IF (NOT EXISTS(	SELECT * FROM LogMapper WHERE MethodName = 'CreateUpdateContactPerson'))
BEGIN
	insert into LogMapper values ('CreateUpdateContactPerson','CustAccount','$.ContactPerson.CustAccount')
END

IF (NOT EXISTS(	SELECT * FROM LogMapper WHERE MethodName = 'CreateContactPerson'))
BEGIN
	insert into LogMapper values ('CreateContactPerson','CustAccount','$.CustAccount')
END
