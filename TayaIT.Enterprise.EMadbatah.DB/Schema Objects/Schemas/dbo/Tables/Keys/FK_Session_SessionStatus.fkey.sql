ALTER TABLE [dbo].[Session]
    ADD CONSTRAINT [FK_Session_SessionStatus] FOREIGN KEY ([SessionStatusID]) REFERENCES [dbo].[SessionStatus] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

