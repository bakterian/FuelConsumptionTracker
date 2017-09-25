USE [CarData]
GO

IF OBJECT_ID('[dbo].[FuelConsumption]', 'U') IS NOT NULL
	BEGIN
		DROP TABLE [dbo].[FuelConsumption]
	END
GO

IF OBJECT_ID('[dbo].[CarDescription]', 'U') IS NOT NULL
	BEGIN
		DROP TABLE [dbo].[CarDescription]
	END
GO
