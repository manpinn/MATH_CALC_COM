USE [MATH_CALC_COM]
GO

/****** Object:  Table [dbo].[RequestData]    Script Date: 15.08.2024 16:51:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RequestData](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[datetime] [datetime] NOT NULL,
	[url] [nvarchar](max) NOT NULL,
	[ip_adress] [nvarchar](max) NOT NULL,
	[ip_type] [tinyint] NOT NULL,
 CONSTRAINT [PK_RequestData] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


