CREATE TABLE [dbo].[UniversityPrograms] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [ProgramName]           VARCHAR (50)   NOT NULL,
    [BelongsToUserId]       INT            NOT NULL,
    [ProcesserId]           INT            DEFAULT ((0)) NULL,
    [Count]                 INT            DEFAULT ((0)) NOT NULL,
    [IsTrainNeeded]         BIT            DEFAULT ((0)) NOT NULL,
    [IsEnabled]             INT            DEFAULT ((0)) NOT NULL,
    [_programJson]          NVARCHAR (MAX) NULL,
    [ProgramIntroduction]   VARCHAR (50)   NULL,
    [_usedProcessorsIdJson] NVARCHAR (MAX) NULL,
    [_programParameterJson] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

