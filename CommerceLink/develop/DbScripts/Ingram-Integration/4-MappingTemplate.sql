IF NOT EXISTS(	SELECT		*
				FROM		sys.tables t
				INNER JOIN	sys.columns c ON t.object_id = c.object_id
				WHERE		t.name = 'MappingTemplate'
				AND			c.name = 'IngramTemplate')
BEGIN
	ALTER TABLE MappingTemplate
		ADD IngramTemplate [XML]
END

IF NOT EXISTS(	SELECT		*
				FROM		sys.tables t
				INNER JOIN	sys.columns c ON t.object_id = c.object_id
				WHERE		t.name = 'ThirdPartyMessage'
				AND			c.name = 'Currency')
BEGIN
	ALTER TABLE ThirdPartyMessage
		ADD Currency NVARCHAR(50)
END