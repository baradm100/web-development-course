﻿SET NUMERIC_ROUNDABORT OFF
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET DATEFORMAT YMD
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO -- SQRIBE/GO;d94b91

-- SQRIBE/TABLE;d94b91
-- Adding 9 rows to dbo.ProductColor

SET IDENTITY_INSERT [dbo].[ProductColor] ON

BEGIN TRANSACTION

-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[ProductColor] ([Id],[Color],[Name]) VALUES (1,N'EE2A00',N'Red');
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[ProductColor] ([Id],[Color],[Name]) VALUES (2,N'000000',N'Black');
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[ProductColor] ([Id],[Color],[Name]) VALUES (3,N'FFFFFF',N'White');
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[ProductColor] ([Id],[Color],[Name]) VALUES (4,N'B9B9B9',N'Grey');
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[ProductColor] ([Id],[Color],[Name]) VALUES (5,N'FFF704',N'Yellow');
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[ProductColor] ([Id],[Color],[Name]) VALUES (6,N'BF08E3',N'Purple');
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[ProductColor] ([Id],[Color],[Name]) VALUES (7,N'0851E3',N'Blue');
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[ProductColor] ([Id],[Color],[Name]) VALUES (8,N'26E308',N'Green');
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[ProductColor] ([Id],[Color],[Name]) VALUES (9,N'E308CF',N'Pink');

COMMIT TRANSACTION

SET IDENTITY_INSERT [dbo].[ProductColor] OFF
