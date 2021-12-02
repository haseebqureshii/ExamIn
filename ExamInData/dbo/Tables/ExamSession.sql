CREATE TABLE [dbo].[ExamSession]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ExamID] NVARCHAR(128) NOT NULL, 
    [ExamDate] DATE NOT NULL, 
    [StartTime] TIME NOT NULL, 
    [EndTime] TIME NOT NULL, 
    [ClassID] NVARCHAR(128) NOT NULL, 
    [Teacher] NVARCHAR(50) NOT NULL
)
