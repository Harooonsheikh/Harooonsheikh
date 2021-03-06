/****** Object:  StoredProcedure [dbo].[spSingleCatalogProduct_CompareAndUpdate]    Script Date: 17/07/2020 10:30:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Aqeel Arshad Siddiqui
-- Create date: Wednesday, July 15, 2020
-- Description: Compare data in tables SingleCatalogProduct and SingleCatalogProductStaging and select difference
-- =============================================
CREATE OR ALTER  PROCEDURE [dbo].[spSingleCatalogProduct_CompareAndUpdate]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	TRUNCATE TABLE SingleCatalogProductDelta;

	IF NOT EXISTS (	SELECT	*
					FROM	[dbo].[SingleCatalogProduct]
					)
	BEGIN
		-- Code here
		--1. Add complete data in SingleCatalogProduct TAble
		INSERT INTO
			[dbo].[SingleCatalogProduct]
				(
				ProductRecId,
				SKU,
				StoreViewCode ,
				ProductJson ,
				CreatedOn ,
				LastModifiedOn,
				RowStatus
				)
		SELECT
				ProductRecId,
				SKU,
				StoreViewCode ,
				ProductJson ,
				GETUTCDATE() ,
				LastModifiedOn,
				'NEW'
		FROM
			[dbo].[SingleCatalogProductStaging];

		--2. Add caomplete data in singleCatalogProductMaster Table
		INSERT INTO
			[dbo].[SingleCatalogProductMaster]
				(
					ProductRecId,
					SKU,
					StoreViewCode ,
					ProductJson ,
					CreatedOn ,
					LastModifiedOn
					
				)
		SELECT
				ProductRecId,
				SKU,
				StoreViewCode ,
				ProductJson ,
				GETUTCDATE() ,
				LastModifiedOn
		FROM
			[dbo].[SingleCatalogProductStaging];

		--3. Select complete data in @Delta to return
		INSERT INTO
			[SingleCatalogProductDelta]
				(ProductRecId,
				SKU,
				StoreViewCode,
				RowStatus
				)
		SELECT
			ProductRecId,
			SKU,
			StoreViewCode,
			'NEW'
		FROM
			[dbo].[SingleCatalogProductStaging];
	END
	ELSE
	BEGIN
		DECLARE
			@RunInsert AS BIT = 1,
			@RunDelete AS BIT = 1,
			@RunUpdate AS BIT = 1;

		IF (@RunDelete = 1) BEGIN /* Mark removed products as 'DELETED */

			INSERT INTO
				[SingleCatalogProductDelta]
					(ProductRecId, SKU, StoreViewCode, RowStatus)
			SELECT
				[Original].[ProductRecId],
				[Original].[SKU],
				[Original].[StoreViewCode],
				'DELETED'
			FROM	[dbo].[SingleCatalogProduct] [Original]
			WHERE	NOT EXISTS	(	SELECT	1
									FROM	[dbo].[SingleCatalogProductStaging] [Staging] 
									WHERE	[Staging].[ProductRecId] = [Original].[ProductRecId]
									AND		[Staging].[SKU] = [Original].[SKU]
									AND		ISNULL([Staging].[StoreViewCode], '') = ISNULL([Original].[StoreViewCode], '')
									)
			AND		[Original].[RowStatus] != 'DELETED';

			UPDATE	[Original]
			SET		[RowStatus] = 'DELETED'
			FROM	[dbo].[SingleCatalogProduct] [Original]
			WHERE	NOT EXISTS	(	SELECT	1
									FROM	[dbo].[SingleCatalogProductStaging] [Staging] 
									WHERE	[Staging].[ProductRecId] = [Original].[ProductRecId]
									AND		[Staging].[SKU] = [Original].[SKU]
									AND		ISNULL([Staging].[StoreViewCode], '') = ISNULL([Original].[StoreViewCode], '')
									)
			AND		[Original].[RowStatus] != 'DELETED';

		 END

		IF (@RunInsert = 1) BEGIN /* Insert new data in SingleCatalogProduct */

			INSERT INTO [SingleCatalogProductDelta]
						(ProductRecId, SKU, StoreViewCode, RowStatus)
			SELECT
				[Staging].[ProductRecId],
				[Staging].[SKU],
				[Staging].[StoreViewCode],
				'NEW'
			FROM	[dbo].[SingleCatalogProductStaging] [Staging]
			WHERE	NOT EXISTS (	SELECT	1
									FROM	[dbo].[SingleCatalogProduct] [Original]
									WHERE	[Staging].[ProductRecId] = [Original].[ProductRecId]
									AND		[Staging].[SKU] = [Original].[SKU]
									AND		ISNULL([Staging].[StoreViewCode], '') = ISNULL([Original].[StoreViewCode], '')
									);

			INSERT INTO
				[dbo].[SingleCatalogProduct]
					(
					ProductRecId,
					SKU,
					StoreViewCode ,
					ProductJson ,
					CreatedOn ,
					LastModifiedOn,
					[RowStatus]
					)
			SELECT	ProductRecId,
					SKU,
					StoreViewCode ,
					ProductJson ,
					GETUTCDATE() ,
					LastModifiedOn,
					'NEW' AS [RowStatus]
			FROM	[dbo].[SingleCatalogProductStaging] [Staging]
			WHERE	NOT EXISTS (	SELECT	1
									FROM	[dbo].[SingleCatalogProduct] [Original]
									WHERE	[Staging].[ProductRecId] = [Original].[ProductRecId]
									AND		[Staging].[SKU] = [Original].[SKU]
									AND		ISNULL([Staging].[StoreViewCode], '') = ISNULL([Original].[StoreViewCode], '')
									);

		END

		IF (@RunUpdate = 1) BEGIN /* Update changed products */

			INSERT INTO
				[SingleCatalogProductDelta]
					(ProductRecId, SKU, StoreViewCode, RowStatus)
			SELECT  [Staging].[ProductRecId],
						[Staging].[SKU],
						[Staging].[StoreViewCode],
						'MODIFIED'
				FROM	[dbo].[SingleCatalogProductStaging] [Staging]
				INNER JOIN	[dbo].[SingleCatalogProduct] [Original]
						ON	[Original].[ProductRecId] = [Staging].[ProductRecId] AND
							[Original].[SKU] = [Staging].[SKU] AND
							[Original].[StoreViewCode] = [Staging].[StoreViewCode] AND 
							[Original].[RowStatus] != 'DELETED' AND
							[Original].[ProductJson] <> [Staging].[ProductJson];

			UPDATE		[Original]
				SET		[Original].[ProductJson] = [Staging].[ProductJson],
						[LastModifiedOn] = GETUTCDATE(),
						[RowStatus] = 'MODIFIED'
			FROM		[dbo].[SingleCatalogProductStaging] [Staging]
			INNER JOIN	[dbo].[SingleCatalogProduct] [Original] 
					ON	[Original].[ProductRecId] = [Staging].[ProductRecId] AND 
						[Staging].[SKU] = [Original].[SKU] AND 
						[Staging].[StoreViewCode] = [Original].[StoreViewCode] AND 
						[Original].[ProductJson] <> [Staging].[ProductJson]


		END

	END

END
GO


