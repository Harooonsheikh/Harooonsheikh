/****** Object:  StoredProcedure [dbo].[GetCustomersByUpdatedAddressVSI]    Script Date: 8/9/2015 4:39:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kashif Ali
-- Create date: 5th June 2015
-- Description:	Fetches all customer whose addresses are updated after the provided timestamp
-- =============================================
-- exec [dbo].[GetCustomersByUpdatedAddressVSI] '2015-08-05 21:36:02.000', 40
CREATE PROCEDURE [dbo].[GetCustomersByUpdatedAddressVSI]
@Timestamp Datetime,
@CustGroup nvarchar(5)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT	C.RECID as CustomerId, C.EMAIL, C.FIRSTNAME, C.LASTNAME, C.MIDDLENAME, C.NAME, C.PHONE,  
			lpa.CITY, lpa.COUNTRYREGIONID, lpa.ZIPCODE, lpa.STREET, lpa.COUNTY, 
			lpa.STREETNUMBER, lpa.POSTBOX, lpa.DISTRICTNAME, lpa.[STATE],lpa.RECID
	FROM [ax].LOGISTICSPOSTALADDRESS lpa
		INNER JOIN [ax].DIRPARTYLOCATION dpl ON lpa.LOCATION = dpl.LOCATION
		INNER JOIN  [ax].DIRPARTYTABLE dpt ON dpl.PARTY = dpt.RECID AND dpt.INSTANCERELATIONTYPE = 2975
		INNER JOIN [crt].[CUSTOMERSVIEW] C ON dpt.RECID = C.PARTY
	WHERE GETUTCDATE() BETWEEN lpa.VALIDFROM AND lpa.VALIDTO
	AND lpa.MODIFIEDDATETIME > @Timestamp and CUSTGROUP=@CustGroup
    
END

GO
/****** Object:  StoredProcedure [dbo].[GetUpdatedCustomersVSI]    Script Date: 8/9/2015 4:39:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [dbo].[GetUpdatedCustomersVSI] '2015-07-30 17:53:28.457',40
CREATE PROCEDURE [dbo].[GetUpdatedCustomersVSI]
@Timestamp Datetime,
@CustGroup nvarchar(5)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT C.RECID as CUSTOMERID, C.ACCOUNTNUMBER, C.Name, C.FIRSTNAME, C.MIDDLENAME, C.LASTNAME, C.PARTYNUMBER 
	, C.PHONE, C.EMAIL, C.CUSTGROUP, C.TAXGROUP, C.VATNUM
	FROM [crt].[CUSTOMERSVIEW] C 
	INNER JOIN ax.DIRPARTYTABLE AS dpt ON C.PARTY = dpt.RECID
	Where C.INSTANCERELATIONTYPE = 2975 AND dpt.MODIFIEDDATETIME > @Timestamp  AND C.CUSTGROUP=@CustGroup 
END

GO
