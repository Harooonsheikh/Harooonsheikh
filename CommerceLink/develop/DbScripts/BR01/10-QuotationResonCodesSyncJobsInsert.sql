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
           (26
           ,'QuotationReasonGroupSync'
           ,0
           ,1
           ,'00:00:00'
           ,1
           ,1
           ,null
           ,0
           ,1),
		   
		   (126
           ,'UploadQuotationReasonGroupSync'
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
  (N'QUOTATIONREASONGROUP.Local_Output_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\quotationreasongroups', N'Qoutation Reason Groups Local Output Path', N'QUOTATIONREASONGROUP', 1, 1, 0, 0, NULL, NULL, 0, 1, 1, NULL, NULL, NULL, NULL),
  (N'QUOTATIONREASONGROUP.Remote_Path', N'{APPLICATION.Local_Base_Path}/quotationreasongroups', N'Offer type groups FTP Path', N'QUOTATIONREASONGROUP', 2, 1, 0, 0, NULL, NULL, 0, 1, 1, NULL, NULL, NULL, NULL),
  (N'QUOTATIONREASONGROUP.Filename_Prefix', N'QuotationReason-', N'Quotation Reason Groups File Tag', N'QUOTATIONREASONGROUP', 3, 1, 0, 0, NULL, NULL, 0, 1, 1, NULL, NULL, NULL, NULL),
  (N'QUOTATIONREASONGROUP.QuotationReason_Id', N'123', N'Quotation Reason Groups', N'QUOTATIONREASONGROUP', 4, 1, 0, 0, NULL, NULL, 0, 1, 1, NULL, NULL, NULL, NULL)

Go