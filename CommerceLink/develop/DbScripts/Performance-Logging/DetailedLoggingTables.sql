/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.DetailedLog
	(
	Id int NOT NULL IDENTITY (1, 1),
	RequestID nvarchar(50) NULL,
	Message nvarchar(500) NULL,
	Action nvarchar(200) NULL,
	Date datetime NOT NULL,
	Time datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.DetailedLog ADD CONSTRAINT
	PK_DetailedLog PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.DetailedLog SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


------

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.ExternalTimeLog
	(
	Id int NOT NULL IDENTITY (1, 1),
	RequestID nvarchar(50) NULL,
	Message nvarchar(500) NULL,
	Action nvarchar(200) NULL,
	Date datetime NOT NULL,
	TimeTaken DECIMAL(18,4) NOT NULL

	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ExternalTimeLog ADD CONSTRAINT
	PK_ExternalTimeLog PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ExternalTimeLog SET (LOCK_ESCALATION = TABLE)
GO
COMMIT