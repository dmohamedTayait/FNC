ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [FK_SessionContentItem_Session] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Session] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

