SET NUMERIC_ROUNDABORT OFF
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET DATEFORMAT YMD
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO -- SQRIBE/GO;0a120d

-- SQRIBE/TABLE;0a120d
-- Adding 19 rows to dbo.OpeningHour

SET IDENTITY_INSERT [dbo].[OpeningHour] ON

BEGIN TRANSACTION

-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (7,N'09:00',N'17:00',2,5);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (8,N'09:00',N'22:00',2,4);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (9,N'21:00',N'22:00',2,3);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (10,N'09:00',N'22:00',2,2);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (11,N'09:00',N'22:00',2,1);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (12,N'09:00',N'22:00',2,0);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (13,N'18:00',N'22:00',3,6);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (14,N'09:00',N'16:00',3,5);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (15,N'09:00',N'19:00',3,4);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (16,N'09:00',N'19:00',3,3);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (17,N'09:00',N'19:00',3,2);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (18,N'09:00',N'19:00',3,1);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (19,N'09:00',N'19:00',3,0);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (20,N'09:00',N'16:00',4,5);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (21,N'10:00',N'19:00',4,4);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (22,N'10:00',N'19:00',4,3);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (23,N'10:00',N'19:00',4,2);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (24,N'10:00',N'19:00',4,1);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[OpeningHour] ([Id],[Open],[Close],[BranchId],[DayOfWeek]) VALUES (25,N'10:00',N'19:00',4,0);

COMMIT TRANSACTION

SET IDENTITY_INSERT [dbo].[OpeningHour] OFF

