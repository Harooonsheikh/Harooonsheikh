CREATE NONCLUSTERED INDEX [NonClusteredIndex-MSRecommendation-VSTS32764] ON [dbo].[IntegrationKey]
(
	[EntityId] ASC,
	[StoreId] ASC,
	[ErpKey] ASC,
	[ComKey] ASC
)
INCLUDE ( 	[CreatedBy],
	[CreatedOn],
	[Description],
	[ModifiedOn],
	[ModifiedBy]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


