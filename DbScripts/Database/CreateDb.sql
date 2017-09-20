USE [master]
GO

IF db_id('CarData') IS NULL 
BEGIN
    CREATE DATABASE [CarData]
END
GO