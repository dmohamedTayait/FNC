CREATE TABLE [dbo].[User] (
    [ID]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [FName]          NVARCHAR (500) COLLATE Arabic_CI_AS NOT NULL,
    [RoleID]         BIGINT         NOT NULL,
    [DomainUserName] VARCHAR (500)  NOT NULL,
    [Email]          NVARCHAR (500) NULL,
    [Status]         BIT            NOT NULL
);

