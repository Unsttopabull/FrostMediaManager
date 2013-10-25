
-- --------------------------------------------------
-- Date Created: 07/04/2013 15:07:20
-- compatible SQLite
-- Generated from EDMX file: E:\Workspace\Ostalo\Projects\WPF_Jukebox\ObdelajXtreamerProdatke\Models\MovieVo\MovieVo.edmx
-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

    
	DROP TABLE if exists [Movie];
    
	DROP TABLE if exists [Audio];
    
	DROP TABLE if exists [File];
    
	DROP TABLE if exists [Video];
    
	DROP TABLE if exists [Art];
    
	DROP TABLE if exists [Ratings];
    
	DROP TABLE if exists [Plot];
    
	DROP TABLE if exists [Person];

	DROP TABLE if exists [Actor];
    
	DROP TABLE if exists [Genre];
    
	DROP TABLE if exists [Certification];
    
	DROP TABLE if exists [MoviePerson];
    
	DROP TABLE if exists [GenreMovie];

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Movie'
CREATE TABLE [Movie] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Title] nvarchar(2147483647)   NOT NULL ,
    [OriginalTitle] nvarchar(2147483647)   NULL ,
	[Country] nvarchar(2147483647) NULL,
    [Year] integer   NULL ,
	[ReleaseDate] nvarchar(2147483647) NULL,
    [Subtitle] nvarchar(2147483647)   NULL ,
    [Specials] nvarchar(2147483647)   NULL ,
    [Runtime] integer   NULL ,
    [FPS] integer   NULL ,
    [RatingAverage] float   NULL ,
    [ImdbID] nvarchar(2147483647)   NULL ,
    [Studio] nvarchar(2147483647)   NULL,
	[MainPlotId] integer NOT NULL,
	
	CONSTRAINT [FK_MoviePlot]
		FOREIGN KEY ([MainPlotId])
		REFERENCES [Plot]([Id])
					
);

-- Creating table 'Audio'
CREATE TABLE [Audio] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Source] nvarchar(2147483647)   NULL ,
    [Type] nvarchar(2147483647)   NULL ,
    [Channels] nvarchar(2147483647)   NULL ,
    [Codec] nvarchar(2147483647)   NULL ,
    [MovieId] integer   NOT NULL 
			
		,CONSTRAINT [FK_MovieAudio]
    		FOREIGN KEY ([MovieId])
    		REFERENCES [Movie] ([Id])					
    		
);

-- Creating table 'File'
CREATE TABLE [File] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Extension] nvarchar(2147483647)   NOT NULL ,
    [Name] nvarchar(2147483647)   NOT NULL ,
    [PathOnDrive] nvarchar(2147483647)   NOT NULL ,
    [InfoXML] nvarchar(2147483647)   NOT NULL ,
    [Size] integer   NULL,
	[MovieId] integer NOT NULL,

	CONSTRAINT [FK_MovieFile]
		FOREIGN KEY ([MovieId])
    	REFERENCES [Movie] ([Id])			
);

-- Creating table 'Video'
CREATE TABLE [Video] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Source] nvarchar(2147483647)   NULL ,
    [Type] nvarchar(2147483647)   NULL ,
    [Format] nvarchar(2147483647)   NULL ,
    [Codec] nvarchar(2147483647)   NULL ,
    [Aspect] float   NULL ,
    [Width] integer   NULL ,
    [Height] integer   NULL, 
	[MovieId] integer NOT NULL,

	CONSTRAINT [FK_MovieVideo]
		FOREIGN KEY ([MovieId])
    	REFERENCES [Movie] ([Id])
);

-- Creating table 'Art'
CREATE TABLE [Art] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Type] nvarchar(2147483647)   NOT NULL ,
    [Path] nvarchar(2147483647)   NOT NULL ,
    [MovieId] integer   NOT NULL 
			
		,CONSTRAINT [FK_MovieArt]
    		FOREIGN KEY ([MovieId])
    		REFERENCES [Movie] ([Id])					
    		
);

-- Creating table 'Ratings'
CREATE TABLE [Rating] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Critic] nvarchar(2147483647)   NOT NULL ,
    [Value] float   NOT NULL ,
    [MovieId] integer   NOT NULL 
			
		,CONSTRAINT [FK_MovieRating]
    		FOREIGN KEY ([MovieId])
    		REFERENCES [Movie] ([Id])					
    		
);

-- Creating table 'Plot'
CREATE TABLE [Plot] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Summary] nvarchar(2147483647)   NULL ,
    [Full] nvarchar(2147483647)   NOT NULL ,
    [Language] nvarchar(2147483647)   NOT NULL ,
    [MovieId] integer   NOT NULL 
			
		,CONSTRAINT [FK_MoviePlot]
    		FOREIGN KEY ([MovieId])
    		REFERENCES [Movie] ([Id])					
    		
);

-- Creating table 'Person'
CREATE TABLE [Person] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Name] nvarchar(2147483647)   NOT NULL
);

CREATE TABLE [Actor] (
	[PersonId] integer PRIMARY KEY NOT NULL,
	[Character] nvarchar(2147483647) NULL,

	CONSTRAINT [FK_ActorPerson]
		FOREIGN KEY ([PersonId])
		REFERENCES [Person]([Id])
);

-- Creating table 'Genre'
CREATE TABLE [Genre] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Type] nvarchar(2147483647)   NOT NULL 
);

-- Creating table 'Certification'
CREATE TABLE [Certification] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Country] nvarchar(2147483647)   NOT NULL ,
    [Rating] float   NOT NULL ,
    [MovieId] integer   NOT NULL 
			
		,CONSTRAINT [FK_MovieCertification]
    		FOREIGN KEY ([MovieId])
    		REFERENCES [Movie] ([Id])					
    		
);

CREATE TABLE [Studio] (
	[Id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	[Name] nvarchar(2147483647) NOT NULL,
	[MovieId] integer NOT NULL,

	CONSTRAINT [FK_MovieStudio]
		FOREIGN KEY ([MovieId])
		REFERENCES [Movie] ([Id])
);

-- Creating table 'MoviePerson'
CREATE TABLE [MoviePerson] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [MovieId] integer   NOT NULL ,
    [PersonId] integer   NOT NULL ,
    [Job] nvarchar(2147483647)   NULL 
			
		,CONSTRAINT [FK_MovieMoviePerson]
    		FOREIGN KEY ([MovieId])
    		REFERENCES [Movie] ([Id])					
    		
						
		,CONSTRAINT [FK_MoviePersonPerson]
    		FOREIGN KEY ([PersonId])
    		REFERENCES [Person] ([Id])					
    		
);

-- Creating table 'MovieGenre'
CREATE TABLE [MovieGenres] (
    [MovieId] integer   NOT NULL ,
    [GenreId] integer   NOT NULL 
 , PRIMARY KEY ([MovieId], [GenreId])	
					
		,CONSTRAINT [FK_MovieGenre_Movie]
    		FOREIGN KEY ([MovieId])
    		REFERENCES [Movie] ([Id])					
    		
						
		,CONSTRAINT [FK_MovieGenre_Genre]
    		FOREIGN KEY ([GenreId])
    		REFERENCES [Genre] ([Id])					
    		
);

-- Creating table 'MovieCountries'
CREATE TABLE [MovieCountries] (
    [MovieId] integer   NOT NULL ,
    [CountryId] integer   NOT NULL 
 , PRIMARY KEY ([MovieId], [CountryId])	
					
		,CONSTRAINT [FK_MovieCountries_Movie]
    		FOREIGN KEY ([MovieId])
    		REFERENCES [Movie] ([Id])					
    		
						
		,CONSTRAINT [FK_MovieCountries_Genre]
    		FOREIGN KEY ([CountryId])
    		REFERENCES [Country] ([Id])					
    		
);

-- Creating table 'MovieStudios'
CREATE TABLE [MovieStudios] (
    [MovieId] integer   NOT NULL ,
    [StudioId] integer   NOT NULL 
 , PRIMARY KEY ([MovieId], [StudioId])	
					
		,CONSTRAINT [FK_MovieStudios_Movie]
    		FOREIGN KEY ([MovieId])
    		REFERENCES [Movie] ([Id])					
    		
						
		,CONSTRAINT [FK_MovieStudios_Genre]
    		FOREIGN KEY ([StudioId])
    		REFERENCES [Studio] ([Id])					
    		
);


-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------