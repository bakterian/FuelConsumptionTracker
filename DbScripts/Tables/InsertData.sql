USE [CarData]
GO

if (not exists (select top 1 * from [dbo].[CarDescription] where Description = 'TestVehicle1'))
INSERT INTO [dbo].[CarDescription]
           ([Description]
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
		   ,[ProductionYear])
     VALUES
           ('TestVehicle1'
           ,'Dodge'
           ,'Charger'
           ,431
           ,6100
		   ,'gasoline'
           ,70
           ,1785
		   ,265.0
		   ,4.9
		   ,19.2
           ,2005);
		   
if (not exists (select top 1 * from [dbo].[CarDescription] where Description = 'TestVehicle2'))
INSERT INTO [dbo].[CarDescription]
           ([Description]
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
		   ,[ProductionYear])
     VALUES
           ('TestVehicle2'
           ,'Citroen'
           ,'2CV'
           ,16
           ,425
		   ,'gasoline'
           ,20
           ,560
		   ,85.0
		   ,35.0
		   ,4.4
           ,1963);

if (not exists (select top 1 * from [dbo].[CarDescription] where Description = 'TestVehicle3'))
INSERT INTO [dbo].[CarDescription]
           ([Description]
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
		   ,[ProductionYear])
     VALUES
           ('TestVehicle3'
           ,'Mazda'
           ,'RX8'
           ,213
           ,1308
		   ,'gasoline'
           ,60
           ,1500
		   ,220
		   ,5
		   ,19
           ,2009);
		   
if (not exists (select top 1 * from [dbo].[FuelConsumption] where PetrolStationDesc = 'Shell 1'))
INSERT INTO [dbo].[FuelConsumption]
           ([CarId]
		   ,[PetrolStationDesc]
           ,[PetrolType]
           ,[FuelingDate]
		   ,[LiterAmount]
		   ,[PricePerLiter]
           ,[FullPrice]
           ,[DistanceMade]
		   ,[FuelConsumption] 
		   ,[Terrain])
     VALUES
           (1
           ,'Shell 1'
           ,'gasoline'
           ,'2016-05-16'
           ,16
		   ,2
           ,32
           ,80
		   ,15
		   ,'city');
		   
if (not exists (select top 1 * from [dbo].[FuelConsumption] where PetrolStationDesc = 'Shell 2'))
INSERT INTO [dbo].[FuelConsumption]
           ([CarId]
		   ,[PetrolStationDesc]
           ,[PetrolType]
           ,[FuelingDate]
		   ,[LiterAmount]
		   ,[PricePerLiter]
           ,[FullPrice]
           ,[DistanceMade]
		   ,[FuelConsumption] 
		   ,[Terrain])
     VALUES
           (1
           ,'Shell 2'
           ,'gasoline'
           ,'2016-06-02'
           ,30
		   ,3
           ,90
           ,200
		   ,15
		   ,'highway');
		   
if (not exists (select top 1 * from [dbo].[FuelConsumption] where PetrolStationDesc = 'Lukoil 1'))
INSERT INTO [dbo].[FuelConsumption]
           ([CarId]
		   ,[PetrolStationDesc]
           ,[PetrolType]
           ,[FuelingDate]
		   ,[LiterAmount]
		   ,[PricePerLiter]
           ,[FullPrice]
           ,[DistanceMade]
		   ,[FuelConsumption] 
		   ,[Terrain])
     VALUES
           (2
           ,'Lukoil 1'
           ,'gasoline'
           ,'1973-12-23'
           ,9
		   ,3
           ,27
           ,200
		   ,4.5
		   ,'city');
		   
if (not exists (select top 1 * from [dbo].[FuelConsumption] where PetrolStationDesc = 'Lukoil 2'))
INSERT INTO [dbo].[FuelConsumption]
           ([CarId]
		   ,[PetrolStationDesc]
           ,[PetrolType]
           ,[FuelingDate]
		   ,[LiterAmount]
		   ,[PricePerLiter]
           ,[FullPrice]
           ,[DistanceMade]
		   ,[FuelConsumption] 
		   ,[Terrain])
     VALUES
           (2
           ,'Lukoil 2'
           ,'gasoline'
           ,'1974-01-27'
           ,20
		   ,2.5
           ,50
           ,400
		   ,5
		   ,'highway');