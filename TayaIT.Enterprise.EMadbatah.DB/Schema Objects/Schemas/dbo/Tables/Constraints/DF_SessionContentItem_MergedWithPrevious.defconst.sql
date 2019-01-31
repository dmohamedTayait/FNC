ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [DF_SessionContentItem_MergedWithPrevious] DEFAULT ((0)) FOR [MergedWithPrevious];

