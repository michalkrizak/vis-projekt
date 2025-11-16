USE [Volejbal]
GO

/****** Object:  Table [dbo].[Zapas]    Script Date: 16.11.2025 18:31:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Zapas](
	[id_zapas] [int] NOT NULL,
	[datum] [date] NOT NULL,
	[id_tym1] [int] NOT NULL,
	[id_tym2] [int] NOT NULL,
	[id_sezona] [int] NOT NULL,
	[skore_tym1] [int] NULL,
	[skore_tym2] [int] NULL,
	[vitez] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id_zapas] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Zapas]  WITH CHECK ADD FOREIGN KEY([id_sezona])
REFERENCES [dbo].[Sezona] ([id_sezona])
GO

ALTER TABLE [dbo].[Zapas]  WITH CHECK ADD FOREIGN KEY([id_tym1])
REFERENCES [dbo].[Tym] ([id_tym])
GO

ALTER TABLE [dbo].[Zapas]  WITH CHECK ADD FOREIGN KEY([id_tym2])
REFERENCES [dbo].[Tym] ([id_tym])
GO

ALTER TABLE [dbo].[Zapas]  WITH CHECK ADD FOREIGN KEY([vitez])
REFERENCES [dbo].[Tym] ([id_tym])
GO


