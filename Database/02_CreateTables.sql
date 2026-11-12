
USE ESolutionsPayments;
GO

-- =============================================
-- Table: ChannelConfiguration
-- =============================================
CREATE TABLE dbo.ChannelConfiguration
(
    ChannelConfigurationId INT IDENTITY(1,1) NOT NULL,
    ChannelCode VARCHAR(10) NOT NULL,
    ServiceUrl VARCHAR(500) NOT NULL,
    ApiKey VARCHAR(250) NOT NULL,
    ShowMenu BIT NOT NULL,
    DestinationPage VARCHAR(200) NOT NULL,
    IsActive BIT NOT NULL
        CONSTRAINT DF_ChannelConfiguration_IsActive DEFAULT (1),
    CreatedAt DATETIME2(7) NOT NULL
        CONSTRAINT DF_ChannelConfiguration_CreatedAt DEFAULT (SYSDATETIME()),
    UpdatedAt DATETIME2(7) NULL,

    CONSTRAINT PK_ChannelConfiguration
        PRIMARY KEY (ChannelConfigurationId),

    CONSTRAINT UQ_ChannelConfiguration_ChannelCode
        UNIQUE (ChannelCode)
);
GO

-- =============================================
-- Table: Companies
-- =============================================
CREATE TABLE dbo.Companies
(
    Id INT IDENTITY(1,1) NOT NULL,
    Name VARCHAR(150) NOT NULL,
    LogoUrl VARCHAR(500) NULL,
    IsActive BIT NOT NULL
        CONSTRAINT DF_Companies_IsActive DEFAULT (1),
    CreatedAt DATETIME2(7) NOT NULL
        CONSTRAINT DF_Companies_CreatedAt DEFAULT (SYSDATETIME()),

    CONSTRAINT PK_Companies
        PRIMARY KEY (Id)
);
GO
