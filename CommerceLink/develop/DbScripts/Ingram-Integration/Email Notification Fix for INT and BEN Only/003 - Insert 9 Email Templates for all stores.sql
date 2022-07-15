SELECT StoreId
INTO #ControlTable 
FROM dbo.Store

DECLARE @StoreId INT

WHILE exists (SELECT * FROM #ControlTable)
BEGIN

    SELECT @StoreId = (SELECT TOP 1 StoreId
                       FROM #ControlTable
                       ORDER BY StoreId ASC)

INSERT [dbo].[EmailTemplate] ([EmailTemplateId], [Name], [Subject], [Body], [Footer], [IsActive], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (2, N'Product', N'Product : {0} Failed on Commerce Link', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>', N'Take Care.', 1, @storeId, CAST(N'2018-03-09T15:01:26.7000000' AS DateTime2), N'1', CAST(N'2018-05-24T07:47:12.803' AS DateTime), N'Admin')

INSERT [dbo].[EmailTemplate] ([EmailTemplateId], [Name], [Subject], [Body], [Footer], [IsActive], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (3, N'Customer', N'Customer: {0} Failed on Commerce Link', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>', N'Take Care.', 1, @storeId, CAST(N'2018-03-09T15:02:23.1200000' AS DateTime2), N'1', CAST(N'2018-05-24T07:47:20.810' AS DateTime), N'Admin')

INSERT [dbo].[EmailTemplate] ([EmailTemplateId], [Name], [Subject], [Body], [Footer], [IsActive], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (4, N'Store', N'Store: {0} Failed on Commerce Link', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>', N'Take Care.', 1, @storeId, CAST(N'2018-03-09T15:02:57.0766667' AS DateTime2), N'1', CAST(N'2018-05-24T07:47:27.333' AS DateTime), N'Admin')

INSERT [dbo].[EmailTemplate] ([EmailTemplateId], [Name], [Subject], [Body], [Footer], [IsActive], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (5, N'SalesOrder', N'Sales Order : {0} Failed on Commerce Link', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>', N'Take Care.', 1, @storeId, CAST(N'2018-03-09T15:03:05.6266667' AS DateTime2), N'1', CAST(N'2018-05-24T07:47:39.150' AS DateTime), N'Admin')

INSERT [dbo].[EmailTemplate] ([EmailTemplateId], [Name], [Subject], [Body], [Footer], [IsActive], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (6, N'Inventory', N'Inventory : {0} Failed on Commerce Link', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>', N'Take Care.', 1, @storeId, CAST(N'2018-03-09T15:03:10.0433333' AS DateTime2), N'1', CAST(N'2018-05-24T07:47:46.300' AS DateTime), N'Admin')

INSERT [dbo].[EmailTemplate] ([EmailTemplateId], [Name], [Subject], [Body], [Footer], [IsActive], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (22, N'SimpleNotification', N'Service Stopped on {0} Commerce Link', N'Hi,  Service Stopped  {0}', N'Take Care', 1, @storeId, CAST(N'2018-03-09T15:03:30.1300000' AS DateTime2), N'1', CAST(N'2018-05-24T07:48:05.913' AS DateTime), N'Admin')

INSERT [dbo].[EmailTemplate] ([EmailTemplateId], [Name], [Subject], [Body], [Footer], [IsActive], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (11, N'Price', N'Price: {0} Failed on Commerce Link', N'<h2>Hi {0}</h2> <h4>{1}</h4> <h3>Exception</h3> <p>{2}</p> <h3>{3}</h3>', N'Take Care', 1, @storeId, CAST(N'2018-05-24T07:48:58.6210874' AS DateTime2), N'Admin', CAST(N'2018-05-24T07:48:58.620' AS DateTime), N'Admin')

INSERT [dbo].[EmailTemplate] ([EmailTemplateId], [Name], [Subject], [Body], [Footer], [IsActive], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (10, N'Discount', N'Discount: {0} Failed on Commerce Link', N'<h2>Hi {0}</h2> <h4>{1}</h4> <h3>Exception</h3> <p>{2}</p> <h3>{3}</h3>', N'Take Care', 1, @storeId, CAST(N'2018-05-24T07:49:28.2621771' AS DateTime2), N'Admin', CAST(N'2018-05-24T07:49:43.790' AS DateTime), N'Admin')

INSERT [dbo].[EmailTemplate] ([EmailTemplateId], [Name], [Subject], [Body], [Footer], [IsActive], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (27, N'QuantityDiscount', N'Quantity Discount : {0} Failed on Commerce Link Dev09', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>




', N'Take Care.', 1, @storeId, CAST(N'2018-09-17T15:53:36.2101576' AS DateTime2), N'System', NULL, NULL)

	DELETE #ControlTable
    WHERE StoreId = @StoreId

END

DROP TABLE #ControlTable