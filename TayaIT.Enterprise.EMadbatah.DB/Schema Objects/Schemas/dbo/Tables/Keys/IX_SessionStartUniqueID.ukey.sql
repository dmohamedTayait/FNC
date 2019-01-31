﻿ALTER TABLE [dbo].[Session]
    ADD CONSTRAINT [IX_SessionStartUniqueID] UNIQUE NONCLUSTERED ([SessionStartID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF) ON [PRIMARY];

