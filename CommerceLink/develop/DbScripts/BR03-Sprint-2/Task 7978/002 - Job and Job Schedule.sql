/****************
INSERT JOBS
****************/
INSERT INTO Job
	(JobId, JobName, [Enabled], JobTypeId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy)
VALUES
	(28, 'DiscountWithAffiliationSync', 1, 0, GETDATE(), 1, GETDATE(), 1),
	(128, 'UploadDiscountWithAffiliationSync', 1, 1, GETDATE(), 1, GETDATE(), 1),
	(29, 'QuantityDiscountWithAffiliationSync', 1, 0, GETDATE(), 1, GETDATE(), 1),
    (129, 'UploadQuantityDiscountWithAffiliationSync', 1, 1, GETDATE(), 1, GETDATE(), 1)

/****************
INSERT JOB SCHEDULE
****************/
SELECT	StoreId
INTO	#ControlTable 
FROM	dbo.Store
WHERE	[Name] = 'Netherland'

DECLARE @StoreId INT

WHILE EXISTS (SELECT * FROM #ControlTable)
BEGIN

    SELECT	@StoreId = (	SELECT		TOP 1 StoreId
							FROM		#ControlTable
							ORDER BY	StoreId ASC)

-- Run for every store
-------------------------------------------------------------

INSERT INTO JobSchedule
	(JobId, JobInterval, IsRepeatable, StartTime, IsActive, JobStatus, StoreId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy)
VALUES
	(28, 60, 1, '00:00:00.0000000', 1, 1, @StoreID, GETDATE(), '1', GETDATE(), '1'),
	(128, 60, 1, '00:00:00.0000000', 1, 1, @StoreID, GETDATE(), '1', GETDATE(), '1'),
	(29, 60, 1, '00:00:00.0000000', 1, 1, @StoreID, GETDATE(), '1', GETDATE(), '1'),
	(129, 60, 1, '00:00:00.0000000', 1, 1, @StoreID, GETDATE(), '1', GETDATE(), '1')

--------------------------------

    DELETE	#ControlTable
    WHERE	StoreId = @StoreId

END

DROP TABLE #ControlTable

-------------------------------------------------------------

SELECT	StoreId
INTO	#ControlTable1 
FROM	dbo.Store
WHERE	[Name] <> 'Netherland'

-- DECLARE @StoreId INT

WHILE EXISTS (SELECT * FROM #ControlTable1)
BEGIN

    SELECT	@StoreId = (	SELECT		TOP 1 StoreId
							FROM		#ControlTable1
							ORDER BY	StoreId ASC)

-- Run for every store
-------------------------------------------------------------

INSERT INTO JobSchedule
	(JobId, JobInterval, IsRepeatable, StartTime, IsActive, JobStatus, StoreId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy)
VALUES
	(28, 60, 1, '00:00:00.0000000', 0, 1, @StoreID, GETDATE(), '1', GETDATE(), '1'),
	(128, 60, 1, '00:00:00.0000000', 0, 1, @StoreID, GETDATE(), '1', GETDATE(), '1'),
	(29, 60, 1, '00:00:00.0000000', 0, 1, @StoreID, GETDATE(), '1', GETDATE(), '1'),
	(129, 60, 1, '00:00:00.0000000', 0, 1, @StoreID, GETDATE(), '1', GETDATE(), '1')

--------------------------------

    DELETE	#ControlTable1
    WHERE	StoreId = @StoreId

END

DROP TABLE #ControlTable1

-------------------------------------------------------------