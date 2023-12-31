USE [master]
GO
/****** Object:  Database [webapp] ******/
CREATE DATABASE [webapp]
 CONTAINMENT = NONE
 ON  PRIMARY 
( name = N'webapp', filename = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\webapp.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( name = N'webapp_log', filename = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\webapp_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [webapp] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [webapp].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [webapp] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [webapp] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [webapp] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [webapp] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [webapp] SET ARITHABORT OFF 
GO
ALTER DATABASE [webapp] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [webapp] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [webapp] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [webapp] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [webapp] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [webapp] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [webapp] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [webapp] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [webapp] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [webapp] SET  DISABLE_BROKER 
GO
ALTER DATABASE [webapp] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [webapp] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [webapp] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [webapp] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [webapp] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [webapp] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [webapp] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [webapp] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [webapp] SET  MULTI_USER 
GO
ALTER DATABASE [webapp] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [webapp] SET DB_CHAINING OFF 
GO
ALTER DATABASE [webapp] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [webapp] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [webapp] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [webapp] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [webapp] SET QUERY_STORE = ON
GO
ALTER DATABASE [webapp] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [webapp]
GO


/****** Object:  Table [dbo].[audit_trails] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[audit_trails](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime] NOT NULL,
	[script] [nvarchar](255) NULL,
	[username] [nvarchar](255) NULL,
	[action] [nvarchar](255) NULL,
	[entity] [nvarchar](255) NULL,
	[field] [nvarchar](255) NULL,
	[key_value] [nvarchar](max) NULL,
	[old_value] [nvarchar](max) NULL,
	[new_value] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/****** Object:  Table [dbo].[dishes] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dishes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NULL,
	[description] [ntext] NULL,
	[image] [nvarchar](255) NULL,
	[price] [float] NULL,
	[restaurant_id] [int] NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
	[created_by] [int] NULL,
	[updated_by] [int] NULL,
 CONSTRAINT [PK_dishes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/****** Object:  Table [dbo].[export_logs] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[export_logs](
	[file_id] [uniqueidentifier] NOT NULL,
	[created_at] [datetime] NOT NULL,
	[username] [nvarchar](255) NOT NULL,
	[export_type] [nvarchar](255) NOT NULL,
	[entity] [nvarchar](255) NOT NULL,
	[key_value] [nvarchar](255) NULL,
	[filename] [nvarchar](255) NOT NULL,
	[request] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[file_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/****** Object:  Table [dbo].[permissions] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[permissions](
	[role_id] [int] NOT NULL,
	[entity] [nvarchar](255) NOT NULL,
	[permission] [int] NOT NULL,
 CONSTRAINT [PK_permissions] PRIMARY KEY CLUSTERED 
(
	[role_id] ASC,
	[entity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[restaurants] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[restaurants](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NULL,
	[description] [ntext] NULL,
	[image] [nvarchar](255) NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
	[created_by] [int] NULL,
	[updated_by] [int] NULL,
 CONSTRAINT [PK_restaurants] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/****** Object:  Table [dbo].[roles] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[roles](
	[id] [int] NOT NULL,
	[role_name] [nvarchar](255) NOT NULL,
	[description] [nvarchar](255) NULL,
	[type] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[users] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[first_name] [nvarchar](255) NULL,
	[last_name] [nvarchar](255) NULL,
	[username] [nvarchar](255) NULL,
	[password] [nvarchar](255) NULL,
	[email] [varchar](255) NULL,
	[mobile] [varchar](255) NULL,
	[photo] [nvarchar](255) NULL,
	[role_id] [int] NULL,
	[provider] [varchar](255) NULL,
	[activated] [nvarchar](255) NULL,
	[profile] [ntext] NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


ALTER TABLE [dbo].[dishes] ADD  CONSTRAINT [DF_dishes_price]  DEFAULT ((0)) FOR [price]
GO
ALTER TABLE [dbo].[dishes] ADD  CONSTRAINT [DF_dishes_restaurant_id]  DEFAULT ((0)) FOR [restaurant_id]
GO
ALTER TABLE [dbo].[dishes] ADD  CONSTRAINT [DF_dishes_created_at]  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[dishes] ADD  CONSTRAINT [DF_dishes_updated_at]  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[dishes] ADD  CONSTRAINT [DF_dishes_created_by]  DEFAULT ((0)) FOR [created_by]
GO
ALTER TABLE [dbo].[dishes] ADD  CONSTRAINT [DF_dishes_updated_by]  DEFAULT ((0)) FOR [updated_by]
GO
ALTER TABLE [dbo].[restaurants] ADD  CONSTRAINT [DF_restaurants_created_at]  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[restaurants] ADD  CONSTRAINT [DF_restaurants_updated_at]  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[restaurants] ADD  CONSTRAINT [DF_restaurants_created_by]  DEFAULT ((0)) FOR [created_by]
GO
ALTER TABLE [dbo].[restaurants] ADD  CONSTRAINT [DF_restaurants_updated_by]  DEFAULT ((0)) FOR [updated_by]
GO
ALTER TABLE [dbo].[users] ADD  CONSTRAINT [DF_users_role_id]  DEFAULT ((0)) FOR [role_id]
GO
ALTER TABLE [dbo].[users] ADD  CONSTRAINT [DF_users_created_at]  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[users] ADD  CONSTRAINT [DF_users_updated_at]  DEFAULT (getdate()) FOR [updated_at]
GO


SET IDENTITY_INSERT [dbo].[dishes] ON 

INSERT [dbo].[dishes] ([id], [name], [description], [image], [price], [restaurant_id], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (1, N'Combo 1 – Arroz Blanco + Hab. Rojas + Pollo Asado + Ens. Rusa o Ens. Verde + Guarnicion del Dia.', N'<p>Nota las guarnición y ensalada puede variar de acuerdo a disponibilidad. Es una cajita de sorpresas.</p>
', N'4218ca1d09174218364162cd0b1a8cc1.jpeg', 395, 3, GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[dishes] ([id], [name], [description], [image], [price], [restaurant_id], [created_at], [updated_at], [created_by], [updated_by])
VALUES (2, N'Bowl 3', N'<p>Arroz Frito Chino + Pechurinas al Limon, Aguacate y Platano Maduro</p>
', N'4218ca1d09174218364162cd0b1a8cc1 (1).jpeg', 550, 3, GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[dishes] ([id], [name], [description], [image], [price], [restaurant_id], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (3, N'Combo 6 - Sancocho + Arroz Blanco.', N'<p>Nota las guarnición y ensalada puede variar de acuerdo a disponibilidad. Es una cajita de sorpresas.</p>
', N'b4facf495c22df52f3ca635379ebe613.jpeg', 495, 3, GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[dishes] ([id], [name], [description], [image], [price], [restaurant_id], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (4, N'Combo Lasagna', N'<p>Lasagna, Tostones o Papas Fritas, Ensalada y una Bebida.</p>
', N'54f190e8-2a79-4966-adea-955109cc6736.jpeg', 550, 2, GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[dishes] ([id], [name], [description], [image], [price], [restaurant_id], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (5, N'La Bandera', N'<p>Arroz, Guisante, Pollo, Ensalada.</p>
', N'c2959e45a9c16d6decbecb22aad6f0c1.jpg', 300, 2, GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[dishes] ([id], [name], [description], [image], [price], [restaurant_id], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (6, N'Combo Ejecutivo', N'<p>Arroz, habichuelas, carne, ensalda y maduros</p>
', N'platos-del-dia-6.jpeg', 550, 2, GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[dishes] ([id], [name], [description], [image], [price], [restaurant_id], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (7, N'Combo 1', N'<p>Plato del dia</p>
', N'348s.jpg', 660, 1, GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[dishes] ([id], [name], [description], [image], [price], [restaurant_id], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (8, N'Combo 2', N'<p>Plato del dia</p>
', N'66ceb-plato-dominicano.jpg', 220, 1,GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[dishes] ([id], [name], [description], [image], [price], [restaurant_id], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (9, N'Combo 3', N'<p>Plato del dia</p>
', N'featured-img-of-post-158197.jpg', 275, 1, GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[dishes] ([id], [name], [description], [image], [price], [restaurant_id], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (10, N'Combo Manjar 2', N'<p>Arroz, Habichuelas, Carne, Ensalada, y Extra</p>
', N'4218ca1d09174218364162cd0b1a8cc1(2).jpeg', 280, 1, GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[dishes] ([id], [name], [description], [image], [price], [restaurant_id], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (11, N'Chicharron Light - 1 Libra con Batata Frita', N'<ul>
	<li>Chicharron Light</li>
	<li>1 Libra con Batata Frita</li>
</ul>
', N'859baff1d76042a45e319d1de80aec7a.jpeg', 695, 4, GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[dishes] ([id], [name], [description], [image], [price], [restaurant_id], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (12, N'Chicharrón Light - 1 Libra con Yuca', N'<ul>
	<li>Chicharrón Light</li>
	<li>1 Libra con Yuca</li>
</ul>
', N'yuca_con_chicharron_1981-reg.jpg', 695, 4, GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[dishes] ([id], [name], [description], [image], [price], [restaurant_id], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (13, N'Chicharrón Light - 1 Libra con Yuca Frita', N'<ul>
	<li>Chicharrón Light</li>
	<li>1 Libra con Yuca Frita</li>
	<li>Ensalada</li>
</ul>
', N'Yuca frita con chicharon.jpg', 750, 4, GETDATE(), GETDATE(), 1, 1)
SET IDENTITY_INSERT [dbo].[dishes] OFF
GO


INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (-2, N'{89456192-A767-4B60-B043-591A4AA6A5D7}audit_trails', 0)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (-2, N'{89456192-A767-4B60-B043-591A4AA6A5D7}export_logs', 0)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (-2, N'{89456192-A767-4B60-B043-591A4AA6A5D7}permissions', 0)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (-2, N'{89456192-A767-4B60-B043-591A4AA6A5D7}roles', 0)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (-2, N'{89456192-A767-4B60-B043-591A4AA6A5D7}users', 0)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (0, N'{89456192-A767-4B60-B043-591A4AA6A5D7}audit_trails', 0)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (0, N'{89456192-A767-4B60-B043-591A4AA6A5D7}dishes', 360)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (0, N'{89456192-A767-4B60-B043-591A4AA6A5D7}export_logs', 0)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (0, N'{89456192-A767-4B60-B043-591A4AA6A5D7}permissions', 0)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (0, N'{89456192-A767-4B60-B043-591A4AA6A5D7}restaurants', 360)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (0, N'{89456192-A767-4B60-B043-591A4AA6A5D7}roles', 0)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (0, N'{89456192-A767-4B60-B043-591A4AA6A5D7}users', 12)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (1, N'{89456192-A767-4B60-B043-591A4AA6A5D7}audit_trails', 0)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (1, N'{89456192-A767-4B60-B043-591A4AA6A5D7}dishes', 1519)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (1, N'{89456192-A767-4B60-B043-591A4AA6A5D7}export_logs', 0)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (1, N'{89456192-A767-4B60-B043-591A4AA6A5D7}permissions', 0)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (1, N'{89456192-A767-4B60-B043-591A4AA6A5D7}restaurants', 1519)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (1, N'{89456192-A767-4B60-B043-591A4AA6A5D7}roles', 0)
INSERT [dbo].[permissions] ([role_id], [entity], [permission]) VALUES (1, N'{89456192-A767-4B60-B043-591A4AA6A5D7}users', 12)
GO


SET IDENTITY_INSERT [dbo].[restaurants] ON 

INSERT [dbo].[restaurants] ([id], [name], [description], [image], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (1, N'Hecho en Casa', N'<p>Comida criolla y catering por encargo.</p>
', N'hecho-en-casa.gif', GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[restaurants] ([id], [name], [description], [image], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (2, N'Restaurante Delicias de mi Cocina', N'<p>Buffet, Pasteles en hoja, niños envueltos, picaderas en general buffet para todas ocaciones.</p>
', N'delicias-de-mi-cocina.jpg', GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[restaurants] ([id], [name], [description], [image], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (3, N'El Chef', N'<p>Buffet, comida empresarial, catering y picaderas</p>
', N'logo_chef_nuevo.png', GETDATE(), GETDATE(), 1, 1)
INSERT [dbo].[restaurants] ([id], [name], [description], [image], [created_at], [updated_at], [created_by], [updated_by]) 
VALUES (4, N'La Zona Chicharrón Light', N'<p>Calle José Reyes, No. 56<br>
Esquina Calle Conde, Zona Colonial</p>
', N'93b5fd796682c6d5302cd5bec164fe90.jpeg', GETDATE(), GETDATE(), 1, 1)
SET IDENTITY_INSERT [dbo].[restaurants] OFF
GO


INSERT [dbo].[roles] ([id], [role_name], [description], [type]) VALUES (-2, N'Public', N'Rol predeterminado otorgado a un usuario no autenticado.', N'public')
INSERT [dbo].[roles] ([id], [role_name], [description], [type]) VALUES (-1, N'Administrator', N'Rol predeterminado dado a un administrador.', N'administrator')
INSERT [dbo].[roles] ([id], [role_name], [description], [type]) VALUES (0, N'Authenticated', N'Rol predeterminado dado a la usuario autenticada.', N'authenticated')
INSERT [dbo].[roles] ([id], [role_name], [description], [type]) VALUES (1, N'Editor', N'Los editores pueden administrar y publicar contenidos, incluidos los de otros usuarios.', N'editor')
GO


SET IDENTITY_INSERT [dbo].[users] ON 

INSERT [dbo].[users] ([id], [first_name], [last_name], [username], [password], [email], [mobile], [photo], [role_id], [provider], [activated], [profile], [created_at], [updated_at]) 
VALUES (1, N'Admin', N'User', N'admin', N'$2a$11$0R8hIT9MXNe0MRE5.Bd1PeONhUh.itJM7fZidqSp6tw.WtM7onVzO', N'edwinet+admin@gmail.com', NULL, NULL, -1, NULL, NULL, NULL, GETDATE(), GETDATE())
INSERT [dbo].[users] ([id], [first_name], [last_name], [username], [password], [email], [mobile], [photo], [role_id], [provider], [activated], [profile], [created_at], [updated_at]) 
VALUES (2, N'Editor', N'User', N'editor', N'$2a$11$dKmiadtEkhWbWwHcgJnRF.UOwSPDjcxlX9NvodG2uKITrOsrL7r8i', N'edwinet+editor@gmail.com', NULL, NULL, 1, NULL, NULL, NULL, GETDATE(), GETDATE())
INSERT [dbo].[users] ([id], [first_name], [last_name], [username], [password], [email], [mobile], [photo], [role_id], [provider], [activated], [profile], [created_at], [updated_at]) 
VALUES (3, N'Edwin', N'Fernandez', N'user', N'$2a$11$8dHKxHwAR4DujBQvJS9msuqMB0kzHHmzSY9675iGNkMZmdUumGgSW', N'edwinet+user@gmail.com', NULL, NULL, 0, NULL, NULL, NULL, GETDATE(), GETDATE())
SET IDENTITY_INSERT [dbo].[users] OFF
GO


USE [master]
GO
ALTER DATABASE [webapp] SET  READ_WRITE 
GO
