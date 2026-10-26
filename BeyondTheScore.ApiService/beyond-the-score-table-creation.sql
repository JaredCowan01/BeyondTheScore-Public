CREATE TABLE Teams
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    Name NVARCHAR(100) NOT NULL UNIQUE,

    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);


CREATE TABLE Players
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    TeamId INT NOT NULL,

    Name NVARCHAR(100) NOT NULL,

    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_Players_Teams
        FOREIGN KEY (TeamId)
            REFERENCES Teams(Id),

    CONSTRAINT UQ_Player_Team
        UNIQUE(TeamId, Name)
);

CREATE TABLE Matches
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    MatchIdentifier NVARCHAR(50) NOT NULL UNIQUE,

    HomeTeamId INT NOT NULL,

    AwayTeamId INT NOT NULL,

    MatchDate DATETIME2 NULL,

    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_Matches_HomeTeam
        FOREIGN KEY (HomeTeamId)
            REFERENCES Teams(Id),

    CONSTRAINT FK_Matches_AwayTeam
        FOREIGN KEY (AwayTeamId)
            REFERENCES Teams(Id)
);

CREATE TABLE MatchEvents
(
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,

    EventId INT NOT NULL,

    MatchId INT NOT NULL,

    TeamId INT NOT NULL,

    PlayerId INT NOT NULL,

    Minute INT NOT NULL,

    Second INT NOT NULL,

    EventType NVARCHAR(50) NOT NULL,

    Outcome NVARCHAR(50) NULL,

    XCoord DECIMAL(9,2) NULL,
    YCoord DECIMAL(9,2) NULL,

    EndXCoord DECIMAL(9,2) NULL,
    EndYCoord DECIMAL(9,2) NULL,

    PossessionId INT NULL,

    AttackSequenceId INT NULL,

    PressureLevel INT NULL,

    PassDistance DECIMAL(9,2) NULL,

    PassDifficulty DECIMAL(9,2) NULL,

    PlayerSpeed DECIMAL(9,2) NULL,

    BallSpeed DECIMAL(9,2) NULL,

    ExpectedGoals DECIMAL(9,4) NULL,

    ScoreHome INT NULL,

    ScoreAway INT NULL,

    SignificanceScore INT NULL,

    NarrativeTrigger BIT NULL,

    MomentumDelta DECIMAL(9,2) NULL,

    StoryArc NVARCHAR(50) NULL,

    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_MatchEvents_Match
        FOREIGN KEY (MatchId)
            REFERENCES Matches(Id),

    CONSTRAINT FK_MatchEvents_Team
        FOREIGN KEY (TeamId)
            REFERENCES Teams(Id),

    CONSTRAINT FK_MatchEvents_Player
        FOREIGN KEY (PlayerId)
            REFERENCES Players(Id)
);

CREATE TABLE Possessions
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    MatchId INT NOT NULL,

    PossessionNumber INT NOT NULL,

    TeamId INT NOT NULL,

    StartMinute INT NULL,

    EndMinute INT NULL,

    EventCount INT NULL,

    CONSTRAINT FK_Possessions_Match
        FOREIGN KEY (MatchId)
            REFERENCES Matches(Id),

    CONSTRAINT FK_Possessions_Team
        FOREIGN KEY (TeamId)
            REFERENCES Teams(Id)
);

CREATE TABLE AttackSequences
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    MatchId INT NOT NULL,

    SequenceNumber INT NOT NULL,

    TeamId INT NOT NULL,

    StartMinute INT NULL,

    EndMinute INT NULL,

    ExpectedGoals DECIMAL(9,4) NULL,

    Outcome NVARCHAR(50) NULL,

    CONSTRAINT FK_AttackSequences_Match
        FOREIGN KEY (MatchId)
            REFERENCES Matches(Id),

    CONSTRAINT FK_AttackSequences_Team
        FOREIGN KEY (TeamId)
            REFERENCES Teams(Id)
);

CREATE TABLE MatchInsights
(
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,

    MatchId INT NOT NULL,

    Minute INT NOT NULL,

    AgentName NVARCHAR(50) NOT NULL,

    InsightType NVARCHAR(100) NOT NULL,

    Title NVARCHAR(300) NOT NULL,

    Explanation NVARCHAR(MAX) NOT NULL,

    ConfidenceScore DECIMAL(5,2) NULL,

    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_MatchInsights_Match
        FOREIGN KEY (MatchId)
            REFERENCES Matches(Id)
);

CREATE TABLE PersonalisedCommentary
(
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,

    MatchId INT NOT NULL,

    TargetAudience NVARCHAR(50) NOT NULL,

    LanguageCode NVARCHAR(10) NOT NULL,

    Minute INT NOT NULL,

    Commentary NVARCHAR(MAX) NOT NULL,

    GeneratedBy NVARCHAR(50) NOT NULL,

    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_PersonalisedCommentary_Match
        FOREIGN KEY (MatchId)
            REFERENCES Matches(Id)
);

CREATE TABLE UserPromptUsages (
      Id INT IDENTITY(1,1) PRIMARY KEY,
      Auth0UserId NVARCHAR(100) NOT NULL,
      PromptCount INT NOT NULL DEFAULT 0,
      LastPromptDate DATETIME2 NULL
);

CREATE INDEX IX_MatchEvents_MatchId
    ON MatchEvents(MatchId);

CREATE INDEX IX_MatchEvents_PlayerId
    ON MatchEvents(PlayerId);

CREATE INDEX IX_MatchEvents_TeamId
    ON MatchEvents(TeamId);

CREATE INDEX IX_MatchEvents_EventType
    ON MatchEvents(EventType);

CREATE INDEX IX_MatchEvents_Minute
    ON MatchEvents(Minute);

CREATE INDEX IX_MatchEvents_Possession
    ON MatchEvents(PossessionId);

CREATE INDEX IX_MatchEvents_AttackSequence
    ON MatchEvents(AttackSequenceId);

CREATE UNIQUE INDEX IX_UserPromptUsages_Auth0UserId
    ON UserPromptUsages (Auth0UserId);