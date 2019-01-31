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

