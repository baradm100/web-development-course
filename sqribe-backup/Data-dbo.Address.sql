SET NUMERIC_ROUNDABORT OFF
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET DATEFORMAT YMD
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO -- SQRIBE/GO;0a120d

-- SQRIBE/TABLE;0a120d
-- Adding 8 rows to dbo.Address

SET IDENTITY_INSERT [dbo].[Address] ON

BEGIN TRANSACTION

-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Address] ([Id],[City],[Street],[BuildingNumber],[Latitude],[Longitude]) VALUES (1,N'Tel Aviv',N'Disingof',263,0.000000000000000e+000,0.000000000000000e+000);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Address] ([Id],[City],[Street],[BuildingNumber],[Latitude],[Longitude]) VALUES (3,N'tel aviv',N'test',4,0.000000000000000e+000,0.000000000000000e+000);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Address] ([Id],[City],[Street],[BuildingNumber],[Latitude],[Longitude]) VALUES (4,N'Tel aviv',N'test',5,0.000000000000000e+000,0.000000000000000e+000);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Address] ([Id],[City],[Street],[BuildingNumber],[Latitude],[Longitude]) VALUES (5,N'Yahood',N'asd',1234,0.000000000000000e+000,0.000000000000000e+000);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Address] ([Id],[City],[Street],[BuildingNumber],[Latitude],[Longitude]) VALUES (6,N'Yokneam',N'dizingof',12,0.000000000000000e+000,0.000000000000000e+000);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Address] ([Id],[City],[Street],[BuildingNumber],[Latitude],[Longitude]) VALUES (7,N'Tel Aviv-Yafo',N'Dizengoff ',50,3.207534820000000e+001,3.477467610000000e+001);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Address] ([Id],[City],[Street],[BuildingNumber],[Latitude],[Longitude]) VALUES (8,N'Tel Aviv-Yafo',N'Carlebach ',6,3.206857870000000e+001,3.478333040000000e+001);
-- SQRIBE/INSERT;0a120d
INSERT INTO [dbo].[Address] ([Id],[City],[Street],[BuildingNumber],[Latitude],[Longitude]) VALUES (9,N'Jerusalem',N'Derech Agudat Sport Beitar',1,3.175125230000000e+001,3.518679270000000e+001);

COMMIT TRANSACTION

SET IDENTITY_INSERT [dbo].[Address] OFF

