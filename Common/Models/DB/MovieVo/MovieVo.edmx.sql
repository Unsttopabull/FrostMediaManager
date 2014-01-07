    DROP TABLE if exists "Sets";
    
    DROP TABLE if exists Specials;
    
    DROP TABLE if exists MovieSpecials;
    
    DROP TABLE if exists Countries;
    
    DROP TABLE if exists Languages;
    
    DROP TABLE if exists Movies;

    DROP TABLE if exists Audios;

    DROP TABLE if exists Files;

    DROP TABLE if exists Videos;

    DROP TABLE if exists Subtitles;

    DROP TABLE if exists Arts;

    DROP TABLE if exists Ratings;

    DROP TABLE if exists Specials;

    DROP TABLE if exists Studios;

    DROP TABLE if exists Plots;

    DROP TABLE if exists People;

    DROP TABLE if exists Actors;

    DROP TABLE if exists Genres;

    DROP TABLE if exists Certifications;

    DROP TABLE if exists MovieActors;

    DROP TABLE if exists MovieDirectors;

    DROP TABLE if exists MovieWriters;

    DROP TABLE if exists MovieGenres;

    DROP TABLE if exists MovieStudios;

    DROP TABLE if exists MovieCountries;

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Set'
CREATE TABLE "Sets" (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT NOT NULL
);

-- Creating table 'Special'
CREATE TABLE Specials (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Value TEXT NOT NULL
);

-- Creating table 'Movie'
CREATE TABLE Movies (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Title TEXT NOT NULL ,
    OriginalTitle TEXT   NULL ,
    SortTitle TEXT   NULL ,
    Goofs TEXT NULL,
    Trivia TEXT NULL,
    ReleaseYear integer   NULL ,
    ReleaseDate TEXT NULL,
    Edithion TEXT NULL,
    DvDRegion int NOT NULL,
    LastPlayed TEXT NULL,
    Premiered TEXT NULL,
    Aired TEXT NULL,
    Trailer TEXT NULL,
    Top250 integer NULL,
    Runtime integer   NULL ,
    Watched boolean NOT NULL,
    PlayCount integer NOT NULL,
    RatingAverage double   NULL ,
    ImdbID TEXT   NULL ,
    TmdbID TEXT   NULL ,
    ReleaseGroup TEXT NULL,
    IsMultipart boolean NOT NULL,
    PartTypes TEXT NULL,
    SetId integer   NULL,
    MainPlotId integer NOT NULL,

    CONSTRAINT FK_MoviePlot
        FOREIGN KEY (MainPlotId)
        REFERENCES Plot(Id),

    CONSTRAINT FK_MovieSet
        FOREIGN KEY (SetId)
        REFERENCES "Sets"(Id)
);

-- Creating table 'MovieSpecials'
CREATE TABLE MovieSpecials (
    MovieId integer   NOT NULL ,
    SpecialId integer   NOT NULL
 , PRIMARY KEY (MovieId, SpecialId)

        ,CONSTRAINT FK_MovieSpecial_Movie
            FOREIGN KEY (MovieId)
            REFERENCES Movie (Id)


        ,CONSTRAINT FK_MovieSpecial_Special
            FOREIGN KEY (SpecialId)
            REFERENCES Special (Id)
);

-- Creating table 'Country'
CREATE TABLE Countries (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT NULL,
    Alpha2 TEXT NULL,
    Alpha3 TEXT NULL
);

-- Creating table 'MovieCountries'
CREATE TABLE MovieCountries (
    MovieId integer   NOT NULL ,
    CountryId integer   NOT NULL
 , PRIMARY KEY (MovieId, CountryId)

        ,CONSTRAINT FK_MovieCountries_Movie
            FOREIGN KEY (MovieId)
            REFERENCES Movie (Id)


        ,CONSTRAINT FK_MovieCountries_Genre
            FOREIGN KEY (CountryId)
            REFERENCES Country (Id)

);

-- Creating table 'Language'
CREATE TABLE Languages (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT NULL,
    Alpha2 TEXT NULL,
    Alpha3 TEXT NULL,
    CountryId integer NULL,

    CONSTRAINT FK_LanguageCountry
        FOREIGN KEY (CountryId)
        REFERENCES Country(Id)
);

-- Creating table 'Audio'
CREATE TABLE Audios (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Source TEXT   NULL ,
    Type TEXT   NULL ,
    ChannelSetup TEXT   NULL ,
    NumberOfChannels integer   NULL ,
    ChannelPositions TEXT   NULL ,
    Codec TEXT   NULL ,
    BitRate float   NULL ,
    BitRateMode integer NOT NULL,
    SamplingRate integer NULL,
    BitDepth integer NULL,
    CompressionMode integer NOT NULL,
    Duration integer NULL,
    LanguageId integer NULL,
    MovieId integer   NOT NULL,
    FileId integer   NOT NULL

        ,CONSTRAINT FK_LanguageAudio
            FOREIGN KEY (LanguageId)
            REFERENCES Language (Id)

        ,CONSTRAINT FK_MovieAudio
            FOREIGN KEY (MovieId)
            REFERENCES Movie (Id)

        ,CONSTRAINT FK_AudioFile
            FOREIGN KEY (FileId)
            REFERENCES File (Id)
);

-- Creating table 'File'
CREATE TABLE Files (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Extension TEXT   NOT NULL ,
    Name TEXT   NOT NULL ,
    FolderPath TEXT   NOT NULL ,
    Size integer   NULL,
    DateAdded TEXT NULL,
    MovieId integer NOT NULL,

    CONSTRAINT FK_MovieFile
        FOREIGN KEY (MovieId)
        REFERENCES Movie (Id)
);

-- Creating table 'Subtitle'
CREATE TABLE Subtitles (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    PodnapisiId integer NULL,
    OpenSubtitlesId integer NULL,
    MD5 TEXT NULL,
    Format TEXT NULL,
    Encoding TEXT NULL,
    EmbededInVideo boolean NOT NULL,
    ForHearingImpaired boolean NOT NULL,
    LanguageId integer NULL,
    MovieId integer NOT NULL,
    FileId integer NOT NULL,

    CONSTRAINT FK_SubtitleLanguage
        FOREIGN KEY (LanguageId)
        REFERENCES Language (Id)

    ,CONSTRAINT FK_SubtitleMovie
        FOREIGN KEY (MovieId)
        REFERENCES Movie (Id)

    ,CONSTRAINT FK_SubtitleFile
        FOREIGN KEY (FileId)
        REFERENCES File (Id)
);

-- Creating table 'Video'
CREATE TABLE Videos (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    MovieHash TEXT NOT NULL,
    Source TEXT   NULL ,
    Type TEXT   NULL ,
    Resolution TEXT NULL,
    FPS TEXT NULL,
    BitRate float NULL,
    BitRateMode integer NOT NULL,
    BitDepth integer NULL,
    CompressionMode integer NOT NULL,
    Duration integer NULL,
    ScanType integer NOT NULL,
    ColorSpace TEXT NULL,
    ChromaSubsampling TEXT NULL,
    Format TEXT   NULL ,
    Codec TEXT   NULL ,
    Aspect float   NULL ,
    AspectCommercialName string null,
    Width integer   NULL ,
    Height integer   NULL,
    LanguageId integer NULL,
    MovieId integer NOT NULL,
    FileId integer NOT NULL,

    CONSTRAINT FK_LanguageVideo
        FOREIGN KEY (LanguageId)
        REFERENCES Language (Id),

    CONSTRAINT FK_MovieVideo
        FOREIGN KEY (MovieId)
        REFERENCES Movie (Id),

    CONSTRAINT FK_VideoFile
        FOREIGN KEY (FileId)
        REFERENCES File (Id)
);

-- Creating table 'Art'
CREATE TABLE Arts (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Type integer   NOT NULL ,
    Path TEXT   NOT NULL ,
    Preview TEXT NOT NULL,
    MovieId integer   NOT NULL

        ,CONSTRAINT FK_MovieArt
            FOREIGN KEY (MovieId)
            REFERENCES Movie (Id)

);

-- Creating table 'Ratings'
CREATE TABLE Ratings (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Critic TEXT   NOT NULL ,
    Value float   NOT NULL ,
    MovieId integer   NOT NULL

        ,CONSTRAINT FK_MovieRating
            FOREIGN KEY (MovieId)
            REFERENCES Movie (Id)

);

-- Creating table 'Plot'
CREATE TABLE Plots (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Tagline TEXT   NULL ,
    Summary TEXT   NULL ,
    Full TEXT   NOT NULL ,
    Language TEXT   NOT NULL ,
    MovieId integer   NOT NULL

        ,CONSTRAINT FK_MoviePlot
            FOREIGN KEY (MovieId)
            REFERENCES Movie (Id)

);

-- Creating table 'People'
CREATE TABLE People (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT   NOT NULL,
    Thumb TEXT NULL
);

-- Creating table 'MovieDirectors'
CREATE TABLE MovieDirectors (
    MovieId integer   NOT NULL ,
    DirectorId integer   NOT NULL
 , PRIMARY KEY (MovieId, DirectorId)

        ,CONSTRAINT FK_MovieDirector_Movie
            FOREIGN KEY (MovieId)
            REFERENCES Movie (Id)


        ,CONSTRAINT FK_MovieDirector_Genre
            FOREIGN KEY (DirectorId)
            REFERENCES People (Id)
);

-- Creating table 'MovieWriters'
CREATE TABLE MovieWriters (
    MovieId integer   NOT NULL ,
    WriterId integer   NOT NULL
 , PRIMARY KEY (MovieId, WriterId)

        ,CONSTRAINT FK_MovieWriter_Movie
            FOREIGN KEY (MovieId)
            REFERENCES Movie (Id)


        ,CONSTRAINT FK_MovieWriter_Genre
            FOREIGN KEY (WriterId)
            REFERENCES People (Id)
);

-- Creating table 'MovieActors'
CREATE TABLE MovieActors (
    Id integer PRIMARY KEY NOT NULL,
    PersonId integer NOT NULL,
    MovieId integer NOT NULL,
    Character TEXT NULL,

    CONSTRAINT FK_ActorPerson
        FOREIGN KEY (PersonId)
        REFERENCES People(Id)

    ,CONSTRAINT FK_ActorMovie
        FOREIGN KEY (MovieId)
        REFERENCES Movie(Id)
);

-- Creating table 'Genres'
CREATE TABLE Genres (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT   NOT NULL
);

-- Creating table 'MovieGenre'
CREATE TABLE MovieGenres (
    MovieId integer   NOT NULL ,
    GenreId integer   NOT NULL
 , PRIMARY KEY (MovieId, GenreId)

        ,CONSTRAINT FK_MovieGenre_Movie
            FOREIGN KEY (MovieId)
            REFERENCES Movie (Id)


        ,CONSTRAINT FK_MovieGenre_Genre
            FOREIGN KEY (GenreId)
            REFERENCES Genre (Id)

);

-- Creating table 'Certifications'
CREATE TABLE Certifications (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Rating float      NOT NULL,
    MovieId integer   NOT NULL,
    CountryId integer NOT NULL

        ,CONSTRAINT FK_MovieCertification
            FOREIGN KEY (MovieId)
            REFERENCES Movie (Id)

        ,CONSTRAINT FK_CertificationLanguage
            FOREIGN KEY (CountryId)
            REFERENCES Country (Id)
);

CREATE TABLE Studios (
    Id integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL,
    Logo TEXT NOT NULL
);


-- Creating table 'MovieStudios'
CREATE TABLE MovieStudios (
    MovieId integer   NOT NULL ,
    StudioId integer   NOT NULL
 , PRIMARY KEY (MovieId, StudioId)

        ,CONSTRAINT FK_MovieStudios_Movie
            FOREIGN KEY (MovieId)
            REFERENCES Movie (Id)


        ,CONSTRAINT FK_MovieStudios_Genre
            FOREIGN KEY (StudioId)
            REFERENCES Studio (Id)

);