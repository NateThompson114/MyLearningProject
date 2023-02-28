CREATE TABLE [dbo].[Movie]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Slug] NCHAR(300) NOT NULL, 
    [Title] NCHAR(250) NOT NULL, 
    [YearOfRelease] INT NOT NULL
)

GO;

CREATE INDEX [IX_Movies_Slug] ON [dbo].[Movie] ([Slug])
GO;