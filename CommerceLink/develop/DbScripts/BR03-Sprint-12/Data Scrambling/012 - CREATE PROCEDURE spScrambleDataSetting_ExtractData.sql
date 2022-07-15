SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Aqeel Arshad Siddiqui
-- Create date: Tuesday, April 28, 2020
-- Description:	Extract data from string based on pattern
-- =============================================
CREATE OR ALTER PROCEDURE spScrambleDataSetting_ExtractData
	-- Add the parameters for the stored procedure here
	@DataString AS NVARCHAR(MAX),
	@Seperator AS NVARCHAR(10),
	@Pattern AS NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	-- Table that will contain resultant data
	DECLARE @tblMatched AS TABLE (
		Data NVARCHAR(MAX)
	)

	-- Index to capture data on the basis of pattern
	DECLARE @CurrentStartIndex INT = 1;
	DECLARE @CurrentEndIndex INT = 0;

	-- Extract the index using pattern
	SET @CurrentStartIndex = PATINDEX(@Pattern, @DataString)

	IF (@CurrentStartIndex > 0)
	BEGIN
		-- Extract the Starting Index using separator
		SELECT	@CurrentStartIndex = LEN(REVERSE(SUBSTRING(@DataString, 1, PATINDEX(@Pattern, @DataString)))) - CHARINDEX(@Seperator, REVERSE(SUBSTRING(@DataString, 1, PATINDEX(@Pattern, @DataString)))) 
		-- PRINT @CurrentStartIndex

		WHILE (@CurrentStartIndex > 0)
		BEGIN

			SET @CurrentStartIndex = @CurrentStartIndex + 2;
			-- PRINT 'Starting Index: ' + CAST(@CurrentStartIndex AS NVARCHAR);

			-- Truncate the data
			SET @DataString = SUBSTRING(@DataString, @CurrentStartIndex, 4000)
			-- PRINT 'Truncated string to extract data from: ' + @DataString

			-- Extract the ending point
			SET @CurrentEndIndex = CHARINDEX(@Seperator, @DataString) - 1
			-- PRINT 'Ending Index:' + CAST(@CurrentEndIndex AS NVARCHAR);

			-- Extract Email Address
			DECLARE @TargetString NVARCHAR(MAX) = SUBSTRING(@DataString, 1, @CurrentEndIndex);

			INSERT INTO
				@tblMatched
					(Data)
				VALUES
					(@TargetString)

			SET @DataString = SUBSTRING(@DataString, @CurrentEndIndex, 4000)

			SELECT	@CurrentStartIndex = LEN(REVERSE(SUBSTRING(@DataString, 1, PATINDEX(@Pattern, @DataString)))) - CHARINDEX(@Seperator, REVERSE(SUBSTRING(@DataString, 1, PATINDEX(@Pattern, @DataString))))
			--PRINT @CurrentStartIndex

		END
	END

	SELECT * FROM @tblMatched
END
GO
