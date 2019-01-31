ALTER TABLE [dbo].[SessionStart]
    ADD CONSTRAINT [FK_SessionStart_Session] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Session] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

