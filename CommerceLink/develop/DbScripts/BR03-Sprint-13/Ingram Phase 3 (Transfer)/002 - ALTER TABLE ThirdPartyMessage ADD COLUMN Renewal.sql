IF NOT EXISTS(	SELECT		*
				FROM		sys.tables [t]
				INNER JOIN	sys.columns [c] on [t].object_id = [c].object_id
				WHERE		[t].name = 'ThirdPartyMessage'
				AND			[c].name = 'RenewalDate'
				)
BEGIN
	ALTER TABLE [ThirdPartyMessage]
		ADD [RenewalDate] DATETIME DEFAULT NULL;
END

IF NOT EXISTS(	SELECT		*
				FROM		sys.tables [t]
				INNER JOIN	sys.columns [c] on [t].object_id = [c].object_id
				WHERE		[t].name = 'ThirdPartyMessage'
				AND			[c].name = 'PacLicense'
				)
BEGIN
	ALTER TABLE [ThirdPartyMessage]
		ADD [PacLicense] NVARCHAR(50) DEFAULT NULL;
END


IF NOT EXISTS(	SELECT		*
				FROM		sys.tables [t]
				INNER JOIN	sys.columns [c] on [t].object_id = [c].object_id
				WHERE		[t].name = 'ThirdPartyMessage'
				AND			[c].name = 'Description'
				)
BEGIN
	ALTER TABLE [ThirdPartyMessage]
		ADD [Description] NVARCHAR(500) DEFAULT NULL;
END
