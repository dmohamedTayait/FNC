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

