
USE ESolutionsPayments;
GO

-- =============================================
-- Initial data: ChannelConfiguration
-- =============================================
IF NOT EXISTS (
    SELECT 1
    FROM dbo.ChannelConfiguration
    WHERE ChannelCode = 'AB'
)
BEGIN
    INSERT INTO dbo.ChannelConfiguration
    (
        ChannelCode,
        ServiceUrl,
        ApiKey,
        ShowMenu,
        DestinationPage
    )
    VALUES
    (
        'AB',
        'https://localhost:44352/api/v1/companies',
        'AB-DEMO-API-KEY',
        1,
        'Destination.aspx'
    );
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM dbo.ChannelConfiguration
    WHERE ChannelCode = 'CJ'
)
BEGIN
    INSERT INTO dbo.ChannelConfiguration
    (
        ChannelCode,
        ServiceUrl,
        ApiKey,
        ShowMenu,
        DestinationPage
    )
    VALUES
    (
        'CJ',
        'https://localhost:44352/api/v1/companies',
        'CJ-DEMO-API-KEY',
        0,
        'Destination.aspx'
    );
END;
GO

-- =============================================
-- Initial data: Companies
-- =============================================
IF NOT EXISTS (SELECT 1 FROM dbo.Companies WHERE Name = 'ANDA')
BEGIN
    INSERT INTO dbo.Companies (Name, LogoUrl)
    VALUES
    (
        'ANDA',
        'https://bts.com.sv/storage/colecturia-servicios/September2022/ANDA.webp'
    );
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Companies WHERE Name = 'CAESS')
BEGIN
    INSERT INTO dbo.Companies (Name, LogoUrl)
    VALUES
    (
        'CAESS',
        'https://bts.com.sv/storage/colecturia-servicios/September2022/CAESS.webp'
    );
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Companies WHERE Name = 'CLARO')
BEGIN
    INSERT INTO dbo.Companies (Name, LogoUrl)
    VALUES
    (
        'CLARO',
        'https://bts.com.sv/storage/colecturia-servicios/September2022/CLARO.webp'
    );
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Companies WHERE Name = 'TIGO')
BEGIN
    INSERT INTO dbo.Companies (Name, LogoUrl)
    VALUES
    (
        'TIGO',
        'https://bts.com.sv/storage/colecturia-servicios/September2022/TIGO.webp'
    );
END;
GO
