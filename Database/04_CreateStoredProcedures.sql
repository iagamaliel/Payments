
USE ESolutionsPayments;
GO

-- =============================================
-- Stored Procedure: usp_GetChannelConfiguration
-- Description: Retrieves an active channel configuration.
-- =============================================
CREATE OR ALTER PROCEDURE dbo.usp_GetChannelConfiguration
(
    @ChannelCode VARCHAR(10)
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ChannelConfigurationId,
        ChannelCode,
        ServiceUrl,
        ApiKey,
        ShowMenu,
        DestinationPage
    FROM dbo.ChannelConfiguration
    WHERE ChannelCode = @ChannelCode
      AND IsActive = 1;
END;
GO
