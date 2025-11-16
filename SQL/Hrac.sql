USE [Volejbal]
GO

/****** Object:  Table [dbo].[Hrac]    Script Date: 16.11.2025 18:30:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Hrac](
	[id_hrac] [int] NOT NULL,
	[jmeno] [varchar](100) NOT NULL,
	[prijmeni] [varchar](100) NOT NULL,
	[pozice] [varchar](50) NULL,
	[id_tym] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id_hrac] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Hrac]  WITH CHECK ADD FOREIGN KEY([id_tym])
REFERENCES [dbo].[Tym] ([id_tym])
GO