ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [FK_SessionContentItem_SessionFile] FOREIGN KEY ([SessionFileID]) REFERENCES [dbo].[SessionFile] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

