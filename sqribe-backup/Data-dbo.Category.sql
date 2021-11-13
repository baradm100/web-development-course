SET NUMERIC_ROUNDABORT OFF
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET DATEFORMAT YMD
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO -- SQRIBE/GO;0a120d

-- SQRIBE/TABLE;0a120d
-- Adding 16 rows to dbo.Category

SET IDENTITY_INSERT [dbo].[Category] ON

BEGIN TRANSACTION

-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (1,N'Men',NULL);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (2,N'Women',NULL);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (3,N'Men Shirts',1);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (4,N'Women Shirts',2);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (5,N'Men Pants',1);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (6,N'Women Pants',2);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (9,N'Boys',NULL);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (10,N'Girls',NULL);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (11,N'Boys Shirts',9);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (12,N'Girls Shirts',10);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (13,N'Boys Pants',9);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (14,N'Girls Pants',10);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (16,N'Men Shirts Sport',3);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (17,N'Boys Sports Shirts',11);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (18,N'Boys Sports Pants',13);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Category] ([Id],[Name],[ParentCategoryId]) VALUES (19,N'Women sport pants',6);

COMMIT TRANSACTION

SET IDENTITY_INSERT [dbo].[Category] OFF

