ALTER TABLE [dbo].[SessionFile]
    ADD CONSTRAINT [FK_SessionFile_SessionFileStatus] FOREIGN KEY ([Status]) REFERENCES [dbo].[SessionFileStatus] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

