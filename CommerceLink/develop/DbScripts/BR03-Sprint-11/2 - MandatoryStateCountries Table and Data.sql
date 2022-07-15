/****** Object:  Table [dbo].[MandatoryStateCountry]    Script Date: 03/07/2020 4:07:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.MandatoryStateCountry', 'U') IS  NULL 

CREATE TABLE [dbo].[MandatoryStateCountry](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ThreeLetterISORegionName] [nvarchar](10) NOT NULL,
	[ShortName] [nvarchar](60) NOT NULL,
	[FullName] [nvarchar](200) NULL,
 CONSTRAINT [PK_MandatoryStateCountry] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


--Data Insertiion

IF NOT EXISTS (SELECT * FROM MandatoryStateCountry WHERE ThreeLetterISORegionName = 'USA') BEGIN INSERT INTO [dbo].[MandatoryStateCountry] ([ThreeLetterISORegionName] , [ShortName] ,[FullName]) VALUES ( 'USA','United States','United States of America' ); END
IF NOT EXISTS (SELECT * FROM MandatoryStateCountry WHERE ThreeLetterISORegionName = 'CAN') BEGIN INSERT INTO [dbo].[MandatoryStateCountry] ([ThreeLetterISORegionName] , [ShortName] ,[FullName]) VALUES ( 'CAN','Canada','Canada' ); END
IF NOT EXISTS (SELECT * FROM MandatoryStateCountry WHERE ThreeLetterISORegionName = 'MEX') BEGIN INSERT INTO [dbo].[MandatoryStateCountry] ([ThreeLetterISORegionName] , [ShortName] ,[FullName]) VALUES ( 'MEX','Mexico','United Mexican States' ); END
IF NOT EXISTS (SELECT * FROM MandatoryStateCountry WHERE ThreeLetterISORegionName = 'BRA') BEGIN INSERT INTO [dbo].[MandatoryStateCountry] ([ThreeLetterISORegionName] , [ShortName] ,[FullName]) VALUES ( 'BRA','Brazil','Federative Republic of Brazil' ); END
