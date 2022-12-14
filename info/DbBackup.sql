USE [master]
GO
/****** Object:  Database [IDCScaleSystem]    Script Date: 10/9/2022 10:37:45 AM ******/
CREATE DATABASE [IDCScaleSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'IDCScaleSystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\IDCScaleSystem.mdf' , SIZE = 204800KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'IDCScaleSystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\IDCScaleSystem_log.ldf' , SIZE = 335872KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [IDCScaleSystem] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [IDCScaleSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [IDCScaleSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [IDCScaleSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [IDCScaleSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET  DISABLE_BROKER 
GO
ALTER DATABASE [IDCScaleSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [IDCScaleSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET RECOVERY FULL 
GO
ALTER DATABASE [IDCScaleSystem] SET  MULTI_USER 
GO
ALTER DATABASE [IDCScaleSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [IDCScaleSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [IDCScaleSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [IDCScaleSystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [IDCScaleSystem] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [IDCScaleSystem] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [IDCScaleSystem] SET QUERY_STORE = OFF
GO
USE [IDCScaleSystem]
GO
/****** Object:  User [idc_ad_system]    Script Date: 10/9/2022 10:37:46 AM ******/
CREATE USER [idc_ad_system] FOR LOGIN [idc_ad_system] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [idc_ad_system]
GO
/****** Object:  Table [dbo].[tblLog]    Script Date: 10/9/2022 10:37:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[MessageTemplate] [nvarchar](max) NULL,
	[Level] [nvarchar](max) NULL,
	[TimeStamp] [datetime] NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblScanData]    Script Date: 10/9/2022 10:37:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblScanData](
	[Id] [uniqueidentifier] NULL,
	[BarcodeString] [nvarchar](500) NULL,
	[OcNo] [nvarchar](100) NULL,
	[ProductNo] [nvarchar](100) NULL,
	[ProductName] [nvarchar](500) NULL,
	[Quantity] [int] NULL,
	[LinePosNo] [nvarchar](50) NULL,
	[Unit] [nvarchar](50) NULL,
	[BoxNo] [nvarchar](50) NULL,
	[CustomerNo] [nvarchar](500) NULL,
	[Location] [int] NULL,
	[BoxPosNo] [nvarchar](50) NULL,
	[Note] [nvarchar](max) NULL,
	[Brand] [nvarchar](50) NULL,
	[Decoration] [int] NULL,
	[MetalScan] [int] NULL,
	[MainProductNo] [nvarchar](500) NULL,
	[Color] [nvarchar](50) NULL,
	[Size] [nvarchar](50) NULL,
	[Weight] [float] NULL,
	[PackingMethod] [nvarchar](50) NULL,
	[LeftWeight] [float] NULL,
	[RightWeight] [float] NULL,
	[BoxType] [nvarchar](50) NULL,
	[ToolingNo] [nvarchar](50) NULL,
	[PackingBoxType] [nvarchar](50) NULL,
	[CustomerUsePlactixBox] [nvarchar](50) NULL,
	[PlacticBox] [int] NULL,
	[PPbagWeight] [float] NULL,
	[BX1Weight] [float] NULL,
	[BX2Weight] [float] NULL,
	[BX3Weight] [float] NULL,
	[BX4Weight] [float] NULL,
	[BX1AWeight] [float] NULL,
	[BX5Weight] [float] NULL,
	[PlacticBoxWeight] [float] NULL,
	[BX1] [int] NULL,
	[BX2] [int] NULL,
	[BX3] [int] NULL,
	[BX4] [int] NULL,
	[BX1A] [int] NULL,
	[BX5] [int] NULL,
	[PEUW] [int] NULL,
	[BagWeight] [float] NULL,
	[PartitionWeight] [float] NULL,
	[QtyBag] [int] NULL,
	[QtyPartition] [int] NULL,
	[QtyWrapSheet] [int] NULL,
	[WrapSheetWeight] [float] NULL,
	[Tolerance] [float] NULL,
	[ToleranceBeforePrint] [float] NULL,
	[ToleranceAfterPrint] [float] NULL,
	[IdLable] [nvarchar](100) NULL,
	[StandardWeight] [float] NULL,
	[CreatedDate] [datetime] NULL,
	[RealWeight] [float] NULL,
	[Pass] [int] NULL,
	[Status] [int] NULL,
	[Actived] [int] NULL,
	[CreatedBy] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUsers]    Script Date: 10/9/2022 10:37:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUsers](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](500) NULL,
	[Password] [nvarchar](max) NULL,
	[Role] [int] NULL,
	[Actived] [int] NULL,
	[CreatedBy] [datetime] NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblAccount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblWinlineProductsInfo]    Script Date: 10/9/2022 10:37:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblWinlineProductsInfo](
	[Id] [uniqueidentifier] NOT NULL,
	[ProductNunmber] [nvarchar](500) NULL,
	[Name] [nvarchar](500) NULL,
	[ProductCategory] [int] NULL,
	[Brand] [nvarchar](500) NULL,
	[Decoration] [int] NULL,
	[MetalScan] [int] NULL,
	[MainProductNo] [nvarchar](100) NULL,
	[Color] [nvarchar](100) NULL,
	[Size] [nvarchar](100) NULL,
	[Weight] [float] NULL,
	[PackingMethod] [nvarchar](100) NULL,
	[LeftWeight] [float] NULL,
	[RightWeight] [float] NULL,
	[BoxType] [nvarchar](50) NULL,
	[ToolingNo] [nvarchar](100) NULL,
	[PackingBoxType] [nvarchar](50) NULL,
	[CustomeUsePb] [nvarchar](500) NULL,
	[PlacticBox] [int] NULL,
	[PPbagWeight] [float] NULL,
	[Bx1Weight] [float] NULL,
	[Bx1AWeight] [float] NULL,
	[Bx2Weight] [float] NULL,
	[Bx3Weight] [float] NULL,
	[Bx4Weight] [float] NULL,
	[Bx5Weight] [float] NULL,
	[Bx1_50_40_34] [int] NULL,
	[Bx1A] [int] NULL,
	[Bx2_50_40_17] [int] NULL,
	[Bx3_41_32_31] [int] NULL,
	[Bx4_32_23_15] [int] NULL,
	[Bx5_41_32_31] [int] NULL,
	[PlaticBoxWeight] [float] NULL,
	[PEUW] [int] NULL,
	[BagWeight] [float] NULL,
	[PartitionWeight] [float] NULL,
	[QtyPerbag] [int] NULL,
	[QtyPerPartition] [int] NULL,
	[QtyPerWrapSheet] [int] NULL,
	[WrapSheetWeight] [float] NULL,
	[Tolerance] [float] NULL,
	[ToleranceBeforePrint] [float] NULL,
	[ToleranceAfterPrint] [float] NULL,
	[ProductFillter] [nvarchar](500) NULL,
	[Actived] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedMachine] [nvarchar](100) NULL,
 CONSTRAINT [PK_tblWinlineProductsInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblScanData] ADD  CONSTRAINT [DF_tblScanData_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[tblScanData] ADD  CONSTRAINT [DF_tblScanData_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[tblScanData] ADD  CONSTRAINT [DF_tblScanData_Status]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[tblScanData] ADD  CONSTRAINT [DF_tblScanData_Actived]  DEFAULT ((1)) FOR [Actived]
GO
ALTER TABLE [dbo].[tblUsers] ADD  CONSTRAINT [DF_tblAccount_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[tblUsers] ADD  CONSTRAINT [DF_tblAccount_Role]  DEFAULT ((0)) FOR [Role]
GO
ALTER TABLE [dbo].[tblUsers] ADD  CONSTRAINT [DF_tblUsers_Actived]  DEFAULT ((1)) FOR [Actived]
GO
ALTER TABLE [dbo].[tblUsers] ADD  CONSTRAINT [DF_tblAccount_CreatedBy]  DEFAULT (getdate()) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[tblWinlineProductsInfo] ADD  CONSTRAINT [DF_tblWinlineProductsInfo_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[tblWinlineProductsInfo] ADD  CONSTRAINT [DF_tblWinlineProductsInfo_MetalScan]  DEFAULT ((1)) FOR [MetalScan]
GO
ALTER TABLE [dbo].[tblWinlineProductsInfo] ADD  CONSTRAINT [DF_tblWinlineProductsInfo_ToleranceBeforePrint]  DEFAULT ((0)) FOR [ToleranceBeforePrint]
GO
ALTER TABLE [dbo].[tblWinlineProductsInfo] ADD  CONSTRAINT [DF_tblWinlineProductsInfo_ToleranceAfterPrint]  DEFAULT ((0)) FOR [ToleranceAfterPrint]
GO
ALTER TABLE [dbo].[tblWinlineProductsInfo] ADD  CONSTRAINT [DF_tblWinlineProductsInfo_Actived]  DEFAULT ((1)) FOR [Actived]
GO
ALTER TABLE [dbo].[tblWinlineProductsInfo] ADD  CONSTRAINT [DF_tblWinlineProductsInfo_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[tblWinlineProductsInfo] ADD  CONSTRAINT [DF_tblWinlineProductsInfo_CreatedMachine]  DEFAULT (host_name()) FOR [CreatedMachine]
GO
/****** Object:  StoredProcedure [dbo].[sp_tblWinlineProductsInfoGet]    Script Date: 10/9/2022 10:37:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_tblWinlineProductsInfoGet] 
	-- Add the parameters for the stored procedure here
	@productCode nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM tblWinlineProductsInfo
	WHERE ProductNunmber = @productCode
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tblWinlineProductsInfoGetId]    Script Date: 10/9/2022 10:37:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_tblWinlineProductsInfoGetId] 
	-- Add the parameters for the stored procedure here
	@Id uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM tblWinlineProductsInfo
	WHERE Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tblWinlineProductsInfoGets]    Script Date: 10/9/2022 10:37:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_tblWinlineProductsInfoGets]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM tblWinlineProductsInfo
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tblWinlineProductsInfoInsert]    Script Date: 10/9/2022 10:37:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_tblWinlineProductsInfoInsert]
	-- Add the parameters for the stored procedure here
	@ProductNunmber nvarchar(500),
	@Name nvarchar(500),
	@ProductCategory int,
	@Brand nvarchar(100),
	@Decoration int,
	@MetalScan int,
	@MainProductNo nvarchar(100),
	@Color nvarchar(100),
	@Size nvarchar(100),
	@Weight float,
	@PackingMethod nvarchar(100),
	@LeftWeight float,
	@RightWeight float,
	@BoxType nvarchar(100),
	@ToolingNo nvarchar(100),
	@PackingBoxType nvarchar(100),
	@CustomeUsePb nvarchar(100),
	@PlacticBox nvarchar(100),
	@PPbagWeight float,
	@Bx1Weight float,
	@Bx1AWeight float,
	@Bx2Weight float,
	@Bx3Weight float,
	@Bx4Weight float,
	@Bx5Weight float,
	@Bx1_50_40_34 int,
	@Bx1A int,
	@Bx2_50_40_17 int,
	@Bx3_41_32_31 int,
	@Bx4_32_23_15 int,
	@Bx5_41_32_31 int,
	@PlaticBoxWeight float,
	@PEUW int,
	@BagWeight float,
	@PartitionWeight float,
	@QtyPerbag int,
	@QtyPerPartition int,
	@QtyPerWrapSheet int,
	@WrapSheetWeight float,
	@Tolerance float,
	@ToleranceBeforePrint float,
	@ToleranceAfterPrint float,
	@ProductFillter nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[tblWinlineProductsInfo]
           (
           [ProductNunmber]
           ,[Name]
		   ,[ProductCategory]
           ,[Brand]
           ,[Decoration]
           ,[MetalScan]
           ,[MainProductNo]
           ,[Color]
           ,[Size]
           ,[Weight]
           ,[PackingMethod]
           ,[LeftWeight]
           ,[RightWeight]
           ,[BoxType]
           ,[ToolingNo]
           ,[PackingBoxType]
           ,[CustomeUsePb]
           ,[PlacticBox]
           ,[PPbagWeight]
           ,[Bx1Weight]
           ,[Bx1AWeight]
           ,[Bx2Weight]
           ,[Bx3Weight]
           ,[Bx4Weight]
           ,[Bx5Weight]
           ,[Bx1_50_40_34]
           ,[Bx1A]
           ,[Bx2_50_40_17]
           ,[Bx3_41_32_31]
           ,[Bx4_32_23_15]
           ,[Bx5_41_32_31]
           ,[PlaticBoxWeight]
           ,[PEUW]
           ,[BagWeight]
           ,[PartitionWeight]
           ,[QtyPerbag]
           ,[QtyPerPartition]
           ,[QtyPerWrapSheet]
           ,[WrapSheetWeight]
           ,[Tolerance]
           ,[ToleranceBeforePrint]
           ,[ToleranceAfterPrint]
           ,[ProductFillter]
           )
     VALUES
           (
				@ProductNunmber,
	@Name ,
	@ProductCategory,
	@Brand ,
	@Decoration ,
	@MetalScan ,
	@MainProductNo ,
	@Color ,
	@Size ,
	@Weight ,
	@PackingMethod ,
	@LeftWeight ,
	@RightWeight ,
	@BoxType ,
	@ToolingNo ,
	@PackingBoxType ,
	@CustomeUsePb,
	@PlacticBox ,
	@PPbagWeight ,
	@Bx1Weight ,
	@Bx1AWeight ,
	@Bx2Weight ,
	@Bx3Weight ,
	@Bx4Weight ,
	@Bx5Weight ,
	@Bx1_50_40_34 ,
	@Bx1A ,
	@Bx2_50_40_17 ,
	@Bx3_41_32_31 ,
	@Bx4_32_23_15 ,
	@Bx5_41_32_31 ,
	@PlaticBoxWeight ,
	@PEUW ,
	@BagWeight ,
	@PartitionWeight ,
	@QtyPerbag ,
	@QtyPerPartition ,
	@QtyPerWrapSheet ,
	@WrapSheetWeight,
	@Tolerance,
	@ToleranceBeforePrint,
	@ToleranceAfterPrint,
	@ProductFillter
		   )
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tblWinlineProductsInfoUpdateTolerance]    Script Date: 10/9/2022 10:37:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_tblWinlineProductsInfoUpdateTolerance]
	-- Add the parameters for the stored procedure here
	@Id uniqueidentifier,
	@Tolerance float,
	@ToleranceBeforePrint float,
	@ToleranceAfterPrint float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
    -- Insert statements for procedure here
	update tblWinlineProductsInfo set Tolerance = @Tolerance, ToleranceBeforePrint = @ToleranceBeforePrint, ToleranceAfterPrint = @ToleranceAfterPrint
	where Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UsersLogin]    Script Date: 10/9/2022 10:37:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_UsersLogin]
	-- Add the parameters for the stored procedure here
	@userName nvarchar(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from tblUsers where Actived = 1 and UserName = @userName
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'P/L/R' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-fVN; 2-fFT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'Location'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-Yes; 0-No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'Decoration'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1- khong quet; 0- quet' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'MetalScan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'LeftWeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'RightWeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plactic box' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'PackingBoxType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'PlacticBox'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'PPbagWeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'BX1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'BX2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'BX3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'BX4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'BX1A'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'BX5'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'PEUW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs. Qty/Bag' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'QtyBag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs. Qty/Partition' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'QtyPartition'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs. Qty/WrapSheet' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'QtyWrapSheet'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1- pass; 0-fail' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'Pass'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Báo trạng thái: 0- thùng fail; 1- chờ đi sơn; 2- Done-hàng FG qua kho Kerry. Ở trạm IDC check nêu hàng noPrinting thì set =2. nếu printing set =1. Khi hàng đi sơn về, vào trạm check afterPrinting, quét OK set =2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblScanData', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0-Operator; 1-Admin' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblUsers', @level2type=N'COLUMN',@level2name=N'Role'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'=1 là heelCounter' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'ProductCategory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Printing. 0-No; 1-Yes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'Decoration'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0-không quét; 1-quét' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'MetalScan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'LeftWeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'RightWeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PackingBox Type(plastic box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'PackingBoxType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pb: plactic box' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'CustomeUsePb'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'PlacticBox'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'PPbagWeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'Bx1Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'Bx1AWeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'Bx2Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'Bx3Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'Bx4Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'Bx5Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'Bx1_50_40_34'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'Bx1A'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'Bx2_50_40_17'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'Bx3_41_32_31'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'Bx4_32_23_15'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'Bx5_41_32_31'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'PlaticBoxWeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'PEUW'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'BagWeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'PartitionWeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'QtyPerbag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'QtyPerPartition'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'QtyPerWrapSheet'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gram' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblWinlineProductsInfo', @level2type=N'COLUMN',@level2name=N'WrapSheetWeight'
GO
USE [master]
GO
ALTER DATABASE [IDCScaleSystem] SET  READ_WRITE 
GO
