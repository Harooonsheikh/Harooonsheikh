Executed the SQL Scripts in order of prefixed number to create CommerceLink database with default settings for only Netherland store. Name and details of the SQL script files is as follow:

000 - CL DB NLD DB:
	Create empty CommerceLink database with name "CLTVD10"

001 - CL DB NLD Schema
	Create Tables, Views and Stored Procedures in CommerceLink database that was created earlier by the name "CLTVD10"

002 - CL DB NLD Data
	Insert Netherland store data in CommerceLink database that was created earlier by the name "CLTVD10"

Check and update the connection string in corresponding projects for following files to establish database connection with newly created database
1. App.config
2. Web.config
3. NLog.config
