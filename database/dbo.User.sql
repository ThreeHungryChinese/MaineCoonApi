CREATE TABLE [dbo].[User] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [email]         VARCHAR (50)  NOT NULL,
    [password]      BINARY (32)   NOT NULL,
    [sysRole]       INT           NOT NULL,
    [registionTime] DATETIME2 (7) NOT NULL,
    [accountStatus] INT           NOT NULL,
    [SALT]          BINARY (64)   NOT NULL,
    [UserName]      VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([email] ASC)
);

