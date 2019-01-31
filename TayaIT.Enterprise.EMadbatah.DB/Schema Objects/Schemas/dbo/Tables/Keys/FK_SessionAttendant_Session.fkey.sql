ALTER TABLE [dbo].[SessionAttendant]
    ADD CONSTRAINT [FK_SessionAttendant_Session] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Session] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

