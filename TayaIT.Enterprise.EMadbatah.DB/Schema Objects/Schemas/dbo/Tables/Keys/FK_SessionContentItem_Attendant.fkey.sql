ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [FK_SessionContentItem_Attendant] FOREIGN KEY ([AttendantID]) REFERENCES [dbo].[Attendant] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

