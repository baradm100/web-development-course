SET NUMERIC_ROUNDABORT OFF
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET DATEFORMAT YMD
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO -- SQRIBE/GO;d94b91

-- SQRIBE/TABLE;d94b91
-- Adding 117 rows to dbo.Product

SET IDENTITY_INSERT [dbo].[Product] ON

BEGIN TRANSACTION

-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (1,1.300000000000000e+001,N'Stripes Boys Shirt',5.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (2,2.000000000000000e+001,N'Elegant Long Sleeves Boys',2.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (3,2.800000000000000e+001,N'Mickey Long Sleeves',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (4,1.800000000000000e+001,N'Hoodie Stripes Long Sleeves',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (5,1.000000000000000e+001,N'Regular Boys Shirt',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (6,2.300000000000000e+001,N'Levis Long Sleeves Boys',3.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (7,3.500000000000000e+001,N'Wedding Long Sleeves Shirt',2.500000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (8,4.000000000000000e+001,N'Armor Sport Shirt Boys',4.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (9,1.500000000000000e+001,N'GAP Boys Shirt',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (10,4.500000000000000e+001,N'Cool Long Sleeves',7.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (11,2.500000000000000e+001,N'Elegant Long Sleeves',3.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (12,5.000000000000000e+001,N'Buttoned Shirt',5.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (13,3.500000000000000e+001,N'Classic Adidas Shirt',1.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (14,4.500000000000000e+001,N'Levis Long Sleeves Style',2.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (15,3.700000000000000e+001,N'Nike Sports T-shirt',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (16,3.000000000000000e+001,N'Amrican Eagle long shirt',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (17,7.000000000000000e+001,N'Skinny Jeans Boys',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (18,4.000000000000000e+001,N'Running Nike T-shirt',2.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (19,8.500000000000000e+001,N'Style Classic Long Pants',5.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (20,6.500000000000000e+001,N'Short Jeans',2.500000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (21,2.500000000000000e+001,N'Evening Shirt',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (22,2.500000000000000e+001,N'Style Sweatpants',5.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (23,3.500000000000000e+001,N'Evening square shirt',5.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (24,2.000000000000000e+001,N'Classic Sweatpants boys',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (25,3.300000000000000e+001,N'Freestyle Sweatpants Boys',3.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (26,4.000000000000000e+001,N'Basketball Nike Pants',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (27,4.000000000000000e+001,N'Nike Running Pants',5.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (28,6.000000000000000e+001,N'Under armor Running Pants',2.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (29,1.500000000000000e+001,N'Nike Classic Running Pants',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (30,8.000000000000000e+001,N'Style Pants Boys',1.500000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (31,3.000000000000000e+001,N'Black Sport singlet',1.500000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (32,3.000000000000000e+001,N'Short Sweatpants Boys',1.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (33,5.000000000000000e+001,N'Bulls Basketball Pants',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (34,2.000000000000000e+001,N'American EagleT-shirt',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (35,8.500000000000000e+001,N'Adidas Classic Short Pants',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (36,1.200000000000000e+002,N'Best Style Short Pants',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (37,4.000000000000000e+001,N'Nike Style Pants',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (38,3.000000000000000e+001,N'Party tank top',2.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (39,2.500000000000000e+001,N'Comfortable Polo',1.500000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (40,2.500000000000000e+001,N'Working Polo',1.500000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (41,2.200000000000000e+001,N'Long sleeve polo',1.500000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (42,3.000000000000000e+001,N'Running tank',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (43,4.000000000000000e+001,N'Adidas T-shirt',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (44,1.500000000000000e+002,N'Fur Coat Girls',5.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (45,4.500000000000000e+001,N'Elegant shirt',5.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (46,1.000000000000000e+002,N'Jeans Jacket Girls',1.200000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (47,2.500000000000000e+001,N'Classic Sweater Girls',2.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (48,5.000000000000000e+001,N'Interview shirt',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (49,2.000000000000000e+001,N'Nike Shirt Girls',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (50,4.000000000000000e+001,N'Cool NYC Shirt Girls',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (52,2.500000000000000e+001,N'Cools School Girls Shirts',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (53,2.000000000000000e+001,N'American Eagle fun shirt',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (54,3.000000000000000e+001,N'Love Girls Shirts',3.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (55,2.000000000000000e+001,N'Classic Girls Shirts',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (56,4.000000000000000e+001,N'Long jeans shirt',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (57,5.000000000000000e+001,N'Boston Girls Shirt',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (58,6.000000000000000e+001,N'Long Light jeans',5.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (59,2.500000000000000e+001,N'Numbers Shirt Girls',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (60,6.000000000000000e+001,N'Adidas Girls Shirts',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (61,5.500000000000000e+001,N'Short Light jeans',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (62,8.000000000000000e+001,N'Style Sports Shirt Girls',4.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (63,1.000000000000000e+002,N'Nike Sports Shirts Girls',5.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (64,7.000000000000000e+001,N'Long dark jeans',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (65,8.500000000000000e+001,N'Long Sleeves Sports Shirt Girls',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (66,5.000000000000000e+001,N'Short dark jeans',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (67,1.000000000000000e+001,N'Classic Short Shirt Girls',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (68,2.000000000000000e+002,N'Classic Girls Pants',6.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (69,1.500000000000000e+002,N'Mottled tights Girls',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (70,2.000000000000000e+001,N'Classic Tights',3.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (71,6.000000000000000e+001,N'Slim light jeans',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (72,1.000000000000000e+002,N'Flowers Jeans Girls',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (73,5.000000000000000e+001,N'Short Pants Girls',5.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (74,5.000000000000000e+001,N'Short khaki pants',2.500000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (75,2.500000000000000e+001,N'Classic Sweatpants Girls',5.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (76,7.000000000000000e+001,N'Long khaki pants',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (77,3.500000000000000e+001,N'Style Tights Girls',5.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (78,7.500000000000000e+001,N'Change color jeans',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (79,3.500000000000000e+001,N'Beautiful Pants Girls',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (80,1.000000000000000e+002,N'Classic Jeans Girls',5.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (81,8.500000000000000e+001,N'Classic Pajamas Girls',3.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (82,6.800000000000000e+001,N'Dark change color jeans',3.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (83,5.500000000000000e+001,N'Flowers Short Pants Girls',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (84,1.800000000000000e+002,N'Uniqe Tights',1.800000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (85,5.000000000000000e+001,N'Torn short jeans',7.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (86,1.000000000000000e+002,N'Style Short Pants Girls',2.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (87,4.300000000000000e+001,N'Design shorts',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (88,7.500000000000000e+001,N'Uniqe Sweatpants Girls',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (89,5.000000000000000e+001,N'Shorts khaki ',5.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (90,7.000000000000000e+001,N'Long slim jeans',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (91,4.400000000000000e+001,N'Loose long pants',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (92,5.000000000000000e+001,N'Short tights pants',2.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (93,4.300000000000000e+001,N'Long tights pants',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (94,3.800000000000000e+001,N'Long loose pants',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (95,6.000000000000000e+001,N'Elegant pants',1.200000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (96,2.500000000000000e+001,N'Home long pants',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (97,5.000000000000000e+001,N'Adidas short pants',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (98,6.500000000000000e+001,N'Elegant long pants',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (99,6.500000000000000e+001,N'Long dark skinny jeans',1.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (100,7.000000000000000e+001,N'Flower long pants',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (101,8.000000000000000e+001,N'Nike Long pants',3.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (102,8.500000000000000e+001,N'Long dark pants',2.200000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (103,8.000000000000000e+001,N'Elegant jacket ',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (104,7.800000000000000e+001,N'Lorry jacket',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (105,8.000000000000000e+001,N'Amma''s top',6.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (106,2.200000000000000e+001,N'Basic T-shirt',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (107,5.500000000000000e+001,N'Elegant short shirt',3.500000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (108,3.500000000000000e+001,N'Lily''s favorite',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (109,3.300000000000000e+001,N'Green shirt',2.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (110,2.000000000000000e+001,N'Simple T-shirt',2.500000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (111,4.200000000000000e+001,N'Sweater',1.500000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (112,5.500000000000000e+001,N'Casual dress',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (113,5.500000000000000e+001,N'Silk shirt',3.500000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (114,5.700000000000000e+001,N'Lily''s long shirt',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (115,2.800000000000000e+001,N'Light long shirt',2.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (116,4.500000000000000e+001,N'Sunny shirt',0.000000000000000e+000,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (117,3.000000000000000e+001,N'Stripes long shirt',2.000000000000000e+001,NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[Product] ([Id],[Price],[Name],[DiscountPercentage],[CategoryId]) VALUES (118,5.500000000000000e+001,N'Casual long shirt',0.000000000000000e+000,NULL);

COMMIT TRANSACTION

SET IDENTITY_INSERT [dbo].[Product] OFF

