SET NUMERIC_ROUNDABORT OFF
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[AspNetRoleClaims]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[AspNetRoleClaims]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [RoleId] [nvarchar](450) COLLATE Hebrew_CI_AS NOT NULL,
    [ClaimType] [nvarchar](max) COLLATE Hebrew_CI_AS NULL,
    [ClaimValue] [nvarchar](max) COLLATE Hebrew_CI_AS NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[AspNetRoles]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[AspNetRoles]
(
    [Id] [nvarchar](450) COLLATE Hebrew_CI_AS NOT NULL,
    [Name] [nvarchar](256) COLLATE Hebrew_CI_AS NULL,
    [NormalizedName] [nvarchar](256) COLLATE Hebrew_CI_AS NULL,
    [ConcurrencyStamp] [nvarchar](max) COLLATE Hebrew_CI_AS NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[AspNetUserClaims]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[AspNetUserClaims]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar](450) COLLATE Hebrew_CI_AS NOT NULL,
    [ClaimType] [nvarchar](max) COLLATE Hebrew_CI_AS NULL,
    [ClaimValue] [nvarchar](max) COLLATE Hebrew_CI_AS NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[AspNetUserLogins]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[AspNetUserLogins]
(
    [LoginProvider] [nvarchar](128) COLLATE Hebrew_CI_AS NOT NULL,
    [ProviderKey] [nvarchar](128) COLLATE Hebrew_CI_AS NOT NULL,
    [ProviderDisplayName] [nvarchar](max) COLLATE Hebrew_CI_AS NULL,
    [UserId] [nvarchar](450) COLLATE Hebrew_CI_AS NOT NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[AspNetUserRoles]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[AspNetUserRoles]
(
    [UserId] [nvarchar](450) COLLATE Hebrew_CI_AS NOT NULL,
    [RoleId] [nvarchar](450) COLLATE Hebrew_CI_AS NOT NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[AspNetUserTokens]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[AspNetUserTokens]
(
    [UserId] [nvarchar](450) COLLATE Hebrew_CI_AS NOT NULL,
    [LoginProvider] [nvarchar](128) COLLATE Hebrew_CI_AS NOT NULL,
    [Name] [nvarchar](128) COLLATE Hebrew_CI_AS NOT NULL,
    [Value] [nvarchar](max) COLLATE Hebrew_CI_AS NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[Branch]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[Branch]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [AddressId] [int] NOT NULL,
    [Name] [nvarchar](100) COLLATE Hebrew_CI_AS NOT NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[AspNetUsers]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[AspNetUsers]
(
    [Id] [nvarchar](450) COLLATE Hebrew_CI_AS NOT NULL,
    [UserName] [nvarchar](256) COLLATE Hebrew_CI_AS NULL,
    [NormalizedUserName] [nvarchar](256) COLLATE Hebrew_CI_AS NULL,
    [Email] [nvarchar](256) COLLATE Hebrew_CI_AS NULL,
    [NormalizedEmail] [nvarchar](256) COLLATE Hebrew_CI_AS NULL,
    [EmailConfirmed] [bit] NOT NULL,
    [PasswordHash] [nvarchar](max) COLLATE Hebrew_CI_AS NULL,
    [SecurityStamp] [nvarchar](max) COLLATE Hebrew_CI_AS NULL,
    [ConcurrencyStamp] [nvarchar](max) COLLATE Hebrew_CI_AS NULL,
    [PhoneNumber] [nvarchar](max) COLLATE Hebrew_CI_AS NULL,
    [PhoneNumberConfirmed] [bit] NOT NULL,
    [TwoFactorEnabled] [bit] NOT NULL,
    [LockoutEnd] [datetimeoffset](7) NULL,
    [LockoutEnabled] [bit] NOT NULL,
    [AccessFailedCount] [int] NOT NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[AddressUser]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[AddressUser]
(
    [AddressesId] [int] NOT NULL,
    [UsersId] [int] NOT NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[Address]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[Address]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [City] [nvarchar](50) COLLATE Hebrew_CI_AS NOT NULL,
    [Street] [nvarchar](50) COLLATE Hebrew_CI_AS NOT NULL,
    [BuildingNumber] [int] NOT NULL,
    [Latitude] [float] NOT NULL,
    [Longitude] [float] NOT NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[ProductColor]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[ProductColor]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Color] [nvarchar](max) COLLATE Hebrew_CI_AS NULL,
    [Name] [nvarchar](max) COLLATE Hebrew_CI_AS NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[User]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[User]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [FirstName] [nvarchar](100) COLLATE Hebrew_CI_AS NOT NULL,
    [LastName] [nvarchar](100) COLLATE Hebrew_CI_AS NOT NULL,
    [Password] [nvarchar](max) COLLATE Hebrew_CI_AS NOT NULL,
    [Email] [nvarchar](max) COLLATE Hebrew_CI_AS NOT NULL,
    [UserType] [int] NOT NULL,
    [DateOfBirth] [datetime2](7) NOT NULL,
    [Phone] [nvarchar](max) COLLATE Hebrew_CI_AS NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[Order]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[Order]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Date] [datetime2](7) NOT NULL,
    [UserId] [int] NOT NULL,
    [IsCart] [bit] NOT NULL,
    [Delivery] [int] NOT NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[OrderItem]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[OrderItem]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [OrderId] [int] NOT NULL,
    [Amount] [int] NOT NULL,
    [TotalPrice] [float] NOT NULL,
    [ProductTypeID] [int] NOT NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[Category]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[Category]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) COLLATE Hebrew_CI_AS NOT NULL,
    [ParentCategoryId] [int] NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[OpeningHour]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[OpeningHour]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Open] [nvarchar](max) COLLATE Hebrew_CI_AS NOT NULL,
    [Close] [nvarchar](max) COLLATE Hebrew_CI_AS NOT NULL,
    [BranchId] [int] NOT NULL,
    [DayOfWeek] [int] NOT NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[__EFMigrationsHistory]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[__EFMigrationsHistory]
(
    [MigrationId] [nvarchar](150) COLLATE Hebrew_CI_AS NOT NULL,
    [ProductVersion] [nvarchar](32) COLLATE Hebrew_CI_AS NOT NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[Product]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[Product]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Price] [real] NOT NULL,
    [Name] [nvarchar](max) COLLATE Hebrew_CI_AS NOT NULL,
    [DiscountPercentage] [real] NOT NULL,
    [CategoryId] [int] NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[ProductCategory]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[ProductCategory]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [ProductId] [int] NOT NULL,
    [CategoryId] [int] NOT NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[ProductImage]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[ProductImage]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](max) COLLATE Hebrew_CI_AS NULL,
    [ImageData] [varbinary](max) NULL,
    [ProductId] [int] NOT NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d

PRINT N'CREATE TABLE [dbo].[ProductType]'
GO -- SQRIBE/GO;0a120d

-- SQRIBE/OBJ;0a120d
CREATE TABLE [dbo].[ProductType]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [ProductId] [int] NOT NULL,
    [Size] [int] NOT NULL,
    [Quantity] [int] NOT NULL,
    [ColorId] [int] NOT NULL

) ON [PRIMARY]
GO -- SQRIBE/GO;0a120d
