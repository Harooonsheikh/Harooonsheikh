CREATE TABLE [dbo].[AosUrlSetting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MethodName] [nvarchar](200) NOT NULL,
	[MethodUrl] [nvarchar](500) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_AosUrlSetting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AosUrlSetting] ADD  CONSTRAINT [DF_AosUrlSetting_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO


