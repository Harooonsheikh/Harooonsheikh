IF EXISTS (SELECT * FROM ApplicationSetting WHERE [Key] = 'SALESORDER.Statuses_For_Sync' AND Value = 'Invoiced,Canceled,Terminated')
BEGIN
	UPDATE ApplicationSetting
	SET
		[Value] = 'Invoiced,Canceled,Terminated,Renewal'
	WHERE [Key] = 'SALESORDER.Statuses_For_Sync'
END