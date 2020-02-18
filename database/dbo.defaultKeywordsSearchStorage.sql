CREATE TABLE [dbo].[defaultKeywordsSearchStorage] (
    [TweetsMediaJson]     NTEXT         NULL,
    [TweetsText]          NTEXT         NULL,
    [TweetsCreatedTime]   DATETIME2 (0) NULL,
    [TweetsCreater]       NTEXT         NULL,
    [TweetsId]            BIGINT        NOT NULL,
    [TweetsCreaterUserId] BIGINT        NULL,
    [TweetsImgInfo]       NTEXT         NULL,
    [TweetsNLPScore]      BIGINT        DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([TweetsId] ASC)
);

