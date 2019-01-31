CREATE TABLE [dbo].[Attachement] (
    [ID]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (500)  COLLATE Arabic_CI_AS NOT NULL,
    [Order]       INT             NOT NULL,
    [SessionID]   BIGINT          NOT NULL,
    [FileType]    NVARCHAR (500)  COLLATE Arabic_CI_AS NOT NULL,
    [FileContent] VARBINARY (MAX) NOT NULL
);

