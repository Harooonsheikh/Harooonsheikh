SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Aqeel Arshad Siddiqui
-- Create date: Tuesday, April 28, 2020
-- Description:	Scramble DataPacket in RequestResponse table
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[spRequestResponse_ScrambleDataPacket]
	-- Add the parameters for the stored procedure here
	@RequestResponseId AS INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @tblDataScrambleSetting AS TABLE (
		DataScrambleSettingId INT,
		Pattern NVARCHAR(MAX),
		Seperator NVARCHAR(10),
		ScrambledData NVARCHAR(MAX)
	);

	DECLARE @tblData AS TABLE (
		DataID INT PRIMARY KEY IDENTITY(1, 1),
		StringData NVARCHAR(MAX)
	);

	INSERT INTO
		@tblDataScrambleSetting
			(DataScrambleSettingId, Pattern, Seperator, ScrambledData)
	SELECT	DataScrambleSettingId, Pattern, Seperator, ScrambledData
	FROM	DataScrambleSetting
	WHERE	TableName = 'RequestResponse';

	DECLARE @MaxIndex INT;
	SELECT @MaxIndex = MAX(DataScrambleSettingId) FROM @tblDataScrambleSetting;

	DECLARE @Index INT = 1;
	DECLARE @DataString AS NVARCHAR(MAX);

	SELECT	@DataString = DataPacket
	FROM	RequestResponse
	WHERE	RequestResponseId = @RequestResponseId;

	WHILE (@Index <= @MaxIndex)
	BEGIN

		IF NOT EXISTS( SELECT * FROM @tblDataScrambleSetting WHERE DataScrambleSettingId = @Index)
		BEGIN
			SET @Index = @Index + 1;
			CONTINUE;
		END

		DECLARE @Seperator AS NVARCHAR(1); 
		DECLARE @Pattern AS NVARCHAR(MAX);
		DECLARE @ScrambledData AS NVARCHAR(MAX);

		SELECT @Seperator = Seperator FROM @tblDataScrambleSetting WHERE DataScrambleSettingId = @Index;
		SELECT @Pattern = Pattern FROM @tblDataScrambleSetting WHERE DataScrambleSettingId = @Index;
		SELECT @ScrambledData = ScrambledData FROM @tblDataScrambleSetting WHERE DataScrambleSettingId = @Index;

		INSERT INTO
			@tblData
				(StringData)
		EXEC spScrambleDataSetting_ExtractData @DataString, @Seperator, @Pattern;

		DECLARE @ScrambleDataIndex INT = 1;
		DECLARE @ScrambleDataMaxIndex INT;
		SELECT @ScrambleDataMaxIndex = MAX(DataID) FROM @tblData;

		WHILE (@ScrambleDataIndex <= @ScrambleDataMaxIndex)
		BEGIN
			DECLARE @DataToBeReplaced NVARCHAR(MAX);
			SELECT	@DataToBeReplaced = StringData
			FROM	@tblData
			WHERE	DataID = @ScrambleDataIndex;

			SET @DataString = REPLACE(@DataString, @DataToBeReplaced, @ScrambledData)

			SET @ScrambleDataIndex = @ScrambleDataIndex + 1;
		END

		SET @Index = @Index + 1;
	END
	
	SET @DataString = REPLACE(@DataString, '"P"phone"', '"Phone"');

	UPDATE	RequestResponse
	SET		DataPacket = @DataString
	WHERE	RequestResponseId = @RequestResponseId;

END
