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

