CREATE TABLE [dbo].[AuditEntries]
(
	Id INT PRIMARY KEY IDENTITY(1,1),
    --EntityName NVARCHAR(50) NOT NULL,
    --Action NVARCHAR(10) NOT NULL,
    --EntityId NVARCHAR(50) NOT NULL,
    Metadata NVARCHAR(MAX),
    StartTimeUtc DATETIME2 NOT NULL,
    EndTimeUtc DATETIME2 NOT NULL,
    Succeeded BIT NOT NULL,
)
