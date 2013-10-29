
-- --------------------------------------------------
-- Date Created: 07/04/2013 15:07:20
-- compatible SQLite
-- Generated from EDMX file: E:\Workspace\Ostalo\Projects\WPF_Jukebox\ObdelajXtreamerProdatke\Models\MovieVo\MovieVo.edmx
-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

    
	DROP TABLE if exists [Movies];
    
	DROP TABLE if exists [Audios];
    
	DROP TABLE if exists [Files];
    
	DROP TABLE if exists [Videos];

	DROP TABLE if exists [Subtitles];
    
	DROP TABLE if exists [Arts];
    
	DROP TABLE if exists [Ratings];

	DROP TABLE if exists [Specials];

	DROP TABLE if exists [Studios];
    
	DROP TABLE if exists [Plots];
    
	DROP TABLE if exists [People];

	DROP TABLE if exists [Actors];
    
	DROP TABLE if exists [Genres];
    
	DROP TABLE if exists [Certifications];
    
	DROP TABLE if exists [MovieActors];

	DROP TABLE if exists [MovieDirectors];

	DROP TABLE if exists [MovieWriters];
    
	DROP TABLE if exists [MovieGenres];

	DROP TABLE if exists [MovieStudios];

	DROP TABLE if exists [MovieCountries];

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Movie'
CREATE TABLE [Movies] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Title] TEXT   NOT NULL ,
    [OriginalTitle] TEXT   NULL ,
    [SortTitle] TEXT   NULL ,
    [Year] integer   NULL ,
	[ReleaseDate] TEXT NULL,
	[LastPlayed] TEXT NULL,
	[Premiered] TEXT NULL,
	[Aired] TEXT NULL,
	[Trailer] TEXT NULL,
	[Top250] integer NULL,
    [Runtime] integer   NULL ,
	[Watched] boolean NOT NULL,
	[PlayCount] integer NOT NULL, 
    [RatingAverage] float   NULL ,
    [ImdbID] TEXT   NULL ,
    [TmdbID] TEXT   NULL ,
    [SetId] integer   NULL,
	[MainPlotId] integer NOT NULL,
	
	CONSTRAINT [FK_MoviePlot]
		FOREIGN KEY ([MainPlotId])
		REFERENCES [Plot]([Id]),

	CONSTRAINT [FK_MovieSet]
		FOREIGN KEY ([SetId])
		REFERENCES [Set]([Id])					
);

-- Creating table 'Audio'
CREATE TABLE [Audios] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Source] TEXT   NULL ,
    [Type] TEXT   NULL ,
    [Channels] TEXT   NULL ,
    [Codec] TEXT   NULL ,
    [MovieId] integer   NOT NULL, 
    [FileId] integer   NOT NULL, 
			
		,CONSTRAINT [FK_MovieAudio]
    		FOREIGN KEY ([MovieId])
    		REFERENCES [Movie] ([Id])					

		,CONSTRAINT [FK_AudioFile]
    		FOREIGN KEY ([FileId])
    		REFERENCES [File] ([Id])					    		
);

-- Creating table 'File'
CREATE TABLE [Files] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Extension] TEXT   NOT NULL ,
    [Name] TEXT   NOT NULL ,
    [FolderPath] TEXT   NOT NULL ,
    [InfoXML] TEXT   NOT NULL ,
    [Size] integer   NULL,
	[DateAdded] TEXT NULL,
	[MovieId] integer NOT NULL,

	CONSTRAINT [FK_MovieFile]
		FOREIGN KEY ([MovieId])
    	REFERENCES [Movie] ([Id])			
);

-- Creating table 'Video'
CREATE TABLE [Videos] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Source] TEXT   NULL ,
    [Type] TEXT   NULL ,
    [Format] TEXT   NULL ,
    [Codec] TEXT   NULL ,
    [Aspect] float   NULL ,
    [Width] integer   NULL ,
    [Height] integer   NULL, 
	[MovieId] integer NOT NULL,
	[FileId] integer NOT NULL,

	CONSTRAINT [FK_MovieVideo]
		FOREIGN KEY ([MovieId])
    	REFERENCES [Movie] ([Id]),

	CONSTRAINT [FK_VideoFile]
		FOREIGN KEY ([FileId])
    	REFERENCES [File] ([Id])
);

-- Creating table 'Art'
CREATE TABLE [Arts] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Type] integer   NOT NULL ,
    [Path] TEXT   NOT NULL ,
    [MovieId] integer   NOT NULL 
			
		,CONSTRAINT [FK_MovieArt]
    		FOREIGN KEY ([MovieId])
    		REFERENCES [Movie] ([Id])					
    		
);

-- Creating table 'Ratings'
CREATE TABLE [Ratings] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Critic] TEXT   NOT NULL ,
    [Value] float   NOT NULL ,
    [MovieId] integer   NOT NULL 
			
		,CONSTRAINT [FK_MovieRating]
    		FOREIGN KEY ([MovieId])
    		REFERENCES [Movie] ([Id])					
    		
);

-- Creating table 'Plot'
CREATE TABLE [Plots] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
	[Tagline] TEXT   NULL ,
    [Summary] TEXT   NULL ,
    [Full] TEXT   NOT NULL ,
    [Language] TEXT   NOT NULL ,
    [MovieId] integer   NOT NULL 
			
		,CONSTRAINT [FK_MoviePlot]
    		FOREIGN KEY ([MovieId])
    		REFERENCES [Movie] ([Id])					
    		
);

-- Creating table 'Person'
CREATE TABLE [Person] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Name] TEXT   NOT NULL,
	[Thumb] TEXT NULL,
	[Character] TEXT NULL,
	[Role] TEXT NULL
);

CREATE TABLE [Actor] (
	[PersonId] integer PRIMARY KEY NOT NULL,
	[Character] TEXT NULL,

	CONSTRAINT [FK_ActorPerson]
		FOREIGN KEY ([PersonId])
		REFERENCES [Person]([Id])
);

-- Creating table 'Genre'
CREATE TABLE [Genre] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Type] TEXT   NOT NULL 
);

-- Creating table 'Certification'
CREATE TABLE [Certification] (
    [Id] integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    [Country] TEXT   NOT NULL ,
    [Rating] float   NOT NULL ,
    [MovieId] integer   NOT NULL 
			
		,CONSTRAINT [FK_MovieCertification]
    		FOREIGN KEY ([MovieId])
    		REFERENCES [Movie] ([Id])					
    		
);

CREATE TABLE [Studio] (
	[Id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	[Name] TEXT NOT NULL,
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
    [Job] TEXT   NULL 
			
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