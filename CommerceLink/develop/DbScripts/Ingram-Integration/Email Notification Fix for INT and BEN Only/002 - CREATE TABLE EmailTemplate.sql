/****** Object:  Table [dbo].[EmailTemplate]    Script Date: 3/3/2020 5:13:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EmailTemplate](
	[EmailTemplateId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Subject] [nvarchar](250) NOT NULL,
	[Body] [nvarchar](3000) NOT NULL,
	[Footer] [nvarchar](2000) NULL,
	[IsActive] [bit] NOT NULL DEFAULT 0,
	[StoreId] [int] NOT NULL REFERENCES Store,
	[CreatedOn] [datetime2](7) NOT NULL DEFAULT GETDATE(),
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY]

GO


