INSERT INTO EmailTemplate
	([EmailTemplateId], Name, subject, Body, Footer, IsActive, StoreId, CreatedOn)
SELECT
	8,
	'SalesOrderStatus',
	'Sales Order Status: {0} Failed on Commerce Link INT',
	'<h2>Hi {0}</h2>  <h4>{1}</h4>  <h3>Exception</h3>    <p>{2}</p>    <h3>{3}</h3>>',
	'Take Care.',
	1,
	StoreId,
	GETDATE()
FROM
	Store

INSERT INTO EmailTemplate
	([EmailTemplateId], NAme, subject, Body, Footer, IsActive, StoreId, CreatedOn)
SELECT
	31,
	'ThirdPartySalesOrder',
	'Third Party Sales Order: {0} Failed on Commerce Link INT',
	'<h2>Hi {0}</h2>  <h4>{1}</h4>  <h3>Exception</h3>    <p>{2}</p>    <h3>{3}</h3>>',
	'Take Care.',
	1,
	StoreId,
	GETDATE()
FROM
	Store
	
