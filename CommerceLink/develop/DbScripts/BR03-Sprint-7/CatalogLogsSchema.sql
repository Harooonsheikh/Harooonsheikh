

CREATE TABLE [dbo].[CatalogLogs](
	[CatalogId] [int] IDENTITY(1,1) Primary key,
	[Key] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[StoreId] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NULL
) 

