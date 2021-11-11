SET NUMERIC_ROUNDABORT OFF
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET DATEFORMAT YMD
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO -- SQRIBE/GO;d94b91

-- SQRIBE/TABLE;d94b91
-- Adding 3 rows to dbo.AspNetUsers

BEGIN TRANSACTION

-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[AspNetUsers] ([Id],[UserName],[NormalizedUserName],[Email],[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnd],[LockoutEnabled],[AccessFailedCount]) VALUES (N'06164710-38ac-42bc-93e4-1fbefab3fcf6',N'badmoni@cloudlock.com',N'BADMONI@CLOUDLOCK.COM',N'badmoni@cloudlock.com',N'BADMONI@CLOUDLOCK.COM',1,N'AQAAAAEAACcQAAAAENLzzTJvub9CLNlB/vwRvL7JVOM8snwz5gHoDyvIJAPl7mgEfvygiAxouQHZlR17IQ==',N'VQTXTESSXRF4JK62XBQG5OMPYBNDC4QY',N'd86ae578-5f34-4b78-b677-09f26b464aab',NULL,0,0,CONVERT(datetimeoffset,NULL,121),1,0);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[AspNetUsers] ([Id],[UserName],[NormalizedUserName],[Email],[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnd],[LockoutEnabled],[AccessFailedCount]) VALUES (N'5bb7d80c-c593-49f7-a8c4-c67fe1d3af07',N'test@test.com',N'TEST@TEST.COM',N'test@test.com',N'TEST@TEST.COM',0,N'AQAAAAEAACcQAAAAENWOr//KwF41nQ/6TrC/JtD9Ru/hHHdVO7OLwEP6Jue8w54nBIEihLLlSj+2+YiT4w==',N'XHKFJO7SKIC4UMFMKZJJFZFZWX5VTCVU',N'c29b5dae-a671-409d-92b2-c345a45ad4f7',NULL,0,0,CONVERT(datetimeoffset,NULL,121),1,0);
-- SQRIBE/INSERT;d94b91
INSERT INTO [dbo].[AspNetUsers] ([Id],[UserName],[NormalizedUserName],[Email],[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnd],[LockoutEnabled],[AccessFailedCount]) VALUES (N'f6023769-b937-4cb2-bd75-1a50904a0801',N'tttt@eee',N'TTTT@EEE',N'tttt@eee',N'TTTT@EEE',0,N'AQAAAAEAACcQAAAAECZXc9hAeYvI8taauNM7+Kr2hH3iU+9B5U7iLm8hMFqbON0HCuvU5qYWURM+JgasGA==',N'WJLMA6AK6Z7C3WO75AJEH22QSNHKS5AB',N'93a1ef02-12f2-404a-9244-2fb180c38fb0',NULL,0,0,CONVERT(datetimeoffset,NULL,121),1,0);

COMMIT TRANSACTION

