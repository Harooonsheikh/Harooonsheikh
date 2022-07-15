UPDATE	ApplicationSetting
SET		[Value] = 'Invoiced,Canceled,Terminated'
WHERE	[Key] = 'SALESORDER.Statuses_For_Sync'

SELECT * FROM ApplicationSetting WHERE [Key] = 'SALESORDER.Statuses_For_Sync'