USE [master]
GO
/****** Object:  Database [Facturas]    Script Date: 28/02/2023 05:10:17 p. m. ******/
CREATE DATABASE [Facturas]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Facturas', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Facturas.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Facturas_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Facturas_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Facturas] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Facturas].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Facturas] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Facturas] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Facturas] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Facturas] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Facturas] SET ARITHABORT OFF 
GO
ALTER DATABASE [Facturas] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Facturas] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Facturas] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Facturas] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Facturas] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Facturas] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Facturas] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Facturas] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Facturas] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Facturas] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Facturas] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Facturas] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Facturas] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Facturas] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Facturas] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Facturas] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Facturas] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Facturas] SET RECOVERY FULL 
GO
ALTER DATABASE [Facturas] SET  MULTI_USER 
GO
ALTER DATABASE [Facturas] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Facturas] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Facturas] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Facturas] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Facturas] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Facturas] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Facturas', N'ON'
GO
ALTER DATABASE [Facturas] SET QUERY_STORE = OFF
GO
USE [Facturas]
GO
/****** Object:  User [facturas]    Script Date: 28/02/2023 05:10:17 p. m. ******/
CREATE USER [facturas] FOR LOGIN [facturas] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Attributes_Client]    Script Date: 28/02/2023 05:10:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attributes_Client](
	[IdClient] [int] NULL,
	[IdXMLAttributes] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 28/02/2023 05:10:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[IdClient] [int] IDENTITY(1,1) NOT NULL,
	[ClientName] [nvarchar](250) NOT NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[IdClient] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ErrorLog]    Script Date: 28/02/2023 05:10:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorLog](
	[IdLog] [int] IDENTITY(1,1) NOT NULL,
	[Message] [varchar](max) NULL,
	[Stack] [varchar](max) NULL,
	[File] [varchar](350) NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED 
(
	[IdLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Template]    Script Date: 28/02/2023 05:10:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Template](
	[IdTemplate] [int] IDENTITY(1,1) NOT NULL,
	[IdMapClient] [int] NULL,
	[Section] [varchar](50) NULL,
	[Row] [varchar](250) NULL,
	[Column] [varchar](250) NULL,
	[ParentElement] [varchar](250) NULL,
	[Element] [varchar](250) NULL,
	[Attribute] [varchar](250) NULL,
	[FillWith] [nvarchar](250) NULL,
	[IdType] [int] NULL,
	[IsRequeried] [bit] NULL,
 CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED 
(
	[IdTemplate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Type]    Script Date: 28/02/2023 05:10:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Type](
	[IdType] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](250) NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Type] PRIMARY KEY CLUSTERED 
(
	[IdType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[XMLAttributes]    Script Date: 28/02/2023 05:10:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[XMLAttributes](
	[IdAttribute] [int] IDENTITY(1,1) NOT NULL,
	[AttributeName] [varchar](250) NULL,
	[AttributeValue] [varchar](max) NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_XMLAttributes] PRIMARY KEY CLUSTERED 
(
	[IdAttribute] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Attributes_Client] ([IdClient], [IdXMLAttributes]) VALUES (1, 1)
INSERT [dbo].[Attributes_Client] ([IdClient], [IdXMLAttributes]) VALUES (1, 2)
INSERT [dbo].[Attributes_Client] ([IdClient], [IdXMLAttributes]) VALUES (1, 3)
INSERT [dbo].[Attributes_Client] ([IdClient], [IdXMLAttributes]) VALUES (1, 4)
INSERT [dbo].[Attributes_Client] ([IdClient], [IdXMLAttributes]) VALUES (1, 5)
GO
SET IDENTITY_INSERT [dbo].[Client] ON 

INSERT [dbo].[Client] ([IdClient], [ClientName], [IsActive]) VALUES (1, N'Client', 1)
SET IDENTITY_INSERT [dbo].[Client] OFF
GO
SET IDENTITY_INSERT [dbo].[Template] ON 

INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (2, 1, N'Comprobante', NULL, NULL, N'root', N'cfdi:Comprobante', N'@@inicio@@', NULL, 1, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (3, 1, N'Comprobante', N'3', N'1', N'root', N'cfdi:Comprobante', N'Version', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (4, 1, N'Comprobante', N'3', N'2', N'root', N'cfdi:Comprobante', N'Fecha', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (5, 1, N'Comprobante', N'3', N'3', N'root', N'cfdi:Comprobante', N'Folio', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (6, 1, N'Comprobante', N'3', N'4', N'root', N'cfdi:Comprobante', N'FormaPago', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (7, 1, N'Comprobante', N'3', N'5', N'root', N'cfdi:Comprobante', N'LugarExpedicion', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (8, 1, N'Comprobante', N'3', N'6', N'root', N'cfdi:Comprobante', N'MetodoPago', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (9, 1, N'Comprobante', N'3', N'7', N'root', N'cfdi:Comprobante', N'Moneda', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (10, 1, N'Comprobante', N'3', N'8', N'root', N'cfdi:Comprobante', N'SubTotal', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (11, 1, N'Comprobante', N'3', N'9', N'root', N'cfdi:Comprobante', N'TipoCambio', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (12, 1, N'Comprobante', N'3', N'10', N'root', N'cfdi:Comprobante', N'Total', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (13, 1, N'Comprobante', N'-1', N'-1', N'root', N'cfdi:Comprobante', N'Certificado', N'Vacio', NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (14, 1, N'Comprobante', N'-1', N'-1', N'root', N'cfdi:Comprobante', N'Sello', N'Vacio', NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (15, 1, N'Emisor', NULL, NULL, N'cfdi:Comprobante', N'cfdi:Emisor', N'@@inicio@@', NULL, 1, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (16, 1, N'Emisor', N'5', N'1', N'cfdi:Comprobante', N'cfdi:Emisor', N'Rfc', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (17, 1, N'Emisor', N'5', N'2', N'cfdi:Comprobante', N'cfdi:Emisor', N'Nombre', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (18, 1, N'Emisor', N'5', N'3', N'cfdi:Comprobante', N'cfdi:Emisor', N'RegimenFiscal', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (19, 1, N'Receptor', NULL, NULL, N'cfdi:Comprobante', N'cfdi:Receptor', N'@@inicio@@', NULL, 1, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (20, 1, N'Receptor', N'7', N'1', N'cfdi:Comprobante', N'cfdi:Receptor', N'Rfc', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (21, 1, N'Receptor', N'7', N'2', N'cfdi:Comprobante', N'cfdi:Receptor', N'Nombre', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (22, 1, N'Receptor', N'7', N'3', N'cfdi:Comprobante', N'cfdi:Receptor', N'UsoCFDI', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (23, 1, N'Receptor', N'7', N'4', N'cfdi:Comprobante', N'cfdi:Receptor', N'RegimenFiscalReceptor', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (24, 1, N'Receptor', N'7', N'5', N'cfdi:Comprobante', N'cfdi:Receptor', N'DomicilioFiscalReceptor', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (25, 1, N'Conceptos', NULL, NULL, N'cfdi:Comprobante', N'cfdi:Conceptos', N'@@inicio@@', NULL, 1, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (26, 1, N'Concepto', NULL, NULL, N'cfdi:Conceptos', N'cfdi:Concepto', N'@@inicio@@', NULL, 2, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (27, 1, N'Concepto', N'10', N'1', N'cfdi:Conceptos', N'cfdi:Concepto', N'Cantidad', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (28, 1, N'Concepto', N'10', N'2', N'cfdi:Conceptos', N'cfdi:Concepto', N'Unidad', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (29, 1, N'Concepto', N'10', N'3', N'cfdi:Conceptos', N'cfdi:Concepto', N'Descripcion', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (30, 1, N'Impuestos', NULL, NULL, N'cfdi:Comprobante', N'cfdi:Impuestos', N'@@inicio@@', NULL, 1, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (31, 1, N'Complementos', NULL, NULL, N'cfdi:Comprobante', N'cfdi:Complemento', N'@@inicio@@', NULL, 1, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (32, 1, N'Donatarias', NULL, NULL, N'cfdi:Complemento', N'donat:Donatarias', N'@@inicio@@', NULL, 2, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (33, 1, N'Donatarias', N'14', N'1', N'cfdi:Complemento', N'donat:Donatarias', N'version', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (34, 1, N'Donatarias', N'14', N'2', N'cfdi:Complemento', N'donat:Donatarias', N'noAutorizacion', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (35, 1, N'Donatarias', N'14', N'3', N'cfdi:Complemento', N'donat:Donatarias', N'fechaAutorizacion', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (36, 1, N'Donatarias', N'14', N'4', N'cfdi:Complemento', N'donat:Donatarias', N'leyenda', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (37, 1, N'Donatarias', NULL, NULL, N'donat:Donatarias', N'leyendasFisc:LeyendasFiscales', N'@@inicio@@', NULL, 1, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (38, 1, N'Donatarias', N'16', N'1', N'donat:Donatarias', N'leyendasFisc:LeyendasFiscales', N'version', N'1.0', NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (39, 1, N'Donatarias', NULL, NULL, N'leyendasFisc:LeyendasFiscales', N'leyendasFisc:Leyenda', N'@@inicio@@', NULL, 2, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (40, 1, N'Donatarias', N'18', N'1', N'leyendasFisc:LeyendasFiscales', N'leyendasFisc:Leyenda', N'disposicionFiscal', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (41, 1, N'Donatarias', N'18', N'2', N'leyendasFisc:LeyendasFiscales', N'leyendasFisc:Leyenda', N'norma', NULL, NULL, 1)
INSERT [dbo].[Template] ([IdTemplate], [IdMapClient], [Section], [Row], [Column], [ParentElement], [Element], [Attribute], [FillWith], [IdType], [IsRequeried]) VALUES (42, 1, N'Donatarias', N'18', N'3', N'leyendasFisc:LeyendasFiscales', N'leyendasFisc:Leyenda', N'textoLeyenda', NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Template] OFF
GO
SET IDENTITY_INSERT [dbo].[Type] ON 

INSERT [dbo].[Type] ([IdType], [Type], [IsActive]) VALUES (1, N'Simple', 1)
INSERT [dbo].[Type] ([IdType], [Type], [IsActive]) VALUES (2, N'Items', 1)
SET IDENTITY_INSERT [dbo].[Type] OFF
GO
SET IDENTITY_INSERT [dbo].[XMLAttributes] ON 

INSERT [dbo].[XMLAttributes] ([IdAttribute], [AttributeName], [AttributeValue], [IsActive]) VALUES (1, N'xmlns:cfdi', N'http://www.sat.gob.mx/cfd/3', 1)
INSERT [dbo].[XMLAttributes] ([IdAttribute], [AttributeName], [AttributeValue], [IsActive]) VALUES (2, N'xmlns:xsi', N'http://www.w3.org/2001/XMLSchema-instance', 1)
INSERT [dbo].[XMLAttributes] ([IdAttribute], [AttributeName], [AttributeValue], [IsActive]) VALUES (3, N'xsi:schemaLocation', N'http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd http://www.sat.gob.mx/donat http://www.sat.gob.mx/sitio_internet/cfd/donat/donat11.xsd http://www.sat.gob.mx/leyendasFiscales http://www.sat.gob.mx/sitio_internet/cfd/leyendasFiscales/leyendasFisc.xsd', 1)
INSERT [dbo].[XMLAttributes] ([IdAttribute], [AttributeName], [AttributeValue], [IsActive]) VALUES (4, N'xmlns:donat', N'http://www.sat.gob.mx/donat', 1)
INSERT [dbo].[XMLAttributes] ([IdAttribute], [AttributeName], [AttributeValue], [IsActive]) VALUES (5, N'xmlns:leyendasFisc', N'http://www.sat.gob.mx/leyendasFiscales', 1)
SET IDENTITY_INSERT [dbo].[XMLAttributes] OFF
GO
ALTER TABLE [dbo].[Attributes_Client]  WITH CHECK ADD  CONSTRAINT [FK_Attributes_Template_Client] FOREIGN KEY([IdClient])
REFERENCES [dbo].[Client] ([IdClient])
GO
ALTER TABLE [dbo].[Attributes_Client] CHECK CONSTRAINT [FK_Attributes_Template_Client]
GO
ALTER TABLE [dbo].[Attributes_Client]  WITH CHECK ADD  CONSTRAINT [FK_Attributes_Template_XMLAttributes] FOREIGN KEY([IdXMLAttributes])
REFERENCES [dbo].[XMLAttributes] ([IdAttribute])
GO
ALTER TABLE [dbo].[Attributes_Client] CHECK CONSTRAINT [FK_Attributes_Template_XMLAttributes]
GO
ALTER TABLE [dbo].[Template]  WITH CHECK ADD  CONSTRAINT [FK_Template_Client] FOREIGN KEY([IdMapClient])
REFERENCES [dbo].[Client] ([IdClient])
GO
ALTER TABLE [dbo].[Template] CHECK CONSTRAINT [FK_Template_Client]
GO
ALTER TABLE [dbo].[Template]  WITH CHECK ADD  CONSTRAINT [FK_Template_Type] FOREIGN KEY([IdType])
REFERENCES [dbo].[Type] ([IdType])
GO
ALTER TABLE [dbo].[Template] CHECK CONSTRAINT [FK_Template_Type]
GO
/****** Object:  StoredProcedure [dbo].[GetAllTemplate]    Script Date: 28/02/2023 05:10:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Garcia Talía
-- Create date: 2023-Feb-20
-- Description:	Get all template Nodes by client
-- =============================================
CREATE PROCEDURE [dbo].[GetAllTemplate] 
	-- Add the parameters for the stored procedure here
	@idMapClient int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT [IdTemplate]
      ,[IdMapClient]
      ,[Section]
      ,[Row]
      ,[Column]
      ,[ParentElement]
      ,[Element]
      ,[Attribute]
      ,[FillWith]
      ,[IdType]
      ,[IsRequeried]
  FROM [Template]
  WHERE [IdMapClient] = @idMapClient
END
GO
/****** Object:  StoredProcedure [dbo].[GetElementsBySection]    Script Date: 28/02/2023 05:10:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		García Talía
-- Create date: 2023-Feb-20
-- Description:	Get all nodes by Section
-- =============================================
CREATE PROCEDURE [dbo].[GetElementsBySection]
	-- Add the parameters for the stored procedure here
	@Section varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [IdTemplate]
      ,[IdMapClient]
      ,[Section]
      ,[Row]
      ,[Column]
      ,[ParentElement]
      ,[Element]
      ,[Attribute]
      ,[FillWith]
      ,[IdType]
      ,[IsRequeried]
  FROM [Template]
  WHERE [Section] = @Section
END
GO
/****** Object:  StoredProcedure [dbo].[GetXMLAttributes]    Script Date: 28/02/2023 05:10:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Garcia Talía
-- Create date: 2023-Feb-23
-- Description:	Get Attributes for XML file
-- =============================================
CREATE PROCEDURE [dbo].[GetXMLAttributes] 
	-- Add the parameters for the stored procedure here
	@IdClient int
AS
BEGIN
DECLARE @IsActive int = 1;
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		-- Insert statements for procedure here
	SELECT [IdAttribute]
		  ,[AttributeName]
		  ,[AttributeValue]
		  ,[IsActive]
	  FROM [XMLAttributes] AS xmlA
	  INNER JOIN [dbo].[Attributes_Client] AS xmlC ON  xmlC.IdXMLAttributes = xmlA.[IdAttribute]
	  WHERE xmlC.IdClient = @IdClient AND  [IsActive] = @IsActive
END
GO
/****** Object:  StoredProcedure [dbo].[InsertErrorLog]    Script Date: 28/02/2023 05:10:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		García Talía
-- Create date: 2023-Feb-21
-- Description:	Insert log error in process  to Create XML
-- Update date : 2023-Feb-28
-- =============================================
CREATE PROCEDURE [dbo].[InsertErrorLog]
	-- Add the parameters for the stored procedure here
		@Message varchar(max),
		@Stack varchar(max),
		@File varchar(350)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
INSERT INTO [dbo].[ErrorLog]
           ([Message]
		   ,Stack
           ,[File]
           ,[Date])
     VALUES
           (@Message
		   ,@Stack
           ,@File
           ,GETDATE())
END
GO
/****** Object:  StoredProcedure [dbo].[Test_GetTemplate]    Script Date: 28/02/2023 05:10:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Garcia Talía
-- Create date: 2023-Feb-20
-- Description:	Get all template Nodes by client
-- =============================================
CREATE PROCEDURE [dbo].[Test_GetTemplate] 
	-- Add the parameters for the stored procedure here
	@idMapClient int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT [IdTemplate]
      ,[IdMapClient]
      ,[Section]
      ,[Row]
      ,[Column]
      ,[ParentElement]
      ,[Element]
      ,[Attribute]
      ,[FillWith]
      ,[IdType]
      ,[IsRequeried]
  FROM [Template]
  WHERE [IdMapClient] = @idMapClient
END
GO
USE [master]
GO
ALTER DATABASE [Facturas] SET  READ_WRITE 
GO
