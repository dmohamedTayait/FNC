﻿ALTER TABLE [dbo].[User]
    ADD CONSTRAINT [FK_User_Role] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Role] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
