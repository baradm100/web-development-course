SET NUMERIC_ROUNDABORT OFF
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET DATEFORMAT YMD
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO -- SQRIBE/GO;0a120d

-- SQRIBE/TABLE;0a120d
-- Adding 13 rows to dbo.OrderItem

SET IDENTITY_INSERT [dbo].[OrderItem] ON

BEGIN TRANSACTION

-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OrderItem] ([Id],[OrderId],[Amount],[TotalPrice],[ProductTypeID]) VALUES (1,5,1,2.400000000000000e+001,345);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OrderItem] ([Id],[OrderId],[Amount],[TotalPrice],[ProductTypeID]) VALUES (2,6,10,3.700000000000000e+002,119);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OrderItem] ([Id],[OrderId],[Amount],[TotalPrice],[ProductTypeID]) VALUES (3,7,5,2.375000000000000e+002,844);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OrderItem] ([Id],[OrderId],[Amount],[TotalPrice],[ProductTypeID]) VALUES (4,7,3,1.950000000000000e+002,988);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OrderItem] ([Id],[OrderId],[Amount],[TotalPrice],[ProductTypeID]) VALUES (14,9,2,1.100000000000000e+002,626);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OrderItem] ([Id],[OrderId],[Amount],[TotalPrice],[ProductTypeID]) VALUES (15,9,3,7.200000000000000e+001,73);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OrderItem] ([Id],[OrderId],[Amount],[TotalPrice],[ProductTypeID]) VALUES (16,9,2,4.000000000000000e+001,502);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OrderItem] ([Id],[OrderId],[Amount],[TotalPrice],[ProductTypeID]) VALUES (17,11,1,3.870000076293945e+001,816);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OrderItem] ([Id],[OrderId],[Amount],[TotalPrice],[ProductTypeID]) VALUES (18,11,2,9.500000000000000e+001,843);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OrderItem] ([Id],[OrderId],[Amount],[TotalPrice],[ProductTypeID]) VALUES (19,11,2,1.300000000000000e+002,987);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OrderItem] ([Id],[OrderId],[Amount],[TotalPrice],[ProductTypeID]) VALUES (20,1,1,3.870000076293945e+001,816);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OrderItem] ([Id],[OrderId],[Amount],[TotalPrice],[ProductTypeID]) VALUES (21,1,1,3.870000076293945e+001,812);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OrderItem] ([Id],[OrderId],[Amount],[TotalPrice],[ProductTypeID]) VALUES (22,13,3,3.999873962402344e+002,464);

COMMIT TRANSACTION

SET IDENTITY_INSERT [dbo].[OrderItem] OFF

