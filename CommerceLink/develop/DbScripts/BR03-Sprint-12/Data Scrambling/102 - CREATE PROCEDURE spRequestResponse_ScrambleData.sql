SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Aqeel Arshad Siddiqui
-- Create date: Tuesday, April 28, 2020
-- Description:	Scramble DataPacket and OutputPacket in RequestResponse table
-- =============================================
CREATE OR ALTER PROCEDURE spRequestResponse_ScrambleData
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @tblRequestResponseToScrumble AS TABLE (
		ID INT PRIMARY KEY IDENTITY(1, 1),
		RequestResponseId INT
	)

	INSERT INTO
		@tblRequestResponseToScrumble
			(RequestResponseId)
	SELECT	RequestResponseId
	FROM	RequestResponse
	WHERE	ISNULL(Scrambled, 0) = 0

	DECLARE @Index INT = 1;
	DECLARE @IndexMax INT;
	SELECT @IndexMax = MAX(ID) FROM @tblRequestResponseToScrumble;

	WHILE (@Index <= @IndexMax)
	BEGIN

		DECLARE @RequestResponseID INT;

		SELECT	@RequestResponseID = RequestResponseId
		FROM	@tblRequestResponseToScrumble
		WHERE	ID = @Index;

		EXEC [dbo].[spRequestResponse_ScrambleDataPacket] @RequestResponseID
		EXEC [dbo].[spRequestResponse_ScrambleOutputPacket] @RequestResponseID

		UPDATE	RequestResponse
		SET		Scrambled = 1
		WHERE	RequestResponseId = @RequestResponseID

		SET @Index = @Index + 1;
	END

END
GO
