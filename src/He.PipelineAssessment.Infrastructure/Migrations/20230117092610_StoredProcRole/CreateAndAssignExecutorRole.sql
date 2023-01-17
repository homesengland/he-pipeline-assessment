IF NOT EXISTS(SELECT top 1 * FROM sys.database_principals  WHERE [type] = N''R'' AND [name] = ''db_executor'')
BEGIN
	CREATE ROLE db_executor
	GRANT EXECUTE TO db_executor
END

IF EXISTS(SELECT TOP 1 * FROM sys.database_principals  WHERE [type] = N''E'' AND [name] like ''mid-pipasmt-%-01'')
BEGIN
	DECLARE @UserName as varchar(100)
	SET @UserName = (SELECT TOP 1 [name] FROM sys.database_principals  WHERE [type] = N''E'' AND [name] like ''mid-pipasmt-%-01'')
	EXEC sp_addrolemember ''db_executor'', @UserName
END
