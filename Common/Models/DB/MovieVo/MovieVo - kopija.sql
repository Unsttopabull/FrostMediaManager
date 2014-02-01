/*
Target Server Type    : SQLite
Target Server Version : 30706
File Encoding         : 65001

Date: 2014-01-08 15:45:38
*/

-- ----------------------------
-- Table structure for "EdmMetadata"
-- ----------------------------
CREATE TABLE IF NOT EXISTS "EdmMetadata" (
	"Id"  TEXT,
	"ModelHash"  TEXT
);

-- ----------------------------
-- Table structure for "__MigrationHistory"
-- ----------------------------
CREATE TABLE IF NOT EXISTS "__MigrationHistory" (
	"MigrationId"  TEXT NOT NULL,
	"Model"  BLOB NOT NULL,
	"ProductVersion"  TEXT NOT NULL
);

-- ----------------------------
-- Table structure for "Arts"
-- ----------------------------
DROP TABLE IF EXISTS "Arts";
CREATE TABLE "Arts" (
	"Id"  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Type"  integer NOT NULL,
	"Path"  TEXT NOT NULL,
	"Preview"  TEXT NULL,
	"MovieId"  integer NOT NULL,
	CONSTRAINT "FK_MovieArt" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------
-- Table structure for "Audios"
-- ----------------------------
DROP TABLE IF EXISTS "Audios";
CREATE TABLE "Audios" (
	"Id"  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Source"  TEXT,
	"Type"  TEXT,
	"ChannelSetup"  TEXT,
	"NumberOfChannels"  integer,
	"ChannelPositions"  TEXT,
	"Codec"  TEXT,
	"CodecId"  TEXT,
	"BitRate"  float,
	"BitRateMode"  integer NOT NULL,
	"SamplingRate"  integer,
	"BitDepth"  integer,
	"CompressionMode"  integer NOT NULL,
	"Duration"  integer,
	"LanguageId"  integer,
	"MovieId"  integer NOT NULL,
	"FileId"  integer NOT NULL,
	CONSTRAINT "FK_LanguageAudio" FOREIGN KEY ("LanguageId") REFERENCES "Languages" ("Id") ON DELETE SET NULL ON UPDATE CASCADE,
	CONSTRAINT "FK_MovieAudio" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT "FK_AudioFile" FOREIGN KEY ("FileId") REFERENCES "Files" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------
-- Table structure for "Certifications"
-- ----------------------------
DROP TABLE IF EXISTS "Certifications";
CREATE TABLE "Certifications" (
	"Id"  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Rating"  TEXT NOT NULL,
	"MovieId"  integer NOT NULL,
	"CountryId"  integer NOT NULL,
	CONSTRAINT "FK_MovieCertification" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT "FK_CertificationLanguage" FOREIGN KEY ("CountryId") REFERENCES "Countries" ("Id") ON DELETE SET NULL ON UPDATE CASCADE
);

-- ----------------------------
-- Table structure for "Countries"
-- ----------------------------
DROP TABLE IF EXISTS "Countries";
CREATE TABLE "Countries" (
	"Id"  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Name"  TEXT,
	"ISO3166_Alpha2"  TEXT COLLATE NOCASE,
	"ISO3166_Alpha3"  TEXT COLLATE NOCASE
);

-- ----------------------------
-- Table structure for "Files"
-- ----------------------------
DROP TABLE IF EXISTS "Files";
CREATE TABLE "Files" (
	"Id"  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Extension"  TEXT NOT NULL,
	"Name"  TEXT NOT NULL,
	"FolderPath"  TEXT NOT NULL,
	"Size"  integer,
	"DateAdded"  TEXT
);

-- ----------------------------
-- Table structure for "Genres"
-- ----------------------------
DROP TABLE IF EXISTS "Genres";
CREATE TABLE Genres (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT   NOT NULL  COLLATE NOCASE
);

DROP TABLE IF EXISTS "PromotionalVideos";
CREATE TABLE PromotionalVideos (
	Id integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	Type integer NOT NULL,
	Title TEXT NOT NULL,
	Url TEXT NOT NULL,
	Duration TEXT,
	Language TEXT,
	SubtitleLanguage TEXT,
	MovieId integer NOT NULL,
	
	CONSTRAINT "FK_MoviePromotionalVideo"
		FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id")
		ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------
-- Table structure for "Awards"
-- ----------------------------
DROP TABLE IF EXISTS "Awards";
CREATE TABLE Awards (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Organization TEXT   NOT NULL  COLLATE NOCASE,
	IsNomination boolean NOT NULL,
	AwardType TEXT NOT NULL 
);

-- ----------------------------
-- Table structure for "Languages"
-- ----------------------------
DROP TABLE IF EXISTS "Languages";
CREATE TABLE "Languages" (
	"Id"  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Name"  TEXT  COLLATE NOCASE,
	"ISO639_Alpha2"  TEXT  COLLATE NOCASE,
	"ISO639_Alpha3"  TEXT  COLLATE NOCASE
);

-- ----------------------------
-- Table structure for "MovieActors"
-- ----------------------------
DROP TABLE IF EXISTS "MovieActors";
CREATE TABLE "MovieActors" (
	"Id"  integer NOT NULL,
	"PersonId"  integer NOT NULL,
	"MovieId"  integer NOT NULL,
	"Character"  TEXT,
	PRIMARY KEY ("Id" ASC),
	CONSTRAINT "FK_ActorPerson" FOREIGN KEY ("PersonId") REFERENCES "People" ("Id") ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT "FK_ActorMovie" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------
-- Table structure for "MovieCountries"
-- ----------------------------
DROP TABLE IF EXISTS "MovieCountries";
CREATE TABLE "MovieCountries" (
	"MovieId"  integer NOT NULL,
	"CountryId"  integer NOT NULL,
	PRIMARY KEY ("MovieId" ASC, "CountryId" ASC),
	CONSTRAINT "FK_MovieCountries_Movie" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT "FK_MovieCountries_Genre" FOREIGN KEY ("CountryId") REFERENCES "Countries" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------
-- Table structure for "MovieDirectors"
-- ----------------------------
DROP TABLE IF EXISTS "MovieDirectors";
CREATE TABLE "MovieDirectors" (
	"MovieId"  integer NOT NULL,
	"DirectorId"  integer NOT NULL,
	PRIMARY KEY ("MovieId" ASC, "DirectorId" ASC),
	CONSTRAINT "FK_MovieDirector_Movie" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT "FK_MovieDirector_Genre" FOREIGN KEY ("DirectorId") REFERENCES "People" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------
-- Table structure for "MovieGenres"
-- ----------------------------
DROP TABLE IF EXISTS "MovieGenres";
CREATE TABLE "MovieGenres" (
	"MovieId"  integer NOT NULL,
	"GenreId"  integer NOT NULL,
	PRIMARY KEY ("MovieId" ASC, "GenreId" ASC),
	CONSTRAINT "FK_MovieGenre_Movie" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT "FK_MovieGenre_Genre" FOREIGN KEY ("GenreId") REFERENCES "Genres" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------
-- Table structure for "MovieAwards"
-- ----------------------------
DROP TABLE IF EXISTS "MovieAwards";
CREATE TABLE "MovieAwards" (
	"MovieId"  integer NOT NULL,
	"AwardId"  integer NOT NULL,
	PRIMARY KEY ("MovieId" ASC, "AwardId" ASC),
	CONSTRAINT "FK_MovieAward_Movie" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT "FK_MovieAward_Award" FOREIGN KEY ("AwardId") REFERENCES "Awards" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------
-- Table structure for "Movies"
-- ----------------------------
DROP TABLE IF EXISTS "Movies";
	CREATE TABLE "Movies" (
	"Id"  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Title"  TEXT,
	"ReleaseYear"  INTEGER,
	"OriginalTitle"  TEXT,
	"SortTitle"  TEXT,
	"Type" TEXT,
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
	"DirectoryPath" TEXT NOT NULL,
	"SetId"  integer,
	CONSTRAINT "FK_MovieSet" FOREIGN KEY ("SetId") REFERENCES "Sets" ("Id")
);

-- ----------------------------
-- Table structure for "MovieSpecials"
-- ----------------------------
DROP TABLE IF EXISTS "MovieSpecials";
CREATE TABLE "MovieSpecials" (
	"MovieId"  integer NOT NULL,
	"SpecialId"  integer NOT NULL,
	PRIMARY KEY ("MovieId" ASC, "SpecialId" ASC),
	CONSTRAINT "FK_MovieSpecial_Movie" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT "FK_MovieSpecial_Special" FOREIGN KEY ("SpecialId") REFERENCES "Specials" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------
-- Table structure for "MovieStudios"
-- ----------------------------
DROP TABLE IF EXISTS "MovieStudios";
CREATE TABLE "MovieStudios" (
	"MovieId"  integer NOT NULL,
	"StudioId"  integer NOT NULL,
	PRIMARY KEY ("MovieId" ASC, "StudioId" ASC),
	CONSTRAINT "FK_MovieStudios_Movie" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT "FK_MovieStudios_Genre" FOREIGN KEY ("StudioId") REFERENCES "Studios" ("Id") ON DELETE CASCADE
);

-- ----------------------------
-- Table structure for "MovieWriters"
-- ----------------------------
DROP TABLE IF EXISTS "MovieWriters";
CREATE TABLE "MovieWriters" (
	"MovieId"  integer NOT NULL,
	"WriterId"  integer NOT NULL,
	PRIMARY KEY ("MovieId" ASC, "WriterId" ASC),
	CONSTRAINT "FK_MovieWriter_Movie" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT "FK_MovieWriter_Genre" FOREIGN KEY ("WriterId") REFERENCES "People" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------
-- Table structure for "People"
-- ----------------------------
DROP TABLE IF EXISTS "People";
CREATE TABLE People (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT   NOT NULL  COLLATE NOCASE,
    Thumb TEXT NULL,
	ImdbID TEXT NULL COLLATE NOCASE
);

-- ----------------------------
-- Table structure for "Plots"
-- ----------------------------
DROP TABLE IF EXISTS "Plots";
CREATE TABLE "Plots" (
	"Id"  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Tagline"  TEXT,
	"Summary"  TEXT,
	"Full"  TEXT NOT NULL,
	"Language"  TEXT NULL,
	"MovieId"  integer NOT NULL,
	CONSTRAINT "FK_MoviePlot" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------
-- Table structure for "Ratings"
-- ----------------------------
DROP TABLE IF EXISTS "Ratings";
CREATE TABLE "Ratings" (
	"Id"  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Critic"  TEXT NOT NULL,
	"Value"  float NOT NULL,
	"MovieId"  integer NOT NULL,
	CONSTRAINT "FK_MovieRating" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);
-- ----------------------------
-- Table structure for "Sets"
-- ----------------------------
DROP TABLE IF EXISTS "Sets";
CREATE TABLE "Sets" (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT NOT NULL  COLLATE NOCASE
);

-- ----------------------------
-- Table structure for "Specials"
-- ----------------------------
DROP TABLE IF EXISTS "Specials";
CREATE TABLE Specials (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Value TEXT NOT NULL  COLLATE NOCASE
);

-- ----------------------------
-- Table structure for "Studios"
-- ----------------------------
DROP TABLE IF EXISTS "Studios";
CREATE TABLE Studios (
    Id integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL  COLLATE NOCASE,
    Logo TEXT NULL
);

-- ----------------------------
-- Table structure for "Subtitles"
-- ----------------------------
DROP TABLE IF EXISTS "Subtitles";
CREATE TABLE "Subtitles" (
	"Id"  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	"PodnapisiId"  integer,
	"OpenSubtitlesId"  integer,
	"MD5"  TEXT,
	"Format"  TEXT,
	"Encoding"  TEXT,
	"EmbededInVideo"  boolean NOT NULL,
	"ForHearingImpaired"  boolean NOT NULL,
	"LanguageId"  integer,
	"MovieId"  integer NOT NULL,
	"FileId"  integer NOT NULL,
	CONSTRAINT "FK_SubtitleLanguage" FOREIGN KEY ("LanguageId") REFERENCES "Languages" ("Id") ON DELETE SET NULL ON UPDATE CASCADE,
	CONSTRAINT "FK_SubtitleMovie" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT "FK_SubtitleFile" FOREIGN KEY ("FileId") REFERENCES "Files" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------
-- Table structure for "Videos"
-- ----------------------------
DROP TABLE IF EXISTS "Videos";
CREATE TABLE "Videos" (
	"Id"  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	"MovieHash"  TEXT NOT NULL,
	"Source"  TEXT,
	"Type"  TEXT,
	"Resolution"  integer,
	"Standard" TEXT,
	"FPS"  float,
	"BitRate"  float,
	"BitRateMode"  integer NOT NULL,
	"BitDepth"  integer,
	"CompressionMode"  integer NOT NULL,
	"Duration"  integer,
	"ScanType"  integer NOT NULL,
	"ColorSpace"  TEXT,
	"ChromaSubsampling"  TEXT,
	"Format"  TEXT,
	"Codec"  TEXT,
	"CodecId"  TEXT,
	"Aspect"  float,
	"AspectCommercialName"  TEXT,
	"Width"  integer,
	"Height"  integer,
	"LanguageId"  integer,
	"MovieId"  integer NOT NULL,
	"FileId"  integer NOT NULL,
	CONSTRAINT "FK_LanguageVideo" FOREIGN KEY ("LanguageId") REFERENCES "Languages" ("Id") ON DELETE SET NULL ON UPDATE CASCADE,
	CONSTRAINT "FK_MovieVideo" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT "FK_VideoFile" FOREIGN KEY ("FileId") REFERENCES "Files" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------
-- View structure for "MovieActorsCharacters"
-- ----------------------------
DROP VIEW IF EXISTS "MovieActorsCharacters";
CREATE VIEW "MovieActorsCharacters" AS 
Select MovieActors.Id, Movies.Title, People.Name, MovieActors.Character from MovieActors
join Movies on MovieActors.MovieId = Movies.Id
join People on MovieActors.PersonId = People.Id
where Character is not null;

-- ----------------------------
-- View structure for "MovieCovers"
-- ----------------------------
DROP VIEW IF EXISTS "MovieCovers";
CREATE VIEW "MovieCovers" AS 
select Arts.Id, Arts.Path, Arts.Preview, Movies.Title as Movie from Arts
join Movies on Arts.MovieId = Movies.Id
where Arts.Type = 1
ORDER BY Movie;

-- ----------------------------
-- View structure for "MovieFanart"
-- ----------------------------
DROP VIEW IF EXISTS "MovieFanart";
CREATE VIEW "MovieFanart" AS 
select Arts.Id, Arts.Path, Arts.Preview, Movies.Title as Movie from Arts
join Movies on Arts.MovieId = Movies.Id
where Arts.Type = 3
ORDER BY Movie;

-- ----------------------------
-- View structure for "MoviePosters"
-- ----------------------------
DROP VIEW IF EXISTS "MoviePosters";
CREATE VIEW "MoviePosters" AS 
select Arts.Id, Arts.Path, Arts.Preview, Movies.Title as Movie from Arts
join Movies on Arts.MovieId = Movies.Id
where Arts.Type = 2
ORDER BY Movie;

-- ----------------------------
-- View structure for "MovieStudioNames"
-- ----------------------------
DROP VIEW IF EXISTS "MovieStudioNames";
CREATE VIEW "MovieStudioNames" AS 
select Movies.Title as Movie, Studios.Name as Studio from MovieStudios
join Movies on MovieStudios.MovieId = Movies.Id
join Studios on MovieStudios.StudioId = Studios.Id
ORDER BY Movie;

-- ----------------------------
-- View structure for "MovieVideos"
-- ----------------------------
DROP VIEW IF EXISTS "MovieVideos";
CREATE VIEW "MovieVideos" AS 
SELECT Videos.*, Movies.Title as Movie from Videos
join Movies on Videos.MovieId = Movies.Id
ORDER BY Movie;

-- ----------------------------
-- View structure for "SubtitlesView"
-- ----------------------------
DROP VIEW IF EXISTS "SubtitlesView";
CREATE VIEW "SubtitlesView" AS 
select Subtitles.Id, PodnapisiId, OpenSubtitlesId, MD5, Format Encoding, EmbededInVideo, ForHearingImpaired, Languages.Name as Language, Movies.Title as Movie
from Subtitles
join Languages on LanguageId = Languages.Id
join Movies on MovieId = Movies.Id;

-- ----------------------------
-- View structure for "MultipartMovies"
-- ----------------------------
DROP VIEW IF EXISTS "MultipartMovies";
CREATE VIEW "MultipartMovies" AS 
select * from Movies
where IsMultipart = 1