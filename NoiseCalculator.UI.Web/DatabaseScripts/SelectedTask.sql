USE [NoiseCalculatorDev]
GO

/****** Object:  Table [dbo].[SelectedTask]    Script Date: 12.10.2015 10:31:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SelectedTask](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NULL,
	[Role] [nvarchar](255) NULL,
	[NoiseProtection] [nvarchar](255) NULL,
	[NoiseLevel] [decimal](4, 1) NULL,
	[Hours] [int] NULL,
	[Minutes] [int] NULL,
	[Percentage] [int] NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsNoiseMeassured] [bit] NOT NULL,
	[TaskId] [int] NULL,
	[HelicopterTaskId] [int] NULL,
	[ButtonPressed] [int] NULL,
	[BackgroundNoise] [int] NULL,
	[NoiseProtectionId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO


