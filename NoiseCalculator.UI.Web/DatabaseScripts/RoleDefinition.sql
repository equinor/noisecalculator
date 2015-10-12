USE [NoiseCalculatorDev]
GO

/****** Object:  Table [dbo].[RoleDefinition]    Script Date: 12.10.2015 10:29:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RoleDefinition](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SystemName] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO


/*
Id	SystemName
1	Operatør
2	Hjelpemann
3	Områdestøy
4	Helideck
5	Rotasjon
*/