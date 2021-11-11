SET NUMERIC_ROUNDABORT OFF
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET DATEFORMAT YMD
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO -- SQRIBE/GO;c2f4f5

-- SQRIBE/TABLE;c2f4f5
-- Adding 25 rows to dbo.__EFMigrationsHistory

BEGIN TRANSACTION

-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210824141128_InitialCreate',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210827132112_AddBranch',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210903143645_add_coordinate',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210906103823_product-order-uer',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210907164054_cast_to_double',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210908081309_add-cart',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210908082600_dayOfweek',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210908143526_branch_name',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210908174748_AddAdminUser',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210908181509_AddParentCategory',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210908181836_AddBaseCatagories',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210909155255_add-product-images',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210910164311_FK_Hour_branch',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210918130136_productCategory',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210920090511_branch_nullable',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210922063900_Color',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210922064739_Added-To-Type-ColorId',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210922142312_add_orderId_to_OrderItems',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210922162611_FixMerge',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210922182647_FixProductCategory',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210922191242_add_productID',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20210924131441_totalPrice_double',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20211022091515_err',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20211023095356_address_users',N'5.0.9');
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES (N'20211023155256_phone_user',N'5.0.9');

COMMIT TRANSACTION

