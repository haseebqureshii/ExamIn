CREATE TABLE [dbo].[Classroom]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] NVARCHAR(50) NOT NULL, 
    [ClassID] NVARCHAR(128) NOT NULL, 
    [JoinCode] NCHAR(10) NOT NULL, 
    [CreatedDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [Teacher] NVARCHAR(50) NOT NULL DEFAULT getusername(), 
    [LastModified] DATETIME2 NOT NULL
)
