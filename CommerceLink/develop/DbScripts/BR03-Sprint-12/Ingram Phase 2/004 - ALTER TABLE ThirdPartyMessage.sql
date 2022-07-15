IF NOT EXISTS(	SELECT	*
				FROM	sys.tables T
				INNER JOIN	sys.columns C ON T.object_id = C.object_id
				WHERE		T.name = 'ThirdPartyMessage'
				AND			C.name = 'SalesId'
				)
BEGIN
	ALTER TABLE	ThirdPartyMessage
	ADD			SalesId NVARCHAR(50);
END