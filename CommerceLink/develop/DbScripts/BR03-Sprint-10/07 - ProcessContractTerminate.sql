SET IDENTITY_INSERT [dbo].[AosUrlSetting] ON 

INSERT [dbo].[AosUrlSetting] ([Id], [MethodName], [MethodUrl], [IsActive]) 
VALUES (3, N'ProcessContractTerminate', 
N'/api/Services/TMVContractOperationsServiceGroup/TMVContractTerminationService/ProcessContractTerminate', 1)

SET IDENTITY_INSERT [dbo].[AosUrlSetting] OFF

-------------------

insert into LogMapper values ('ProcessContractTerminate','SalesOrderId','$.SalesOrderId')
