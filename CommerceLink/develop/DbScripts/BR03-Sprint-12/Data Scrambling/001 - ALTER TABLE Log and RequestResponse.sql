IF	(	NOT EXISTS (SELECT		*
					FROM		sys.tables T
					INNER JOIN	sys.columns C ON T.object_id = C.object_id
					WHERE		T.name = 'Log'
					AND			C.name = 'Scrambled')
		)
BEGIN 
	ALTER TABLE
		Log
			ADD Scrambled BIT;
END

-------------------------

IF	(	NOT EXISTS (SELECT		*
					FROM		sys.tables T
					INNER JOIN	sys.columns C ON T.object_id = C.object_id
					WHERE		T.name = 'RequestResponse'
					AND			C.name = 'Scrambled')
		)
BEGIN 
	ALTER TABLE
		RequestResponse
			ADD Scrambled BIT;
END

-------------------------

IF NOT EXISTS (
				SELECT DATA_TYPE 
				FROM INFORMATION_SCHEMA.COLUMNS
				WHERE 
					 TABLE_NAME = 'RequestResponseIngram' 
				AND COLUMN_NAME = 'TotalProcessingDuration'
				AND DATA_TYPE = 'decimal'
)
BEGIN
	PRINT('Updating invalid total processing string to 0');
	-- Update empty / Invalid data to 0
	UPDATE RequestResponseIngram 
	SET
		TotalProcessingDuration = 0
	WHERE ISNUMERIC(TotalProcessingDuration) = 0;
	
	ALTER TABLE RequestResponseIngram
	ALTER COLUMN TotalProcessingDuration decimal(20,5);

	PRINT('TABLE RequestResponseIngram column TotalProcessingDuration data type changed from nvarchar to decimal(20,5)');
END
GO