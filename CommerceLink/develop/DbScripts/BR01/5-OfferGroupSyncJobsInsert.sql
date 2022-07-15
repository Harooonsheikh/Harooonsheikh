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
           (25
           ,'OfferGroupSync'
           ,0
           ,1
           ,'00:00:00'
           ,1
           ,1
           ,null
           ,0
           ,1),
		   
		   (125
           ,'UploadOfferGroupSync'
           ,0
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
		(N'OFFERTYPEGROUPS.Local_Output_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\offertypegroups', N'Offer type groups Local Output Path', N'OfferTypeGroups', 1, 1, 0, 0, NULL, NULL, 0, 1, 1, NULL, NULL, NULL, NULL),
		(N'OFFERTYPEGROUPS.Remote_Path', N'{APPLICATION.Remote_Base_Path}/offertypegroups', N'Offer type groups FTP Path', N'OfferTypeGroups', 2, 1, 0, 0, NULL, NULL, 0, 1, 1, NULL, NULL, NULL, NULL),
		(N'OFFERTYPEGROUPS.Filename_Prefix', N'offertypegroups-', N'Offertypegroups File Tag', N'OfferTypeGroups', 3, 1, 0, 0, NULL, NULL, 0, 1, 1, NULL, NULL, NULL, NULL)
		--,(N'OFFERTYPEGROUPS.Offertypegroups_Id', N'123', N'Offer type groups Id', N'OfferTypeGroups', 4, 1, 0, 0, NULL, NULL, 0, 1, 1, NULL, NULL, NULL, NULL)

Go