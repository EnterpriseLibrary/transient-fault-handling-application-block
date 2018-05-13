IF EXISTS (SELECT * FROM sysdatabases WHERE NAME = 'TransientFaultHandlingTest')
BEGIN
	ALTER DATABASE TransientFaultHandlingTest
	SET SINGLE_USER
	WITH ROLLBACK IMMEDIATE;
	
	DROP DATABASE TransientFaultHandlingTest
END

GO