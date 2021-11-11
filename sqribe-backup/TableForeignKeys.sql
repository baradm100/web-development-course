SET NUMERIC_ROUNDABORT OFF
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO -- SQRIBE/GO;d94b91
PRINT N'CREATE foreign key constraints'
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[AspNetRoleClaims] WITH NOCHECK ADD CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId]) 
REFERENCES [dbo].[AspNetRoles] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[AspNetUserClaims] WITH NOCHECK ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId]) 
REFERENCES [dbo].[AspNetUsers] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[AspNetUserLogins] WITH NOCHECK ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId]) 
REFERENCES [dbo].[AspNetUsers] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[AspNetUserRoles] WITH NOCHECK ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId]) 
REFERENCES [dbo].[AspNetRoles] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[AspNetUserRoles] WITH NOCHECK ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId]) 
REFERENCES [dbo].[AspNetUsers] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[AspNetUserTokens] WITH NOCHECK ADD CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId]) 
REFERENCES [dbo].[AspNetUsers] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[Branch] WITH NOCHECK ADD CONSTRAINT [FK_Branch_Address_AddressId] FOREIGN KEY([AddressId]) 
REFERENCES [dbo].[Address] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[ProductType] WITH NOCHECK ADD CONSTRAINT [FK_ProductType_Product_ProductId] FOREIGN KEY([ProductId]) 
REFERENCES [dbo].[Product] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[OrderItem] WITH NOCHECK ADD CONSTRAINT [FK_OrderItem_ProductType_ProductTypeID] FOREIGN KEY([ProductTypeID]) 
REFERENCES [dbo].[ProductType] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[OrderItem] WITH NOCHECK ADD CONSTRAINT [FK_OrderItem_Order_OrderId] FOREIGN KEY([OrderId]) 
REFERENCES [dbo].[Order] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[Order] WITH NOCHECK ADD CONSTRAINT [FK_Order_User_UserId] FOREIGN KEY([UserId]) 
REFERENCES [dbo].[User] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[Category] WITH NOCHECK ADD CONSTRAINT [FK_Category_Category_ParentCategoryId] FOREIGN KEY([ParentCategoryId]) 
REFERENCES [dbo].[Category] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[ProductImage] WITH NOCHECK ADD CONSTRAINT [FK_ProductImage_Product_ProductId] FOREIGN KEY([ProductId]) 
REFERENCES [dbo].[Product] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[OpeningHour] WITH NOCHECK ADD CONSTRAINT [FK_OpeningHour_Branch_BranchId] FOREIGN KEY([BranchId]) 
REFERENCES [dbo].[Branch] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[ProductType] WITH NOCHECK ADD CONSTRAINT [FK_ProductType_ProductColor_ColorId] FOREIGN KEY([ColorId]) 
REFERENCES [dbo].[ProductColor] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[Product] WITH NOCHECK ADD CONSTRAINT [FK_Product_Category_CategoryId] FOREIGN KEY([CategoryId]) 
REFERENCES [dbo].[Category] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[ProductCategory] WITH NOCHECK ADD CONSTRAINT [FK_ProductCategory_Category_CategoryId] FOREIGN KEY([CategoryId]) 
REFERENCES [dbo].[Category] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[ProductCategory] WITH NOCHECK ADD CONSTRAINT [FK_ProductCategory_Product_ProductId] FOREIGN KEY([ProductId]) 
REFERENCES [dbo].[Product] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[AddressUser] WITH NOCHECK ADD CONSTRAINT [FK_AddressUser_Address_AddressesId] FOREIGN KEY([AddressesId]) 
REFERENCES [dbo].[Address] ([Id]) 
GO -- SQRIBE/GO;d94b91

-- SQRIBE/OBJ;d94b91
ALTER TABLE [dbo].[AddressUser] WITH NOCHECK ADD CONSTRAINT [FK_AddressUser_User_UsersId] FOREIGN KEY([UsersId]) 
REFERENCES [dbo].[User] ([Id]) 
GO -- SQRIBE/GO;d94b91
