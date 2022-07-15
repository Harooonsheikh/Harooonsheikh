/****** Object:  Table [dbo].[CountryNames]    Script Date: 5/14/2019 6:50:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CountryNames](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NOT NULL,
	[CountryName] [varchar](50) NOT NULL,
	[ThreeLetterRegion] [varchar](50) NOT NULL,
	[CreatedOn] [datetime2](7) NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_CountryNames] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CountryNames] ON 
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (1, 1, N'Netherland', N'NLD', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (2, 2, N'Belgium', N'BEL', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (3, 7, N'Estonia', N'EST', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (4, 8, N'Portugal', N'PRT', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (5, 9, N'Malta', N'MLT', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (6, 10, N'Bulgaria', N'BGR', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (7, 11, N'Albania', N'ALB', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (8, 12, N'South Korea', N'KOR', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (9, 13, N'Japan', N'JPN', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (10, 14, N'Puerto Rico', N'PRI', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (11, 15, N'Costa Rica', N'CRI', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (12, 16, N'Panama', N'PAN', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (13, 18, N'Dominic Republic', N'DOM', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (14, 19, N'Uruguay', N'URY', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (15, 20, N'Ecuador', N'ECU', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (16, 21, N'Poland', N'POL', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (17, 22, N'Apple App', N'IRL', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (18, 23, N'China', N'CHN', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (19, 24, N'Austria', N'AUT', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (20, 25, N'Czech Republic', N'CZE', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (21, 26, N'Sweden', N'SWE', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (22, 27, N'Turkey', N'TUR', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (23, 28, N'Brazil', N'BRA', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (24, 29, N'Canada', N'CAN', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (25, 30, N'Mexico', N'MEX', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (26, 31, N'United States', N'USA', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (27, 85, N'Argentina', N'ARG', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (28, 86, N'Colombia', N'COL', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (29, 91, N'Spain', N'ESP', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (30, 92, N'Luxembourg', N'LUX', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (31, 93, N'Ireland', N'IRL', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (32, 94, N'Hungary', N'HUN', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (33, 95, N'Germany', N'DEU', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (34, 96, N'Chile', N'CHL', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (35, 99, N'Taiwan', N'TWN', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (36, 100, N'Thailand', N'THA', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (37, 101, N'Slovakia', N'SVK', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (38, 103, N'Croatia', N'HRV', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (39, 104, N'Italy', N'ITA', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (40, 105, N'Slovenia', N'SVN', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (41, 107, N'Latvia', N'LVA', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (42, 108, N'Cyprus', N'CYP', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (43, 109, N'France', N'FRA', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (44, 113, N'Norway', N'NOR', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (45, 114, N'Australia', N'AUS', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (46, 115, N'Finland', N'FIN', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (47, 116, N'Greece', N'GRC', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (48, 118, N'India', N'IND', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (49, 123, N'Lithuania', N'LTU', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CountryNames] ([Id], [StoreId], [CountryName], [ThreeLetterRegion], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [Description]) VALUES (50, 124, N'Russia', N'RUS', NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[CountryNames] OFF
GO
ALTER TABLE [dbo].[CountryNames]  WITH CHECK ADD  CONSTRAINT [FK_CountryNames_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
GO
ALTER TABLE [dbo].[CountryNames] CHECK CONSTRAINT [FK_CountryNames_Store]
GO
