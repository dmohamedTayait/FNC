ALTER TABLE [dbo].[SessionAttendant]
    ADD CONSTRAINT [FK_SessionAttendant_Attendant] FOREIGN KEY ([AttendantID]) REFERENCES [dbo].[Attendant] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

