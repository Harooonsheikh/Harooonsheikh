IF (NOT EXISTS(SELECT * FROM LogMapper WHERE MethodName = 'UpdateSubscriptionContract'))
BEGIN
	INSERT INTO
		LogMapper
			(MethodName, IdentifierKey, IdentifierPath)
		VALUES
			('UpdateSubscriptionContract', 'PrimaryPacLicense', '$.PrimaryPacLicense')
END
ELSE
BEGIN
	UPDATE	LogMapper
	SET		IdentifierKey = 'PrimaryPacLicense',
			IdentifierPath = '$.PrimaryPacLicense'
	WHERE	MethodName = 'UpdateSubscriptionContract'
END