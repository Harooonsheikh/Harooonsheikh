SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Aqeel Arshad Siddiqui
-- Create date: Tuesday, May 05, 2020
-- Description:	Scramble EventMessage in Log table
-- =============================================
CREATE OR ALTER PROCEDURE spLog_ScrambleData
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @tblLogToScrumble AS TABLE (
		ID INT PRIMARY KEY IDENTITY(1, 1),
		LogId INT
	)

	INSERT INTO
		@tblLogToScrumble
			(LogId)
	SELECT	LogId
	FROM	Log
	WHERE	ISNULL(Scrambled, 0) = 0

	DECLARE @Index INT = 1;
	DECLARE @IndexMax INT;
	SELECT @IndexMax = MAX(ID) FROM @tblLogToScrumble;

	WHILE (@Index <= @IndexMax)
	BEGIN

		DECLARE @LogID INT;

		SELECT	@LogID = LogId
		FROM	@tblLogToScrumble
		WHERE	ID = @Index;

		EXEC [dbo].[spLog_ScrambleEventMessage] @LogID

		SET @Index = @Index + 1;
	END

END
GO
