ALTER TABLE [dbo].[AgendaSubItem]
    ADD CONSTRAINT [FK_AgendaSubItem_AgendaItem] FOREIGN KEY ([AgendaItemID]) REFERENCES [dbo].[AgendaItem] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

