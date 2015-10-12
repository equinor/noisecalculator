﻿USE [NoiseCalculatorDev]
GO

/****** Object:  Table [dbo].[Rotation]    Script Date: 12.10.2015 10:30:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Rotation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Task_id] [int] NULL,
	[OperatorTask_id] [int] NULL,
	[AssistantTask_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[Rotation]  WITH CHECK ADD  CONSTRAINT [FK68C7C29DCAD8B1E1] FOREIGN KEY([Task_id])
REFERENCES [dbo].[Task] ([Id])
GO

ALTER TABLE [dbo].[Rotation] CHECK CONSTRAINT [FK68C7C29DCAD8B1E1]
GO

ALTER TABLE [dbo].[Rotation]  WITH CHECK ADD  CONSTRAINT [FK68C7C29DE59F8C6C] FOREIGN KEY([OperatorTask_id])
REFERENCES [dbo].[Task] ([Id])
GO

ALTER TABLE [dbo].[Rotation] CHECK CONSTRAINT [FK68C7C29DE59F8C6C]
GO

ALTER TABLE [dbo].[Rotation]  WITH CHECK ADD  CONSTRAINT [FK68C7C29DEDF0137A] FOREIGN KEY([AssistantTask_id])
REFERENCES [dbo].[Task] ([Id])
GO

ALTER TABLE [dbo].[Rotation] CHECK CONSTRAINT [FK68C7C29DEDF0137A]
GO


