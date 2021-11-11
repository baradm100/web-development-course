SET NUMERIC_ROUNDABORT OFF
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET DATEFORMAT YMD
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO -- SQRIBE/GO;d94b91

-- SQRIBE/TABLE;d94b91
-- Adding 5 rows to dbo.User

SET IDENTITY_INSERT [dbo].[User] ON

BEGIN TRANSACTION

-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[User] ([Id],[FirstName],[LastName],[Password],[Email],[UserType],[DateOfBirth],[Phone]) VALUES (1,N'Admin',N'Admin',N'1234',N'admin@admin.com',2,CONVERT(datetime2,'0001-01-01 00:00:00.0000000',121),NULL);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[User] ([Id],[FirstName],[LastName],[Password],[Email],[UserType],[DateOfBirth],[Phone]) VALUES (2,N'Noam',N'Cohen',N'1234',N'noam@gmail.com',0,CONVERT(datetime2,'2000-01-01 00:00:00.0000000',121),N'0532654487');
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[User] ([Id],[FirstName],[LastName],[Password],[Email],[UserType],[DateOfBirth],[Phone]) VALUES (3,N'Ronen',N'Levi',N'1234',N'Ronem@gmail.com',0,CONVERT(datetime2,'1998-02-02 00:00:00.0000000',121),N'0587664312');
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[User] ([Id],[FirstName],[LastName],[Password],[Email],[UserType],[DateOfBirth],[Phone]) VALUES (4,N'Maya',N'Dagan',N'1234',N'Maya@gmail.com',0,CONVERT(datetime2,'1990-03-03 00:00:00.0000000',121),N'0526486933');
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[User] ([Id],[FirstName],[LastName],[Password],[Email],[UserType],[DateOfBirth],[Phone]) VALUES (5,N'Maixm',N'Meltzer',N'1234',N'maxim@gmail.com',0,CONVERT(datetime2,'1989-03-05 00:00:00.0000000',121),N'0544769933');

COMMIT TRANSACTION

SET IDENTITY_INSERT [dbo].[User] OFF

