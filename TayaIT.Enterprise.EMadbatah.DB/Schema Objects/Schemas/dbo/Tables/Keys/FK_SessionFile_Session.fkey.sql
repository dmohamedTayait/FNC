ALTER TABLE [dbo].[SessionFile]
    ADD CONSTRAINT [FK_SessionFile_Session] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Session] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

