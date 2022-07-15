
BEGIN TRANSACTION
	select * from ConfigurableObject
	where EntityType = 7


	update ConfigurableObject
	set
		ComValue = REPLACE(ComValue, '_', '-')
	where EntityType = 7

	select * from ConfigurableObject
	where EntityType = 7

--TODO: remove Rollback and enable commit in below script after validation.
ROLLBACK TRAN 
--COMMIT TRAN -- Transaction Success!
