ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [FK_SessionContentItem_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

