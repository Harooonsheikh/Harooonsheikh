USE [CLTVD10]
GO
/****** Object:  Table [dbo].[Archive_RequestResponse]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Archive_RequestResponse](
	[RequestResponseId] [int] NOT NULL,
	[StoreId] [int] NOT NULL,
	[DataDirectionId] [int] NOT NULL,
	[EcomTransactionId] [nvarchar](256) NULL,
	[ApplicationName] [nvarchar](50) NOT NULL,
	[MethodName] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DataPacket] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[RequestResponseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestResponse]    Script Date: 6/11/2019 7:55:14 AM ******/
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
/****** Object:  View [dbo].[vwRequestResponse]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Aqeel Arshad Siddiqui
-- Create date: Monday, February 25, 2019
-- Description:	Select request response data from both archive and live tables
-- =============================================
CREATE   VIEW [dbo].[vwRequestResponse]
AS
	SELECT	*
	FROM	Archive_RequestResponse

	UNION ALL

	SELECT	*
	FROM	RequestResponse 
GO
/****** Object:  Table [dbo].[ActionRequest]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActionRequest](
	[ActionRequestId] [int] IDENTITY(1,1) NOT NULL,
	[ActionName] [nvarchar](100) NOT NULL,
	[StoreId] [int] NOT NULL,
	[Request] [nvarchar](4000) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_ActionRequest] PRIMARY KEY CLUSTERED 
(
	[ActionName] ASC,
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationSetting]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationSetting](
	[ApplicationSettingId] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](250) NOT NULL,
	[Value] [nvarchar](2000) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[ScreenName] [nvarchar](70) NULL,
	[SortOrder] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[StoreId] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[FieldTypeId] [int] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[IsUserForDuplicateStore] [bit] NOT NULL,
 CONSTRAINT [PK_ApplicationSetting] PRIMARY KEY CLUSTERED 
(
	[ApplicationSettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConfigurableObject]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConfigurableObject](
	[ConfigurableObjectId] [int] IDENTITY(1,1) NOT NULL,
	[ComValue] [varchar](100) NULL,
	[ErpValue] [varchar](100) NULL,
	[EntityType] [int] NOT NULL,
	[ConnectorKey] [int] NULL,
	[StoreId] [int] NULL,
	[Description] [nvarchar](1024) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_ConfigurableObjecs] PRIMARY KEY CLUSTERED 
(
	[ConfigurableObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DataDirection]    Script Date: 6/11/2019 7:55:14 AM ******/
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
/****** Object:  Table [dbo].[DeliveryMethod]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeliveryMethod](
	[DeliveryMethodId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[ItemId] [nvarchar](50) NOT NULL,
	[ErpKey] [bigint] NOT NULL,
	[StoreId] [int] NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_DeliveryMethod] PRIMARY KEY CLUSTERED 
(
	[DeliveryMethodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DimensionSet]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DimensionSet](
	[DimensionSetId] [int] IDENTITY(1,1) NOT NULL,
	[ErpValue] [varchar](50) NOT NULL,
	[ComValue] [varchar](50) NOT NULL,
	[IsActive] [bit] NULL,
	[AdditionalErpValue] [varchar](50) NULL,
	[StoreId] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_DimensionSet] PRIMARY KEY CLUSTERED 
(
	[DimensionSetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EcomType]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EcomType](
	[EcomTypeId] [int] IDENTITY(1,1) NOT NULL,
	[EcomName] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_EcomType] PRIMARY KEY CLUSTERED 
(
	[EcomTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailSubscriber]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailSubscriber](
	[EmailSubscriberId] [int] IDENTITY(1,1) NOT NULL,
	[TemplateId] [int] NOT NULL,
	[SubscriberId] [int] NOT NULL,
	[StoreId] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_EmailSubscriber] PRIMARY KEY CLUSTERED 
(
	[EmailSubscriberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailTemplate]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTemplate](
	[EmailTemplateId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Subject] [nvarchar](250) NOT NULL,
	[Body] [nvarchar](3000) NOT NULL,
	[Footer] [nvarchar](2000) NULL,
	[IsActive] [bit] NOT NULL,
	[StoreId] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED 
(
	[EmailTemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailTemplate_old]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTemplate_old](
	[EmailTemplateId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Subject] [nvarchar](250) NOT NULL,
	[Body] [nvarchar](3000) NOT NULL,
	[Footer] [nvarchar](2000) NULL,
	[IsActive] [bit] NOT NULL,
	[StoreId] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entity]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entity](
	[EntityId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[WorkFlowId] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Entity] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityFileNameParameter]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityFileNameParameter](
	[EntityFileNameParametersId] [int] NOT NULL,
	[StoreId] [int] NOT NULL,
	[EntityId] [int] NOT NULL,
	[Prefix] [nvarchar](50) NOT NULL,
	[StartingParameter] [bigint] NOT NULL,
	[Parameters] [bigint] NOT NULL,
	[Postfix] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_EntityFileNameParameter] PRIMARY KEY CLUSTERED 
(
	[EntityFileNameParametersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ERPType]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ERPType](
	[ERPTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ERPName] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ERPType] PRIMARY KEY CLUSTERED 
(
	[ERPTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FieldType]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FieldType](
	[FieldTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Description] [nvarchar](1000) NULL,
	[CreatedOn] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_FieldType] PRIMARY KEY CLUSTERED 
(
	[FieldTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FieldValue]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FieldValue](
	[FiedlValueId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationSettingId] [int] NULL,
	[FieldName] [nvarchar](1000) NULL,
	[Description] [nvarchar](1000) NULL,
	[SortOrder] [int] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_FieldValue] PRIMARY KEY CLUSTERED 
(
	[FiedlValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IntegrationKey]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IntegrationKey](
	[IntegrationKeyId] [bigint] IDENTITY(1,1) NOT NULL,
	[EntityId] [int] NOT NULL,
	[ErpKey] [varchar](150) NOT NULL,
	[ComKey] [varchar](150) NOT NULL,
	[Description] [varchar](max) NULL,
	[StoreId] [int] NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_IntegrationKey] PRIMARY KEY CLUSTERED 
(
	[IntegrationKeyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Job]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Job](
	[JobID] [bigint] NOT NULL,
	[JobName] [nvarchar](50) NULL,
	[Enabled] [bit] NULL,
	[JobTypeId] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Functions] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobLog]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobLog](
	[JobLogId] [bigint] IDENTITY(1,1) NOT NULL,
	[JobId] [bigint] NOT NULL,
	[LastExecutionTimeStamp] [datetime2](7) NULL,
	[Status] [int] NULL,
	[StoreId] [int] NULL,
	[CreatedOn] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_JobLog] PRIMARY KEY CLUSTERED 
(
	[JobLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobLogStatus]    Script Date: 6/11/2019 7:55:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobLogStatus](
	[JobLogStatus] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](1000) NULL,
 CONSTRAINT [PK_JobLogStatus] PRIMARY KEY CLUSTERED 
(
	[JobLogStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobSchedule]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobSchedule](
	[JobSchduleId] [bigint] IDENTITY(1,1) NOT NULL,
	[JobId] [bigint] NOT NULL,
	[JobInterval] [bigint] NOT NULL,
	[IsRepeatable] [bit] NOT NULL,
	[StartTime] [time](7) NULL,
	[IsActive] [bit] NOT NULL,
	[JobStatus] [int] NULL,
	[StoreId] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_JobSchedule] PRIMARY KEY CLUSTERED 
(
	[JobSchduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobType]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobType](
	[JobTypeId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_JobType] PRIMARY KEY CLUSTERED 
(
	[JobTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[EventLevel] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[MachineName] [nvarchar](100) NOT NULL,
	[EventMessage] [nvarchar](max) NOT NULL,
	[ErrorSource] [nvarchar](3000) NULL,
	[ErrorClass] [nvarchar](500) NULL,
	[ErrorMethod] [nvarchar](max) NULL,
	[ErrorMessage] [nvarchar](max) NULL,
	[InnerErrorMessage] [nvarchar](max) NULL,
	[IdentityId] [varchar](50) NULL,
	[ErrorModule] [nvarchar](50) NULL,
	[StoreId] [int] NOT NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MappingTemplate]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MappingTemplate](
	[MappingTemplateId] [int] IDENTITY(1,1) NOT NULL,
	[SourceEntity] [nvarchar](250) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[MappingTemplateTypeId] [int] NOT NULL,
	[XML] [xml] NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[StoreId] [int] NOT NULL,
	[ReadMode] [varchar](50) NULL,
 CONSTRAINT [PK_MappingTemplate] PRIMARY KEY CLUSTERED 
(
	[MappingTemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MappingTypeTemplate]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MappingTypeTemplate](
	[MappingTypeTemplateId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_MappingTypeTemplate] PRIMARY KEY CLUSTERED 
(
	[MappingTypeTemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organization]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organization](
	[OrganizationId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ShortCode] [nvarchar](50) NULL,
	[OrganizationTimeZone] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED 
(
	[OrganizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentConnector]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentConnector](
	[PaymentConnectorId] [int] NOT NULL,
	[PaymentConnectorName] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ERPCreditCardProcessorName] [nvarchar](max) NULL,
	[EComCreditCardProcessorName] [nvarchar](max) NULL,
 CONSTRAINT [PK_PaymentConnector] PRIMARY KEY CLUSTERED 
(
	[PaymentConnectorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethod]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethod](
	[PaymentMethodId] [int] IDENTITY(1,1) NOT NULL,
	[ParentPaymentMethodId] [int] NULL,
	[ECommerceValue] [varchar](100) NULL,
	[ErpValue] [varchar](100) NULL,
	[HasSubMethod] [bit] NULL,
	[ErpCode] [varchar](100) NULL,
	[IsPrepayment] [bit] NOT NULL,
	[StoreId] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[IsCreditCard] [bit] NOT NULL,
	[UsePaymentConnector] [bit] NOT NULL,
	[ServiceAccountId] [nvarchar](50) NULL,
	[PaymentConnectorId] [int] NULL,
 CONSTRAINT [PK_PaymentMethod] PRIMARY KEY CLUSTERED 
(
	[PaymentMethodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[State]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[State](
	[StateId] [int] NOT NULL,
	[Code] [nvarchar](10) NULL,
	[State] [nvarchar](50) NULL,
	[Country] [nvarchar](10) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED 
(
	[StateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Store]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Store](
	[StoreId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[EcomTypeId] [int] NOT NULL,
	[OrganizationId] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[StoreKey] [nvarchar](256) NULL,
	[Description] [nvarchar](1024) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[ERPTypeId] [int] NOT NULL,
	[DuplicateOf] [int] NULL,
	[IsSFTPDirTreeCreated] [bit] NULL,
 CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED 
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subscriber]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscriber](
	[SubscriberId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[StoreId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Subscribers] PRIMARY KEY CLUSTERED 
(
	[SubscriberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlow]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlow](
	[WorkFlowId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](70) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_WorkFlow] PRIMARY KEY CLUSTERED 
(
	[WorkFlowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlowState]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlowState](
	[WorkFlowStateID] [int] NOT NULL,
	[WorkFlowID] [int] NOT NULL,
	[Name] [nvarchar](70) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[OrderSequence] [int] NULL,
 CONSTRAINT [PK_WorkFlowState] PRIMARY KEY CLUSTERED 
(
	[WorkFlowStateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlowStatus]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlowStatus](
	[WorkFlowStatusId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceName] [varchar](100) NULL,
	[WorkFlowStateId] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[EntityId] [int] NOT NULL,
	[StoreId] [int] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_WorkFlowStatus] PRIMARY KEY CLUSTERED 
(
	[WorkFlowStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlowTransition]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlowTransition](
	[WorkFlowTransitionId] [bigint] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NOT NULL,
	[WorkFlowStateId] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[StoreId] [int] NULL,
	[CreatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_WorkFlowTransition] PRIMARY KEY CLUSTERED 
(
	[WorkFlowTransitionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ActionRequest] ADD  CONSTRAINT [DF_ActionRequest_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[ApplicationSetting] ADD  CONSTRAINT [DF_ApplicationSetting_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ApplicationSetting] ADD  CONSTRAINT [DF_ApplicationSetting_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[ApplicationSetting] ADD  DEFAULT ((0)) FOR [IsUserForDuplicateStore]
GO
ALTER TABLE [dbo].[ConfigurableObject] ADD  CONSTRAINT [DF_ConfigurableObject_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[DeliveryMethod] ADD  CONSTRAINT [DF_DeliveryMethod_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[DimensionSet] ADD  CONSTRAINT [DF_DimensionSet_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[DimensionSet] ADD  CONSTRAINT [DF_DimensionSet_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[EmailSubscriber] ADD  CONSTRAINT [DF_EmailSubscriber_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[EmailTemplate_old] ADD  CONSTRAINT [DF_EmailTemplate_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[EmailTemplate_old] ADD  CONSTRAINT [DF_EmailTemplate_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Entity] ADD  CONSTRAINT [DF_Entity_IsMerchandized]  DEFAULT ((0)) FOR [WorkFlowId]
GO
ALTER TABLE [dbo].[Entity] ADD  CONSTRAINT [DF_Entity_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Entity] ADD  CONSTRAINT [DF_Entity_ModifiedOn]  DEFAULT (getdate()) FOR [ModifiedOn]
GO
ALTER TABLE [dbo].[IntegrationKey] ADD  CONSTRAINT [DF_IntegrationKey_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[JobType] ADD  CONSTRAINT [DF_JobType_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[MappingTypeTemplate] ADD  CONSTRAINT [DF_MappingTypeTemplate_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Organization] ADD  CONSTRAINT [DF_Organization_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Organization] ADD  CONSTRAINT [DF_Organization_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PaymentMethod] ADD  CONSTRAINT [DF_PaymentMethod_IsCreditCard]  DEFAULT ((0)) FOR [IsCreditCard]
GO
ALTER TABLE [dbo].[PaymentMethod] ADD  CONSTRAINT [DF_PaymentMethod_UsePaymentConnector]  DEFAULT ((0)) FOR [UsePaymentConnector]
GO
ALTER TABLE [dbo].[State] ADD  CONSTRAINT [DF_State_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Store] ADD  CONSTRAINT [DF_Store_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Store] ADD  CONSTRAINT [DF_Store_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Store] ADD  CONSTRAINT [DF_Store_IsSFTPDirTreeCreated]  DEFAULT ((0)) FOR [IsSFTPDirTreeCreated]
GO
ALTER TABLE [dbo].[Subscriber] ADD  CONSTRAINT [DF_Subscriber_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[WorkFlow] ADD  CONSTRAINT [DF_WorkFlow_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[WorkFlowStatus] ADD  CONSTRAINT [DF_WorkFlowStatus_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[WorkFlowTransition] ADD  CONSTRAINT [DF_WorkFlowTransition_Created]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[ActionRequest]  WITH CHECK ADD  CONSTRAINT [FK_ActionRequest_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
GO
ALTER TABLE [dbo].[ActionRequest] CHECK CONSTRAINT [FK_ActionRequest_Store]
GO
ALTER TABLE [dbo].[ApplicationSetting]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationSetting_Store] FOREIGN KEY([FieldTypeId])
REFERENCES [dbo].[FieldType] ([FieldTypeId])
GO
ALTER TABLE [dbo].[ApplicationSetting] CHECK CONSTRAINT [FK_ApplicationSetting_Store]
GO
ALTER TABLE [dbo].[ConfigurableObject]  WITH CHECK ADD  CONSTRAINT [FK_ConfigurableObject_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
GO
ALTER TABLE [dbo].[ConfigurableObject] CHECK CONSTRAINT [FK_ConfigurableObject_Store]
GO
ALTER TABLE [dbo].[DeliveryMethod]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryMethod_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
GO
ALTER TABLE [dbo].[DeliveryMethod] CHECK CONSTRAINT [FK_DeliveryMethod_Store]
GO
ALTER TABLE [dbo].[DimensionSet]  WITH CHECK ADD  CONSTRAINT [FK_DimensionSet_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
GO
ALTER TABLE [dbo].[DimensionSet] CHECK CONSTRAINT [FK_DimensionSet_Store]
GO
ALTER TABLE [dbo].[EmailSubscriber]  WITH CHECK ADD  CONSTRAINT [FK_EmailSubscriber_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
GO
ALTER TABLE [dbo].[EmailSubscriber] CHECK CONSTRAINT [FK_EmailSubscriber_Store]
GO
ALTER TABLE [dbo].[EmailTemplate_old]  WITH CHECK ADD  CONSTRAINT [FK_EmailTemplate_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
GO
ALTER TABLE [dbo].[EmailTemplate_old] CHECK CONSTRAINT [FK_EmailTemplate_Store]
GO
ALTER TABLE [dbo].[EntityFileNameParameter]  WITH CHECK ADD  CONSTRAINT [FK_EntityFileNameParameter_EntityFileNameParameter] FOREIGN KEY([EntityFileNameParametersId])
REFERENCES [dbo].[EntityFileNameParameter] ([EntityFileNameParametersId])
GO
ALTER TABLE [dbo].[EntityFileNameParameter] CHECK CONSTRAINT [FK_EntityFileNameParameter_EntityFileNameParameter]
GO
ALTER TABLE [dbo].[FieldValue]  WITH CHECK ADD  CONSTRAINT [FK_FieldValue_FieldValue] FOREIGN KEY([ApplicationSettingId])
REFERENCES [dbo].[ApplicationSetting] ([ApplicationSettingId])
GO
ALTER TABLE [dbo].[FieldValue] CHECK CONSTRAINT [FK_FieldValue_FieldValue]
GO
ALTER TABLE [dbo].[IntegrationKey]  WITH CHECK ADD  CONSTRAINT [FK_IntegrationKey_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
GO
ALTER TABLE [dbo].[IntegrationKey] CHECK CONSTRAINT [FK_IntegrationKey_Store]
GO
ALTER TABLE [dbo].[JobLog]  WITH CHECK ADD  CONSTRAINT [FK_JobLog_Job] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([JobID])
GO
ALTER TABLE [dbo].[JobLog] CHECK CONSTRAINT [FK_JobLog_Job]
GO
ALTER TABLE [dbo].[JobLog]  WITH CHECK ADD  CONSTRAINT [FK_JobLog_JobLogStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[JobLogStatus] ([JobLogStatus])
GO
ALTER TABLE [dbo].[JobLog] CHECK CONSTRAINT [FK_JobLog_JobLogStatus]
GO
ALTER TABLE [dbo].[JobSchedule]  WITH CHECK ADD  CONSTRAINT [FK_JobSchedule_Store] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([JobID])
GO
ALTER TABLE [dbo].[JobSchedule] CHECK CONSTRAINT [FK_JobSchedule_Store]
GO
ALTER TABLE [dbo].[MappingTemplate]  WITH CHECK ADD  CONSTRAINT [FK_MappingTemplate_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
GO
ALTER TABLE [dbo].[MappingTemplate] CHECK CONSTRAINT [FK_MappingTemplate_Store]
GO
ALTER TABLE [dbo].[PaymentMethod]  WITH CHECK ADD  CONSTRAINT [FK_PaymentMethod_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
GO
ALTER TABLE [dbo].[PaymentMethod] CHECK CONSTRAINT [FK_PaymentMethod_Store]
GO
ALTER TABLE [dbo].[Store]  WITH CHECK ADD  CONSTRAINT [FK_Store_EcomType] FOREIGN KEY([EcomTypeId])
REFERENCES [dbo].[EcomType] ([EcomTypeId])
GO
ALTER TABLE [dbo].[Store] CHECK CONSTRAINT [FK_Store_EcomType]
GO
ALTER TABLE [dbo].[Store]  WITH CHECK ADD  CONSTRAINT [FK_Store_Type] FOREIGN KEY([ERPTypeId])
REFERENCES [dbo].[ERPType] ([ERPTypeId])
GO
ALTER TABLE [dbo].[Store] CHECK CONSTRAINT [FK_Store_Type]
GO
ALTER TABLE [dbo].[Subscriber]  WITH CHECK ADD  CONSTRAINT [FK_Subscriber_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
GO
ALTER TABLE [dbo].[Subscriber] CHECK CONSTRAINT [FK_Subscriber_Store]
GO
ALTER TABLE [dbo].[WorkFlowState]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowState_WorkFlow] FOREIGN KEY([WorkFlowID])
REFERENCES [dbo].[WorkFlow] ([WorkFlowId])
GO
ALTER TABLE [dbo].[WorkFlowState] CHECK CONSTRAINT [FK_WorkFlowState_WorkFlow]
GO
ALTER TABLE [dbo].[WorkFlowStatus]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowStatus_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
GO
ALTER TABLE [dbo].[WorkFlowStatus] CHECK CONSTRAINT [FK_WorkFlowStatus_Store]
GO
ALTER TABLE [dbo].[WorkFlowStatus]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowStatus_WorkFlowState] FOREIGN KEY([WorkFlowStateId])
REFERENCES [dbo].[WorkFlowState] ([WorkFlowStateID])
GO
ALTER TABLE [dbo].[WorkFlowStatus] CHECK CONSTRAINT [FK_WorkFlowStatus_WorkFlowState]
GO
ALTER TABLE [dbo].[WorkFlowTransition]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowTransition_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
GO
ALTER TABLE [dbo].[WorkFlowTransition] CHECK CONSTRAINT [FK_WorkFlowTransition_Store]
GO
ALTER TABLE [dbo].[WorkFlowTransition]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowTransition_WorkFlowState] FOREIGN KEY([WorkFlowStateId])
REFERENCES [dbo].[WorkFlowState] ([WorkFlowStateID])
GO
ALTER TABLE [dbo].[WorkFlowTransition] CHECK CONSTRAINT [FK_WorkFlowTransition_WorkFlowState]
GO
/****** Object:  StoredProcedure [dbo].[spArchiveRequestResponse]    Script Date: 6/11/2019 7:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Aqeel Arshad Siddiqui
-- Create date: 20 Feb 2019
-- Description:	Move old data to archive table for Request Response
-- NOTE:	A maintenance plan will be made against this stored procedure so that thisstored procedure is executed at pre-scheduled time.
--			Steps to create a maintenance plan is as follow:
--			1.	Connect the target SQL Server using SQL Server Management Studio
--			2.	In Object Explorer navigate to “Management” > “Maintenance Plans”
--			3.	Right click the “Maintenance Plans”, system will open context menu, click on “New Maintenance Plan…”
--			4.	System will prompt for maintenance plan’s name in a new window with title “New Maintenance Plan”
--			5.	Provide the name “CLTVENV_MaintenancePlan_ArchiveRequestResponse” for maintenance plan. Replace the environment name with EVN in the name for respective environment. For production the name could be “CLTVPRD_MaintenancePlan_ArchiveRequestResponse”
--			6.	System will display the Maintenance Plan design area
--			7.	From “Toolbox”, drag-drop task with title “Execute T-SQL Statement Task” in the design area
--			8.	System will display add a new task in design area
--			9.	Double click the newly added task
--			10.	System will display new window with title “Execute T-SQL Statement Task”
--			11.	Select target connection from the list or add a new target connection if it does not exists in the list
--			12.	By default the “Execution time out” will be 0
--			13.	In “T-SQL statement” text box type the EXEC command with fully qualified name of the stored procedure. Database name followed by schema followed by name of the stored procedure, “EXEC [CLTVINT].[dbo].[spArchiveRequestResponse]”. Replace the environment name with EVN in the name for respective environment. For production the name could be “EXEC [CLTVPRD].[dbo].[spArchiveRequestResponse]”
--			14.	Click the “Calender” icon in the “schedule column”
--			15.	System will display a new window “New Job Schedule” to enter the schedule
--			16.	Select the appropriate/desired schedule
--			17.	Press the OK button the save the schedule for maintenance plan
--			18.	Save the maintenance plan. Maintenance plan is created and now it will be visible in “Maintenance Plans” and in Jobs as well.
-- =============================================
CREATE   PROCEDURE [dbo].[spArchiveRequestResponse]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	BEGIN TRAN

	BEGIN TRY

		DECLARE @NumberOfDays AS INT
		SET @NumberOfDays = 90;

		DECLARE @ThresholdDate AS DATETIME;
		IF @NumberOfDays > 0
		BEGIN
			SET @NumberOfDays = @NumberOfDays * -1;
		END
		SET @ThresholdDate = CAST(DATEADD(d, @NumberOfDays, GETDATE()) AS DATE);

		INSERT INTO	Archive_RequestResponse
		SELECT		*
		FROM		RequestResponse
		WHERE		CreatedOn < @ThresholdDate;

		DELETE FROM	RequestResponse
		WHERE		CreatedOn < @ThresholdDate;

		PRINT('Archiving sucessfull');
		COMMIT TRAN

	END TRY

	BEGIN CATCH

		PRINT('Archiving failed with reason: ' + ERROR_MESSAGE());
		ROLLBACK TRAN

	END CATCH

END
GO
