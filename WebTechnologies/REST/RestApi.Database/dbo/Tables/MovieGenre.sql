CREATE TABLE [dbo].[MovieGenre]
(
	[MovieId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [GenreId] UNIQUEIDENTIFIER NOT NULL,
	
	CONSTRAINT [FK_MovieGenre_Movie_Id] FOREIGN KEY (MovieId) REFERENCES [dbo].[Movie],
	CONSTRAINT [FK_MovieGenre_Genre_Id] FOREIGN KEY (GenreId) REFERENCES [dbo].[Genre]
);

GO
CREATE NONCLUSTERED INDEX [IX_MovieGenre_PaymentContainerLifecycleEventExternalId]
    ON [dbo].[MovieGenre]([MovieId] ASC)
	WITH(DATA_COMPRESSION = PAGE, FILLFACTOR=90);
GO
