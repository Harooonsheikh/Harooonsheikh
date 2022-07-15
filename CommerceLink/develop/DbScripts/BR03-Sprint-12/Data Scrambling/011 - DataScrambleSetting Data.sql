INSERT INTO
	DataScrambleSetting
		(DataScrambleSettingName, Pattern, Seperator, ScrambledData, TableName, CreatedBy, CreatedOn)
	VALUES
		('Email Address', '%[a-z0-9]@[a-z0-9]%', '"', 'scrambledeamiladdress@emailaddress.com', 'RequestResponse', 'Aqeel', GETDATE()),
		('Email Address', '%Email:%[a-z0-9]@[a-z0-9]%', ' ', 'scrambledeamiladdress@emailaddress.com', 'Log', 'Aqeel', GETDATE()),
		('Address', '%"address1":"%"%', ',', '"address1":"0000, No Street, No Town"', 'RequestResponse', 'Aqeel', GETDATE()),
		('Address', '%"address1": "%"%', ',', '"address1": "0000, No Street, No Town"', 'RequestResponse', 'Aqeel', GETDATE()),
		('Address', '%"FullAddress":"%"%', ',', '"FullAddress":"0000, No Street, No Town"', 'RequestResponse', 'Aqeel', GETDATE()),
		('Address', '%"FullAddress": "%"%', ',', '"FullAddress": "0000, No Street, No Town"', 'RequestResponse', 'Aqeel', GETDATE()),
		('Street', '%"Street":"%"%', ',', '"Street":"0000, No Street, No Town"', 'RequestResponse', 'Aqeel', GETDATE()),
		('Street', '%"Street": "%"%', ',', '"Street": "0000, No Street, No Town"', 'RequestResponse', 'Aqeel', GETDATE()),
		('Street', '%"Street2":"%"%', ',', '"Street2":"0000, No Street, No Town"', 'RequestResponse', 'Aqeel', GETDATE()),
		('Street', '%"Street2": "%"%', ',', '"Street2": "0000, No Street, No Town"', 'RequestResponse', 'Aqeel', GETDATE()),
		('Phone Number', '%"phone":"%"%', ',', '"phone":"+12345678901"', 'RequestResponse', 'Aqeel', GETDATE()),
		('Phone Number', '%"phone": "%"%', ',', '"phone": "+12345678901"', 'RequestResponse', 'Aqeel', GETDATE())

SELECT * FROM DataScrambleSetting