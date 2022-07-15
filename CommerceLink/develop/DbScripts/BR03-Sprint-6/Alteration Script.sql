--Alteration for RequestResponse 
IF EXISTS(SELECT	*
				FROM	INFORMATION_SCHEMA.TABLES
				WHERE	TABLE_SCHEMA = N'dbo'
				AND		TABLE_NAME = N'RequestResponse')
BEGIN
IF COL_LENGTH('dbo.RequestResponse','DataDirectionId') IS NOT NULL
BEGIN
ALTER TABLE dbo.RequestResponse ALTER COLUMN [DataDirectionId] [int] NULL
PRINT 'DataDirectionId column updated successfully.'
END
ELSE
BEGIN
PRINT 'DataDirectionId column does not exists'
END

IF COL_LENGTH('dbo.RequestResponse','RequestInitiatedIP') IS NULL
BEGIN
ALTER TABLE dbo.RequestResponse ADD [RequestInitiatedIP] [nvarchar](50) NULL
PRINT 'RequestInitiatedIP column added successfully.'
END
ELSE
BEGIN
PRINT 'RequestInitiatedIP column already exists'
END

IF COL_LENGTH('dbo.RequestResponse','OutputPacket') IS NULL
BEGIN
ALTER TABLE dbo.RequestResponse ADD [OutputPacket] [nvarchar](max) NULL
PRINT 'OutputPacket column added successfully.'
END
ELSE
BEGIN
PRINT 'OutputPacket column already exists'
END

IF COL_LENGTH('dbo.RequestResponse','OutputSentAt') IS NULL
BEGIN
ALTER TABLE dbo.RequestResponse ADD [OutputSentAt] [datetime] NULL
PRINT 'OutputSentAt column added successfully.'
END
ELSE
BEGIN
PRINT 'OutputSentAt column already exists'
END

IF COL_LENGTH('dbo.RequestResponse','IdentifierKey') IS NULL
BEGIN
ALTER TABLE dbo.RequestResponse ADD [IdentifierKey] [nvarchar](max) NULL
PRINT 'IdentifierKey column added successfully.'
END
ELSE
BEGIN
PRINT 'IdentifierKey column already exists'
END

IF COL_LENGTH('dbo.RequestResponse','IdentifierValue') IS NULL
BEGIN
ALTER TABLE dbo.RequestResponse ADD [IdentifierValue] [nvarchar](max) NULL
PRINT 'IdentifierValue column added successfully.'
END
ELSE
BEGIN
PRINT 'IdentifierValue column already exists'
END

IF COL_LENGTH('dbo.RequestResponse','IsSuccess') IS NULL
BEGIN
ALTER TABLE dbo.RequestResponse ADD [IsSuccess] [bit] NULL
PRINT 'IsSuccess column added successfully.'
END
ELSE
BEGIN
PRINT 'IsSuccess column already exists'
END

IF COL_LENGTH('dbo.RequestResponse','TotalProcessingDuration') IS NULL
BEGIN
ALTER TABLE dbo.RequestResponse ADD [TotalProcessingDuration] [nvarchar](max) NULL
PRINT 'TotalProcessingDuration column added successfully.'
END
ELSE
BEGIN
PRINT 'TotalProcessingDuration column already exists'
END
END


--Alteration for dbo.Archive_RequestResponse 
IF EXISTS(SELECT	*
				FROM	INFORMATION_SCHEMA.TABLES
				WHERE	TABLE_SCHEMA = N'dbo'
				AND		TABLE_NAME = N'Archive_RequestResponse')
BEGIN
IF COL_LENGTH('dbo.Archive_RequestResponse','DataDirectionId') IS NOT NULL
BEGIN
ALTER TABLE dbo.Archive_RequestResponse ALTER COLUMN [DataDirectionId] [int] NULL
PRINT 'DataDirectionId column updated successfully.'
END
ELSE
BEGIN
PRINT 'DataDirectionId column does not exists'
END

IF COL_LENGTH('dbo.Archive_RequestResponse','RequestInitiatedIP') IS NULL
BEGIN
ALTER TABLE dbo.Archive_RequestResponse ADD [RequestInitiatedIP] [nvarchar](50) NULL
PRINT 'RequestInitiatedIP column added successfully.'
END
ELSE
BEGIN
PRINT 'RequestInitiatedIP column already exists'
END

IF COL_LENGTH('dbo.Archive_RequestResponse','OutputPacket') IS NULL
BEGIN
ALTER TABLE dbo.Archive_RequestResponse ADD [OutputPacket] [nvarchar](max) NULL
PRINT 'OutputPacket column added successfully.'
END
ELSE
BEGIN
PRINT 'OutputPacket column already exists'
END

IF COL_LENGTH('dbo.Archive_RequestResponse','OutputSentAt') IS NULL
BEGIN
ALTER TABLE dbo.Archive_RequestResponse ADD [OutputSentAt] [datetime] NULL
PRINT 'OutputSentAt column added successfully.'
END
ELSE
BEGIN
PRINT 'OutputSentAt column already exists'
END

IF COL_LENGTH('dbo.Archive_RequestResponse','IdentifierKey') IS NULL
BEGIN
ALTER TABLE dbo.Archive_RequestResponse ADD [IdentifierKey] [nvarchar](max) NULL
PRINT 'IdentifierKey column added successfully.'
END
ELSE
BEGIN
PRINT 'IdentifierKey column already exists'
END

IF COL_LENGTH('dbo.Archive_RequestResponse','IdentifierValue') IS NULL
BEGIN
ALTER TABLE dbo.Archive_RequestResponse ADD [IdentifierValue] [nvarchar](max) NULL
PRINT 'IdentifierValue column added successfully.'
END
ELSE
BEGIN
PRINT 'IdentifierValue column already exists'
END

IF COL_LENGTH('dbo.Archive_RequestResponse','IsSuccess') IS NULL
BEGIN
ALTER TABLE dbo.Archive_RequestResponse ADD [IsSuccess] [bit] NULL
PRINT 'IsSuccess column added successfully.'
END
ELSE
BEGIN
PRINT 'IsSuccess column already exists'
END

IF COL_LENGTH('dbo.Archive_RequestResponse','TotalProcessingDuration') IS NULL
BEGIN
ALTER TABLE dbo.Archive_RequestResponse ADD [TotalProcessingDuration] [nvarchar](max) NULL
PRINT 'TotalProcessingDuration column added successfully.'
END
ELSE
BEGIN
PRINT 'TotalProcessingDuration column already exists'
END
END


