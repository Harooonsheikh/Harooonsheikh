
IF NOT EXISTS (select * from MandatoryStateCountry WHERE ThreeLetterISORegionName = 'IND')
BEGIN
	INSERT INTO MandatoryStateCountry(ThreeLetterISORegionName,ShortName,FullName)
	VALUES('IND','INDIA','INDIA')
END
