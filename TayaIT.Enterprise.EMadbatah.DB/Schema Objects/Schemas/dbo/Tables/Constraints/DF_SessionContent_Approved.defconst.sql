ALTER TABLE [dbo].[SessionContentItem]
    ADD CONSTRAINT [DF_SessionContent_Approved] DEFAULT ((1)) FOR [StatusID];

