ALTER TABLE [dbo].[AgendaItem]
    ADD CONSTRAINT [FK_AgendaItem_Session] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Session] ([ID]) ON DELETE CASCADE ON UPDATE NO ACTION;

