SET IDENTITY_INSERT [dbo].[AosUrlSetting] ON 

INSERT [dbo].[AosUrlSetting] ([Id], [MethodName], [MethodUrl], [IsActive]) VALUES (1, N'AifUserSessionService_GetUserSessionInfo', N'/api/services/UserSessionService/AifUserSessionService/GetUserSessionInfo', 1)
INSERT [dbo].[AosUrlSetting] ([Id], [MethodName], [MethodUrl], [IsActive]) VALUES (2, N'UpdateSubscriptionContract', N'/api/Services/TMVContractOperationsServiceGroup/TMVContractOperationsService/ProcessContractOperations', 1)
SET IDENTITY_INSERT [dbo].[AosUrlSetting] OFF
