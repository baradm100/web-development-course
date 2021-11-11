SET NUMERIC_ROUNDABORT OFF
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET DATEFORMAT YMD
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO -- SQRIBE/GO;c2f4f5

-- SQRIBE/TABLE;c2f4f5
-- Adding 6 rows to dbo.OpeningHour

SET IDENTITY_INSERT [dbo].[OpeningHour] ON

BEGIN TRANSACTION

-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (1,N'09:00',N'13:00',1,5);
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (2,N'09:00',N'21:30',1,4);
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (3,N'09:00',N'20:00',1,3);
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (4,N'10:00',N'21:00',1,2);
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (5,N'09:00',N'19:00',1,1);
-- SQRIBE/INSERT;c2f4f5
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (6,N'09:00',N'21:00',1,0);

COMMIT TRANSACTION

SET IDENTITY_INSERT [dbo].[OpeningHour] OFF

