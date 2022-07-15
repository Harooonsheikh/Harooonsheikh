SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Aqeel Arshad Siddiqui
-- Create date: Wednesday, July 15, 2020
-- Description:	Insert data in SingleCatalogProductViewModel table
-- =============================================
CREATE OR ALTER PROCEDURE spSingleCatalogProductStaging_Insert
	-- Add the parameters for the stored procedure here
	@Products SingleCatalogProductStagingType READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO
		SingleCatalogProductStaging
		(	
			ProductRecId,
			SKU,
			StoreViewCode ,
			ProductJson ,
			CreatedOn
		)
	SELECT
		ProductRecId,
		SKU,
		StoreViewCode ,
		ProductJson ,
		GETUTCDATE()
	FROM
		@Products;
END
GO
