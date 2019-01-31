/*
Deployment script for TayaIT.Enterprise.EMadbatah.DB
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar Path1 "C:\Databases\$(DatabaseName)\"
:setvar Path2 "C:\Databases\$(DatabaseName)\"
:setvar DatabaseName "TayaIT.Enterprise.EMadbatah.DB"
:setvar DefaultDataPath ""
:setvar DefaultLogPath ""

GO
:on error exit
GO
USE [master]
GO
IF (DB_ID(N'$(DatabaseName)') IS NOT NULL
    AND DATABASEPROPERTYEX(N'$(DatabaseName)','Status') <> N'ONLINE')
BEGIN
    RAISERROR(N'The state of the target database, %s, is not set to ONLINE. To deploy to this database, its state must be set to ONLINE.', 16, 127,N'$(DatabaseName)') WITH NOWAIT
    RETURN
END

GO
IF (DB_ID(N'$(DatabaseName)') IS NOT NULL) 
BEGIN
    ALTER DATABASE [$(DatabaseName)]
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$(DatabaseName)];
END

GO
PRINT N'Creating $(DatabaseName)...'
GO
CREATE DATABASE [$(DatabaseName)]
    ON 
    PRIMARY(NAME = [EMadbatah], FILENAME = '$(Path2)$(DatabaseName).mdf', FILEGROWTH = 1024 KB)
    LOG ON (NAME = [EMadbatah_log], FILENAME = '$(Path1)$(DatabaseName)_log.ldf', MAXSIZE = 2097152 MB, FILEGROWTH = 10 %) COLLATE Arabic_CI_AI
GO
EXECUTE sp_dbcmptlevel [$(DatabaseName)], 90;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                NUMERIC_ROUNDABORT OFF,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL,
                RECOVERY FULL,
                CURSOR_CLOSE_ON_COMMIT OFF,
                AUTO_CREATE_STATISTICS ON,
                AUTO_SHRINK OFF,
                AUTO_UPDATE_STATISTICS ON,
                RECURSIVE_TRIGGERS OFF 
            WITH ROLLBACK IMMEDIATE;
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_CLOSE OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ALLOW_SNAPSHOT_ISOLATION OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET READ_COMMITTED_SNAPSHOT OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_UPDATE_STATISTICS_ASYNC OFF,
                PAGE_VERIFY NONE,
                DATE_CORRELATION_OPTIMIZATION OFF,
                DISABLE_BROKER,
                PARAMETERIZATION SIMPLE,
                SUPPLEMENTAL_LOGGING OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET TRUSTWORTHY OFF,
        DB_CHAINING OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'The database settings cannot be modified. You must be a SysAdmin to apply these settings.';
    END


GO
USE [$(DatabaseName)]
GO
IF fulltextserviceproperty(N'IsFulltextInstalled') = 1
    EXECUTE sp_fulltext_database 'enable';


GO
/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

GO
PRINT N'Creating [DEVELOP\ihosny]...';


GO
CREATE USER [DEVELOP\ihosny] FOR LOGIN [DEVELOP\ihosny];


GO
PRINT N'Creating [DEVELOP\unada]...';


GO
CREATE USER [DEVELOP\unada] FOR LOGIN [DEVELOP\unada];


GO
PRINT N'Creating <unnamed>...';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'DEVELOP\ihosny';


GO
PRINT N'Creating <unnamed>...';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'DEVELOP\unada';


GO
PRINT N'Creating [dbo].[AgendaItem]...';


GO
CREATE TABLE [dbo].[AgendaItem] (
    [ID]                  BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]                VARCHAR (500) COLLATE Arabic_CI_AS NULL,
    [EParliamentID]       BIGINT        NULL,
    [EParliamentParentID] BIGINT        NULL,
    [IsCustom]            BIT           NULL,
    [SessionID]           BIGINT        NULL,
    [Order]               INT           NULL
);


GO
PRINT N'Creating PK_Agenda...';


GO
ALTER TABLE [dbo].[AgendaItem]
    ADD CONSTRAINT [PK_Agenda] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[AgendaSubItem]...';


GO
CREATE TABLE [dbo].[AgendaSubItem] (
    [ID]                  BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]                VARCHAR (500) NOT NULL,
    [AgendaItemID]        BIGINT        NOT NULL,
    [EParliamentID]       BIGINT        NULL,
    [EParliamentParentID] BIGINT        NULL,
    [Order]               INT           NULL,
    [QFrom]               VARCHAR (500) NULL,
    [QTo]                 VARCHAR (500) NULL
);


GO
PRINT N'Creating PK_AgendaSubItem...';


GO
ALTER TABLE [dbo].[AgendaSubItem]
    ADD CONSTRAINT [PK_AgendaSubItem] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[Attachement]...';


GO
CREATE TABLE [dbo].[Attachement] (
    [ID]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (500)  COLLATE Arabic_CI_AS NOT NULL,
    [Order]       INT             NOT NULL,
    [SessionID]   BIGINT          NOT NULL,
    [FileType]    NVARCHAR (500)  COLLATE Arabic_CI_AS NOT NULL,
    [FileContent] VARBINARY (MAX) NOT NULL
);


GO
PRINT N'Creating PK_Attachement...';


GO
ALTER TABLE [dbo].[Attachement]
    ADD CONSTRAINT [PK_Attachement] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[Attendant]...';


GO
CREATE TABLE [dbo].[Attendant] (
    [ID]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (500)  COLLATE Arabic_CI_AS NOT NULL,
    [JobTitle]     NVARCHAR (500) COLLATE Arabic_CI_AS NULL,
    [Type]         INT            NULL,
    [State]        INT            NULL,
    [EparlimentID] BIGINT         NOT NULL
);


GO
PRINT N'Creating PK_Attendant...';


GO
ALTER TABLE [dbo].[Attendant]
    ADD CONSTRAINT [PK_Attendant] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[AttendantState]...';


GO
CREATE TABLE [dbo].[AttendantState] (
    [ID]   INT           NOT NULL,
    [Name] VARCHAR (200) NOT NULL
);


GO
PRINT N'Creating PK_AttendantState...';


GO
ALTER TABLE [dbo].[AttendantState]
    ADD CONSTRAINT [PK_AttendantState] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[AttendantType]...';


GO
CREATE TABLE [dbo].[AttendantType] (
    [ID]   INT           NOT NULL,
    [Name] VARCHAR (200) NOT NULL
);


GO
PRINT N'Creating PK_AttendantType...';


GO
ALTER TABLE [dbo].[AttendantType]
    ADD CONSTRAINT [PK_AttendantType] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[Role]...';


GO
CREATE TABLE [dbo].[Role] (
    [ID]   BIGINT         NOT NULL,
    [Name] NVARCHAR (500) COLLATE Arabic_CI_AS NOT NULL
);


GO
PRINT N'Creating PK_Role...';


GO
ALTER TABLE [dbo].[Role]
    ADD CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[Session]...';


GO
CREATE TABLE [dbo].[Session] (
    [ID]                       BIGINT          IDENTITY (1, 1) NOT NULL,
    [Date]                     DATETIME        NOT NULL,
    [StartTime]                DATETIME        NULL,
    [EndTime]                  DATETIME        NULL,
    [Type]                     VARCHAR (200)   NULL,
    [President]                VARCHAR (200)   NULL,
    [Place]                    VARCHAR (200)   NULL,
    [SessionStatusID]          INT             NOT NULL,
    [EParliamentID]            BIGINT          NOT NULL,
    [Season]                   BIGINT          NOT NULL,
    [Stage]                    BIGINT          NOT NULL,
    [StageType]                VARCHAR (500)   NOT NULL,
    [Serial]                   BIGINT          NOT NULL,
    [SessionStartID]           BIGINT          NULL,
    [SessionMadbatahWord]      VARBINARY (MAX) NULL,
    [SessionMadbatahPDF]       VARBINARY (MAX) NULL,
    [SessionMadbatahWordFinal] VARBINARY (MAX) NULL,
    [SessionMadbatahPDFFinal]  VARBINARY (MAX) NULL,
    [ReviewerID]               BIGINT          NULL,
    [Subject]                  VARCHAR (500)   NULL
);


GO
PRINT N'Creating PK_Session...';


GO
ALTER TABLE [dbo].[Session]
    ADD CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating IX_SessionStartUniqueID...';


GO
ALTER TABLE [dbo].[Session]
    ADD CONSTRAINT [IX_SessionStartUniqueID] UNIQUE NONCLUSTERED ([SessionStartID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF) ON [PRIMARY];


GO
PRINT N'Creating [dbo].[SessionAttendant]...';


GO
CREATE TABLE [dbo].[SessionAttendant] (
    [SessionID]        BIGINT NOT NULL,
    [AttendantID]      BIGINT NOT NULL,
    [AttendantTitleID] BIGINT NOT NULL
);


GO
PRINT N'Creating PK_SessionAttendant...';


GO
ALTER TABLE [dbo].[SessionAttendant]
    ADD CONSTRAINT [PK_SessionAttendant] PRIMARY KEY CLUSTERED ([SessionID] ASC, [AttendantID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[SessionContentItem]...';


GO
CREATE TABLE [dbo].[SessionContentItem] (
    [ID]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [SessionFileID]      BIGINT         NULL,
    [SessionID]          BIGINT         NOT NULL,
    [Text]               NTEXT          COLLATE Arabic_CI_AS NOT NULL,
    [AttendantID]        BIGINT         NOT NULL,
    [AgendaItemID]       BIGINT         NOT NULL,
    [AgendaSubItemID]    BIGINT         NULL,
    [UserID]             BIGINT         NULL,
    [StatusID]           INT            NOT NULL,
    [ReviewerUserID]     BIGINT         NULL,
    [ReviewerNote]       NTEXT          COLLATE Arabic_CI_AS NULL,
    [CommentOnText]      NVARCHAR (MAX) NULL,
    [CommentOnAttendant] NVARCHAR (500) NULL,
    [PageFooter]         NVARCHAR (500) NULL,
    [UpdatedByReviewer]  BIT            NOT NULL,
    [MergedWithPrevious] BIT            NULL,
    [FragOrderInXml]     INT            NOT NULL,
    [StartTime]          FLOAT          NULL,
    [EndTime]            FLOAT          NULL,
    [Duration]           FLOAT          NULL
);


GO
PRINT N'Creating PK_SessionContent...';


GO
ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [PK_SessionContent] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[SessionContentItemStatus]...';


GO
CREATE TABLE [dbo].[SessionContentItemStatus] (
    [ID]   INT           NOT NULL,
    [Name] VARCHAR (200) NOT NULL
);


GO
PRINT N'Creating PK_SessionContentItemStatus...';


GO
ALTER TABLE [dbo].[SessionContentItemStatus]
    ADD CONSTRAINT [PK_SessionContentItemStatus] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[SessionFile]...';


GO
CREATE TABLE [dbo].[SessionFile] (
    [ID]                       BIGINT         IDENTITY (1, 1) NOT NULL,
    [SessionID]                BIGINT         NOT NULL,
    [Name]                     NVARCHAR (300) COLLATE Arabic_CI_AS NOT NULL,
    [DurationSecs]             BIGINT         NOT NULL,
    [Status]                   INT            NOT NULL,
    [UserID]                   BIGINT         NULL,
    [LastInsertedFragNumInXml] INT            NOT NULL,
    [Order]                    INT            NOT NULL,
    [LastModefied]             DATETIME       NULL
);


GO
PRINT N'Creating PK_SessionFile...';


GO
ALTER TABLE [dbo].[SessionFile]
    ADD CONSTRAINT [PK_SessionFile] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating IX_SessionFile...';


GO
ALTER TABLE [dbo].[SessionFile]
    ADD CONSTRAINT [IX_SessionFile] UNIQUE NONCLUSTERED ([Name] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF) ON [PRIMARY];


GO
PRINT N'Creating [dbo].[SessionFileStatus]...';


GO
CREATE TABLE [dbo].[SessionFileStatus] (
    [ID]   INT           NOT NULL,
    [Name] VARCHAR (100) NOT NULL
);


GO
PRINT N'Creating PK_SessionFileStatus...';


GO
ALTER TABLE [dbo].[SessionFileStatus]
    ADD CONSTRAINT [PK_SessionFileStatus] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[SessionStart]...';


GO
CREATE TABLE [dbo].[SessionStart] (
    [ID]        BIGINT NOT NULL,
    [Text]      NTEXT  NOT NULL,
    [UserID]    BIGINT NULL,
    [SessionID] BIGINT NOT NULL
);


GO
PRINT N'Creating PK_SessionStart...';


GO
ALTER TABLE [dbo].[SessionStart]
    ADD CONSTRAINT [PK_SessionStart] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[SessionStatus]...';


GO
CREATE TABLE [dbo].[SessionStatus] (
    [ID]   INT           NOT NULL,
    [Name] VARCHAR (500) COLLATE Arabic_CI_AS NOT NULL
);


GO
PRINT N'Creating PK_SessionStatus...';


GO
ALTER TABLE [dbo].[SessionStatus]
    ADD CONSTRAINT [PK_SessionStatus] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[User]...';


GO
CREATE TABLE [dbo].[User] (
    [ID]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [FName]          NVARCHAR (500) COLLATE Arabic_CI_AS NOT NULL,
    [RoleID]         BIGINT         NOT NULL,
    [DomainUserName] VARCHAR (500)  NOT NULL,
    [Email]          NVARCHAR (500) NULL,
    [Status]         BIT            NOT NULL
);


GO
PRINT N'Creating PK_User...';


GO
ALTER TABLE [dbo].[User]
    ADD CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating DF_AgendaItem_IsCustom...';


GO
ALTER TABLE [dbo].[AgendaItem]
    ADD CONSTRAINT [DF_AgendaItem_IsCustom] DEFAULT ((0)) FOR [IsCustom];


GO
PRINT N'Creating DF_SessionContent_Approved...';


GO
ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [DF_SessionContent_Approved] DEFAULT ((1)) FOR [StatusID];


GO
PRINT N'Creating DF_SessionContent_UpdatedByReviewer...';


GO
ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [DF_SessionContent_UpdatedByReviewer] DEFAULT ((0)) FOR [UpdatedByReviewer];


GO
PRINT N'Creating DF_SessionContentItem_MergedWithPrevious...';


GO
ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [DF_SessionContentItem_MergedWithPrevious] DEFAULT ((0)) FOR [MergedWithPrevious];


GO
PRINT N'Creating DF_SessionFile_LastInsertedFragNum...';


GO
ALTER TABLE [dbo].[SessionFile]
    ADD CONSTRAINT [DF_SessionFile_LastInsertedFragNum] DEFAULT ((0)) FOR [LastInsertedFragNumInXml];


GO
PRINT N'Creating DF_User_Status...';


GO
ALTER TABLE [dbo].[User]
    ADD CONSTRAINT [DF_User_Status] DEFAULT ((1)) FOR [Status];


GO
PRINT N'Creating FK_AgendaItem_Session...';


GO
ALTER TABLE [dbo].[AgendaItem] WITH NOCHECK
    ADD CONSTRAINT [FK_AgendaItem_Session] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Session] ([ID]) ON DELETE CASCADE ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_AgendaSubItem_AgendaItem...';


GO
ALTER TABLE [dbo].[AgendaSubItem] WITH NOCHECK
    ADD CONSTRAINT [FK_AgendaSubItem_AgendaItem] FOREIGN KEY ([AgendaItemID]) REFERENCES [dbo].[AgendaItem] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_Attachement_Session...';


GO
ALTER TABLE [dbo].[Attachement] WITH NOCHECK
    ADD CONSTRAINT [FK_Attachement_Session] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Session] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_Attendant_AttendantState...';


GO
ALTER TABLE [dbo].[Attendant] WITH NOCHECK
    ADD CONSTRAINT [FK_Attendant_AttendantState] FOREIGN KEY ([State]) REFERENCES [dbo].[AttendantState] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_Attendant_AttendantType...';


GO
ALTER TABLE [dbo].[Attendant] WITH NOCHECK
    ADD CONSTRAINT [FK_Attendant_AttendantType] FOREIGN KEY ([Type]) REFERENCES [dbo].[AttendantType] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_Session_SessionStatus...';


GO
ALTER TABLE [dbo].[Session] WITH NOCHECK
    ADD CONSTRAINT [FK_Session_SessionStatus] FOREIGN KEY ([SessionStatusID]) REFERENCES [dbo].[SessionStatus] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionAttendant_Attendant...';


GO
ALTER TABLE [dbo].[SessionAttendant] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionAttendant_Attendant] FOREIGN KEY ([AttendantID]) REFERENCES [dbo].[Attendant] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionAttendant_Session...';


GO
ALTER TABLE [dbo].[SessionAttendant] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionAttendant_Session] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Session] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionContentItem_AgendaItem...';


GO
ALTER TABLE [dbo].[SessionContentItem] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionContentItem_AgendaItem] FOREIGN KEY ([AgendaItemID]) REFERENCES [dbo].[AgendaItem] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionContentItem_AgendaSubItem...';


GO
ALTER TABLE [dbo].[SessionContentItem] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionContentItem_AgendaSubItem] FOREIGN KEY ([AgendaSubItemID]) REFERENCES [dbo].[AgendaSubItem] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionContentItem_Attendant...';


GO
ALTER TABLE [dbo].[SessionContentItem] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionContentItem_Attendant] FOREIGN KEY ([AttendantID]) REFERENCES [dbo].[Attendant] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionContentItem_Reviewer...';


GO
ALTER TABLE [dbo].[SessionContentItem] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionContentItem_Reviewer] FOREIGN KEY ([ReviewerUserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionContentItem_Session...';


GO
ALTER TABLE [dbo].[SessionContentItem] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionContentItem_Session] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Session] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionContentItem_SessionContentItemStatus...';


GO
ALTER TABLE [dbo].[SessionContentItem] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionContentItem_SessionContentItemStatus] FOREIGN KEY ([StatusID]) REFERENCES [dbo].[SessionContentItemStatus] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionContentItem_SessionFile...';


GO
ALTER TABLE [dbo].[SessionContentItem] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionContentItem_SessionFile] FOREIGN KEY ([SessionFileID]) REFERENCES [dbo].[SessionFile] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionContentItem_User...';


GO
ALTER TABLE [dbo].[SessionContentItem] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionContentItem_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionFile_Session...';


GO
ALTER TABLE [dbo].[SessionFile] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionFile_Session] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Session] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionFile_SessionFileStatus...';


GO
ALTER TABLE [dbo].[SessionFile] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionFile_SessionFileStatus] FOREIGN KEY ([Status]) REFERENCES [dbo].[SessionFileStatus] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionFile_User...';


GO
ALTER TABLE [dbo].[SessionFile] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionFile_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionStart_Session...';


GO
ALTER TABLE [dbo].[SessionStart] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionStart_Session] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Session] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_SessionStart_User...';


GO
ALTER TABLE [dbo].[SessionStart] WITH NOCHECK
    ADD CONSTRAINT [FK_SessionStart_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_User_Role...';


GO
ALTER TABLE [dbo].[User] WITH NOCHECK
    ADD CONSTRAINT [FK_User_Role] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Role] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating [dbo].[DeleteData]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE DeleteData
	
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
delete from sessionstart
delete from [session]
    
END
GO
PRINT N'Creating [dbo].[usp_delete_cascade]...';


GO
create procedure usp_delete_cascade (
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
-- Refactoring step to update target server with deployed transaction logs
CREATE TABLE  [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
GO
sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
GO

GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[AgendaItem] WITH CHECK CHECK CONSTRAINT [FK_AgendaItem_Session];

ALTER TABLE [dbo].[AgendaSubItem] WITH CHECK CHECK CONSTRAINT [FK_AgendaSubItem_AgendaItem];

ALTER TABLE [dbo].[Attachement] WITH CHECK CHECK CONSTRAINT [FK_Attachement_Session];

ALTER TABLE [dbo].[Attendant] WITH CHECK CHECK CONSTRAINT [FK_Attendant_AttendantState];

ALTER TABLE [dbo].[Attendant] WITH CHECK CHECK CONSTRAINT [FK_Attendant_AttendantType];

ALTER TABLE [dbo].[Session] WITH CHECK CHECK CONSTRAINT [FK_Session_SessionStatus];

ALTER TABLE [dbo].[SessionAttendant] WITH CHECK CHECK CONSTRAINT [FK_SessionAttendant_Attendant];

ALTER TABLE [dbo].[SessionAttendant] WITH CHECK CHECK CONSTRAINT [FK_SessionAttendant_Session];

ALTER TABLE [dbo].[SessionContentItem] WITH CHECK CHECK CONSTRAINT [FK_SessionContentItem_AgendaItem];

ALTER TABLE [dbo].[SessionContentItem] WITH CHECK CHECK CONSTRAINT [FK_SessionContentItem_AgendaSubItem];

ALTER TABLE [dbo].[SessionContentItem] WITH CHECK CHECK CONSTRAINT [FK_SessionContentItem_Attendant];

ALTER TABLE [dbo].[SessionContentItem] WITH CHECK CHECK CONSTRAINT [FK_SessionContentItem_Reviewer];

ALTER TABLE [dbo].[SessionContentItem] WITH CHECK CHECK CONSTRAINT [FK_SessionContentItem_Session];

ALTER TABLE [dbo].[SessionContentItem] WITH CHECK CHECK CONSTRAINT [FK_SessionContentItem_SessionContentItemStatus];

ALTER TABLE [dbo].[SessionContentItem] WITH CHECK CHECK CONSTRAINT [FK_SessionContentItem_SessionFile];

ALTER TABLE [dbo].[SessionContentItem] WITH CHECK CHECK CONSTRAINT [FK_SessionContentItem_User];

ALTER TABLE [dbo].[SessionFile] WITH CHECK CHECK CONSTRAINT [FK_SessionFile_Session];

ALTER TABLE [dbo].[SessionFile] WITH CHECK CHECK CONSTRAINT [FK_SessionFile_SessionFileStatus];

ALTER TABLE [dbo].[SessionFile] WITH CHECK CHECK CONSTRAINT [FK_SessionFile_User];

ALTER TABLE [dbo].[SessionStart] WITH CHECK CHECK CONSTRAINT [FK_SessionStart_Session];

ALTER TABLE [dbo].[SessionStart] WITH CHECK CHECK CONSTRAINT [FK_SessionStart_User];

ALTER TABLE [dbo].[User] WITH CHECK CHECK CONSTRAINT [FK_User_Role];


GO
ALTER DATABASE [$(DatabaseName)]
    SET MULTI_USER 
    WITH ROLLBACK IMMEDIATE;


GO
