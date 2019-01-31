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

