USE [NoiseCalculatorDev]
GO

/****** Object:  Table [dbo].[HelicopterTask]    Script Date: 12.10.2015 10:20:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HelicopterTask](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HelicopterType_id] [int] NULL,
	[NoiseProtectionDefinition_id] [int] NULL,
	[NoiseLevel] [decimal](4, 1) NULL,
	[Task_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[HelicopterTask]  WITH CHECK ADD  CONSTRAINT [FK_HelicopterTask_HelicopterTask] FOREIGN KEY([Id])
REFERENCES [dbo].[HelicopterTask] ([Id])
GO

ALTER TABLE [dbo].[HelicopterTask] CHECK CONSTRAINT [FK_HelicopterTask_HelicopterTask]
GO

ALTER TABLE [dbo].[HelicopterTask]  WITH CHECK ADD  CONSTRAINT [FK72CBE6409C49547E] FOREIGN KEY([HelicopterType_id])
REFERENCES [dbo].[HelicopterType] ([Id])
GO

ALTER TABLE [dbo].[HelicopterTask] CHECK CONSTRAINT [FK72CBE6409C49547E]
GO


/*
Id	HelicopterType_id	NoiseProtectionDefinition_id	NoiseLevel	Task_id
52	1	NULL	113.7	1149
53	1	NULL	113.8	1154
54	1	NULL	113.7	1151
55	1	NULL	115.5	1152
56	1	NULL	115.5	1153
57	2	NULL	110.3	1149
58	2	NULL	109.8	1154
59	2	NULL	110.3	1151
60	2	NULL	111.6	1152
61	2	NULL	111.6	1153
62	3	NULL	107.3	1149
63	3	NULL	106.8	1154
64	3	NULL	107.3	1151
65	3	NULL	108.6	1152
66	3	NULL	108.6	1153
67	1	NULL	96.1	1160
68	1	NULL	97.3	1161
69	1	NULL	98.2	1162
70	2	NULL	94.9	1160
71	2	NULL	95.9	1161
72	2	NULL	96.6	1162
73	3	NULL	94.9	1160
74	3	NULL	95.9	1161
75	3	NULL	96.6	1162
*/