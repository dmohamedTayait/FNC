ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [DF_SessionContent_UpdatedByReviewer] DEFAULT ((0)) FOR [UpdatedByReviewer];

