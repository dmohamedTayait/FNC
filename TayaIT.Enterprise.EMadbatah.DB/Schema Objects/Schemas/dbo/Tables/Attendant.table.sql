CREATE TABLE [dbo].[Attendant] (
    [ID]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (500)  COLLATE Arabic_CI_AS NOT NULL,
    [JobTitle]     NVARCHAR (500) COLLATE Arabic_CI_AS NULL,
    [Type]         INT            NULL,
    [State]        INT            NULL,
    [EparlimentID] BIGINT         NOT NULL
);

