USE [CarData]
GO

IF OBJECT_ID('[dbo].[CarDescription]', 'U') IS NULL
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	BEGIN
		CREATE TABLE [dbo].[CarDescription](
		[Id] [uniqueidentifier] NOT NULL,
		[Manufucaturer] [nvarchar](100)  NOT NULL,
		[Model] [nvarchar](100)  NOT NULL,
		[HorsePower] [int] NOT NULL,
		[FuelTankSize] [int] NOT NULL,
		[Weight] [int] NOT NULL,
		CONSTRAINT [PK_CarDescription] PRIMARY KEY CLUSTERED
		([Id] ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	END  
GO  

IF OBJECT_ID('[dbo].[FuelConsumption]', 'U') IS NULL
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	BEGIN
		CREATE TABLE [dbo].[FuelConsumption](
		[Id] [uniqueidentifier] NOT NULL,
		[CarId] [uniqueidentifier] NOT NULL,
		[FuelingDate] [datetime] NOT NULL,
		[PetrolType] [nvarchar](50) NOT NULL,
		[LiterAmount] [nvarchar](50) NOT NULL,
		[PricePerLiter] [nvarchar](50) NOT NULL,
		[FullPrice] [nvarchar](50) NOT NULL,
		[DistanceMade] [nvarchar](50) NOT NULL,
		[FuelConsumption] [nvarchar](50) NOT NULL,
		[Terrain] [int] NOT NULL,
		CONSTRAINT [PK_FuelConsumption] PRIMARY KEY CLUSTERED
		([Id] ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
		
		ALTER TABLE [dbo].[FuelConsumption] WITH CHECK ADD CONSTRAINT [FK_FuelConsumption_CarId] FOREIGN KEY (CarId)     
		REFERENCES [dbo].[CarDescription] ([Id])     
		ALTER TABLE [dbo].[FuelConsumption] CHECK CONSTRAINT [FK_FuelConsumption_CarId]   
	END
GO