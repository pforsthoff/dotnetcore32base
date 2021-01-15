CREATE TABLE [dbo].[Files]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
    [StartTime] DATETIME NOT NULL,
    [EndTime] DATETIME NOT NULL,
    [Status] INT NOT NULL,
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreationDateTime] DATETIME NOT NULL,
    [LastModifiedBy] NVARCHAR(50) NULL,
    [LastModifiedDateTime] DATETIME NULL
)
