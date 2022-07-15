IF NOT EXISTS (select * from AosUrlSetting WHERE MethodName = 'ProcessContractReactivate')
BEGIN
	INSERT INTO AosUrlSetting(MethodName,IsActive,MethodUrl)
	VALUES('ProcessContractReactivate',1,'/api/Services/TMVContractOperationsServiceGroup/TMVContractReactivateService/ProcessContractReactivate')	                                      
END