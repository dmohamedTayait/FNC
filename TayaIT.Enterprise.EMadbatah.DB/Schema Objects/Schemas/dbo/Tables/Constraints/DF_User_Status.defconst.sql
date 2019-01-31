ALTER TABLE [dbo].[User]
    ADD CONSTRAINT [DF_User_Status] DEFAULT ((1)) FOR [Status];

