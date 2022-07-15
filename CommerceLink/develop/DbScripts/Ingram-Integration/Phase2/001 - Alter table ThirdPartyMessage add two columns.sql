IF NOT EXISTS(	SELECT		*
				FROM		sys.tables t
				INNER JOIN	sys.columns c ON t.object_id = c.object_id
				WHERE		t.name = 'ThirdPartyMessage'
				AND			c.name = 'AssetId')
BEGIN
	ALTER TABLE ThirdPartyMessage
		ADD AssetId NVARCHAR(100);
END

IF NOT EXISTS(	SELECT		*
				FROM		sys.tables t
				INNER JOIN	sys.columns c ON t.object_id = c.object_id
				WHERE		t.name = 'ThirdPartyMessage'
				AND			c.name = 'OrderType')
BEGIN
	ALTER TABLE ThirdPartyMessage
		ADD OrderType NVARCHAR(100);
END