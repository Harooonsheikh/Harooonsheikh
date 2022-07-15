USE [CLTVProd]

/****** Object:  Table [dbo].[DataDirection]    Script Date: 10/25/2018 8:05:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DataDirection](
	[DataDirectionId] [int] NOT NULL,
	[DataDirectionName] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_DataDirection] PRIMARY KEY CLUSTERED 
(
	[DataDirectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[RequestResponse]    Script Date: 10/25/2018 8:04:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RequestResponse](
	[RequestResponseId] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NOT NULL,
	[DataDirectionId] [int] NOT NULL,
	[EcomTransactionId] [nvarchar](256) NULL,
	[ApplicationName] [nvarchar](50) NOT NULL,
	[MethodName] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DataPacket] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_CLRequestResponse] PRIMARY KEY CLUSTERED 
(
	[RequestResponseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


INSERT [dbo].[DataDirection] ([DataDirectionId], [DataDirectionName], [IsActive]) VALUES (1, N'EcomRequestToCL', 1)
GO
INSERT [dbo].[DataDirection] ([DataDirectionId], [DataDirectionName], [IsActive]) VALUES (2, N'CLResponseToEcom', 1)
GO
