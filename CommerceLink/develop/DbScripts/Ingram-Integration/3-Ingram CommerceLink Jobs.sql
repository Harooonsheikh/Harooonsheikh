DECLARE @StoreId INT = 0;

SELECT	@StoreId = StoreId
FROM	Store
WHERE	[Name] = 'Ingram';

/* Begin: Add New Jobs */
IF NOT EXISTS(SELECT * FROM Job WHERE JobName =  'DownloadThirdPartySalesOrderSync')
BEGIN
	DECLARE @JobId int = ((SELECT MAX(JOBID) FROM Job) + 1 )
	INSERT INTO Job ([JobID],[JobName],[Enabled],[JobTypeId],[CreatedOn],[CreatedBy])
	VALUES			(@JobId, 'DownloadThirdPartySalesOrderSync', 1, 0, GETDATE(), 1)

	SELECT @JobId = JobID FROM Job WHERE JobName =  'DownloadThirdPartySalesOrderSync'

	INSERT INTO JobSchedule ([JobId],[JobInterval],[IsRepeatable],[StartTime],[IsActive],[JobStatus],[StoreId],[CreatedOn],[CreatedBy])
	VALUES(@JobId, 5, 1, '00:00:00.0000000', 1, 1, @StoreId, GETDATE(), 'System')
END

IF NOT EXISTS(SELECT * FROM Job WHERE JobName =  'SyncSalesOrderStatus')
BEGIN
	SET @JobId = ((SELECT MAX(JOBID) FROM Job) + 1 )
	INSERT INTO Job ([JobID],[JobName],[Enabled],[JobTypeId],[CreatedOn],[CreatedBy])
	VALUES			(@JobId, 'SyncSalesOrderStatus', 1, 0, GETDATE(), 1)

	SELECT @JobId = JobID FROM Job WHERE JobName =  'SyncSalesOrderStatus'

	INSERT INTO JobSchedule ([JobId],[JobInterval],[IsRepeatable],[StartTime],[IsActive],[JobStatus],[StoreId],[CreatedOn],[CreatedBy])
	VALUES(@JobId, 5, 1, '00:00:00.0000000', 1, 1, @StoreId, GETDATE(), 'System')
END
/* End: Add new Job */

/* Begin: Enable Jobs */

UPDATE	Job
SET		Enabled = 1
WHERE	JobName IN ('SyncSalesOrderStatus', 'DownloadThirdPartySalesOrderSync', 'SalesOrderSyncJob')

/* End: Enable Jobs */

/* Begin: Set Job Schedules */

UPDATE		JobSchedule
SET			IsActive = 0
FROM		Job J
INNER JOIN	JobSchedule JS ON J.JobId = JS.JobId
WHERE		JS.StoreId = @StoreId
AND			J.JobName NOT IN ('SyncSalesOrderStatus', 'DownloadThirdPartySalesOrderSync', 'SalesOrderSyncJob')

UPDATE		JobSchedule
SET			IsActive = 1
FROM		Job J
INNER JOIN	JobSchedule JS ON J.JobId = JS.JobId
WHERE		JS.StoreId = @StoreId
AND			J.JobName IN ('SyncSalesOrderStatus', 'DownloadThirdPartySalesOrderSync', 'SalesOrderSyncJob')

/* End: Set Job Schedules */