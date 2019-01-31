ALTER TABLE [dbo].[Attachement]
    ADD CONSTRAINT [FK_Attachement_Session] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Session] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

