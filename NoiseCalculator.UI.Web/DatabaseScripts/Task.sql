USE [NoiseCalculatorDev]
GO

/****** Object:  Table [dbo].[Task]    Script Date: 12.10.2015 10:31:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Task](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NULL,
	[NoiseLevelGuideline] [decimal](4, 1) NULL,
	[AllowedExposureMinutes] [int] NULL,
	[CultureName] [nvarchar](255) NULL,
	[Role_id] [int] NULL,
	[NoiseProtection_id] [int] NULL,
	[TaskDefinition_id] [int] NULL,
	[SortOrder] [int] NOT NULL CONSTRAINT [DF_Task_SortOrder]  DEFAULT ((0)),
	[ButtonPressed] [int] NULL,
	[BackgroundNoise] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK1B248E97A95B1B03] FOREIGN KEY([Role_id])
REFERENCES [dbo].[Role] ([Id])
GO

ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK1B248E97A95B1B03]
GO

ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK1B248E97C6E386BF] FOREIGN KEY([NoiseProtection_id])
REFERENCES [dbo].[NoiseProtection] ([Id])
GO

ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK1B248E97C6E386BF]
GO


/*
Id	Title	NoiseLevelGuideline	AllowedExposureMinutes	CultureName	Role_id	NoiseProtection_id	TaskDefinition_id	SortOrder	ButtonPressed	BackgroundNoise
1029	Høytrykkspyling - Operatør	102.0	0	nb-NO	1	3	1060	0	33	NULL
1030	Høytrykkspyling - Hjelpemann	91.0	0	nb-NO	3	1	1060	0	33	NULL
1031	High pressure washing - Operator	102.0	0	en-US	2	4	1060	0	33	NULL
1032	High pressure washing - Helper	91.0	0	en-US	4	2	1060	0	33	NULL
1033	Stillas - bygging	99.0	0	nb-NO	1	3	1061	0	25	NULL
1034	Stillas - riving	99.0	0	nb-NO	1	3	1061	0	25	NULL
1035	Stillas - transport	97.0	0	nb-NO	1	3	1061	0	25	NULL
1036	Scaffolding - erection	99.0	0	en-US	2	4	1061	0	25	NULL
1037	Scaffolding - dismantling	99.0	0	en-US	2	4	1061	0	25	NULL
1038	Scaffolding - transport	97.0	0	en-US	2	4	1061	0	25	NULL
1041	Lokkformer	75.0	0	nb-NO	1	9	1064	0	100	NULL
1042	Lid forming machine	75.0	0	en-US	2	10	1064	0	100	NULL
1043	Tilslåing av falser	90.0	0	nb-NO	1	1	1064	0	100	NULL
1044	Hammering of flanges	90.0	0	en-US	2	2	1064	0	100	NULL
1045	Visp (maling/chartec)	92.0	0	nb-NO	1	1	1064	0	100	NULL
1046	Mixer (paint/Chartec)	92.0	0	en-US	2	2	1064	0	100	NULL
1049	Båndsag	76.0	0	nb-NO	1	9	1064	0	100	NULL
1050	Band saw	76.0	0	en-US	2	10	1064	0	100	NULL
1051	Båndsliper	90.0	0	nb-NO	1	1	1064	0	100	NULL
1052	Belt grinder	90.0	0	en-US	2	2	1064	0	100	NULL
1053	Dreiebenk	81.0	0	nb-NO	1	1	1064	0	100	NULL
1054	Lathe	81.0	0	en-US	2	2	1064	0	100	NULL
1055	Fresing	76.0	0	nb-NO	1	9	1064	0	100	NULL
1056	Milling (metal)	76.0	0	en-US	2	10	1064	0	100	NULL
1061	Batteridrevet drill	78.0	0	nb-NO	1	9	1064	0	100	NULL
1062	Battery powered drill	78.0	0	en-US	2	10	1064	0	100	NULL
1063	Slangepresse	73.0	0	nb-NO	1	9	1064	0	100	NULL
1064	Hose crimping machine	73.0	0	en-US	2	10	1064	0	100	NULL
1065	Skiltbanking	82.0	0	nb-NO	1	1	1064	0	100	NULL
1066	Hammering signposts	82.0	0	en-US	2	2	1064	0	100	NULL
1067	Slagnøkkel for å løsne PSV	96.0	0	nb-NO	1	3	1064	0	100	NULL
1068	Impact wrench, loosening Pressure Safety Valves (PSV)	96.0	0	en-US	2	4	1064	0	100	NULL
1069	Vinkelsliper ved kutting og sliping - Operatør	109.0	0	nb-NO	1	3	1066	0	25	NULL
1070	Vinkelsliper ved kutting og sliping - Hjelpemann 4 m	97.0	0	nb-NO	3	3	1066	0	25	NULL
1071	Angle grinder, cutting and grinding - Operator	109.0	0	en-US	2	4	1066	0	25	NULL
1072	Angle grinder, cutting and grinding - Helper	97.0	0	en-US	4	4	1066	0	25	NULL
1073	Luftdreven meisel ved fjerning av gitterrister - Operatør	118.0	0	nb-NO	1	3	1067	0	25	NULL
1074	Luftdreven meisel ved fjerning av gitterrister - Hjelpemann 4 m	109.0	0	nb-NO	1	3	1067	0	25	NULL
1075	Air powered chisel, removing gratings - Operator	118.0	0	en-US	2	4	1067	0	25	NULL
1076	Air powered chisel, removing gratings - Helper (4 m / 4.4 yards)	109.0	0	en-US	2	4	1067	0	25	NULL
1077	Slegge og tigairsag ved fjerning av gitterrister - Operatør	101.0	0	nb-NO	1	3	1068	0	25	NULL
1078	Slegge og tigairsag ved fjerning av gitterrister - Hjelpemann 2 m	97.0	0	nb-NO	3	3	1068	0	25	NULL
1079	Sledge hammer and Tigair saw, removing gratings - Operator	101.0	0	en-US	2	4	1068	0	25	NULL
1080	Sledge hammer and Tigair saw, removing gratings - Helper (2 m / 2.2 yards)	97.0	0	en-US	4	4	1068	0	25	NULL
1081	Høytrykksvannjet ved fjerning av betong - Fjernoperert	100.0	0	nb-NO	1	3	1064	0	25	NULL
1082	High pressure water jetting, removing concrete - Remotely controlled	100.0	0	en-US	2	4	1064	0	25	NULL
1083	Luftdreven håndholdt meisel ved fjerning av betong	105.0	0	nb-NO	1	3	1064	0	5	NULL
1084	Air powered hand held chisel, removing concrete	105.0	0	en-US	2	4	1064	0	5	NULL
1085	Trykkluftsblåsing med ulike munnstykker - Operatør	117.0	0	nb-NO	1	3	1069	0	50	NULL
1086	Trykkluftsblåsing med ulike munnstykker - Hjelpemann 4 m	109.0	0	nb-NO	3	3	1069	0	50	NULL
1087	Trykkluftsblåsing med rundt munnstykke - Operatør	116.0	0	nb-NO	1	3	1069	0	50	NULL
1088	Trykkluftsblåsing med rundt munnstykke - Hjelpemann 4 m	109.0	0	nb-NO	3	3	1069	0	50	NULL
1089	Trykkluftsblåsing med flatt munnstykke - Operatør	109.0	0	nb-NO	1	3	1069	0	50	NULL
1090	Trykkluftsblåsing med flatt munnstykke - Hjelpemann 4 m	103.0	0	nb-NO	3	3	1069	0	50	NULL
1091	Trykkluftsblåsing uten munnstykke - Operatør	120.0	0	nb-NO	1	3	1069	0	50	NULL
1092	Trykkluftsblåsing uten munnstykke - Hjelpemann 4 m	112.0	0	nb-NO	3	3	1069	0	50	NULL
1093	Compressed air blowing, various nozzles – Operator 	117.0	0	en-US	2	4	1069	0	50	NULL
1094	Compressed air blowing, various nozzles – Helper (4 m / 4.4 yards)	109.0	0	en-US	4	4	1069	0	50	NULL
1095	Compressed air blowing, circular nozzle – Operator 	116.0	0	en-US	2	4	1069	0	50	NULL
1096	Compressed air blowing, circular nozzle – Helper (4 m / 4.4 yards)	109.0	0	en-US	4	4	1069	0	50	NULL
1097	Compressed air blowing, flat nozzle – Operator 	109.0	0	en-US	2	4	1069	0	50	NULL
1098	Compressed air blowing, flat nozzle – Helper (4 m / 4.4 yards)	103.0	0	en-US	4	4	1069	0	50	NULL
1099	Compressed air blowing, without nozzle – Operator 	120.0	0	en-US	2	4	1069	0	50	NULL
1100	Compressed air blowing, without nozzle – Helper (4 m / 4.4 yards)	112.0	0	en-US	4	4	1069	0	50	NULL
1101	Ultrahøyttrykkspyling (UHT) ved fjerning av rust og maling - Operatør	111.0	0	nb-NO	1	3	1070	0	50	NULL
1102	Ultrahøyttrykkspyling (UHT) ved fjerning av rust og maling - Nødstoppoperatør	106.0	0	nb-NO	1	3	1070	0	50	NULL
1103	Ultra-High Pressure (UHP) water jetting, removing rust and paint - Operator	111.0	0	en-US	2	4	1070	0	50	NULL
1104	Ultra-High Pressure (UHP) water jetting, removing rust and paint – Emergency shutdown operator	106.0	0	en-US	2	4	1070	0	50	NULL
1105	Sponging - Operatør	117.0	0	nb-NO	1	3	1071	0	33	NULL
1106	Sponging - Hjelpemann 2 m	111.0	0	nb-NO	3	3	1071	0	33	NULL
1107	Sponging - Hjelpemann 4 m	105.0	0	nb-NO	3	3	1071	0	33	NULL
1108	Sponging - Hjelpemann 8 m	103.0	0	nb-NO	3	3	1071	0	33	NULL
1109	Sponge blasting – Operator	117.0	0	en-US	2	4	1071	0	33	NULL
1110	Sponge blasting – Helper (2 m, 2.2 yards)	111.0	0	en-US	4	4	1071	0	33	NULL
1111	Sponge blasting – Helper (4 m, 4.4 yards)	105.0	0	en-US	4	4	1071	0	33	NULL
1112	Sponge blasting – Helper (8 m, 8.8 yards)	103.0	0	en-US	4	4	1071	0	33	NULL
1113	Nålepikking - Operatør 	105.0	0	nb-NO	1	3	1072	0	100	NULL
1114	Nålepikking - Hjelpemann	93.0	0	nb-NO	3	1	1072	0	100	NULL
1115	Air needle scaler – Operator	105.0	0	en-US	2	4	1072	0	100	NULL
1116	Air needle scaler – Helper	93.0	0	en-US	4	2	1072	0	100	NULL
1117	Slurryblåsing - Operatør	110.0	0	nb-NO	1	3	1073	0	40	NULL
1118	Slurryblåsing - Hjelpemann 5 m	101.0	0	nb-NO	3	3	1073	0	40	NULL
1119	Slurry blasting – Operator	110.0	0	en-US	2	4	1073	0	40	NULL
1120	Slurry blasting – Helper	101.0	0	en-US	4	4	1073	0	40	NULL
1121	Sandblåsing, tørr, i habitat	111.0	0	nb-NO	1	3	1074	0	70	NULL
1122	Sandblåsing, tørr, i tank - Operatør	124.0	0	nb-NO	1	3	1074	0	50	NULL
1123	Sandblåsing, tørr, i tank - Hjelpemann	99.0	0	nb-NO	3	3	1074	0	50	NULL
1124	Sandblåsing, tørr, i telt - Operatør	113.0	0	nb-NO	1	3	1074	0	50	NULL
1125	Sandblåsing, tørr, i telt - Hjelpemann	96.0	0	nb-NO	3	3	1074	0	50	NULL
1126	Sand blasting, dry, inside habitat	111.0	0	en-US	2	4	1074	0	70	NULL
1127	Sand blasting, dry, inside tank – Operator	124.0	0	en-US	2	4	1074	0	50	NULL
1128	Sand blasting, dry, inside tank – Helper	99.0	0	en-US	4	4	1074	0	50	NULL
1129	Sand blasting, dry, inside tent – Operator	113.0	0	en-US	2	4	1074	0	50	NULL
1130	Sand blasting, dry, inside tent – Helper	96.0	0	en-US	4	4	1074	0	50	NULL
1131	Sliping med luftdrevet rettsliper	101.0	0	nb-NO	1	3	1064	0	50	NULL
1132	Luftdrevet boremaskin	93.0	0	nb-NO	1	1	1064	0	40	NULL
1133	Luftdrevet muttertrekker med 3/4" tapp	103.0	0	nb-NO	1	3	1064	0	40	NULL
1134	Luftdrevet Tigair-sag	93.0	0	nb-NO	1	1	1064	0	40	NULL
1135	Blåsepistol med rundt munnstykke	96.0	0	nb-NO	1	3	1064	0	50	NULL
1136	Luftdrevet rett meiselhammer	116.0	0	nb-NO	1	3	1064	0	35	NULL
1137	Magnetboremaskin	94.0	0	nb-NO	1	1	1064	0	50	NULL
1138	Grinding with air powered Straight grinder	101.0	0	en-US	2	4	1064	0	50	NULL
1139	Air powered drill	93.0	0	en-US	2	2	1064	0	40	NULL
1140	Air powered nut driver, 3/4" tap	103.0	0	en-US	2	4	1064	0	40	NULL
1141	Air powered Tigair saw	93.0	0	en-US	2	2	1064	0	40	NULL
1142	Air blower with circular nozzle	96.0	0	en-US	2	4	1064	0	50	NULL
1143	Air powered chisel hammer	116.0	0	en-US	2	4	1064	0	35	NULL
1144	Magnetic drill	94.0	0	en-US	2	2	1064	0	50	NULL
1145	Tiltrekking av bolter med Hytorc Air - Aggregatoperatør	91.0	0	nb-NO	1	1	1075	0	35	NULL
1146	Tiltrekking av bolter med Hytorc Air - Nøkkeloperatør	85.0	0	nb-NO	1	1	1075	0	35	NULL
1147	Tightening bolts with Hytorc Air – Aggregate operator	91.0	0	en-US	2	2	1075	0	35	NULL
1148	Tightening bolts with Hytorc Air – Tool operator	85.0	0	en-US	2	2	1075	0	35	NULL
1149	Flight w/fuel	0.0	20	nb-NO	7	3	1076	0	100	NULL
1151	Split flight	0.0	10	nb-NO	7	3	1076	0	100	NULL
1152	Shuttle/SAR	0.0	5	nb-NO	7	3	1076	0	100	NULL
1153	Startup/Shutdown	0.0	3	nb-NO	7	3	1076	0	100	NULL
1154	Full flights	0.0	15	nb-NO	7	3	1076	0	100	NULL
1160	Flight	0.0	79	nb-NO	7	1	1078	0	100	NULL
1161	Flight - 1 stopover	0.0	87	nb-NO	7	1	1078	0	100	NULL
1162	Flight - 2 stopovers	0.0	94	nb-NO	7	1	1078	0	100	NULL
1163	IBIX - Operatør	110.0	0	nb-NO	1	3	1079	0	50	NULL
1164	IBIX - Hjelpemann	90.0	0	nb-NO	3	1	1079	0	50	NULL
1165	IBIX – Operator	110.0	0	en-US	2	4	1079	0	50	NULL
1166	IBIX – Helper	90.0	0	en-US	4	2	1079	0	50	NULL
1170	Område 80 - 85 dBA	85.0	0	nb-NO	5	1	1080	0	100	NULL
1171	Område 85 - 90 dBA	90.0	0	nb-NO	5	1	1080	0	100	NULL
1172	Område 90 - 95 dBA	95.0	0	nb-NO	5	1	1080	0	100	NULL
1173	Område 95 - 100 dBA	100.0	0	nb-NO	5	3	1080	0	100	NULL
1174	Område 100 - 105 dBA	105.0	0	nb-NO	5	3	1080	0	100	NULL
1175	Område 105 - 110 dBA	110.0	0	nb-NO	5	3	1080	0	100	NULL
1177	Noise 80 - 85 dBA	85.0	0	en-US	5	1	1080	0	100	NULL
1178	Noise 85 - 90 dBA	90.0	0	en-US	5	1	1080	0	100	NULL
1179	Noise 90 - 95 dBA	95.0	0	en-US	5	1	1080	0	100	NULL
1180	Noise 95 - 100 dBA	100.0	0	en-US	5	3	1080	0	100	NULL
1181	Noise 100 - 105 dBA	105.0	0	en-US	5	3	1080	0	100	NULL
1182	Noise 105 - 110 dBA	110.0	0	en-US	5	3	1080	0	100	NULL
1183	Sandblåsing i verksted	83.0	0	nb-NO	1	1	1074	0	100	NULL
1184	Sand blasting in workshop	83.0	0	en-US	2	2	1074	0	100	NULL
1185	Ultrahøyttrykkspyling (UHT)	110.0	0	nb-NO	1	3	1070	0	50	NULL
1186	Ultra-High Pressure (UHP) water jetting	110.0	0	en-US	1	3	1070	0	50	NULL
*/