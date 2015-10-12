USE [NoiseCalculatorDev]
GO

/****** Object:  Table [dbo].[Role]    Script Date: 12.10.2015 10:28:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NULL,
	[CultureName] [nvarchar](255) NULL,
	[RoleType] [nvarchar](255) NULL,
	[SystemTitle] [nvarchar](255) NULL,
	[RoleDefinition_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[Role]  WITH CHECK ADD  CONSTRAINT [FK6117A80C874F61F6] FOREIGN KEY([RoleDefinition_id])
REFERENCES [dbo].[RoleDefinition] ([Id])
GO

ALTER TABLE [dbo].[Role] CHECK CONSTRAINT [FK6117A80C874F61F6]
GO


/*
Id	Title	CultureName	RoleType	SystemTitle	RoleDefinition_id
1	Operatør	nb-NO	Regular	Operator	1
2	Operator	en-US	Regular	Operator	1
3	Hjelpemann	nb-NO	Regular	Assistant	2
4	Assistant	en-US	Regular	Assistant	2
5	Områdestøy	nb-NO	AreaNoise	AreaNoise	3
6	Area Noise	en-US	AreaNoise	AreaNoise	3
7	Helideck	nb-NO	Helideck	Helideck	4
8	Helideck	en-US	Helideck	Helideck	4
9	Rotasjon	nb-NO	Rotation	Rotation	5
10	Rotation	en-US	Rotation	Rotation	5
*/