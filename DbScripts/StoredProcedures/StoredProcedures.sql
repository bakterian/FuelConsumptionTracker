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
		,[Manufucaturer]
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
	FROM [dbo].[CarDescription]
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
	FROM [dbo].[CarDescription]
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
		,[PetrolStationDesc]
		,[PetrolType]
		,[FuelingDate] 
		,[LiterAmount]
		,[PricePerLiter]
		,[FullPrice]
		,[DistanceMade]
		,[FuelConsumption]
		,[Terrain]
	FROM [dbo].[FuelConsumption]
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
		,[PetrolStationDesc]
		,[PetrolType]
		,[FuelingDate] 
		,[LiterAmount]
		,[PricePerLiter]
		,[FullPrice]
		,[DistanceMade]
		,[FuelConsumption]
		,[Terrain]
	FROM [dbo].[FuelConsumption]
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
	@Manufucaturer [nvarchar](100),
	@Model [nvarchar](100),
	@HorsePower [int],
	@EngineSize [int],
	@PetrolType [nvarchar](100),
	@FuelTankSize [int],
	@Weight [int],
	@TopSpeed [float],
	@Acceleration [float],
	@AvgFuelConsumption [float],		
	@ProductionYear [int]
)
AS
BEGIN
	SET NOCOUNT ON;	
	
	IF EXISTS (Select 1 from  [dbo].[CarDescription] where Description = @Description )
	
	UPDATE  [CarDescription]  SET
	Manufucaturer = @Manufucaturer,
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
	WHERE Description = @Description

	ELSE

	INSERT INTO  [dbo].[CarDescription] 
	(
	   [Description]
	   ,[Manufucaturer]
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
	@Manufucaturer,
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
	@PetrolStationDesc [nvarchar](100),
	@PetrolType [nvarchar](50),
	@FuelingDate [datetime],
	@LiterAmount [float],
	@PricePerLiter [float],
	@FullPrice [float],
	@DistanceMade [float],
	@FuelConsumption [float],
	@Terrain [nvarchar](100)
)
AS
BEGIN
	SET NOCOUNT ON;	
	
	IF EXISTS (Select 1 from  [dbo].[CarDescription] where Id = @CarId )
	
	INSERT INTO  [dbo].[FuelConsumption] 
	(
		[CarId]
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
	@DistanceMade [float]
)
AS
BEGIN
	SET NOCOUNT ON;	
	
	IF EXISTS (Select 1 from  [dbo].[FuelConsumption] where Id = @Id )
	
	UPDATE  [FuelConsumption]  SET
	DistanceMade = @DistanceMade
	WHERE Id = @Id
END
GO