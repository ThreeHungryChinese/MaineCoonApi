CREATE TABLE [dbo].[StudentScore] (
    [Id]  INT        IDENTITY (1, 1) NOT NULL,
    [T]   FLOAT (53) NULL,
    [G]   FLOAT (53) NULL,
    [UR]  FLOAT (53) NULL,
    [SOP] FLOAT (53) NULL,
    [LOR] FLOAT (53) NULL,
    [GPA] FLOAT (53) NULL,
    [RES] FLOAT (53) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

