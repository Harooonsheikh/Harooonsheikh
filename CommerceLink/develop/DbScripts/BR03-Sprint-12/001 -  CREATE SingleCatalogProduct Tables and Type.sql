
----- SingleCatalogProductDelta

IF NOT EXISTS (	SELECT	*
				FROM	sys.tables
				WHERE	[name] = 'SingleCatalogProductDelta'
				)
BEGIN
	CREATE TABLE SingleCatalogProductDelta (
		ProductRecId NVARCHAR(MAX),
		SKU NVARCHAR(MAX),
		StoreViewCode NVARCHAR(MAX),
		RowStatus NVARCHAR(MAX)
	);
END


----- SingleCatalogProductMaster

IF NOT EXISTS (	SELECT	*
				FROM	sys.tables
				WHERE	[name] = 'SingleCatalogProductMaster'
				)
BEGIN
	CREATE TABLE SingleCatalogProductMaster (
		SingleCatalogProductMasterId INT PRIMARY KEY IDENTITY(1, 1),
		ProductRecId NVARCHAR(MAX),
		SKU NVARCHAR(MAX),
		StoreViewCode NVARCHAR(MAX),
		ProductJson NVARCHAR(MAX),
		CreatedOn Datetime2,
		LastModifiedOn Datetime2
	)
END


----- SingleCatalogProduct

IF NOT EXISTS (	SELECT	*
				FROM	sys.tables
				WHERE	[name] = 'SingleCatalogProduct'
				)
BEGIN
	CREATE TABLE SingleCatalogProduct (
		SingleCatalogProductId INT PRIMARY KEY IDENTITY(1, 1),
		ProductRecId NVARCHAR(MAX),
		SKU NVARCHAR(MAX),
		StoreViewCode NVARCHAR(MAX),
		ProductJson NVARCHAR(MAX),
		CreatedOn Datetime2,
		LastModifiedOn Datetime2,
		RowStatus NVARCHAR(MAX)
	)
END


----- SingleCatalogProductDelta

IF NOT EXISTS (	SELECT	*
				FROM	sys.tables
				WHERE	[name] = 'SingleCatalogProductStaging'
				)
BEGIN
	CREATE TABLE SingleCatalogProductStaging (
		SingleCatalogProductStagingId BIGINT PRIMARY KEY IDENTITY(1, 1),
		ProductRecId NVARCHAR(MAX),
		SKU NVARCHAR(MAX),
		StoreViewCode NVARCHAR(MAX),
		ProductJson NVARCHAR(MAX),
		CreatedOn Datetime2,
		LastModifiedOn Datetime2
	)
END


----- SingleCatalogProductStagingType

IF type_id('[Dbo].[SingleCatalogProductStagingType]') IS NULL

CREATE TYPE SingleCatalogProductStagingType AS TABLE (
		ProductRecId NVARCHAR(MAX),
		SKU NVARCHAR(MAX),
		StoreViewCode NVARCHAR(MAX),
		ProductJson NVARCHAR(MAX)
)


