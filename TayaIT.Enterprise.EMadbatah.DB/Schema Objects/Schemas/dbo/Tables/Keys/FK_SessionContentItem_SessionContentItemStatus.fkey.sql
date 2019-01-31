ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [FK_SessionContentItem_SessionContentItemStatus] FOREIGN KEY ([StatusID]) REFERENCES [dbo].[SessionContentItemStatus] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

