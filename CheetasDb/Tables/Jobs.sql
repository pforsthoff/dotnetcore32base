CREATE TABLE [dbo].[Jobs]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
    [TaskId] INT NULL,
    [DateTimeJobRcvd] DATETIME NOT NULL,
    [TimeSpan] BIGINT NOT NULL,
    [Status] INT NOT NULL,
    [DateTimeJobStarted] DATETIME NULL,
    [DateTimeJobCompleted] DATETIME NULL,
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreationDateTime] DATETIME NOT NULL,
    [LastModifiedBy] NVARCHAR(50) NULL,
    [LastModifiedDateTime] DATETIME NULL
    CONSTRAINT [FK_Jobs_JobProvisioningTasks] FOREIGN KEY (TaskId) REFERENCES JobProvisioningTasks(Id),
)
