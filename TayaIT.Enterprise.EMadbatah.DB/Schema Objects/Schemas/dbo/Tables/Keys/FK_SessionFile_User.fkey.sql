﻿ALTER TABLE [dbo].[SessionFile]
    ADD CONSTRAINT [FK_SessionFile_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

