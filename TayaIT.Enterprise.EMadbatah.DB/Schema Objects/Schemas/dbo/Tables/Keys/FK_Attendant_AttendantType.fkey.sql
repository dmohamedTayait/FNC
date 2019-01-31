ALTER TABLE [dbo].[Attendant]
    ADD CONSTRAINT [FK_Attendant_AttendantType] FOREIGN KEY ([Type]) REFERENCES [dbo].[AttendantType] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

