CREATE TABLE [dbo].[Processors] (
    [Id]                          INT            IDENTITY (1, 1) NOT NULL,
    [friendlyName]                NVARCHAR (MAX) NOT NULL,
    [isTrained]                   BIT            NOT NULL,
    [trainCallbackURL]            VARCHAR (MAX)  NOT NULL,
    [getResultURL]                VARCHAR (MAX)  NOT NULL,
    [isGetResultNeedWaitCallback] BIT            NOT NULL,
    [resetURL]                    VARCHAR (MAX)  NOT NULL,
    [TLSversion]                  INT            NOT NULL,
    [publicKey]                   VARCHAR (MAX)  NULL,
    [belongsToUserID]             INT            NOT NULL,
    [Count]                       INT            DEFAULT ((0)) NOT NULL,
    [_AlgorithmParameterJson]     NVARCHAR (MAX) NULL,
    [Instruction]                 VARCHAR (50)   DEFAULT (NULL) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

