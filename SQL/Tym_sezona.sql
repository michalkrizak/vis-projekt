USE [Volejbal]
GO

/****** Object:  Table [dbo].[Tym_Sezona]    Script Date: 16.11.2025 18:30:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tym_Sezona](
	[id_tym] [int] NOT NULL,
	[id_sezona] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_tym] ASC,
	[id_sezona] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Tym_Sezona]  WITH CHECK ADD FOREIGN KEY([id_sezona])
REFERENCES [dbo].[Sezona] ([id_sezona])
GO

ALTER TABLE [dbo].[Tym_Sezona]  WITH CHECK ADD FOREIGN KEY([id_tym])
REFERENCES [dbo].[Tym] ([id_tym])
GO


