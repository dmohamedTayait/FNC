ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [FK_SessionContentItem_Reviewer] FOREIGN KEY ([ReviewerUserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

