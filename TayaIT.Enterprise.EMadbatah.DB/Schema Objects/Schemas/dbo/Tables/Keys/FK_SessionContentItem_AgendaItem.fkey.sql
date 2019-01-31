ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [FK_SessionContentItem_AgendaItem] FOREIGN KEY ([AgendaItemID]) REFERENCES [dbo].[AgendaItem] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

