USE [master]
GO
/****** Object:  Database [KiddyCheckDB]    Script Date: 23/04/2024 02:28:54 p. m. ******/
CREATE DATABASE [KiddyCheckDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'KiddyCheckDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\KiddyCheckDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'KiddyCheckDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\KiddyCheckDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [KiddyCheckDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [KiddyCheckDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [KiddyCheckDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [KiddyCheckDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [KiddyCheckDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [KiddyCheckDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [KiddyCheckDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET RECOVERY FULL 
GO
ALTER DATABASE [KiddyCheckDB] SET  MULTI_USER 
GO
ALTER DATABASE [KiddyCheckDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [KiddyCheckDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [KiddyCheckDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [KiddyCheckDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [KiddyCheckDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [KiddyCheckDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'KiddyCheckDB', N'ON'
GO
ALTER DATABASE [KiddyCheckDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [KiddyCheckDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [KiddyCheckDB]
GO
/****** Object:  Table [dbo].[Permisos]    Script Date: 23/04/2024 02:28:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permisos](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Permiso] [varchar](50) NOT NULL,
	[PermisoPapaId] [bigint] NOT NULL,
	[Orden] [bigint] NOT NULL,
	[Vista] [varchar](100) NULL,
	[EsPapa] [bit] NULL,
 CONSTRAINT [PK_Permisos_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 23/04/2024 02:28:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Roles_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuarioPermisos]    Script Date: 23/04/2024 02:28:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioPermisos](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdPermiso] [bigint] NOT NULL,
 CONSTRAINT [PK_UsuarioPermisos_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 23/04/2024 02:28:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NombreCompleto] [nvarchar](150) NOT NULL,
	[Correo] [nvarchar](300) NOT NULL,
	[PasswordHash] [varbinary](max) NOT NULL,
	[PasswordSalt] [varbinary](max) NOT NULL,
	[NombreUsuario] [nvarchar](100) NOT NULL,
	[UltimoAcceso] [datetime] NOT NULL,
	[ChangePassword] [bit] NULL,
	[UserAlt] [int] NULL,
	[FechaAlt] [datetime] NULL,
	[UserUpd] [int] NULL,
	[FechaUpd] [datetime] NULL,
	[Activo] [bit] NULL,
 CONSTRAINT [PK_Usuarios_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuariosRoles]    Script Date: 23/04/2024 02:28:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuariosRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdRol] [int] NOT NULL,
 CONSTRAINT [PK_UsuariosRoles_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Permisos] ON 

INSERT [dbo].[Permisos] ([Id], [Permiso], [PermisoPapaId], [Orden], [Vista], [EsPapa]) VALUES (1, N'Usuario', 0, 1, N'Usuarios', 0)
SET IDENTITY_INSERT [dbo].[Permisos] OFF
GO
SET IDENTITY_INSERT [dbo].[UsuarioPermisos] ON 

INSERT [dbo].[UsuarioPermisos] ([Id], [IdUsuario], [IdPermiso]) VALUES (1, 1, 1)
SET IDENTITY_INSERT [dbo].[UsuarioPermisos] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuarios] ON 

INSERT [dbo].[Usuarios] ([Id], [NombreCompleto], [Correo], [PasswordHash], [PasswordSalt], [NombreUsuario], [UltimoAcceso], [ChangePassword], [UserAlt], [FechaAlt], [UserUpd], [FechaUpd], [Activo]) VALUES (1, N'Directorior General', N'Director@gmail.com', 0xACDEE6E25014F03B5D84685CD669E5751D7973B32A8EB1BD3DD23977DE359CC02E3DF47388E3D6262664009CE2F96EF1F2EDD9472C6420001E1579A09F82A619, 0xFBD6959377659C93305782DBA0A8D15C07F113B541E3866CED5A55238F2208DB93E9520E0EA71C8B3B6C7D7B883C7903B8CB9FCF2DF399AF245D5A5572AB35D776524F9ADFFB36A1D268789487EFAE5D1E5DB2BD45C0EA627C00665B05E07B0E5BA48F2FFD503E67D903FD81C584E6918C4F3306C2B9E32D8467A3DB24D3E682, N'director', CAST(N'2024-04-23T13:46:17.583' AS DateTime), NULL, 1, CAST(N'2024-04-23T13:44:59.560' AS DateTime), NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Usuarios] OFF
GO
USE [master]
GO
ALTER DATABASE [KiddyCheckDB] SET  READ_WRITE 
GO
