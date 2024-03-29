USE [master]
GO
/****** Object:  Database [MyPass]    Script Date: 14-Mar-19 12:13:13 AM ******/
/*********************************************************************************************************************
https://docs.microsoft.com/en-us/sql/t-sql/functions/serverproperty-transact-sql?view=sql-server-2017

///////////////////////////////////////////////////////
SELECT  
  SERVERPROPERTY('MachineName') AS ComputerName,
  SERVERPROPERTY('ServerName') AS InstanceName,  
  SERVERPROPERTY('Edition') AS Edition,
  SERVERPROPERTY('ProductVersion') AS ProductVersion,  
  SERVERPROPERTY('ProductLevel') AS ProductLevel;  
GO
//////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
select 
    InstanceDefaultDataPath = serverproperty('InstanceDefaultDataPath'),
    InstanceDefaultLogPath = serverproperty('InstanceDefaultLogPath')

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
NAME = N'MyPass', FILENAME = InstanceDefaultDataPath + \MyPass.mdf'   gibi sytax a dikkat  edilerek yazılmalı ( @ 'xxx' )

/*************************** CREATE DATABASE bu  özelliklere bakılarak çalıştırılmalı  *********************************************/

************************************************************************************************************************/

CREATE DATABASE [MyPass]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MyPass', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\MyPass.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MyPass_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\MyPass_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [MyPass] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MyPass].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MyPass] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MyPass] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MyPass] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MyPass] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MyPass] SET ARITHABORT OFF 
GO
ALTER DATABASE [MyPass] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MyPass] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MyPass] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MyPass] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MyPass] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MyPass] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MyPass] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MyPass] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MyPass] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MyPass] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MyPass] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MyPass] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MyPass] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MyPass] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MyPass] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MyPass] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MyPass] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MyPass] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MyPass] SET  MULTI_USER 
GO
ALTER DATABASE [MyPass] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MyPass] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MyPass] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MyPass] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MyPass] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MyPass] SET QUERY_STORE = OFF
GO
USE [MyPass]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 14-Mar-19 12:13:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](50) NOT NULL,
	[Description] [varchar](500) NULL,
	[OwnerUserId] [int] NOT NULL,
	[Status] [bit] NOT NULL,
	[AddedDate] [datetime] NOT NULL,  /**** Eklenme tarihini system vermeli bunun için --- getDate()  --- kullanılmalı *****/
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupUsers]    Script Date: 14-Mar-19 12:13:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
 CONSTRAINT [PK_GroupUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Items]    Script Date: 14-Mar-19 12:13:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Items](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](50) NOT NULL,
	[Description] [varchar](500) NULL,
	[Password] [varchar](50) NOT NULL,
	[GroupId] [int] NOT NULL,
	[ItemTypeId] [int] NOT NULL,
	[AddedDate] [datetime] NOT NULL,  /**** Eklenme tarihini system vermeli bunun için --- getDate()  --- kullanılmalı *****/
	[UpdatedDate] [datetime] NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 14-Mar-19 12:13:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Surname] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](400) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[AddedDate] [datetime] NOT NULL,  /**** Eklenme tarihini system vermeli bunun için --- getDate()  --- kullanılmalı *****/
	[UpdatedDate] [datetime] NULL,
	[Status] [bit] NOT NULL,
	[PasswordResetExpiryDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_OwnerUserId]  DEFAULT ((0)) FOR [OwnerUserId]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_PasswordResetRequest]  DEFAULT ((0)) FOR [PasswordResetExpiryDate]
GO
USE [master]
GO
ALTER DATABASE [MyPass] SET  READ_WRITE 
GO
