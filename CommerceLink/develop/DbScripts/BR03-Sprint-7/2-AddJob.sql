IF NOT EXISTS(SELECT * FROM Job WHERE JobName =  'DataDeleteSync')
BEGIN
	DECLARE @JobId int = ((SELECT MAX(JOBID) FROM Job) + 1 )
	INSERT INTO Job ([JobID],[JobName],[Enabled],[JobTypeId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy])
	VALUES			(@JobId, 'DataDeleteSync', 1, 0, GETDATE(), 1, GETDATE(), 1)

	SELECT @JobId = JobID FROM Job WHERE JobName =  'DataDeleteSync'

	INSERT INTO JobSchedule ([JobId],[JobInterval],[IsRepeatable],[StartTime],[IsActive],[JobStatus],[StoreId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy])
	VALUES(@JobId, 180, 1, '00:00:00.0000000', 1, 1, 1, GETDATE(), 'uy', GETDATE(), 'uy')

END
