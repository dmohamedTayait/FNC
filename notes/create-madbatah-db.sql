USE [master]
GO
ALTER DATABASE [EMadbatah2] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
GO

/****** Object:  Database [EMadbatah2]    Script Date: 10/16/2011 10:35:36 ******/
CREATE DATABASE [EMadbatah2] ON  PRIMARY 
( NAME = N'EMadbatah', FILENAME = N'C:\Databases\EMadbatah\EMadbatah2.mdf' , SIZE = 15360KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'EMadbatah_log', FILENAME = N'C:\Databases\EMadbatah\EMadbatah2_log.ldf' , SIZE = 57664KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 COLLATE Arabic_CI_AI
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'EMadbatah2', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EMadbatah2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EMadbatah2] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [EMadbatah2] SET ANSI_NULLS OFF
GO
ALTER DATABASE [EMadbatah2] SET ANSI_PADDING OFF
GO
ALTER DATABASE [EMadbatah2] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [EMadbatah2] SET ARITHABORT OFF
GO
ALTER DATABASE [EMadbatah2] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [EMadbatah2] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [EMadbatah2] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [EMadbatah2] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [EMadbatah2] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [EMadbatah2] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [EMadbatah2] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [EMadbatah2] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [EMadbatah2] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [EMadbatah2] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [EMadbatah2] SET  DISABLE_BROKER
GO
ALTER DATABASE [EMadbatah2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [EMadbatah2] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [EMadbatah2] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [EMadbatah2] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [EMadbatah2] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [EMadbatah2] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [EMadbatah2] SET  READ_WRITE
GO
ALTER DATABASE [EMadbatah2] SET RECOVERY FULL
GO
ALTER DATABASE [EMadbatah2] SET  MULTI_USER
GO
ALTER DATABASE [EMadbatah2] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [EMadbatah2] SET DB_CHAINING OFF
GO
USE [EMadbatah2]
GO
ALTER TABLE [dbo].[SessionContentItem] DROP CONSTRAINT [DF_SessionContent_Approved]
GO
ALTER TABLE [dbo].[SessionContentItem] DROP CONSTRAINT [DF_SessionContent_UpdatedByReviewer]
GO
ALTER TABLE [dbo].[SessionContentItem] DROP CONSTRAINT [DF_SessionContentItem_MergedWithPrevious]
GO
ALTER TABLE [dbo].[SessionFile] DROP CONSTRAINT [DF_SessionFile_LastInsertedFragNum]
GO
ALTER TABLE [dbo].[SessionFile] DROP CONSTRAINT [DF_SessionFile_IsSessionStart]
GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [DF_User_Status]
GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [DF_User_Deleted]
GO
ALTER TABLE [dbo].[AgendaItem] DROP CONSTRAINT [DF_AgendaItem_IsCustom]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_AgendaItem]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[SessionContentItem] DROP CONSTRAINT [FK_SessionContentItem_AgendaItem]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_AgendaSubItem]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[SessionContentItem] DROP CONSTRAINT [FK_SessionContentItem_AgendaSubItem]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_Attendant]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[SessionContentItem] DROP CONSTRAINT [FK_SessionContentItem_Attendant]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_Reviewer]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[SessionContentItem] DROP CONSTRAINT [FK_SessionContentItem_Reviewer]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_Session]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[SessionContentItem] DROP CONSTRAINT [FK_SessionContentItem_Session]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_SessionContentItemStatus]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[SessionContentItem] DROP CONSTRAINT [FK_SessionContentItem_SessionContentItemStatus]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_SessionFile]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[SessionContentItem] DROP CONSTRAINT [FK_SessionContentItem_SessionFile]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_User]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[SessionContentItem] DROP CONSTRAINT [FK_SessionContentItem_User]
GO
/****** Object:  ForeignKey [FK_Session_MadbatahFilesStatus]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[Session] DROP CONSTRAINT [FK_Session_MadbatahFilesStatus]
GO
/****** Object:  ForeignKey [FK_Session_SessionStatus]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[Session] DROP CONSTRAINT [FK_Session_SessionStatus]
GO
/****** Object:  ForeignKey [FK_SessionFile_Reviewer]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[SessionFile] DROP CONSTRAINT [FK_SessionFile_Reviewer]
GO
/****** Object:  ForeignKey [FK_SessionFile_Session]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[SessionFile] DROP CONSTRAINT [FK_SessionFile_Session]
GO
/****** Object:  ForeignKey [FK_SessionFile_SessionFileStatus]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[SessionFile] DROP CONSTRAINT [FK_SessionFile_SessionFileStatus]
GO
/****** Object:  ForeignKey [FK_SessionFile_User]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[SessionFile] DROP CONSTRAINT [FK_SessionFile_User]
GO
/****** Object:  ForeignKey [FK_User_Role]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_Role]
GO
/****** Object:  ForeignKey [FK_Attendant_AttendantState]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[Attendant] DROP CONSTRAINT [FK_Attendant_AttendantState]
GO
/****** Object:  ForeignKey [FK_Attendant_AttendantType]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[Attendant] DROP CONSTRAINT [FK_Attendant_AttendantType]
GO
/****** Object:  ForeignKey [FK_Attachement_Session]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[Attachement] DROP CONSTRAINT [FK_Attachement_Session]
GO
/****** Object:  ForeignKey [FK_AgendaItem_Session]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[AgendaItem] DROP CONSTRAINT [FK_AgendaItem_Session]
GO
/****** Object:  ForeignKey [FK_SessionAttendant_Attendant]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[SessionAttendant] DROP CONSTRAINT [FK_SessionAttendant_Attendant]
GO
/****** Object:  ForeignKey [FK_SessionAttendant_Session]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[SessionAttendant] DROP CONSTRAINT [FK_SessionAttendant_Session]
GO
/****** Object:  ForeignKey [FK_AgendaSubItem_AgendaItem]    Script Date: 10/16/2011 10:35:38 ******/
ALTER TABLE [dbo].[AgendaSubItem] DROP CONSTRAINT [FK_AgendaSubItem_AgendaItem]
GO
/****** Object:  StoredProcedure [dbo].[DeleteSessionData]    Script Date: 10/16/2011 10:35:38 ******/
DROP PROCEDURE [dbo].[DeleteSessionData]
GO
/****** Object:  StoredProcedure [dbo].[DeleteData]    Script Date: 10/16/2011 10:35:38 ******/
DROP PROCEDURE [dbo].[DeleteData]
GO
/****** Object:  Table [dbo].[AgendaSubItem]    Script Date: 10/16/2011 10:35:38 ******/
DROP TABLE [dbo].[AgendaSubItem]
GO
/****** Object:  Table [dbo].[SessionAttendant]    Script Date: 10/16/2011 10:35:38 ******/
DROP TABLE [dbo].[SessionAttendant]
GO
/****** Object:  Table [dbo].[AgendaItem]    Script Date: 10/16/2011 10:35:38 ******/
DROP TABLE [dbo].[AgendaItem]
GO
/****** Object:  Table [dbo].[Attachement]    Script Date: 10/16/2011 10:35:38 ******/
DROP TABLE [dbo].[Attachement]
GO
/****** Object:  Table [dbo].[Attendant]    Script Date: 10/16/2011 10:35:38 ******/
DROP TABLE [dbo].[Attendant]
GO
/****** Object:  Table [dbo].[User]    Script Date: 10/16/2011 10:35:38 ******/
DROP TABLE [dbo].[User]
GO
/****** Object:  Table [dbo].[SessionFile]    Script Date: 10/16/2011 10:35:38 ******/
DROP TABLE [dbo].[SessionFile]
GO
/****** Object:  Table [dbo].[Session]    Script Date: 10/16/2011 10:35:38 ******/
DROP TABLE [dbo].[Session]
GO
/****** Object:  Table [dbo].[SessionContentItem]    Script Date: 10/16/2011 10:35:38 ******/
DROP TABLE [dbo].[SessionContentItem]
GO
/****** Object:  StoredProcedure [dbo].[usp_delete_cascade]    Script Date: 10/16/2011 10:35:38 ******/
DROP PROCEDURE [dbo].[usp_delete_cascade]
GO
/****** Object:  Table [dbo].[AttendantState]    Script Date: 10/16/2011 10:35:36 ******/
DROP TABLE [dbo].[AttendantState]
GO
/****** Object:  Table [dbo].[AttendantType]    Script Date: 10/16/2011 10:35:36 ******/
DROP TABLE [dbo].[AttendantType]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 10/16/2011 10:35:36 ******/
DROP TABLE [dbo].[Role]
GO
/****** Object:  Table [dbo].[SessionFileStatus]    Script Date: 10/16/2011 10:35:36 ******/
DROP TABLE [dbo].[SessionFileStatus]
GO
/****** Object:  Table [dbo].[MadbatahFilesStatus]    Script Date: 10/16/2011 10:35:36 ******/
DROP TABLE [dbo].[MadbatahFilesStatus]
GO
/****** Object:  Table [dbo].[SessionStatus]    Script Date: 10/16/2011 10:35:36 ******/
DROP TABLE [dbo].[SessionStatus]
GO
/****** Object:  Table [dbo].[SessionContentItemStatus]    Script Date: 10/16/2011 10:35:36 ******/
DROP TABLE [dbo].[SessionContentItemStatus]
GO

/****** Object:  User [emadbatahuser]    Script Date: 10/16/2011 10:35:36 ******/
DROP USER [emadbatahuser]
GO
USE [master]
GO

/****** Object:  Login [emadbatahuser]    Script Date: 10/16/2011 10:35:36 ******/
DROP LOGIN [emadbatahuser]
GO

/****** Object:  Login [emadbatahuser]    Script Date: 10/16/2011 10:35:36 ******/
/* For security reasons the login is created disabled and with a random password. */
CREATE LOGIN [emadbatahuser] WITH PASSWORD=N'emadbatahuserpass', DEFAULT_DATABASE=[EMadbatah], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
--ALTER LOGIN [emadbatahuser] DISABLE
GO

USE [EMadbatah2]
GO
/****** Object:  User [emadbatahuser]    Script Date: 10/16/2011 10:35:36 ******/
CREATE USER [emadbatahuser] FOR LOGIN [emadbatahuser] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[SessionContentItemStatus]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SessionContentItemStatus](
	[ID] [int] NOT NULL,
	[Name] [varchar](200) COLLATE Arabic_CI_AI NOT NULL,
 CONSTRAINT [PK_SessionContentItemStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SessionStatus]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SessionStatus](
	[ID] [int] NOT NULL,
	[Name] [varchar](500) COLLATE Arabic_CI_AS NOT NULL,
 CONSTRAINT [PK_SessionStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MadbatahFilesStatus]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MadbatahFilesStatus](
	[ID] [int] NOT NULL,
	[Name] [varchar](50) COLLATE Arabic_CI_AI NOT NULL,
 CONSTRAINT [PK_MadbatahFilesStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SessionFileStatus]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SessionFileStatus](
	[ID] [int] NOT NULL,
	[Name] [varchar](100) COLLATE Arabic_CI_AI NOT NULL,
 CONSTRAINT [PK_SessionFileStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Role]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[ID] [bigint] NOT NULL,
	[Name] [nvarchar](500) COLLATE Arabic_CI_AS NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AttendantType]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AttendantType](
	[ID] [int] NOT NULL,
	[Name] [varchar](200) COLLATE Arabic_CI_AI NOT NULL,
 CONSTRAINT [PK_AttendantType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AttendantState]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AttendantState](
	[ID] [int] NOT NULL,
	[Name] [varchar](200) COLLATE Arabic_CI_AI NOT NULL,
 CONSTRAINT [PK_AttendantState] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[usp_delete_cascade]    Script Date: 10/27/2011 11:26:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[usp_delete_cascade] (
        @base_table_name varchar(200), @base_criteria nvarchar(1000)
)
as begin
        -- Adapted from http://www.sqlteam.com/article/performing-a-cascade-delete-in-sql-server-7
        -- Expects the name of a table, and a conditional for selecting rows
        -- within that table that you want deleted.
        -- Produces SQL that, when run, deletes all table rows referencing the ones
        -- you initially selected, cascading into any number of tables,
        -- without the need for "ON DELETE CASCADE".
        -- Does not appear to work with self-referencing tables, but it will
        -- delete everything beneath them.
        -- To make it easy on the server, put a "GO" statement between each line.

        declare @to_delete table (
                id int identity(1, 1) primary key not null,
                criteria nvarchar(1000) not null,
                table_name varchar(200) not null,
                processed bit not null,
                delete_sql varchar(1000)
        )

        insert into @to_delete (criteria, table_name, processed) values (@base_criteria, @base_table_name, 0)

        declare @id int, @criteria nvarchar(1000), @table_name varchar(200)
        while exists(select 1 from @to_delete where processed = 0) begin
                select top 1 @id = id, @criteria = criteria, @table_name = table_name from @to_delete where processed = 0 order by id desc

                insert into @to_delete (criteria, table_name, processed)
                        select referencing_column.name + ' in (select [' + referenced_column.name + '] from [' + @table_name +'] where ' + @criteria + ')',
                                referencing_table.name,
                                0
                        from  sys.foreign_key_columns fk
                                inner join sys.columns referencing_column on fk.parent_object_id = referencing_column.object_id 
                                        and fk.parent_column_id = referencing_column.column_id 
                                inner join  sys.columns referenced_column on fk.referenced_object_id = referenced_column.object_id 
                                        and fk.referenced_column_id = referenced_column.column_id 
                                inner join  sys.objects referencing_table on fk.parent_object_id = referencing_table.object_id 
                                inner join  sys.objects referenced_table on fk.referenced_object_id = referenced_table.object_id 
                                inner join  sys.objects constraint_object on fk.constraint_object_id = constraint_object.object_id
                        where referenced_table.name = @table_name
                                and referencing_table.name != referenced_table.name

                update @to_delete set
                        processed = 1
                where id = @id
        end

        select 'print ''deleting from ' + table_name + '...''; delete from [' + table_name + '] where ' + criteria from @to_delete order by id desc
end

exec usp_delete_cascade 'root_table_name', 'id = 123'
GO
/****** Object:  Table [dbo].[SessionContentItem]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SessionContentItem](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SessionFileID] [bigint] NULL,
	[SessionID] [bigint] NOT NULL,
	[Text] [ntext] COLLATE Arabic_CI_AS NOT NULL,
	[AttendantID] [bigint] NOT NULL,
	[AgendaItemID] [bigint] NOT NULL,
	[AgendaSubItemID] [bigint] NULL,
	[UserID] [bigint] NULL,
	[StatusID] [int] NOT NULL,
	[ReviewerUserID] [bigint] NULL,
	[ReviewerNote] [ntext] COLLATE Arabic_CI_AS NULL,
	[CommentOnText] [nvarchar](max) COLLATE Arabic_CI_AI NULL,
	[CommentOnAttendant] [nvarchar](500) COLLATE Arabic_CI_AI NULL,
	[PageFooter] [nvarchar](500) COLLATE Arabic_CI_AI NULL,
	[UpdatedByReviewer] [bit] NOT NULL,
	[MergedWithPrevious] [bit] NULL,
	[FragOrderInXml] [int] NOT NULL,
	[StartTime] [float] NULL,
	[EndTime] [float] NULL,
	[Duration] [float] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
    [Ignored] [bit] NULL,
	[FileReviewerID] [bigint] NULL,
 CONSTRAINT [PK_SessionContent] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Session]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Session](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Type] [varchar](200) COLLATE Arabic_CI_AI NULL,
	[President] [varchar](200) COLLATE Arabic_CI_AI NULL,
	[Place] [varchar](200) COLLATE Arabic_CI_AI NULL,
	[EParliamentID] [int] NOT NULL,
	[Season] [bigint] NOT NULL,
	[Stage] [bigint] NOT NULL,
	[StageType] [varchar](500) COLLATE Arabic_CI_AI NOT NULL,
	[Serial] [bigint] NOT NULL,
	[SessionStatusID] [int] NULL,
	[SessionMadbatahWord] [varbinary](max) NULL,
	[SessionMadbatahPDF] [varbinary](max) NULL,
	[SessionMadbatahWordFinal] [varbinary](max) NULL,
	[SessionMadbatahPDFFinal] [varbinary](max) NULL,
	[ReviewerID] [bigint] NULL,
	[Subject] [varchar](500) COLLATE Arabic_CI_AI NULL,
	[MadbatahFilesStatusID] [int] NULL,
	[MP3FolderPath] [varchar](500) COLLATE Arabic_CI_AI NULL,
 CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SessionFile]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SessionFile](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SessionID] [bigint] NOT NULL,
	[Name] [nvarchar](300) COLLATE Arabic_CI_AS NOT NULL,
	[DurationSecs] [bigint] NOT NULL,
	[Status] [int] NOT NULL,
	[UserID] [bigint] NULL,
	[LastInsertedFragNumInXml] [int] NOT NULL,
	[Order] [int] NOT NULL,
	[LastModefied] [datetime] NULL,
	[IsSessionStart] [bit] NOT NULL,
	[SessionStartText] [ntext] COLLATE Arabic_CI_AI NULL,
	[SessionStartReviewNote] [nvarchar](500) COLLATE Arabic_CI_AI NULL,
	[SessionStartReviewerID] [bigint] NULL,
	[FileReviewerID] [bigint] NULL,
 CONSTRAINT [PK_SessionFile] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_SessionFile] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FName] [nvarchar](500) COLLATE Arabic_CI_AS NOT NULL,
	[RoleID] [bigint] NOT NULL,
	[DomainUserName] [varchar](500) COLLATE Arabic_CI_AI NOT NULL,
	[Email] [nvarchar](500) COLLATE Arabic_CI_AI NULL,
	[Status] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Attendant]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Attendant](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) COLLATE Arabic_CI_AS NOT NULL,
	[JobTitle] [nvarchar](500) COLLATE Arabic_CI_AS NULL,
	[Type] [int] NULL,
	[State] [int] NULL,
	[EparlimentID] [int] NOT NULL,
 CONSTRAINT [PK_Attendant] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AgendaSubItem]    Script Date: 10/27/2011 11:26:24 ******/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AgendaSubItem](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) COLLATE Arabic_CI_AI NOT NULL,
	[AgendaItemID] [bigint] NOT NULL,
	[EParliamentID] [int] NULL,
	[EParliamentParentID] [int] NULL,
	[Order] [int] NULL,
	[QFrom] [varchar](500) COLLATE Arabic_CI_AI NULL,
	[QTo] [varchar](500) COLLATE Arabic_CI_AI NULL,
	[IsCustom] [bit] NULL,
 CONSTRAINT [PK_AgendaSubItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[SessionAttendant]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SessionAttendant](
	[SessionID] [bigint] NOT NULL,
	[AttendantID] [bigint] NOT NULL,
	[AttendantTitleID] [bigint] NOT NULL
	
 CONSTRAINT [PK_SessionAttendant] PRIMARY KEY CLUSTERED 
(
	[SessionID] ASC,
	[AttendantID] ASC

)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AgendaItem]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AgendaItem](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) COLLATE Arabic_CI_AS NULL,
	[EParliamentID] [int] NULL,
	[EParliamentParentID] [int] NULL,
	[IsCustom] [bit] NULL,
	[SessionID] [bigint] NULL,
	[Order] [int] NULL,
 CONSTRAINT [PK_Agenda] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Attachement]    Script Date: 10/27/2011 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Attachement](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) COLLATE Arabic_CI_AS NOT NULL,
	[Order] [int] NOT NULL,
	[SessionID] [bigint] NOT NULL,
	[FileType] [nvarchar](500) COLLATE Arabic_CI_AS NOT NULL,
	[FileContent] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Attachement] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  StoredProcedure [dbo].[DeleteData]    Script Date: 10/27/2011 11:26:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteData]
	
AS
BEGIN
	
	SET NOCOUNT ON;
delete from sessioncontentitem
delete from SessionAttendant
delete from attendant
delete from agendasubitem
delete from agendaitem
delete from Attachement
delete from sessionfile
delete from [session]
    
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteSessionData]    Script Date: 10/27/2011 11:26:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteSessionData]
	@sessionid bigint
AS
BEGIN
	
	SET NOCOUNT ON;
delete from sessioncontentitem where sessionid = @sessionid
delete from SessionAttendant where sessionid = @sessionid
--delete from attendant where sessionid = @sessionid
--delete from agendasubitem where sessionid = @sessionid
delete from agendaitem where sessionid = @sessionid
delete from Attachement where sessionid = @sessionid
delete from sessionfile where  sessionid = @sessionid
delete from [session] where id = @sessionid 
END
GO
/****** Object:  Default [DF_SessionContent_Approved]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionContentItem] ADD  CONSTRAINT [DF_SessionContent_Approved]  DEFAULT ((1)) FOR [StatusID]
GO
/****** Object:  Default [DF_SessionContent_UpdatedByReviewer]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionContentItem] ADD  CONSTRAINT [DF_SessionContent_UpdatedByReviewer]  DEFAULT ((0)) FOR [UpdatedByReviewer]
GO
/****** Object:  Default [DF_SessionContentItem_MergedWithPrevious]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionContentItem] ADD  CONSTRAINT [DF_SessionContentItem_MergedWithPrevious]  DEFAULT ((0)) FOR [MergedWithPrevious]
GO
/****** Object:  Default [DF_SessionContentItem_Ignored]    Script Date: 11/28/2011 09:08:19 ******/
ALTER TABLE [dbo].[SessionContentItem] ADD  CONSTRAINT [DF_SessionContentItem_Ignored]  DEFAULT ((0)) FOR [Ignored]
GO
/****** Object:  Default [DF_SessionFile_LastInsertedFragNum]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionFile] ADD  CONSTRAINT [DF_SessionFile_LastInsertedFragNum]  DEFAULT ((0)) FOR [LastInsertedFragNumInXml]
GO
/****** Object:  Default [DF_SessionFile_IsSessionStart]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionFile] ADD  CONSTRAINT [DF_SessionFile_IsSessionStart]  DEFAULT ((0)) FOR [IsSessionStart]
GO
/****** Object:  Default [DF_User_Status]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_Status]  DEFAULT ((1)) FOR [Status]
GO
/****** Object:  Default [DF_User_Deleted]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
/****** Object:  Default [DF_AgendaSubItem_IsCustom]    Script Date: 12/18/2011 21:43:37 ******/
ALTER TABLE [dbo].[AgendaSubItem] ADD  CONSTRAINT [DF_AgendaSubItem_IsCustom]  DEFAULT ((0)) FOR [IsCustom]
/****** Object:  ForeignKey [FK_AgendaItem_Session]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[AgendaItem]  WITH CHECK ADD  CONSTRAINT [FK_AgendaItem_Session] FOREIGN KEY([SessionID])
REFERENCES [dbo].[Session] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgendaItem] CHECK CONSTRAINT [FK_AgendaItem_Session]
GO
/****** Object:  ForeignKey [FK_AgendaSubItem_AgendaItem]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[AgendaSubItem]  WITH CHECK ADD  CONSTRAINT [FK_AgendaSubItem_AgendaItem] FOREIGN KEY([AgendaItemID])
REFERENCES [dbo].[AgendaItem] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgendaSubItem] CHECK CONSTRAINT [FK_AgendaSubItem_AgendaItem]
GO
/****** Object:  ForeignKey [FK_Attachement_Session]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[Attachement]  WITH CHECK ADD  CONSTRAINT [FK_Attachement_Session] FOREIGN KEY([SessionID])
REFERENCES [dbo].[Session] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Attachement] CHECK CONSTRAINT [FK_Attachement_Session]
GO
/****** Object:  ForeignKey [FK_Attendant_AttendantState]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[Attendant]  WITH CHECK ADD  CONSTRAINT [FK_Attendant_AttendantState] FOREIGN KEY([State])
REFERENCES [dbo].[AttendantState] ([ID])
GO
ALTER TABLE [dbo].[Attendant] CHECK CONSTRAINT [FK_Attendant_AttendantState]
GO
/****** Object:  ForeignKey [FK_Attendant_AttendantType]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[Attendant]  WITH CHECK ADD  CONSTRAINT [FK_Attendant_AttendantType] FOREIGN KEY([Type])
REFERENCES [dbo].[AttendantType] ([ID])
GO
ALTER TABLE [dbo].[Attendant] CHECK CONSTRAINT [FK_Attendant_AttendantType]
GO
/****** Object:  ForeignKey [FK_Session_MadbatahFilesStatus]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_MadbatahFilesStatus] FOREIGN KEY([MadbatahFilesStatusID])
REFERENCES [dbo].[MadbatahFilesStatus] ([ID])
GO
ALTER TABLE [dbo].[Session] CHECK CONSTRAINT [FK_Session_MadbatahFilesStatus]
GO
/****** Object:  ForeignKey [FK_Session_SessionStatus]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_SessionStatus] FOREIGN KEY([SessionStatusID])
REFERENCES [dbo].[SessionStatus] ([ID])
GO
ALTER TABLE [dbo].[Session] CHECK CONSTRAINT [FK_Session_SessionStatus]
GO
/****** Object:  ForeignKey [FK_SessionAttendant_Attendant]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionAttendant]  WITH CHECK ADD  CONSTRAINT [FK_SessionAttendant_Attendant] FOREIGN KEY([AttendantID])
REFERENCES [dbo].[Attendant] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SessionAttendant] CHECK CONSTRAINT [FK_SessionAttendant_Attendant]
GO
/****** Object:  ForeignKey [FK_SessionAttendant_Session]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionAttendant]  WITH CHECK ADD  CONSTRAINT [FK_SessionAttendant_Session] FOREIGN KEY([SessionID])
REFERENCES [dbo].[Session] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SessionAttendant] CHECK CONSTRAINT [FK_SessionAttendant_Session]
/****** Object:  Default [DF_AgendaItem_IsCustom]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[AgendaItem] ADD  CONSTRAINT [DF_AgendaItem_IsCustom]  DEFAULT ((0)) FOR [IsCustom]
GO

/****** Object:  ForeignKey [FK_SessionContentItem_AgendaItem]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionContentItem]  WITH CHECK ADD  CONSTRAINT [FK_SessionContentItem_AgendaItem] FOREIGN KEY([AgendaItemID])
REFERENCES [dbo].[AgendaItem] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SessionContentItem] CHECK CONSTRAINT [FK_SessionContentItem_AgendaItem]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_AgendaSubItem]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionContentItem]  WITH CHECK ADD  CONSTRAINT [FK_SessionContentItem_AgendaSubItem] FOREIGN KEY([AgendaSubItemID])
REFERENCES [dbo].[AgendaSubItem] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[SessionContentItem] CHECK CONSTRAINT [FK_SessionContentItem_AgendaSubItem]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_Attendant]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionContentItem]  WITH CHECK ADD  CONSTRAINT [FK_SessionContentItem_Attendant] FOREIGN KEY([AttendantID])
REFERENCES [dbo].[Attendant] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SessionContentItem] CHECK CONSTRAINT [FK_SessionContentItem_Attendant]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_FileReviewer]    Script Date: 12/18/2011 21:43:37 ******/
ALTER TABLE [dbo].[SessionContentItem]  WITH CHECK ADD  CONSTRAINT [FK_SessionContentItem_FileReviewer] FOREIGN KEY([FileReviewerID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[SessionContentItem] CHECK CONSTRAINT [FK_SessionContentItem_FileReviewer]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_Reviewer]    Script Date: 12/18/2011 21:43:37 ******/
ALTER TABLE [dbo].[SessionContentItem]  WITH CHECK ADD  CONSTRAINT [FK_SessionContentItem_Reviewer] FOREIGN KEY([ReviewerUserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[SessionContentItem] CHECK CONSTRAINT [FK_SessionContentItem_Reviewer]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_Session]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionContentItem]  WITH CHECK ADD  CONSTRAINT [FK_SessionContentItem_Session] FOREIGN KEY([SessionID])
REFERENCES [dbo].[Session] ([ID])
GO
ALTER TABLE [dbo].[SessionContentItem] CHECK CONSTRAINT [FK_SessionContentItem_Session]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_SessionContentItemStatus]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionContentItem]  WITH CHECK ADD  CONSTRAINT [FK_SessionContentItem_SessionContentItemStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[SessionContentItemStatus] ([ID])
GO
ALTER TABLE [dbo].[SessionContentItem] CHECK CONSTRAINT [FK_SessionContentItem_SessionContentItemStatus]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_SessionFile]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionContentItem]  WITH CHECK ADD  CONSTRAINT [FK_SessionContentItem_SessionFile] FOREIGN KEY([SessionFileID])
REFERENCES [dbo].[SessionFile] ([ID])
GO
ALTER TABLE [dbo].[SessionContentItem] CHECK CONSTRAINT [FK_SessionContentItem_SessionFile]
GO
/****** Object:  ForeignKey [FK_SessionContentItem_User]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionContentItem]  WITH CHECK ADD  CONSTRAINT [FK_SessionContentItem_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[SessionContentItem] CHECK CONSTRAINT [FK_SessionContentItem_User]
GO
/****** Object:  ForeignKey [FK_SessionFile_FileReviewer]    Script Date: 12/18/2011 21:43:37 ******/
ALTER TABLE [dbo].[SessionFile]  WITH CHECK ADD  CONSTRAINT [FK_SessionFile_FileReviewer] FOREIGN KEY([FileReviewerID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[SessionFile] CHECK CONSTRAINT [FK_SessionFile_FileReviewer]
GO











/****** Object:  ForeignKey [FK_SessionFile_Reviewer]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionFile]  WITH CHECK ADD  CONSTRAINT [FK_SessionFile_Reviewer] FOREIGN KEY([SessionStartReviewerID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[SessionFile] CHECK CONSTRAINT [FK_SessionFile_Reviewer]
GO
/****** Object:  ForeignKey [FK_SessionFile_Session]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionFile]  WITH CHECK ADD  CONSTRAINT [FK_SessionFile_Session] FOREIGN KEY([SessionID])
REFERENCES [dbo].[Session] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SessionFile] CHECK CONSTRAINT [FK_SessionFile_Session]
GO
/****** Object:  ForeignKey [FK_SessionFile_SessionFileStatus]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionFile]  WITH CHECK ADD  CONSTRAINT [FK_SessionFile_SessionFileStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[SessionFileStatus] ([ID])
GO
ALTER TABLE [dbo].[SessionFile] CHECK CONSTRAINT [FK_SessionFile_SessionFileStatus]
GO
/****** Object:  ForeignKey [FK_SessionFile_User]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[SessionFile]  WITH CHECK ADD  CONSTRAINT [FK_SessionFile_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[SessionFile] CHECK CONSTRAINT [FK_SessionFile_User]
GO
/****** Object:  ForeignKey [FK_User_Role]    Script Date: 10/27/2011 11:26:24 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
/**********Add MadbatahUser as owner*******/
EXEC sp_addrolemember N'db_owner', N'emadbatahuser'
/************************/
/* lookup tables*/
INSERT [dbo].[SessionContentItemStatus] ([ID], [Name]) VALUES (1, N'Approved')
INSERT [dbo].[SessionContentItemStatus] ([ID], [Name]) VALUES (2, N'Rejected')
INSERT [dbo].[SessionContentItemStatus] ([ID], [Name]) VALUES (3, N'Fixed')
INSERT [dbo].[SessionContentItemStatus] ([ID], [Name]) VALUES (4, N'ModefiedAfterApprove')

INSERT [dbo].[SessionStatus] ([ID], [Name]) VALUES (1, N'New')
INSERT [dbo].[SessionStatus] ([ID], [Name]) VALUES (2, N'InProgress')
INSERT [dbo].[SessionStatus] ([ID], [Name]) VALUES (3, N'Completed')
INSERT [dbo].[SessionStatus] ([ID], [Name]) VALUES (4, N'Approved')
INSERT [dbo].[SessionStatus] ([ID], [Name]) VALUES (5, N'FinalApproved')

INSERT [dbo].[MadbatahFilesStatus] ([ID], [Name]) VALUES (1, N'NotCreated')
INSERT [dbo].[MadbatahFilesStatus] ([ID], [Name]) VALUES (2, N'InProgress')
INSERT [dbo].[MadbatahFilesStatus] ([ID], [Name]) VALUES (3, N'DraftCreated')
INSERT [dbo].[MadbatahFilesStatus] ([ID], [Name]) VALUES (4, N'FinalCreated')
INSERT [dbo].[MadbatahFilesStatus] ([ID], [Name]) VALUES (5, N'DraftFail')
INSERT [dbo].[MadbatahFilesStatus] ([ID], [Name]) VALUES (6, N'FinalFail')

INSERT [dbo].[SessionFileStatus] ([ID], [Name]) VALUES (1, N'New')
INSERT [dbo].[SessionFileStatus] ([ID], [Name]) VALUES (2, N'InProgress')
INSERT [dbo].[SessionFileStatus] ([ID], [Name]) VALUES (3, N'Completed')
INSERT [dbo].[SessionFileStatus] ([ID], [Name]) VALUES (4, N'SessionStartApproved')
INSERT [dbo].[SessionFileStatus] ([ID], [Name]) VALUES (5, N'SessionStartRejected')
INSERT [dbo].[SessionFileStatus] ([ID], [Name]) VALUES (6, N'SessionStartFixed')
INSERT [dbo].[SessionFileStatus] ([ID], [Name]) VALUES (7, N'SessionStartModifiedAfterApprove')

INSERT [dbo].[Role] ([ID], [Name]) VALUES (1, N'Admin')
INSERT [dbo].[Role] ([ID], [Name]) VALUES (2, N'DataEntry')
INSERT [dbo].[Role] ([ID], [Name]) VALUES (3, N'Reviewer')
INSERT [dbo].[Role] ([ID], [Name]) VALUES (4, N'ReviewrDataEntry')

INSERT [dbo].[AttendantType] ([ID], [Name]) VALUES (1, N'FromTheCouncilMembers')
INSERT [dbo].[AttendantType] ([ID], [Name]) VALUES (2, N'FromOutsideTheCouncil')
INSERT [dbo].[AttendantType] ([ID], [Name]) VALUES (3, N'GovernmentRepresentative')
INSERT [dbo].[AttendantType] ([ID], [Name]) VALUES (4, N'Secretariat')
INSERT [dbo].[AttendantType] ([ID], [Name]) VALUES (5, N'NA')

INSERT [dbo].[AttendantState] ([ID], [Name]) VALUES (1, N'Attended')
INSERT [dbo].[AttendantState] ([ID], [Name]) VALUES (2, N'Absent')
INSERT [dbo].[AttendantState] ([ID], [Name]) VALUES (3, N'Abology')
INSERT [dbo].[AttendantState] ([ID], [Name]) VALUES (4, N'InMission')

USE [master]
GO

ALTER DATABASE [EMadbatah2] SET  MULTI_USER WITH ROLLBACK IMMEDIATE
GO
