ALTER TABLE [dbo].[AgendaItem]
    ADD CONSTRAINT [DF_AgendaItem_IsCustom] DEFAULT ((0)) FOR [IsCustom];

