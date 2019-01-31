CREATE TABLE [dbo].[AgendaItem] (
    [ID]                  BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]                VARCHAR (500) COLLATE Arabic_CI_AS NULL,
    [EParliamentID]       BIGINT        NULL,
    [EParliamentParentID] BIGINT        NULL,
    [IsCustom]            BIT           NULL,
    [SessionID]           BIGINT        NULL,
    [Order]               INT           NULL
);

