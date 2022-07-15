IF Not EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'APPLICATION.AbandonedCartDays')  
BEGIN
	INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) 
		VALUES (N'APPLICATION.AbandonedCartDays', N'7', N'Abandoned Cart Days', N'ERPAdapterGeneral', 1, 1, NULL, NULL, 0);
END
GO

IF Not EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'APPLICATION.AbandonedCartSalesOriginId')  
BEGIN
	INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) 
		VALUES (N'APPLICATION.AbandonedCartSalesOriginId', N'Abon Purch', N'Abandoned Cart SalesOriginId', N'ERPAdapterGeneral', 1, 1, NULL, NULL, 0);
END
GO

IF Not EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'APPLICATION.AbandonedCartTitle')  
BEGIN
	INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) 
		VALUES (N'APPLICATION.AbandonedCartTitle', N'Abandoned Shopping Cart - {0} - {1:yyyyMMdd}', N'Abandoned Cart Title', N'ERPAdapterGeneral', 1, 1, NULL, NULL, 0);
END
GO

IF Not EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'APPLICATION.AbandonedCartOrderType')  
BEGIN
	INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) 
		VALUES (N'APPLICATION.AbandonedCartOrderType', N'Quote', N'Abandoned Cart OrderType', N'ERPAdapterGeneral', 1, 1, NULL, NULL, 0);
END
GO

IF Not EXISTS(SELECT * FROM [dbo].[Jobs] WHERE [JobID] = 27)   
BEGIN
	INSERT INTO [dbo].[Jobs]
           ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI])
     VALUES
           (27, 'CartSync', 0, 1, '01:00:00', 1, 1, null, 0, 1);
END
GO
