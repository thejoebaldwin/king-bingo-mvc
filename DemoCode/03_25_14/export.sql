
if OBJECT_ID('KingBingo') is not null
drop database KingBingo;
go
create database KingBingo;
go

USE [KingBingo]

if not exists(select * from sys.database_principals where name = 'db_user')
BEGIN
	CREATE LOGIN db_user WITH PASSWORD = 'test!234'

	IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'db_user')
	BEGIN
		CREATE USER [db_user] FOR LOGIN [db_user]
		EXEC sp_addrolemember N'db_owner', N'db_user'
	END;
END;
GO





GO
/****** Object:  Table [dbo].[Game]    Script Date: 3/25/2014 8:55:03 AM ******/
DROP TABLE [dbo].[Game]
GO
USE [master]
GO
/****** Object:  Database [KingBingo]    Script Date: 3/25/2014 8:55:03 AM ******/
DROP DATABASE [KingBingo]
GO
/****** Object:  Database [KingBingo]    Script Date: 3/25/2014 8:55:03 AM ******/
CREATE DATABASE [KingBingo]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'KingBingo', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\KingBingo.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'KingBingo_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\KingBingo_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [KingBingo] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [KingBingo].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [KingBingo] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [KingBingo] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [KingBingo] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [KingBingo] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [KingBingo] SET ARITHABORT OFF 
GO
ALTER DATABASE [KingBingo] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [KingBingo] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [KingBingo] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [KingBingo] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [KingBingo] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [KingBingo] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [KingBingo] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [KingBingo] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [KingBingo] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [KingBingo] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [KingBingo] SET  DISABLE_BROKER 
GO
ALTER DATABASE [KingBingo] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [KingBingo] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [KingBingo] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [KingBingo] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [KingBingo] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [KingBingo] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [KingBingo] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [KingBingo] SET RECOVERY FULL 
GO
ALTER DATABASE [KingBingo] SET  MULTI_USER 
GO
ALTER DATABASE [KingBingo] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [KingBingo] SET DB_CHAINING OFF 
GO
ALTER DATABASE [KingBingo] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [KingBingo] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'KingBingo', N'ON'
GO
USE [KingBingo]
GO
/****** Object:  Table [dbo].[Game]    Script Date: 3/25/2014 8:55:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Game](
	[GameID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NULL,
	[Description] [text] NULL,
	[WinCount] [int] NULL,
	[WinLimit] [int] NULL,
	[UserCount] [int] NULL,
	[UserLimit] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Game] ON 

INSERT [dbo].[Game] ([GameID], [Name], [Description], [WinCount], [WinLimit], [UserCount], [UserLimit]) VALUES (1, N'Test Game 1', N'This is a great game', 0, 3, 0, 3)
INSERT [dbo].[Game] ([GameID], [Name], [Description], [WinCount], [WinLimit], [UserCount], [UserLimit]) VALUES (2, N'Test Game 2', N'Game 2 Description', 0, 5, 0, 6)
SET IDENTITY_INSERT [dbo].[Game] OFF
USE [master]
GO
ALTER DATABASE [KingBingo] SET  READ_WRITE 
GO
