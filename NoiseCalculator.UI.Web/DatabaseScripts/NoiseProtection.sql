USE [NoiseCalculatorDev]
GO

/****** Object:  Table [dbo].[NoiseProtection]    Script Date: 12.10.2015 10:26:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NoiseProtection](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NULL,
	[CultureName] [nvarchar](255) NULL,
	[NoiseProtectionDefinition_id] [int] NULL,
	[NoiseDampening] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[NoiseProtection]  WITH CHECK ADD  CONSTRAINT [FK99AE929974CCF762] FOREIGN KEY([NoiseProtectionDefinition_id])
REFERENCES [dbo].[NoiseProtectionDefinition] ([Id])
GO

ALTER TABLE [dbo].[NoiseProtection] CHECK CONSTRAINT [FK99AE929974CCF762]
GO


/*
Id	Title	CultureName	NoiseProtectionDefinition_id	NoiseDampening
1	Enkelt	nb-NO	1	14
2	Single	en-US	1	14
3	Dobbelt	nb-NO	2	20
4	Double	en-US	2	20
5	QuietPro	nb-NO	3	30
6	QuietPro	en-US	3	30
9	Uten	nb-NO	5	0
10	Without	en-US	5	0
*/