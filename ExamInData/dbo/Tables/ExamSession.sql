CREATE TABLE [dbo].[ExamSession]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [ClassID] INT NOT NULL, 
    [ClassTitle] NVARCHAR(50) NOT NULL, 
    [ExamDate] DATETIME2 NOT NULL,
    [Duration] INT NOT NULL, 
    [Examiner] NVARCHAR(50) NOT NULL, 
    [CreatedDate] DATETIME2 NULL DEFAULT getutcdate()
)
