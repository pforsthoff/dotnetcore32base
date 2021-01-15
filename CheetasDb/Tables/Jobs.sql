CREATE TABLE [dbo].[Jobs]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
    [FileId] INT NULL,
    [TimeSpan] BIGINT NOT NULL,
    [Status] INT NOT NULL,
    [StartedDateTime] DATETIME NULL,
    [CompletedDateTime] DATETIME NULL,
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreationDateTime] DATETIME NOT NULL,
    [LastModifiedBy] NVARCHAR(50) NULL,
    [LastModifiedDateTime] DATETIME NULL
    CONSTRAINT [FK_Jobs_Files] FOREIGN KEY (FileId) REFERENCES Files(Id),
)
