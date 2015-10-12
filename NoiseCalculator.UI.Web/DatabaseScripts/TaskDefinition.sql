USE [NoiseCalculatorDev]
GO

/****** Object:  Table [dbo].[TaskDefinition]    Script Date: 12.10.2015 10:33:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TaskDefinition](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SystemName] [nvarchar](255) NULL,
	[RoleType] [nvarchar](255) NULL,
	[SystemNameEN] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO


/*
Id	SystemName	RoleType	SystemNameEN
1060	Høytrykkspyling	Regular	High pressure washing
1061	Stillas	Regular	Scaffolding
1064	Diverse andre arbeidsoppgaver	Regular	Various other tasks
1066	Vinkelsliper	Regular	Angle grinder
1067	Luftdreven meisel	Regular	Air powered chisel
1068	Slegge og tigairsag	Regular	Sledge hammer and Tigair saw
1069	Trykkluftsblåsing	Regular	Compressed air blowing
1070	Ultrahøyttrykkspyling (UHT)	Regular	Ultra-High Pressure (UHP) water jetting
1071	Sponging	Regular	Sponging
1072	Nålepikking	Regular	Air needle scaler
1073	Slurryblåsing	Regular	Slurry blasting
1074	Sandblåsing	Regular	Sand blasting
1075	Tiltrekking av bolter med Hytorc Air	Regular	Tightening bolts with Hytorc Air
1076	Helidekkpersonell	Helideck	Helicopter noise at helicopter deck
1078	Helikopterpassasjer	Helideck	Helicopter -passenger
1079	IBIX	Regular	IBIX
1080	Arbeid i støysone	AreaNoise	Non-noisy work in areas with noise
*/