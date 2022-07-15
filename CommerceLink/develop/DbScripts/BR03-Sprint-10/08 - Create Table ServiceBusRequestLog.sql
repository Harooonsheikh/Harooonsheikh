IF (OBJECT_ID('ServiceBusRequestLog') IS NULL)
BEGIN
	CREATE TABLE ServiceBusRequestLog (
		Id BIGINT PRIMARY KEY IDENTITY(1, 1),
		StoreId INT,
		EcomTransactionId UNIQUEIDENTIFIER,
		Status BIT,
		MethodName NVARCHAR(500),
		CreatedBy NVARCHAR(100),
		CreatedOn DATETIME,
		ModifiedBy NVARCHAR(100),
		ModifiedOn DATETIME
	)
END