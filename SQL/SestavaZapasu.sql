USE [Volejbal]
GO

/****** Object:  Table [dbo].[SestavaZapasu]    Script Date: 16.11.2025 18:30:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SestavaZapasu](
	[id_zapas] [int] NOT NULL,
	[id_hrac] [int] NOT NULL,
	[id_tym] [int] NOT NULL,
	[pozice] [varchar](50) NULL,
	[JeKapitan] [bit] NULL,
	[JeLibero] [bit] NULL,
	[sety] [int] NULL,
	[body] [int] NULL,
	[esa] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id_zapas] ASC,
	[id_hrac] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SestavaZapasu]  WITH CHECK ADD FOREIGN KEY([id_hrac])
REFERENCES [dbo].[Hrac] ([id_hrac])
GO

ALTER TABLE [dbo].[SestavaZapasu]  WITH CHECK ADD FOREIGN KEY([id_tym])
REFERENCES [dbo].[Tym] ([id_tym])
GO

ALTER TABLE [dbo].[SestavaZapasu]  WITH CHECK ADD FOREIGN KEY([id_zapas])
REFERENCES [dbo].[Zapas] ([id_zapas])
GO


