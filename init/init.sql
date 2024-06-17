USE [master];
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TodoList')
BEGIN
  CREATE DATABASE TodoList;
END;
GO

USE [TodoList];
GO

IF NOT EXISTS (SELECT * FROM sys.sql_logins WHERE name = 'todo_user')
BEGIN
    CREATE LOGIN [todo_user] WITH PASSWORD = 'Passw0rd!23', CHECK_POLICY = OFF, DEFAULT_DATABASE = [TodoList];
    CREATE USER [todo_user] FOR LOGIN [todo_user] WITH DEFAULT_SCHEMA=[dbo]
    ALTER ROLE [db_owner] ADD MEMBER [todo_user];
END
GO
