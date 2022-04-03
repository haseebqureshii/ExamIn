CREATE TABLE [dbo].[ExamSession]
(
	[Id] INT NOT NULL PRIMARY KEY,
    [ExamDate] DATETIME2 NOT NULL,
    [StartTime] DATETIME2 NOT NULL, 
    [Examiner] NVARCHAR(50) NOT NULL, 
    [CreatedDate] DATETIME2 NULL DEFAULT getutcdate(), 
    [EndTime] DATETIME2 NULL
)
