CREATE NONCLUSTERED INDEX [IX_Log_IdentityId_VSTS32764] ON [dbo].[Log]
(
	[IdentityId] ASC
)
INCLUDE ( 	[CreatedOn],
	[EventLevel],
	[CreatedBy],
	[MachineName],
	[EventMessage],
	[ErrorSource],
	[ErrorClass],
	[ErrorMethod],
	[ErrorMessage],
	[InnerErrorMessage],
	[ErrorModule],
	[StoreId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


