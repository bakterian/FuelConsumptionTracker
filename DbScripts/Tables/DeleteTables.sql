USE [CarData]
GO

IF OBJECT_ID('[dbo].[FuelConsumptions]', 'U') IS NOT NULL
	BEGIN
		DROP TABLE [dbo].[FuelConsumptions]
	END
GO

IF OBJECT_ID('[dbo].[CarDescriptions]', 'U') IS NOT NULL
	BEGIN
		DROP TABLE [dbo].[CarDescriptions]
	END
GO
