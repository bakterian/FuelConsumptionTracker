USE [CarData]
GO

IF OBJECT_ID('[dbo].[CarDescriptions]', 'U') IS NULL
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	BEGIN
		CREATE TABLE [dbo].[CarDescriptions](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Description] [nvarchar](100)  NOT NULL,
		[Manufacturer] [nvarchar](100)  NOT NULL,
		[Model] [nvarchar](100)  NOT NULL,
		[HorsePower] [int] NOT NULL,
		[EngineSize] [int] NOT NULL,
		[PetrolType] [nvarchar](100)  NOT NULL,
		[FuelTankSize] [int] NOT NULL,
		[Weight] [int] NOT NULL,
		[TopSpeed] [decimal](18,2) NOT NULL,
		[Acceleration] [decimal](18,2) NOT NULL,
		[AvgFuelConsumption] [decimal](18,2) NOT NULL,		
		[ProductionYear] [int] NOT NULL,
		CONSTRAINT [PK_CarDescription] PRIMARY KEY CLUSTERED
		([Id] ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	END  
GO  

IF OBJECT_ID('[dbo].[FuelConsumptions]', 'U') IS NULL
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	BEGIN
		CREATE TABLE [dbo].[FuelConsumptions](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CarId] [int] NOT NULL,
		[CarDescription] [nvarchar](100)  NOT NULL,
		[PetrolStationDesc] [nvarchar](100) NOT NULL,
		[PetrolType] [nvarchar](50) NOT NULL,
		[FuelingDate] [date] NOT NULL,
		[LiterAmount] [decimal](18,2) NOT NULL,
		[PricePerLiter] [decimal](18,2) NOT NULL,
		[FullPrice] [decimal](18,2) NOT NULL,
		[DistanceMade] [decimal](18,2) NOT NULL,
		[FuelConsumption] [decimal](18,2) NOT NULL,
		[Terrain] [nvarchar](100) NOT NULL,
		CONSTRAINT [PK_FuelConsumption] PRIMARY KEY CLUSTERED
		([Id] ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
		
		ALTER TABLE [dbo].[FuelConsumptions] WITH CHECK ADD CONSTRAINT [FK_FuelConsumption_CarId] FOREIGN KEY (CarId)     
		REFERENCES [dbo].[CarDescriptions] ([Id]) ON DELETE CASCADE   
		ALTER TABLE [dbo].[FuelConsumptions] CHECK CONSTRAINT [FK_FuelConsumption_CarId]   
	END
GO