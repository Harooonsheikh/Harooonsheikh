-- CREATE THE TABLE FOR RequestResponse ARCHIVING
CREATE TABLE [dbo].[Archive_RequestResponse](
	[RequestResponseId] [int] PRIMARY KEY NOT NULL,
	[StoreId] [int] NOT NULL,
	[DataDirectionId] [int] NOT NULL,
	[EcomTransactionId] [nvarchar](256) NULL,
	[ApplicationName] [nvarchar](50) NOT NULL,
	[MethodName] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DataPacket] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL 
)
GO

-- =============================================
-- Author:		Aqeel Arshad Siddiqui
-- Create date: 20 Feb 2019
-- Description:	Move old data to archive table for Request Response
-- NOTE:	A maintenance plan will be made against this stored procedure so that thisstored procedure is executed at pre-scheduled time.
--			Steps to create a maintenance plan is as follow:
--			1.	Connect the target SQL Server using SQL Server Management Studio
--			2.	In Object Explorer navigate to “Management” > “Maintenance Plans”
--			3.	Right click the “Maintenance Plans”, system will open context menu, click on “New Maintenance Plan…”
--			4.	System will prompt for maintenance plan’s name in a new window with title “New Maintenance Plan”
--			5.	Provide the name “CLTVENV_MaintenancePlan_ArchiveRequestResponse” for maintenance plan. Replace the environment name with EVN in the name for respective environment. For production the name could be “CLTVPRD_MaintenancePlan_ArchiveRequestResponse”
--			6.	System will display the Maintenance Plan design area
--			7.	From “Toolbox”, drag-drop task with title “Execute T-SQL Statement Task” in the design area
--			8.	System will display add a new task in design area
--			9.	Double click the newly added task
--			10.	System will display new window with title “Execute T-SQL Statement Task”
--			11.	Select target connection from the list or add a new target connection if it does not exists in the list
--			12.	By default the “Execution time out” will be 0
--			13.	In “T-SQL statement” text box type the EXEC command with fully qualified name of the stored procedure. Database name followed by schema followed by name of the stored procedure, “EXEC [CLTVINT].[dbo].[spArchiveRequestResponse]”. Replace the environment name with EVN in the name for respective environment. For production the name could be “EXEC [CLTVPRD].[dbo].[spArchiveRequestResponse]”
--			14.	Click the “Calender” icon in the “schedule column”
--			15.	System will display a new window “New Job Schedule” to enter the schedule
--			16.	Select the appropriate/desired schedule
--			17.	Press the OK button the save the schedule for maintenance plan
--			18.	Save the maintenance plan. Maintenance plan is created and now it will be visible in “Maintenance Plans” and in Jobs as well.
-- =============================================
CREATE OR ALTER PROCEDURE spArchiveRequestResponse
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	BEGIN TRAN

	BEGIN TRY

		DECLARE @NumberOfDays AS INT
		SET @NumberOfDays = 90;

		DECLARE @ThresholdDate AS DATETIME;
		IF @NumberOfDays > 0
		BEGIN
			SET @NumberOfDays = @NumberOfDays * -1;
		END
		SET @ThresholdDate = CAST(DATEADD(d, @NumberOfDays, GETDATE()) AS DATE);

		INSERT INTO	Archive_RequestResponse
		SELECT		*
		FROM		RequestResponse
		WHERE		CreatedOn < @ThresholdDate;

		DELETE FROM	RequestResponse
		WHERE		CreatedOn < @ThresholdDate;

		PRINT('Archiving sucessfull');
		COMMIT TRAN

	END TRY

	BEGIN CATCH

		PRINT('Archiving failed with reason: ' + ERROR_MESSAGE());
		ROLLBACK TRAN

	END CATCH

END
GO

-- =============================================
-- Author:		Aqeel Arshad Siddiqui
-- Create date: Monday, February 25, 2019
-- Description:	Select request response data from both archive and live tables
-- =============================================
CREATE OR ALTER VIEW [dbo].[vwRequestResponse]
AS
	SELECT	*
	FROM	Archive_RequestResponse

	UNION ALL

	SELECT	*
	FROM	RequestResponse 
GO
