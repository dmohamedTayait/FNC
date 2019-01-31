ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [FK_SessionContentItem_AgendaSubItem] FOREIGN KEY ([AgendaSubItemID]) REFERENCES [dbo].[AgendaSubItem] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

