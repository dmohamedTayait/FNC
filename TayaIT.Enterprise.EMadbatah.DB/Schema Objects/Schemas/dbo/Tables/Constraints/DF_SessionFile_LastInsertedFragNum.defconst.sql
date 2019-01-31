ALTER TABLE [dbo].[SessionFile]
    ADD CONSTRAINT [DF_SessionFile_LastInsertedFragNum] DEFAULT ((0)) FOR [LastInsertedFragNumInXml];

