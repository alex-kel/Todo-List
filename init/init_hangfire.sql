USE [master];
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Hangfire')
BEGIN
  CREATE DATABASE Hangfire;
END;
GO

USE [Hangfire];
GO

IF NOT EXISTS (SELECT * FROM sys.sql_logins WHERE name = 'hangfire_user')
BEGIN
    CREATE LOGIN [hangfire_user] WITH PASSWORD = 'Passw0rd!23', CHECK_POLICY = OFF, DEFAULT_DATABASE = [Hangfire];
    CREATE USER [hangfire_user] FOR LOGIN [hangfire_user] WITH DEFAULT_SCHEMA=[dbo]
    ALTER ROLE [db_owner] ADD MEMBER [hangfire_user];
END
GO
