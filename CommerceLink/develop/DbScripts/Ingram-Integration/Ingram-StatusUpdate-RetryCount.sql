IF NOT EXISTS( SELECT		*
					FROM		[sys].[schemas] [s]
					INNER JOIN	[sys].[tables] [t] ON [s].[schema_id] = [t].[schema_id]
					INNER JOIN	[sys].[columns] [c] ON [t].[object_id] = [c].[object_id]
					WHERE		[s].[name] = 'dbo'
					AND			[t].[name] = 'ThirdPartyMessage'
					AND			[c].[name] = 'RetryCount')
BEGIN
		PRINT 'RetryCount column added in ThirdPartyMessage table successfully';
		alter table ThirdPartyMessage
		Add RetryCount tinyint default(0) not null
END
ELSE
BEGIN
PRINT 'RetryCount already exists in ThirdPartyMessage table';
END
GO