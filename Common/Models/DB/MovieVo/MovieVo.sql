/*

Target Server Type    : SQLite
Target Server Version : 30706
File Encoding         : 65001

Date: 2014-01-06 19:53:18
*/

/*PRAGMA foreign_keys = OFF;*/

-- ----------------------------
-- Table structure for "Arts"
-- ----------------------------
DROP TABLE "Arts";
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

-- ----------------------------
-- Records of Arts
-- ----------------------------

-- ----------------------------
-- Table structure for "Audios"
-- ----------------------------
DROP TABLE "Audios";
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

-- ----------------------------
-- Records of Audios
-- ----------------------------

-- ----------------------------
-- Table structure for "Certifications"
-- ----------------------------
DROP TABLE "Certifications";
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

-- ----------------------------
-- Records of Certifications
-- ----------------------------

-- ----------------------------
-- Table structure for "Countries"
-- ----------------------------
DROP TABLE "Countries";
CREATE TABLE Countries (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT NULL,
    Alpha2 TEXT NULL,
    Alpha3 TEXT NULL
);

-- ----------------------------
-- Records of Countries
-- ----------------------------

-- ----------------------------
-- Table structure for "Files"
-- ----------------------------
DROP TABLE "Files";
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

-- ----------------------------
-- Records of Files
-- ----------------------------

-- ----------------------------
-- Table structure for "Genres"
-- ----------------------------
DROP TABLE "Genres";
CREATE TABLE Genres (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT   NOT NULL
);

-- ----------------------------
-- Records of Genres
-- ----------------------------

-- ----------------------------
-- Table structure for "Languages"
-- ----------------------------
DROP TABLE "Languages";
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

-- ----------------------------
-- Records of Languages
-- ----------------------------

-- ----------------------------
-- Table structure for "MovieActors"
-- ----------------------------
DROP TABLE "MovieActors";
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

-- ----------------------------
-- Records of MovieActors
-- ----------------------------

-- ----------------------------
-- Table structure for "MovieCountries"
-- ----------------------------
DROP TABLE "MovieCountries";
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

-- ----------------------------
-- Records of MovieCountries
-- ----------------------------

-- ----------------------------
-- Table structure for "MovieDirectors"
-- ----------------------------
DROP TABLE "MovieDirectors";
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

-- ----------------------------
-- Records of MovieDirectors
-- ----------------------------

-- ----------------------------
-- Table structure for "MovieGenres"
-- ----------------------------
DROP TABLE "MovieGenres";
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

-- ----------------------------
-- Records of MovieGenres
-- ----------------------------

-- ----------------------------
-- Table structure for "Movies"
-- ----------------------------
DROP TABLE "Movies";
CREATE TABLE "Movies" (
"Id"  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
"ReleaseYear"  INTEGER,
"Title"  TEXT NOT NULL,
"OriginalTitle"  TEXT,
"SortTitle"  TEXT,
"Goofs"  TEXT,
"Trivia"  TEXT,
"Year"  integer,
"ReleaseDate"  TEXT,
"Edithion"  TEXT,
"DvDRegion"  int NOT NULL,
"LastPlayed"  TEXT,
"Premiered"  TEXT,
"Aired"  TEXT,
"Trailer"  TEXT,
"Top250"  integer,
"Runtime"  integer,
"Watched"  boolean NOT NULL,
"PlayCount"  integer NOT NULL,
"RatingAverage"  double,
"ImdbID"  TEXT,
"TmdbID"  TEXT,
"ReleaseGroup"  TEXT,
"IsMultipart"  boolean NOT NULL,
"PartTypes"  TEXT,
"SetId"  integer,
"MainPlotId"  integer NOT NULL,
CONSTRAINT "FK_MoviePlot" FOREIGN KEY ("MainPlotId") REFERENCES "Plot" ("Id"),
CONSTRAINT "FK_MovieSet" FOREIGN KEY ("SetId") REFERENCES "Sets" ("Id")
);

-- ----------------------------
-- Table structure for "MovieSpecials"
-- ----------------------------
DROP TABLE "MovieSpecials";
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

-- ----------------------------
-- Records of MovieSpecials
-- ----------------------------

-- ----------------------------
-- Table structure for "MovieStudios"
-- ----------------------------
DROP TABLE "MovieStudios";
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

-- ----------------------------
-- Records of MovieStudios
-- ----------------------------

-- ----------------------------
-- Table structure for "MovieWriters"
-- ----------------------------
DROP TABLE "MovieWriters";
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

-- ----------------------------
-- Records of MovieWriters
-- ----------------------------

-- ----------------------------
-- Table structure for "People"
-- ----------------------------
DROP TABLE "People";
CREATE TABLE People (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT   NOT NULL,
    Thumb TEXT NULL
);

-- ----------------------------
-- Records of People
-- ----------------------------

-- ----------------------------
-- Table structure for "Plots"
-- ----------------------------
DROP TABLE "Plots";
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

-- ----------------------------
-- Records of Plots
-- ----------------------------

-- ----------------------------
-- Table structure for "Ratings"
-- ----------------------------
DROP TABLE "Ratings";
CREATE TABLE Ratings (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Critic TEXT   NOT NULL ,
    Value float   NOT NULL ,
    MovieId integer   NOT NULL

        ,CONSTRAINT FK_MovieRating
            FOREIGN KEY (MovieId)
            REFERENCES Movie (Id)

);

-- ----------------------------
-- Records of Ratings
-- ----------------------------

-- ----------------------------
-- Table structure for "Set"
-- ----------------------------
DROP TABLE "Set";
CREATE TABLE "Set" (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT NOT NULL
);

-- ----------------------------
-- Records of Set
-- ----------------------------

-- ----------------------------
-- Table structure for "Sets"
-- ----------------------------
DROP TABLE "Sets";
CREATE TABLE "Sets" (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT NOT NULL
);

-- ----------------------------
-- Records of Sets
-- ----------------------------

-- ----------------------------
-- Table structure for "Specials"
-- ----------------------------
DROP TABLE "Specials";
CREATE TABLE Specials (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Value TEXT NOT NULL
);

-- ----------------------------
-- Records of Specials
-- ----------------------------

-- ----------------------------
-- Table structure for "sqlite_sequence"
-- ----------------------------
DROP TABLE "sqlite_sequence";
CREATE TABLE sqlite_sequence(name,seq);

-- ---------------------------

-- ----------------------------
-- Table structure for "Studios"
-- ----------------------------
DROP TABLE "Studios";
CREATE TABLE Studios (
    Id integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL,
    Logo TEXT NOT NULL
);

-- ----------------------------
-- Records of Studios
-- ----------------------------

-- ----------------------------
-- Table structure for "Subtitles"
-- ----------------------------
DROP TABLE "Subtitles";
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

-- ----------------------------
-- Records of Subtitles
-- ----------------------------

-- ----------------------------
-- Table structure for "Videos"
-- ----------------------------
DROP TABLE "Videos";
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

-- ----------------------------
-- Records of Videos
-- ----------------------------