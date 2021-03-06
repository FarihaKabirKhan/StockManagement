USE [inventoryDB]
GO
/****** Object:  Database [inventoryDB]    Script Date: 9/16/2021 6:13:25 AM ******/
CREATE DATABASE [inventoryDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'inventoryDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\inventoryDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'inventoryDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\inventoryDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [inventoryDB] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [inventoryDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [inventoryDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [inventoryDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [inventoryDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [inventoryDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [inventoryDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [inventoryDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [inventoryDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [inventoryDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [inventoryDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [inventoryDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [inventoryDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [inventoryDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [inventoryDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [inventoryDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [inventoryDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [inventoryDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [inventoryDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [inventoryDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [inventoryDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [inventoryDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [inventoryDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [inventoryDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [inventoryDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [inventoryDB] SET  MULTI_USER 
GO
ALTER DATABASE [inventoryDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [inventoryDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [inventoryDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [inventoryDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [inventoryDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [inventoryDB] SET QUERY_STORE = OFF
GO
USE [inventoryDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [inventoryDB]
GO
/****** Object:  Table [dbo].[tblCustomer]    Script Date: 9/16/2021 6:13:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCustomer](
	[customerID] [int] IDENTITY(1,1) NOT NULL,
	[customerName] [varchar](30) NOT NULL,
	[customerAddress] [varchar](50) NOT NULL,
	[email] [varchar](30) NULL,
	[phoneNumber] [varchar](30) NOT NULL,
 CONSTRAINT [PK__tblCusto__B611CB9D9A8CD41D] PRIMARY KEY CLUSTERED 
(
	[customerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProduct]    Script Date: 9/16/2021 6:13:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProduct](
	[productID] [int] IDENTITY(1,1) NOT NULL,
	[productName] [varchar](25) NOT NULL,
	[purchasePrice] [decimal](8, 2) NULL,
	[categoryID] [int] NOT NULL,
	[salesPrice] [decimal](8, 2) NOT NULL,
	[ImagePath] [nvarchar](50) NULL,
	[status] [varchar](20) NULL,
 CONSTRAINT [PK__tblProdu__2D10D14A6F547FD8] PRIMARY KEY CLUSTERED 
(
	[productID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProductCategory]    Script Date: 9/16/2021 6:13:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProductCategory](
	[categoryID] [int] IDENTITY(1,1) NOT NULL,
	[categoryName] [varchar](30) NOT NULL,
 CONSTRAINT [PK__tblProdu__23CAF1F8A6820113] PRIMARY KEY CLUSTERED 
(
	[categoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPurchase]    Script Date: 9/16/2021 6:13:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPurchase](
	[purchaseID] [int] IDENTITY(1,1) NOT NULL,
	[productID] [int] NOT NULL,
	[supplierID] [int] NOT NULL,
	[warehouseID] [int] NOT NULL,
	[purchasePrice] [decimal](8, 2) NOT NULL,
	[quantity] [int] NOT NULL,
	[totalPrice] [decimal](8, 2) NOT NULL,
	[purchaseDate] [datetime] NOT NULL,
	[description] [varchar](50) NULL,
 CONSTRAINT [PK__tblPurch__0261224CC8ED0EF8] PRIMARY KEY CLUSTERED 
(
	[purchaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSales]    Script Date: 9/16/2021 6:13:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSales](
	[salesID] [int] IDENTITY(1,1) NOT NULL,
	[productID] [int] NOT NULL,
	[customerID] [int] NOT NULL,
	[productStatus] [varchar](30) NOT NULL,
	[quantity] [int] NOT NULL,
	[rate] [decimal](8, 2) NOT NULL,
	[totalPrice] [decimal](8, 2) NOT NULL,
	[purchaseDate] [date] NOT NULL,
	[vat] [int] NULL,
	[discount] [int] NULL,
	[netTotal] [decimal](8, 2) NOT NULL,
	[transactionType] [varchar](15) NULL,
	[transactionDate] [datetime] NOT NULL,
	[invoiceNo] [varchar](50) NULL,
 CONSTRAINT [PK__tblSales__E31CBA90DBEF2323] PRIMARY KEY CLUSTERED 
(
	[salesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblStock]    Script Date: 9/16/2021 6:13:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblStock](
	[stockID] [int] IDENTITY(1,1) NOT NULL,
	[productID] [int] NOT NULL,
	[purchaseID] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[status] [varchar](30) NULL,
 CONSTRAINT [PK__tblStock__CBAD8743C6675D23] PRIMARY KEY CLUSTERED 
(
	[stockID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSupplier]    Script Date: 9/16/2021 6:13:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSupplier](
	[supplierID] [int] IDENTITY(1,1) NOT NULL,
	[supplierName] [varchar](30) NOT NULL,
	[mobileNumber] [int] NULL,
	[address] [varchar](50) NULL,
	[balance] [decimal](8, 2) NULL,
	[managerName] [varchar](30) NULL,
 CONSTRAINT [PK__tblSuppl__DB8E62CD7C883EA9] PRIMARY KEY CLUSTERED 
(
	[supplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUser]    Script Date: 9/16/2021 6:13:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUser](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NULL,
	[UserPass] [varchar](50) NULL,
	[UserType] [varchar](50) NULL,
 CONSTRAINT [PK__tblUser__1788CCAC081E2091] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUserRole]    Script Date: 9/16/2021 6:13:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUserRole](
	[UserRoleID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[PageName] [varchar](50) NULL,
	[IsCreate] [bit] NULL,
	[IsRead] [bit] NULL,
	[IsUpdate] [bit] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK__tblUserR__3D978A55C2036FF6] PRIMARY KEY CLUSTERED 
(
	[UserRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblWarehouse]    Script Date: 9/16/2021 6:13:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblWarehouse](
	[warehouseID] [int] IDENTITY(1,1) NOT NULL,
	[warehouseNo] [int] NOT NULL,
	[warehouseName] [varchar](50) NULL,
	[warehouseManager] [varchar](50) NULL,
	[address] [varchar](30) NULL,
 CONSTRAINT [PK__tblWareh__102CD5F7EA01D512] PRIMARY KEY CLUSTERED 
(
	[warehouseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblPurchase]  WITH CHECK ADD  CONSTRAINT [FK_tblPurchase_tblProduct] FOREIGN KEY([productID])
REFERENCES [dbo].[tblProduct] ([productID])
GO
ALTER TABLE [dbo].[tblPurchase] CHECK CONSTRAINT [FK_tblPurchase_tblProduct]
GO
ALTER TABLE [dbo].[tblPurchase]  WITH CHECK ADD  CONSTRAINT [FK_tblPurchase_tblSupplier] FOREIGN KEY([supplierID])
REFERENCES [dbo].[tblSupplier] ([supplierID])
GO
ALTER TABLE [dbo].[tblPurchase] CHECK CONSTRAINT [FK_tblPurchase_tblSupplier]
GO
ALTER TABLE [dbo].[tblPurchase]  WITH CHECK ADD  CONSTRAINT [FK_tblPurchase_tblWarehouse] FOREIGN KEY([warehouseID])
REFERENCES [dbo].[tblWarehouse] ([warehouseID])
GO
ALTER TABLE [dbo].[tblPurchase] CHECK CONSTRAINT [FK_tblPurchase_tblWarehouse]
GO
ALTER TABLE [dbo].[tblSales]  WITH CHECK ADD  CONSTRAINT [FK_tblSales_tblCustomer] FOREIGN KEY([customerID])
REFERENCES [dbo].[tblCustomer] ([customerID])
GO
ALTER TABLE [dbo].[tblSales] CHECK CONSTRAINT [FK_tblSales_tblCustomer]
GO
ALTER TABLE [dbo].[tblSales]  WITH CHECK ADD  CONSTRAINT [FK_tblSales_tblProduct] FOREIGN KEY([productID])
REFERENCES [dbo].[tblProduct] ([productID])
GO
ALTER TABLE [dbo].[tblSales] CHECK CONSTRAINT [FK_tblSales_tblProduct]
GO
ALTER TABLE [dbo].[tblStock]  WITH CHECK ADD  CONSTRAINT [FK_tblStock_tblProduct] FOREIGN KEY([productID])
REFERENCES [dbo].[tblProduct] ([productID])
GO
ALTER TABLE [dbo].[tblStock] CHECK CONSTRAINT [FK_tblStock_tblProduct]
GO
ALTER TABLE [dbo].[tblStock]  WITH CHECK ADD  CONSTRAINT [FK_tblStock_tblPurchase] FOREIGN KEY([purchaseID])
REFERENCES [dbo].[tblPurchase] ([purchaseID])
GO
ALTER TABLE [dbo].[tblStock] CHECK CONSTRAINT [FK_tblStock_tblPurchase]
GO
ALTER TABLE [dbo].[tblUserRole]  WITH CHECK ADD  CONSTRAINT [FK_tblUserRole_tblUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[tblUser] ([UserID])
GO
ALTER TABLE [dbo].[tblUserRole] CHECK CONSTRAINT [FK_tblUserRole_tblUser]
GO
USE [master]
GO
ALTER DATABASE [inventoryDB] SET  READ_WRITE 
GO
