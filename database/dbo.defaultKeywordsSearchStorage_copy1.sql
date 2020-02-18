CREATE TABLE [dbo].[defaultKeywordsSearchStorage_copy1] (
    [TweetsMediaJson]     NTEXT         NULL,
    [TweetsText]          NTEXT         NULL,
    [TweetsCreatedTime]   DATETIME2 (0) NULL,
    [TweetsCreater]       NTEXT         NULL,
    [TweetsId]            BIGINT        NOT NULL,
    [TweetsCreaterUserId] BIGINT        NULL,
    [TweetsImgInfo]       NTEXT         NULL,
    [TweetsNLPScore]      BIGINT        DEFAULT ((0)) NULL,
    CONSTRAINT [PK__defaultK__F749D23FD78ADFE5_copy2] PRIMARY KEY CLUSTERED ([TweetsId] ASC)
);

