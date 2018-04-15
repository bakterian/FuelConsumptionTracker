USE [CarData]
GO

IF OBJECT_ID('spGetAllCarDesciptions', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[spGetAllCarDesciptions]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[spGetAllCarDesciptions]
AS
BEGIN
	SET NOCOUNT ON;	
	SELECT [Id]
		,[Description]
		,[Manufacturer]
		,[Model]
		,[HorsePower] 
		,[EngineSize]
		,[PetrolType]
		,[FuelTankSize]
		,[Weight]
		,[TopSpeed]
		,[Acceleration]
		,[AvgFuelConsumption]
		,[ProductionYear]
	FROM [dbo].[CarDescriptions]
END
GO


IF OBJECT_ID('spGetAllCarDesciptionsShort', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[spGetAllCarDesciptionsShort]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[spGetAllCarDesciptionsShort]
AS
BEGIN
	SET NOCOUNT ON;	
	SELECT [Id]
		,[Description]
	FROM [dbo].[CarDescriptions]
END
GO



IF OBJECT_ID('spGetAllFuelConsumption', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[spGetAllFuelConsumption]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[spGetAllFuelConsumption]
AS
BEGIN
	SET NOCOUNT ON;	
	SELECT [Id]
		,[CarId]
		,[CarDescription]
		,[PetrolStationDesc]
		,[PetrolType]
		,[FuelingDate] 
		,[LiterAmount]
		,[PricePerLiter]
		,[FullPrice]
		,[DistanceMade]
		,[FuelConsumption]
		,[Terrain]
	FROM [dbo].[FuelConsumptions]
END
GO



IF OBJECT_ID('spGetFuelConsumptionByCarId', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[spGetFuelConsumptionByCarId]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[spGetFuelConsumptionByCarId]
(
	@CarId [int]
)
AS
BEGIN
	SET NOCOUNT ON;	
	SELECT [Id]
		,[CarId]
		,[CarDescription]
		,[PetrolStationDesc]
		,[PetrolType]
		,[FuelingDate] 
		,[LiterAmount]
		,[PricePerLiter]
		,[FullPrice]
		,[DistanceMade]
		,[FuelConsumption]
		,[Terrain]
	FROM [dbo].[FuelConsumptions]
	WHERE CarId = @CarId
END
GO



IF OBJECT_ID('spInsertCarDescription', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[spInsertCarDescription]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[spInsertCarDescription]
(
	@Description [nvarchar](100),
	@Manufacturer [nvarchar](100),
	@Model [nvarchar](100),
	@HorsePower [int],
	@EngineSize [int],
	@PetrolType [nvarchar](100),
	@FuelTankSize [int],
	@Weight [int],
	@TopSpeed [decimal](18,2),
	@Acceleration [decimal](18,2),
	@AvgFuelConsumption [decimal](18,2),		
	@ProductionYear [int]
)
AS
BEGIN
	IF NOT EXISTS (Select 1 from  [dbo].[CarDescriptions] where Description = @Description)

	INSERT INTO  [dbo].[CarDescriptions] 
	(
	   [Description]
	   ,[Manufacturer]
	   ,[Model]
	   ,[HorsePower]
	   ,[EngineSize]
	   ,[PetrolType]
	   ,[FuelTankSize]
	   ,[Weight]
	   ,[TopSpeed] 
	   ,[Acceleration]
	   ,[AvgFuelConsumption]
	   ,[ProductionYear]
	)

	VALUES
	(
	@Description,
	@Manufacturer,
	@Model,
	@HorsePower,
	@EngineSize,
	@PetrolType,
	@FuelTankSize,
	@Weight,
	@TopSpeed,
	@Acceleration,
	@AvgFuelConsumption,		
	@ProductionYear
	)
END
GO



IF OBJECT_ID('spDeleteCarDescription', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[spDeleteCarDescription]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[spDeleteCarDescription]
(
	@Id [int]
)
AS
BEGIN
	DELETE FROM [dbo].[CarDescriptions]
	WHERE Id = @Id;
END
GO



IF OBJECT_ID('spUpdateCarDescription', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[spUpdateCarDescription]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[spUpdateCarDescription]
(
	@Id [int],
	@Description [nvarchar](100),
	@Manufacturer [nvarchar](100),
	@Model [nvarchar](100),
	@HorsePower [int],
	@EngineSize [int],
	@PetrolType [nvarchar](100),
	@FuelTankSize [int],
	@Weight [int],
	@TopSpeed [decimal](18,2),
	@Acceleration [decimal](18,2),
	@AvgFuelConsumption [decimal](18,2),		
	@ProductionYear [int]
)
AS
BEGIN
	IF EXISTS (Select 1 from  [dbo].[CarDescriptions] where Id = @Id )
	
	UPDATE  [CarDescriptions]  SET
	Description = @Description,
	Manufacturer = @Manufacturer,
	Model = @Model,
	HorsePower = @HorsePower,
	EngineSize = @EngineSize,
	PetrolType = @PetrolType,
	FuelTankSize = @FuelTankSize,
	Weight = @Weight,
	TopSpeed = @TopSpeed,
	Acceleration = @Acceleration,
	AvgFuelConsumption = @AvgFuelConsumption,		
	ProductionYear = @ProductionYear
	WHERE Id = @Id
	
END
GO



IF OBJECT_ID('spInsertFuelConsumption', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[spInsertFuelConsumption]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[spInsertFuelConsumption]
(
	@CarId [int],
	@CarDescription [nvarchar](100),
	@PetrolStationDesc [nvarchar](100),
	@PetrolType [nvarchar](50),
	@FuelingDate [date],
	@LiterAmount [decimal](18,2),
	@PricePerLiter [decimal](18,2),
	@FullPrice [decimal](18,2),
	@DistanceMade [decimal](18,2),
	@FuelConsumption [decimal](18,2),
	@Terrain [nvarchar](100)
)
AS
BEGIN
	IF EXISTS (Select 1 from  [dbo].[CarDescriptions] where Id = @CarId )
	
	INSERT INTO  [dbo].[FuelConsumptions] 
	(
		[CarId]
	   ,[CarDescription]
	   ,[PetrolStationDesc]
	   ,[PetrolType]
	   ,[FuelingDate]
	   ,[LiterAmount]
	   ,[PricePerLiter]
	   ,[FullPrice]
	   ,[DistanceMade]
	   ,[FuelConsumption] 
	   ,[Terrain]
	)

	VALUES
	(
	@CarId,
	@CarDescription,
	@PetrolStationDesc,
	@PetrolType,
	@FuelingDate,
	@LiterAmount,
	@PricePerLiter,
	@FullPrice,
	@DistanceMade,
	@FuelConsumption,
	@Terrain
	)
END
GO



IF OBJECT_ID('spUpdateFuelConsumption', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[spUpdateFuelConsumption]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[spUpdateFuelConsumption]
(
	@Id [int],
	@CarId [int],
	@CarDescription [nvarchar](100),
	@PetrolStationDesc [nvarchar](100),
	@PetrolType [nvarchar](50),
	@FuelingDate [date],
	@LiterAmount [decimal](18,2),
	@PricePerLiter [decimal](18,2),
	@FullPrice [decimal](18,2),
	@DistanceMade [decimal](18,2),
	@FuelConsumption [decimal](18,2),
	@Terrain [nvarchar](100)
)
AS
BEGIN
	IF EXISTS (Select 1 from  [dbo].[FuelConsumptions] where Id = @Id )
	
	UPDATE  [FuelConsumptions]  SET
	CarId = @CarId,
	CarDescription=	@CarDescription,
	PetrolStationDesc= @PetrolStationDesc,
	PetrolType = @PetrolType,
	FuelingDate= @FuelingDate,
	LiterAmount= @LiterAmount,
	PricePerLiter = @PricePerLiter ,
	FullPrice = @FullPrice,
	DistanceMade = @DistanceMade,
	FuelConsumption = @FuelConsumption,
	Terrain = @Terrain
	WHERE Id = @Id
END
GO



IF OBJECT_ID('spDeleteFuelConsumption', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[spDeleteFuelConsumption]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[spDeleteFuelConsumption]
(
	@Id [int]
)
AS
BEGIN
	DELETE FROM [dbo].[FuelConsumptions]
	WHERE Id = @Id;
END
GO