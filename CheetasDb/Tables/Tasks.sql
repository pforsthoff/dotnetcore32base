CREATE TABLE [dbo].[JobProvisioningTasks]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
    [FileTimeSpan] BIGINT NOT NULL,
    [Status] INT NOT NULL,
    [JobProvisionedDateTime] DATETIME NULL,
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreationDateTime] DATETIME NOT NULL,
    [LastModifiedBy] NVARCHAR(50) NULL,
    [LastModifiedDateTime] DATETIME NULL
)
