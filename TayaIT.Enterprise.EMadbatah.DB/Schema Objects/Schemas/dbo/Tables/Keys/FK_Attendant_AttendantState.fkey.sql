ALTER TABLE [dbo].[Attendant]
    ADD CONSTRAINT [FK_Attendant_AttendantState] FOREIGN KEY ([State]) REFERENCES [dbo].[AttendantState] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

