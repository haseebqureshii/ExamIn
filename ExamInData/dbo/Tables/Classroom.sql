CREATE TABLE [dbo].[Classroom]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserID] NVARCHAR(128) NOT NULL, 
    [Title] NVARCHAR(50) NOT NULL, 
    [JoinCode] CHAR(6) NOT NULL
)
