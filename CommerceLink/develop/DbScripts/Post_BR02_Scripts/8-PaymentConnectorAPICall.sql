/***** Object:  Table [dbo].[ActionRequest]    Script Date: 04/11/18 3:16:36 PM *****/
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
ALTER TABLE [dbo].[ActionRequest] ADD  CONSTRAINT [DF_ActionRequest_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[ActionRequest]  WITH CHECK ADD  CONSTRAINT [FK_ActionRequest_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
GO
ALTER TABLE [dbo].[ActionRequest] CHECK CONSTRAINT [FK_ActionRequest_Store]
GO