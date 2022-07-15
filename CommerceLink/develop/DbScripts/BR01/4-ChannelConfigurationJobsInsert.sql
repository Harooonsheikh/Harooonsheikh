
INSERT INTO [dbo].[Jobs]
           ([JobID]
           ,[JobName]
           ,[JobInterval]
           ,[IsRepeatable]
           ,[StartTime]
           ,[IsActive]
           ,[JobStatus]
           ,[CronExpression]
           ,[JobType]
           ,[ShowInUI])
     VALUES
           (24
           ,'ConfigurationSync'
           ,30
           ,1
           ,'00:00:00'
           ,1
           ,1
           ,null
           ,0
           ,1),
		   
		   (124
           ,'UploadConfigurationSync'
           ,15
           ,1
           ,'00:00:00'
           ,1
           ,1
           ,null
           ,1
           ,1)
GO


INSERT [dbo].[ApplicationSetting] 
		([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [IsCustom], [StoreId_FK], [CreatedOn], [UpdatedOn], [IsPassword], [SettingForDemandWare], [SettingForMagento], [Created], [CreateByUser_FK], [Modified], [ModifiedByUser_FK]) 
		VALUES 
		(N'CHANNELCONFIGURATION.Local_Output_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\Configuration', N'Channel Configuration Local Output Path', N'ChannelConfiguration', 1, 1, 0, 0, NULL, NULL, 0, 1, 1, NULL, NULL, NULL, NULL),
		(N'CHANNELCONFIGURATION.Remote_Path', N'{APPLICATION.Remote_Base_Path}/Configuration', N'Channel Configuration FTP Path', N'ChannelConfiguration', 2, 1, 0, 0, NULL, NULL, 0, 1, 1, NULL, NULL, NULL, NULL),
		(N'CHANNELCONFIGURATION.Filename_Prefix', N'ChannelConfiguration-', N'Channel Configuration File Tag', N'ChannelConfiguration', 3, 1, 0, 0, NULL, NULL, 0, 1, 1, NULL, NULL, NULL, NULL)
		--,(N'CHANNELCONFIGURATION.ChannelConfiguration_Id', N'123', N'Channel Configuration Id', N'ChannelConfiguration', 4, 1, 0, 0, NULL, NULL, 0, 1, 1, NULL, NULL, NULL, NULL)

Go