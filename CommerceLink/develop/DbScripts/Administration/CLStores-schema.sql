Go
create database [CLStores]
go
ALTER DATABASE [CLStores] SET COMPATIBILITY_LEVEL = 120
GO

GO
ALTER DATABASE [CLStores] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CLStores] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CLStores] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CLStores] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CLStores] SET ARITHABORT OFF 
GO
ALTER DATABASE [CLStores] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CLStores] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CLStores] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CLStores] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CLStores] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CLStores] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CLStores] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CLStores] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CLStores] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CLStores] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CLStores] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CLStores] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CLStores] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CLStores] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CLStores] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CLStores] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CLStores] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CLStores] SET RECOVERY FULL 
GO
ALTER DATABASE [CLStores] SET  MULTI_USER 
GO
ALTER DATABASE [CLStores] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CLStores] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CLStores] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CLStores] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [CLStores] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'CLStores', N'ON'
GO
USE [CLStores]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 4/12/2018 2:15:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ActionParams]    Script Date: 4/12/2018 2:15:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActionParams](
	[ParamId] [int] IDENTITY(1,1) NOT NULL,
	[ActionId] [int] NOT NULL,
	[Key] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.ActionParams] PRIMARY KEY CLUSTERED 
(
	[ParamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApplicationStores]    Script Date: 4/12/2018 2:15:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationStores](
	[StoreId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[MongoConnection] [nvarchar](max) NULL,
	[MongoDbName] [nvarchar](max) NULL,
	[APIURL] [nvarchar](max) NULL,
	[APIKey] [nvarchar](max) NULL,
	[DBName] [nvarchar](max) NULL,
	[DBServer] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[ModifiedAt] [datetime] NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationStores] PRIMARY KEY CLUSTERED 
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 4/12/2018 2:15:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 4/12/2018 2:15:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 4/12/2018 2:15:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 4/12/2018 2:15:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 4/12/2018 2:15:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[StoreId] [int] NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreActions]    Script Date: 4/12/2018 2:15:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreActions](
	[ActionId] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NOT NULL,
	[ActionName] [nvarchar](max) NULL,
	[ActionRoute] [nvarchar](max) NULL,
	[RequestType] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.StoreActions] PRIMARY KEY CLUSTERED 
(
	[ActionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[ActionParams]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ActionParams_dbo.StoreActions_ActionId] FOREIGN KEY([ActionId])
REFERENCES [dbo].[StoreActions] ([ActionId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActionParams] CHECK CONSTRAINT [FK_dbo.ActionParams_dbo.StoreActions_ActionId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUsers_dbo.ApplicationStores_StoreId] FOREIGN KEY([StoreId])
REFERENCES [dbo].[ApplicationStores] ([StoreId])
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_dbo.AspNetUsers_dbo.ApplicationStores_StoreId]
GO
ALTER TABLE [dbo].[StoreActions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.StoreActions_dbo.ApplicationStores_StoreId] FOREIGN KEY([StoreId])
REFERENCES [dbo].[ApplicationStores] ([StoreId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreActions] CHECK CONSTRAINT [FK_dbo.StoreActions_dbo.ApplicationStores_StoreId]
GO
USE [master]
GO
ALTER DATABASE [CLStores] SET  READ_WRITE 
GO
