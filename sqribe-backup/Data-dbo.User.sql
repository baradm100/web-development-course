SET NUMERIC_ROUNDABORT OFF
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET DATEFORMAT YMD
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO -- SQRIBE/GO;0a120d

-- SQRIBE/TABLE;0a120d
-- Adding 9 rows to dbo.User

SET IDENTITY_INSERT [dbo].[User] ON

BEGIN TRANSACTION

-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[User] ([Id],[FirstName],[LastName],[Password],[Email],[UserType],[DateOfBirth],[Phone]) VALUES (1,N'Admin',N'Admin',N'1234',N'admin@admin.com',2,CONVERT(datetime2,'0001-01-01 00:00:00.0000000',121),NULL);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[User] ([Id],[FirstName],[LastName],[Password],[Email],[UserType],[DateOfBirth],[Phone]) VALUES (2,N'Noam',N'Cohen',N'1234',N'noam@gmail.com',0,CONVERT(datetime2,'2000-01-01 00:00:00.0000000',121),N'0532654487');
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[User] ([Id],[FirstName],[LastName],[Password],[Email],[UserType],[DateOfBirth],[Phone]) VALUES (3,N'Ronen',N'Levi',N'1234',N'Ronem@gmail.com',0,CONVERT(datetime2,'1998-02-02 00:00:00.0000000',121),N'0587664312');
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[User] ([Id],[FirstName],[LastName],[Password],[Email],[UserType],[DateOfBirth],[Phone]) VALUES (4,N'Maya',N'Dagan',N'1234',N'Maya@gmail.com',0,CONVERT(datetime2,'1990-03-03 00:00:00.0000000',121),N'0526486933');
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[User] ([Id],[FirstName],[LastName],[Password],[Email],[UserType],[DateOfBirth],[Phone]) VALUES (5,N'Maixm',N'Meltzer',N'1234',N'maxim@gmail.com',0,CONVERT(datetime2,'1989-03-05 00:00:00.0000000',121),N'0544582766');
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[User] ([Id],[FirstName],[LastName],[Password],[Email],[UserType],[DateOfBirth],[Phone]) VALUES (6,N'Store',N'Editor',N'1234',N'editor@admin.com',1,CONVERT(datetime2,'0001-01-01 00:00:00.0000000',121),NULL);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[User] ([Id],[FirstName],[LastName],[Password],[Email],[UserType],[DateOfBirth],[Phone]) VALUES (7,N'Doron',N'David',N'1234',N'doron@gmail.com',0,CONVERT(datetime2,'1995-06-22 00:00:00.0000000',121),N'0545801111');
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[User] ([Id],[FirstName],[LastName],[Password],[Email],[UserType],[DateOfBirth],[Phone]) VALUES (8,N'Meirav',N'Friedman',N'1234',N'meiravF@gmail.com',0,CONVERT(datetime2,'1986-03-12 00:00:00.0000000',121),N'0547894455');
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[User] ([Id],[FirstName],[LastName],[Password],[Email],[UserType],[DateOfBirth],[Phone]) VALUES (9,N'Or',N'Peleg',N'1234',N'or@gmail.com',0,CONVERT(datetime2,'1995-01-01 00:00:00.0000000',121),N'0526987436');

COMMIT TRANSACTION

SET IDENTITY_INSERT [dbo].[User] OFF

