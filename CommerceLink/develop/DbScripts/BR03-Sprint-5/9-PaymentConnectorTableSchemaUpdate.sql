ALTER TABLE
	PaymentConnector
ADD
	ERPCreditCardProcessorName NVARCHAR(MAX),
	EComCreditCardProcessorName NVARCHAR(MAX);
GO

UPDATE	PaymentConnector
SET		ERPCreditCardProcessorName = 'Paypal',
		EComCreditCardProcessorName = 'PAYPAL_EXPRESS'
WHERE	PaymentConnectorId = 3;

UPDATE	PaymentConnector
SET		ERPCreditCardProcessorName = 'Wiredcard',
		EComCreditCardProcessorName = 'BASIC_CREDIT'
WHERE	PaymentConnectorId = 4;

UPDATE	PaymentConnector
SET		ERPCreditCardProcessorName = 'Adyen',
		EComCreditCardProcessorName = 'ADYEN_CC'
WHERE	PaymentConnectorId = 6;