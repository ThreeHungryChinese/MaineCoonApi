CREATE TABLE [dbo].[test_defaultKeywordsSearchStorage] (
    [TweetsMediaJson]     NTEXT         NULL,
    [TweetsText]          NTEXT         NULL,
    [TweetsCreatedTime]   DATETIME2 (0) NULL,
    [TweetsCreater]       NTEXT         NULL,
    [TweetsId]            BIGINT        NOT NULL,
    [TweetsCreaterUserId] BIGINT        NULL,
    [TweetsImgInfo]       NTEXT         NULL,
    [TweetsNLPScore]      BIGINT        NULL,
    CONSTRAINT [PK__defaultK__F749D23FD78ADFE5_copy1] PRIMARY KEY CLUSTERED ([TweetsId] ASC)
);

