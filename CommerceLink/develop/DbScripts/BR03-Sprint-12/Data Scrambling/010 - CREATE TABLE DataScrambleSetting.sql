IF (OBJECT_ID('DataScrambleSetting') IS NULL)
BEGIN
	CREATE TABLE DataScrambleSetting (
		DataScrambleSettingId INT PRIMARY KEY IDENTITY(1, 1),
		DataScrambleSettingName NVARCHAR(100),
		Pattern NVARCHAR(MAX),
		Seperator NVARCHAR(10),
		ScrambledData NVARCHAR(MAX),
		TableName NVARCHAR(1000),
		CreatedBy NVARCHAR(50),
		CreatedOn DATETIME,
		ModifiedBy NVARCHAR(50),
		ModifiedOn DATETIME
	)
END