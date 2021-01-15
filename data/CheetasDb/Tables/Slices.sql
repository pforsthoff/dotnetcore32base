CREATE TABLE [dbo].[Slices]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
    [JobId] INT NOT NULL,
    [SliceId] INT NOT NULL,
    [Status] INT NOT NULL,
    [SliceStarted] DATETIME NULL,
    [SliceCompleted] DATETIME NULL,
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreationDateTime] DATETIME NOT NULL,
    [LastModifiedBy] NVARCHAR(50) NULL,
    [LastModifiedDateTime] DATETIME NULL
    CONSTRAINT [FK_Slices_Jobs] FOREIGN KEY (JobId) REFERENCES Jobs(Id),
    CONSTRAINT [CK_Slices_Column] CHECK (1 = 1),
    CONSTRAINT [UQ_Slices_JobId_SliceId] UNIQUE(JobId, SliceId)
)
