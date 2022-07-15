 -- Change DataType of Status column from bit to INT
 ALTER TABLE JobLog ALTER COLUMN Status INT
 
 
 -- Create a new table JobLogStatus
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
INSERT [dbo].[JobLogStatus] ([JobLogStatus], [Name], [Description]) VALUES (0, N'Error', N'Error in Executing Job')
INSERT [dbo].[JobLogStatus] ([JobLogStatus], [Name], [Description]) VALUES (1, N'Started', N'Job started')
INSERT [dbo].[JobLogStatus] ([JobLogStatus], [Name], [Description]) VALUES (2, N'Completed', N'Job Completed')


ALTER TABLE [dbo].[JobLog]  WITH CHECK ADD  CONSTRAINT [FK_JobLog_JobLogStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[JobLogStatus] ([JobLogStatus])
GO
ALTER TABLE [dbo].[JobLog] CHECK CONSTRAINT [FK_JobLog_JobLogStatus]
GO