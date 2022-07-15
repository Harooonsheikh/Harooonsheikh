SELECT	StoreId
INTO	#ControlTable 
FROM	dbo.Store

DECLARE @StoreId INT

WHILE EXISTS (SELECT * FROM #ControlTable)
BEGIN

    SELECT	@StoreId = (	SELECT		TOP 1 StoreId
							FROM		#ControlTable
							ORDER BY	StoreId ASC)

-- Run for every store
-------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM PaymentMethod WHERE ECommerceValue = 'BOLETO' and StoreId = @StoreId)
BEGIN
	INSERT INTO PaymentMethod(ParentPaymentMethodId,ECommerceValue,ErpValue,HasSubMethod,ErpCode,IsPrepayment,StoreId,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsCreditCard,UsePaymentConnector,ServiceAccountId,PaymentConnectorId)
	VALUES(NULL,'BOLETO',101,0,'Boleto',0,@StoreId,GETDATE(),1,NULL,1,0,0,NULL,NULL)
END

--------------------------------

    DELETE	#ControlTable
    WHERE	StoreId = @StoreId

END

DROP TABLE #ControlTable

-------------------------------------------------------------