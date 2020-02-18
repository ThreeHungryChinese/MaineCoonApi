CREATE TABLE [dbo].[TagsDailyCount] (
    [Id]     BIGINT IDENTITY (1, 1) NOT NULL,
    [TagsId] BIGINT NOT NULL,
    [Date]   DATE   NOT NULL,
    [Count]  INT    DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

