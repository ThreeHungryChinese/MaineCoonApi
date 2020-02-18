CREATE TABLE [dbo].[defaultKewordsSearchCVStorage_copy1] (
    [TweetsId]          BIGINT        NOT NULL,
    [Tags]              NTEXT         NULL,
    [TweetsCreatedTime] DATETIME2 (0) NULL,
    CONSTRAINT [PK__defaultK__F749D23FFCF0CBFC_copy1] PRIMARY KEY CLUSTERED ([TweetsId] ASC)
);

