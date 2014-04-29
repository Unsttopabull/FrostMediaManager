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
	"ContextKey" TEXT,
	"MigrationId"  TEXT NOT NULL,
	"Model"  BLOB NOT NULL,
	"ProductVersion"  TEXT NOT NULL
);

-- ----------------------------
-- Table structure for "Art"
-- ----------------------------
DROP TABLE IF EXISTS "Art";
CREATE TABLE "Art" (
	"Id"  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Type"  integer NOT NULL,
	"Path"  TEXT NOT NULL,
	"Preview"  TEXT NULL,
	"MovieId"  integer NOT NULL,
	CONSTRAINT "FK_MovieArt" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE UNIQUE INDEX "Art_Id" on "Art" ("Id" ASC);
CREATE INDEX "Art_Movie" on "Art" ("MovieId" ASC);

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

CREATE UNIQUE INDEX "Audio_Id" on "Audios" ("Id" ASC);
CREATE INDEX "Audio_Movie"     on "Audios" ("MovieId" ASC);

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

CREATE UNIQUE INDEX "Certifications_Id" on "Certifications" ("Id" ASC);

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

CREATE UNIQUE INDEX "Country_Id" on "Countries" ("Id" ASC);
CREATE INDEX "Country_Name"     on "Countries" ("Name" ASC);

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

CREATE UNIQUE INDEX "File_Id" on "Files" ("Id" ASC);

-- ----------------------------
-- Table structure for "Genres"
-- ----------------------------
DROP TABLE IF EXISTS "Genres";
CREATE TABLE Genres (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT   NOT NULL  COLLATE NOCASE
);

CREATE UNIQUE INDEX "Genre_Id" on "Genres" ("Id" ASC);
CREATE INDEX "Genre_Name"      on "Genres" ("Name" ASC);

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

CREATE UNIQUE INDEX "PromotionalVideo_Id" on "PromotionalVideos" ("Id" ASC);
CREATE INDEX "PromotionalVideo_Movie"     on "PromotionalVideos" ("MovieId" ASC);

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

CREATE UNIQUE INDEX "Award_Id" on "Awards" ("Id" ASC);

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

CREATE UNIQUE INDEX "Language_Id" on "Languages" ("Id" ASC);
CREATE INDEX "Language_Name"      on "Languages" ("Name" ASC);

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
	"Type" INTEGER NOT NULL,
	"Goofs"  TEXT,
	"Trivia"  TEXT,
	"Year"  integer,
	"ReleaseDate"  TEXT,
	"Edithion"  TEXT,
	"DvDRegion"  INTEGER NOT NULL,
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
	"FirstFileName" TEXT,
	"VideoCodec" TEXT,
	"VideoResolution" TEXT,
	"AudioCodec" TEXT,
	"NumberOfAudioChannels" INTEGER,
	"SetId"  integer,
	"PlotId"  integer,
	"CoverId"  integer,
	"FanartId"  integer,
	CONSTRAINT "FK_MovieSet" FOREIGN KEY ("SetId") REFERENCES "Sets" ("Id"),
	CONSTRAINT "FK_MoviePlot" FOREIGN KEY ("PlotId") REFERENCES "Plots" ("Id"),
	CONSTRAINT "FK_MovieCover" FOREIGN KEY ("CoverId") REFERENCES "Art" ("Id"),
	CONSTRAINT "FK_MovieFanart" FOREIGN KEY ("FanartId") REFERENCES "Art" ("Id")
);

CREATE UNIQUE INDEX "Movie_Id" on "Movies" ("Id" ASC);
CREATE INDEX "Movie_Title" on "Movies" ("Title" ASC);
CREATE INDEX "Movie_Imdb" on "Movies" ("ImdbID" ASC);

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

CREATE UNIQUE INDEX "Person_Id" on "People" ("Id" ASC);
CREATE INDEX "Person_Name"      on "People" ("Name" ASC);

-- ----------------------------
-- Table structure for "Actors"
-- ----------------------------
DROP TABLE IF EXISTS "Actors";
CREATE TABLE Actors (
	Id integer PRIMARY KEY NOT NULL ,
	PersonId integer NOT NULL,
	MovieId integer NOT NULL,
	Character TEXT COLLATE NOCASE,
	CONSTRAINT "FK_ActorsPerson" FOREIGN KEY ("PersonId") REFERENCES "People" ("Id")  ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT "FK_ActorsMovie" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id")  ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE UNIQUE INDEX "Actors_Id" on "Actors" ("Id" ASC);
CREATE INDEX "Actor_MovieId"    on "Actors" ("MovieId" ASC);
CREATE INDEX "Actor_PersonId"   on "Actors" ("PersonId" ASC);

-- ----------------------------
-- Table structure for "Plots"
-- ----------------------------
DROP TABLE IF EXISTS "Plots";
CREATE TABLE "Plots" (
	"Id"  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Tagline"  TEXT,
	"Summary"  TEXT,
	"Full"  TEXT,
	"Language"  TEXT NULL,
	"MovieId"  integer NOT NULL,
	CONSTRAINT "FK_MoviePlot" FOREIGN KEY ("MovieId") REFERENCES "Movies" ("Id") ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE UNIQUE INDEX "Plot_Id" on "Plots" ("Id" ASC);
CREATE INDEX "Plot_Movie"     on "Plots" ("MovieId" ASC);

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

CREATE UNIQUE INDEX "Ratings_Id" on "Ratings" ("Id" ASC);
CREATE INDEX "Ratings_Movie"     on "Ratings" ("MovieId" ASC);

-- ----------------------------
-- Table structure for "Sets"
-- ----------------------------
DROP TABLE IF EXISTS "Sets";
CREATE TABLE "Sets" (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Name TEXT NOT NULL  COLLATE NOCASE
);

CREATE UNIQUE INDEX "Set_Id" on "Sets" ("Id" ASC);

-- ----------------------------
-- Table structure for "Specials"
-- ----------------------------
DROP TABLE IF EXISTS "Specials";
CREATE TABLE Specials (
    Id integer PRIMARY KEY AUTOINCREMENT  NOT NULL ,
    Value TEXT NOT NULL  COLLATE NOCASE
);

CREATE UNIQUE INDEX "Special_Id" on "Specials" ("Id" ASC);
CREATE INDEX "Special_Value"     on "Specials" ("Value" ASC);

-- ----------------------------
-- Table structure for "Studios"
-- ----------------------------
DROP TABLE IF EXISTS "Studios";
CREATE TABLE Studios (
    Id integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL  COLLATE NOCASE
);

CREATE UNIQUE INDEX "Studio_Id" on "Studios" ("Id" ASC);
CREATE INDEX "Studio_Name"      on "Studios" ("Name" ASC);

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

CREATE UNIQUE INDEX "Subtitles_Id" on "Subtitles" ("Id" ASC);
CREATE INDEX "Subtitles_Movie"     on "Subtitles" ("MovieId" ASC);

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
	"ResolutionName"  TEXT,
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

CREATE UNIQUE INDEX "Video_Id" on "Videos" ("Id" ASC);
CREATE INDEX "Video_Movie"     on "Videos" ("MovieId" ASC);


-- ----------------------------
-- View structure for "main"."AwardDuplicates"
-- ----------------------------
DROP VIEW IF EXISTS "main"."AwardDuplicates";
CREATE VIEW "AwardDuplicates" AS 
select AwardType, count(*)
from Awards
group by AwardType
having count(*) > 1;

-- ----------------------------
-- View structure for "main"."CountryDuplicates"
-- ----------------------------
DROP VIEW IF EXISTS "main"."CountryDuplicates";
CREATE VIEW "CountryDuplicates" AS 
select Name, count(*)
from Countries
group by Name
having count(*) > 1;

-- ----------------------------
-- View structure for "main"."GenreDuplicates"
-- ----------------------------
DROP VIEW IF EXISTS "main"."GenreDuplicates";
CREATE VIEW "GenreDuplicates" AS 
select Name, count(*)
from Genres
group by Name
having count(*) > 1;

-- ----------------------------
-- View structure for "main"."LanguageDuplicates"
-- ----------------------------
DROP VIEW IF EXISTS "main"."LanguageDuplicates";
CREATE VIEW "LanguageDuplicates" AS 
select Name, count(*)
from Languages
group by Name
having count(*) > 1;


-- ----------------------------
-- View structure for "main"."SetDuplicates"
-- ----------------------------
DROP VIEW IF EXISTS "main"."SetDuplicates";
CREATE VIEW "SetDuplicates" AS 
select Name, count(*)
from Sets
group by Name
having count(*) > 1;

-- ----------------------------
-- View structure for "main"."SpecialDuplicates"
-- ----------------------------
DROP VIEW IF EXISTS "main"."SpecialDuplicates";
CREATE VIEW "SpecialDuplicates" AS 
select Value, count(*)
from Specials
group by Value
having count(*) > 1;

-- ----------------------------
-- View structure for "main"."StudioDuplicates"
-- ----------------------------
DROP VIEW IF EXISTS "main"."StudioDuplicates";
CREATE VIEW "StudioDuplicates" AS 
select Name, count(*)
from Studios
group by Name
having count(*) > 1;

-- ----------------------------
-- View structure for "MovieActorsCharacters"
-- ----------------------------
DROP VIEW IF EXISTS "MovieActorsCharacters";
CREATE VIEW "MovieActorsCharacters" AS 
Select Actors.Id, Movies.Title, People.Name, Actors.Character from Actors
join Movies on Actors.MovieId = Movies.Id
join People on Actors.Id = People.Id
where Character is not null;

-- ----------------------------
-- View structure for "MovieCovers"
-- ----------------------------
DROP VIEW IF EXISTS "MovieCovers";
CREATE VIEW "MovieCovers" AS 
select Art.Id, Art.Path, Art.Preview, Movies.Title as Movie from Art
join Movies on Art.MovieId = Movies.Id
where Art.Type = 1
ORDER BY Movie;

-- ----------------------------
-- View structure for "MovieFanart"
-- ----------------------------
DROP VIEW IF EXISTS "MovieFanart";
CREATE VIEW "MovieFanart" AS 
select Art.Id, Art.Path, Art.Preview, Movies.Title as Movie from Art
join Movies on Art.MovieId = Movies.Id
where Art.Type = 3
ORDER BY Movie;

-- ----------------------------
-- View structure for "MoviePosters"
-- ----------------------------
DROP VIEW IF EXISTS "MoviePosters";
CREATE VIEW "MoviePosters" AS 
select Art.Id, Art.Path, Art.Preview, Movies.Title as Movie from Art
join Movies on Art.MovieId = Movies.Id
where Art.Type = 2
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
where IsMultipart = 1;

---- ----------------------------
---- Triggers structure for table "Audios"
---- ----------------------------
DROP TRIGGER IF EXISTS "OnAudioDeleteCodecUpdate";
CREATE TRIGGER "OnAudioDeleteCodecUpdate" AFTER UPDATE ON "Audios"
BEGIN
    update Movies
		set AudioCodec = (select CodecId from Audios
			where Audios.MovieId = Movies.Id
			GROUP BY CodecId
			ORDER BY count(CodecId) DESC
			LIMIT 1),
		NumberOfAudioChannels = (select NumberOfChannels from Audios
				where Audios.MovieId = Movies.Id
				GROUP BY NumberOfChannels
				ORDER BY count(NumberOfChannels) DESC
				LIMIT 1);
END;

--DROP TRIGGER IF EXISTS "OnAudioInsertCodecUpdate";
--CREATE TRIGGER "OnAudioInsertCodecUpdate" AFTER INSERT ON "Audios"
--BEGIN
--    update Movies
--		set AudioCodec = (select CodecId from Audios
--			where Audios.MovieId = Movies.Id
--			GROUP BY CodecId
--			ORDER BY count(CodecId) DESC
--			LIMIT 1),
--		NumberOfAudioChannels = (select NumberOfChannels from Audios
--				where Audios.MovieId = Movies.Id
--				GROUP BY NumberOfChannels
--				ORDER BY count(NumberOfChannels) DESC
--				LIMIT 1);
--END;

DROP TRIGGER IF EXISTS "OnAudioNumberOfChanelsUpdate";
CREATE TRIGGER "OnAudioNumberOfChanelsUpdate" AFTER UPDATE OF "NumberOfChannels" ON "Audios"
BEGIN
	update Movies
	set NumberOfAudioChannels = (select NumberOfChannels from Audios
			where Audios.MovieId = Movies.Id
			GROUP BY NumberOfChannels
			ORDER BY count(NumberOfChannels) DESC
			LIMIT 1);
END;

DROP TRIGGER IF EXISTS "OnAudioCodecIdUpdate";
CREATE TRIGGER "OnAudioCodecIdUpdate" AFTER UPDATE OF "CodecId" ON "Audios"
BEGIN
  update Movies
		set AudioCodec = (select CodecId from Audios
		where Audios.MovieId = Movies.Id
		GROUP BY CodecId
		ORDER BY count(CodecId) DESC
		LIMIT 1);
END;

-- ----------------------------
-- Triggers structure for table "Videos"
-- ----------------------------
DROP TRIGGER IF EXISTS "OnVideoCodecChangedUpdate";
CREATE TRIGGER "OnVideoCodecChangedUpdate" AFTER UPDATE OF "CodecId" ON "Videos"
BEGIN
    update Movies
		set VideoCodec = (select CodecId from Videos
			where Videos.MovieId = Movies.Id
			GROUP BY CodecId
			ORDER BY count(CodecId) DESC
			LIMIT 1);  
END;

DROP TRIGGER IF EXISTS "OnVideoDeleteUpdateCodec";
CREATE TRIGGER "OnVideoDeleteUpdateCodec" AFTER DELETE ON "Videos"
BEGIN
    update Movies
		set AudioCodec = (select CodecId from Videos
			where Videos.MovieId = Movies.Id
			GROUP BY CodecId
			ORDER BY count(CodecId) DESC
			LIMIT 1),
		VideoResolution = (select Resolution from Videos
			where Videos.MovieId = Movies.Id
			GROUP BY Resolution
			ORDER BY count(Resolution) DESC
			LIMIT 1);
END;

--DROP TRIGGER IF EXISTS "OnVideoInsertUpdateCodec";
--CREATE TRIGGER "OnVideoInsertUpdateCodec" AFTER INSERT ON "Videos"
--BEGIN
--    update Movies
--		set AudioCodec = (select CodecId from Audios
--			where Audios.MovieId = Movies.Id
--			GROUP BY CodecId
--			ORDER BY count(CodecId) DESC
--			LIMIT 1),
--		VideoResolution = (select Resolution from Videos
--			where Videos.MovieId = Movies.Id
--			GROUP BY Resolution
--			ORDER BY count(Resolution) DESC
--			LIMIT 1);
--END;

DROP TRIGGER IF EXISTS "OnVideoResolutionUpdate";
CREATE TRIGGER "OnVideoResolutionUpdate" AFTER UPDATE OF "Resolution" ON "Videos"
BEGIN
  update Movies
	set VideoResolution = (select Resolution from Videos
			where Videos.MovieId = Movies.Id
			GROUP BY Resolution
			ORDER BY count(Resolution) DESC
			LIMIT 1);
END;

-- ----------------------------
-- Data for 'Countries'
-- ----------------------------"
INSERT INTO "Countries" VALUES (1, "Afghanistan", "AF", "AFG"),
(2, "Åland Islands", "AX", "ALA"),
(3, "Albania", "AL", "ALB"),
(4, "Algeria", "DZ", "DZA"),
(5, "American Samoa", "AS", "ASM"),
(6, "Andorra", "AD", "AND"),
(7, "Angola", "AO", "AGO"),
(8, "Anguilla", "AI", "AIA"),
(9, "Antarctica", "AQ", "ATA"),
(10, "Antigua and Barbuda", "AG", "ATG"),
(11, "Argentina", "AR", "ARG"),
(12, "Armenia", "AM", "ARM"),
(13, "Aruba", "AW", "ABW"),
(14, "Australia", "AU", "AUS"),
(15, "Austria", "AT", "AUT"),
(16, "Azerbaijan", "AZ", "AZE"),
(17, "Bahamas", "BS", "BHS"),
(18, "Bahrain", "BH", "BHR"),
(19, "Bangladesh", "BD", "BGD"),
(20, "Barbados", "BB", "BRB"),
(21, "Belarus", "BY", "BLR"),
(22, "Belgium", "BE", "BEL"),
(23, "Belize", "BZ", "BLZ"),
(24, "Benin", "BJ", "BEN"),
(25, "Bermuda", "BM", "BMU"),
(26, "Bhutan", "BT", "BTN"),
(27, "Bolivia", "BO", "BOL"),
(28, "Bonaire, Sint Eustatius and Saba", "BQ", "BES"),
(29, "Bosnia and Herzegovina", "BA", "BIH"),
(30, "Botswana", "BW", "BWA"),
(31, "Bouvet Island", "BV", "BVT"),
(32, "Brazil", "BR", "BRA"),
(33, "British Indian Ocean Territory", "IO", "IOT"),
(34, "Brunei Darussalam", "BN", "BRN"),
(35, "Bulgaria", "BG", "BGR"),
(36, "Burkina Faso", "BF", "BFA"),
(37, "Burundi", "BI", "BDI"),
(38, "Cambodia", "KH", "KHM"),
(39, "Cameroon", "CM", "CMR"),
(40, "Canada", "CA", "CAN"),
(41, "Cape Verde", "CV", "CPV"),
(42, "Cayman Islands", "KY", "CYM"),
(43, "Central African Republic", "CF", "CAF"),
(44, "Chad", "TD", "TCD"),
(45, "Chile", "CL", "CHL"),
(46, "China", "CN", "CHN"),
(47, "Christmas Island", "CX", "CXR"),
(48, "Cocos (Keeling) Islands", "CC", "CCK"),
(49, "Colombia", "CO", "COL"),
(50, "Comoros", "KM", "COM"),
(51, "Congo", "CG", "COG"),
(52, "Congo (the Democratic Republic of the)", "CD", "COD"),
(53, "Cook Islands", "CK", "COK"),
(54, "Costa Rica", "CR", "CRI"),
(55, "Côte d'Ivoire", "CI", "CIV"),
(56, "Croatia", "HR", "HRV"),
(57, "Cuba", "CU", "CUB"),
(58, "Curaçao", "CW", "CUW"),
(59, "Cyprus", "CY", "CYP"),
(60, "Czech Republic", "CZ", "CZE"),
(61, "Denmark", "DK", "DNK"),
(62, "Djibouti", "DJ", "DJI"),
(63, "Dominica", "DM", "DMA"),
(64, "Dominican Republic", "DO", "DOM"),
(65, "Ecuador", "EC", "ECU"),
(66, "Egypt", "EG", "EGY"),
(67, "El Salvador", "SV", "SLV"),
(68, "Equatorial Guinea", "GQ", "GNQ"),
(69, "Eritrea", "ER", "ERI"),
(70, "Estonia", "EE", "EST"),
(71, "Ethiopia", "ET", "ETH"),
(72, "Falkland Islands [Malvinas]", "FK", "FLK"),
(73, "Faroe Islands", "FO", "FRO"),
(74, "Fiji", "FJ", "FJI"),
(75, "Finland", "FI", "FIN"),
(76, "France", "FR", "FRA"),
(77, "French Guiana", "GF", "GUF"),
(78, "French Polynesia", "PF", "PYF"),
(79, "French Southern Territories", "TF", "ATF"),
(80, "Gabon", "GA", "GAB"),
(81, "Gambia", "GM", "GMB"),
(82, "Georgia", "GE", "GEO"),
(83, "Germany", "DE", "DEU"),
(84, "Ghana", "GH", "GHA"),
(85, "Gibraltar", "GI", "GIB"),
(86, "Greece", "GR", "GRC"),
(87, "Greenland", "GL", "GRL"),
(88, "Grenada", "GD", "GRD"),
(89, "Guadeloupe", "GP", "GLP"),
(90, "Guam", "GU", "GUM"),
(91, "Guatemala", "GT", "GTM"),
(92, "Guernsey", "GG", "GGY"),
(93, "Guinea", "GN", "GIN"),
(94, "Guinea-Bissau", "GW", "GNB"),
(95, "Guyana", "GY", "GUY"),
(96, "Haiti", "HT", "HTI"),
(97, "Heard Island and McDonald Islands", "HM", "HMD"),
(98, "Holy See [Vatican City State]", "VA", "VAT"),
(99, "Honduras", "HN", "HND"),
(100, "Hong Kong", "HK", "HKG"),
(101, "Hungary", "HU", "HUN"),
(102, "Iceland", "IS", "ISL"),
(103, "India", "IN", "IND"),
(104, "Indonesia", "ID", "IDN"),
(105, "Iran", "IR", "IRN"),
(106, "Iraq", "IQ", "IRQ"),
(107, "Ireland", "IE", "IRL"),
(108, "Isle of Man", "IM", "IMN"),
(109, "Israel", "IL", "ISR"),
(110, "Italy", "IT", "ITA"),
(111, "Jamaica", "JM", "JAM"),
(112, "Japan", "JP", "JPN"),
(113, "Jersey", "JE", "JEY"),
(114, "Jordan", "JO", "JOR"),
(115, "Kazakhstan", "KZ", "KAZ"),
(116, "Kenya", "KE", "KEN"),
(117, "Kiribati", "KI", "KIR"),
(118, "Korea (the Democratic People's Republic of)", "KP", "PRK"),
(119, "South Korea", "KR", "KOR"),
(120, "Kuwait", "KW", "KWT"),
(121, "Kyrgyzstan", "KG", "KGZ"),
(122, "Lao People's Democratic Republic", "LA", "LAO"),
(123, "Latvia", "LV", "LVA"),
(124, "Lebanon", "LB", "LBN"),
(125, "Lesotho", "LS", "LSO"),
(126, "Liberia", "LR", "LBR"),
(127, "Libya", "LY", "LBY"),
(128, "Liechtenstein", "LI", "LIE"),
(129, "Lithuania", "LT", "LTU"),
(130, "Luxembourg", "LU", "LUX"),
(131, "Macao", "MO", "MAC"),
(132, "Macedonia", "MK", "MKD"),
(133, "Madagascar", "MG", "MDG"),
(134, "Malawi", "MW", "MWI"),
(135, "Malaysia", "MY", "MYS"),
(136, "Maldives", "MV", "MDV"),
(137, "Mali", "ML", "MLI"),
(138, "Malta", "MT", "MLT"),
(139, "Marshall Islands", "MH", "MHL"),
(140, "Martinique", "MQ", "MTQ"),
(141, "Mauritania", "MR", "MRT"),
(142, "Mauritius", "MU", "MUS"),
(143, "Mayotte", "YT", "MYT"),
(144, "Mexico", "MX", "MEX"),
(145, "Micronesia", "FM", "FSM"),
(146, "Moldova", "MD", "MDA"),
(147, "Monaco", "MC", "MCO"),
(148, "Mongolia", "MN", "MNG"),
(149, "Montenegro", "ME", "MNE"),
(150, "Montserrat", "MS", "MSR"),
(151, "Morocco", "MA", "MAR"),
(152, "Mozambique", "MZ", "MOZ"),
(153, "Myanmar", "MM", "MMR"),
(154, "Namibia", "NA", "NAM"),
(155, "Nauru", "NR", "NRU"),
(156, "Nepal", "NP", "NPL"),
(157, "Netherlands", "NL", "NLD"),
(158, "New Caledonia", "NC", "NCL"),
(159, "New Zealand", "NZ", "NZL"),
(160, "Nicaragua", "NI", "NIC"),
(161, "Niger", "NE", "NER"),
(162, "Nigeria", "NG", "NGA"),
(163, "Niue", "NU", "NIU"),
(164, "Norfolk Island", "NF", "NFK"),
(165, "Northern Mariana Islands", "MP", "MNP"),
(166, "Norway", "NO", "NOR"),
(167, "Oman", "OM", "OMN"),
(168, "Pakistan", "PK", "PAK"),
(169, "Palau", "PW", "PLW"),
(170, "Palestine, State of", "PS", "PSE"),
(171, "Panama", "PA", "PAN"),
(172, "Papua New Guinea", "PG", "PNG"),
(173, "Paraguay", "PY", "PRY"),
(174, "Peru", "PE", "PER"),
(175, "Philippines", "PH", "PHL"),
(176, "Pitcairn", "PN", "PCN"),
(177, "Poland", "PL", "POL"),
(178, "Portugal", "PT", "PRT"),
(179, "Puerto Rico", "PR", "PRI"),
(180, "Qatar", "QA", "QAT"),
(181, "Réunion", "RE", "REU"),
(182, "Romania", "RO", "ROU"),
(183, "Russian Federation", "RU", "RUS"),
(184, "Rwanda", "RW", "RWA"),
(185, "Saint Barthélemy", "BL", "BLM"),
(186, "Saint Helena, Ascension and Tristan da Cunha", "SH", "SHN"),
(187, "Saint Kitts and Nevis", "KN", "KNA"),
(188, "Saint Lucia", "LC", "LCA"),
(189, "Saint Martin (French part)", "MF", "MAF"),
(190, "Saint Pierre and Miquelon", "PM", "SPM"),
(191, "Saint Vincent and the Grenadines", "VC", "VCT"),
(192, "Samoa", "WS", "WSM"),
(193, "San Marino", "SM", "SMR"),
(194, "Sao Tome and Principe", "ST", "STP"),
(195, "Saudi Arabia", "SA", "SAU"),
(196, "Senegal", "SN", "SEN"),
(197, "Serbia", "RS", "SRB"),
(198, "Seychelles", "SC", "SYC"),
(199, "Sierra Leone", "SL", "SLE"),
(200, "Singapore", "SG", "SGP"),
(201, "Sint Maarten (Dutch part)", "SX", "SXM"),
(202, "Slovakia", "SK", "SVK"),
(203, "Slovenia", "SI", "SVN"),
(204, "Solomon Islands", "SB", "SLB"),
(205, "Somalia", "SO", "SOM"),
(206, "South Africa", "ZA", "ZAF"),
(207, "South Georgia and the South Sandwich Islands", "GS", "SGS"),
(208, "South Sudan", "SS", "SSD"),
(209, "Spain", "ES", "ESP"),
(210, "Sri Lanka", "LK", "LKA"),
(211, "Sudan", "SD", "SDN"),
(212, "Suriname", "SR", "SUR"),
(213, "Svalbard and Jan Mayen", "SJ", "SJM"),
(214, "Swaziland", "SZ", "SWZ"),
(215, "Sweden", "SE", "SWE"),
(216, "Switzerland", "CH", "CHE"),
(217, "Syrian Arab Republic", "SY", "SYR"),
(218, "Taiwan (Province of China)", "TW", "TWN"),
(219, "Tajikistan", "TJ", "TJK"),
(220, "Tanzania, United Republic of", "TZ", "TZA"),
(221, "Thailand", "TH", "THA"),
(222, "Timor-Leste", "TL", "TLS"),
(223, "Togo", "TG", "TGO"),
(224, "Tokelau", "TK", "TKL"),
(225, "Tonga", "TO", "TON"),
(226, "Trinidad and Tobago", "TT", "TTO"),
(227, "Tunisia", "TN", "TUN"),
(228, "Turkey", "TR", "TUR"),
(229, "Turkmenistan", "TM", "TKM"),
(230, "Turks and Caicos Islands", "TC", "TCA"),
(231, "Tuvalu", "TV", "TUV"),
(232, "Uganda", "UG", "UGA"),
(233, "Ukraine", "UA", "UKR"),
(234, "United Arab Emirates", "AE", "ARE"),
(235, "United Kingdom", "GB", "GBR"),
(236, "United States", "US", "USA"),
(237, "United States Minor Outlying Islands", "UM", "UMI"),
(238, "Uruguay", "UY", "URY"),
(239, "Uzbekistan", "UZ", "UZB"),
(240, "Vanuatu", "VU", "VUT"),
(241, "Venezuela, Bolivarian Republic of ", "VE", "VEN"),
(242, "Viet Nam", "VN", "VNM"),
(243, "Virgin Islands (British)", "VG", "VGB"),
(244, "Virgin Islands (U.S.)", "VI", "VIR"),
(245, "Wallis and Futuna", "WF", "WLF"),
(246, "Western Sahara*", "EH", "ESH"),
(247, "Yemen", "YE", "YEM"),
(248, "Zambia", "ZM", "ZMB"),
(249, "Zimbabwe", "ZW", "ZWE");

-- ----------------------------
-- Data for 'Languages'
-- ----------------------------
INSERT INTO "Languages" VALUES (1, "Afrikaans", "af", "afr");
INSERT INTO "Languages" VALUES (2, "Arabic", "ar", "ara");
INSERT INTO "Languages" VALUES (3, "Armenian", "hy", "arm");
INSERT INTO "Languages" VALUES (4, "Azerbaijani", "az", "aze");
INSERT INTO "Languages" VALUES (5, "Belarusian", "be", "bel");
INSERT INTO "Languages" VALUES (6, "Bengali", "bn", "ben");
INSERT INTO "Languages" VALUES (7, "Bosnian", "bs", "bos");
INSERT INTO "Languages" VALUES (8, "Bulgarian", "bg", "bul");
INSERT INTO "Languages" VALUES (9, "Catalan, Valencian", "ca", "cat");
INSERT INTO "Languages" VALUES (10, "Czech", "cs", "cze");
INSERT INTO "Languages" VALUES (11, "Chinese", "zh", "chi");
INSERT INTO "Languages" VALUES (12, "Danish", "da", "dan");
INSERT INTO "Languages" VALUES (13, "Dutch", "nl", "dut");
INSERT INTO "Languages" VALUES (14, "English", "en", "eng");
INSERT INTO "Languages" VALUES (15, "Estonian", "et", "est");
INSERT INTO "Languages" VALUES (16, "Finnish", "fi", "fin");
INSERT INTO "Languages" VALUES (17, "French", "fr", "fre");
INSERT INTO "Languages" VALUES (18, "Georgian", "ka", "geo");
INSERT INTO "Languages" VALUES (19, "German", "de", "ger");
INSERT INTO "Languages" VALUES (20, "Irish", "ga", "gle");
INSERT INTO "Languages" VALUES (21, "Guarani", "gn", "grn");
INSERT INTO "Languages" VALUES (22, "Hebrew", "he", "heb");
INSERT INTO "Languages" VALUES (23, "Hindi", "hi", "hin");
INSERT INTO "Languages" VALUES (24, "Croatian", "hr", "hrv");
INSERT INTO "Languages" VALUES (25, "Hungarian", "hu", "hun");
INSERT INTO "Languages" VALUES (26, "Icelandic", "is", "ice");
INSERT INTO "Languages" VALUES (27, "Indonesian", "id", "ind");
INSERT INTO "Languages" VALUES (28, "Italian", "it", "ita");
INSERT INTO "Languages" VALUES (29, "Japanese", "ja", "jpn");
INSERT INTO "Languages" VALUES (30, "Kazakh", "kk", "kaz");
INSERT INTO "Languages" VALUES (31, "Khmer", "km", "khm");
INSERT INTO "Languages" VALUES (32, "Kirghiz", "ky", "kir");
INSERT INTO "Languages" VALUES (33, "Korean", "ko", "kor");
INSERT INTO "Languages" VALUES (34, "Lao", "lo", "lao");
INSERT INTO "Languages" VALUES (35, "Latvian", "lv", "lav");
INSERT INTO "Languages" VALUES (36, "Lithuanian", "lt", "lit");
INSERT INTO "Languages" VALUES (37, "Luxembourgish", "lb", "ltz");
INSERT INTO "Languages" VALUES (38, "Macedonian", "mk", "mac");
INSERT INTO "Languages" VALUES (39, "Maori", "mi", "mao");
INSERT INTO "Languages" VALUES (40, "Malay", "ms", "may");
INSERT INTO "Languages" VALUES (41, "Maltese", "mt", "mlt");
INSERT INTO "Languages" VALUES (42, "Mongolian", "mn", "mon");
INSERT INTO "Languages" VALUES (43, "Nepali", "ne", "nep");
INSERT INTO "Languages" VALUES (44, "Norwegian", "no", "nor");
INSERT INTO "Languages" VALUES (45, "Persian", "fa", "per");
INSERT INTO "Languages" VALUES (46, "Polish", "pl", "pol");
INSERT INTO "Languages" VALUES (47, "Portuguese", "pt", "por");
INSERT INTO "Languages" VALUES (48, "Romansh", "rm", "roh");
INSERT INTO "Languages" VALUES (49, "Romanian", "ro", "rum");
INSERT INTO "Languages" VALUES (50, "Russian", "ru", "rus");
INSERT INTO "Languages" VALUES (51, "Sinhala", "si", "sin");
INSERT INTO "Languages" VALUES (52, "Slovak", "sk", "slo");
INSERT INTO "Languages" VALUES (53, "Slovenian", "sl", "slv");
INSERT INTO "Languages" VALUES (54, "Spanish", "es", "spa");
INSERT INTO "Languages" VALUES (55, "Serbian", "sr", "srp");
INSERT INTO "Languages" VALUES (56, "Swedish", "sv", "swe");
INSERT INTO "Languages" VALUES (57, "Tetum", null, "tet");
INSERT INTO "Languages" VALUES (58, "Thai", "th", "tha");
INSERT INTO "Languages" VALUES (59, "Turkmen", "tk", "tuk");
INSERT INTO "Languages" VALUES (60, "Turkish", "tr", "tur");
INSERT INTO "Languages" VALUES (61, "Ukrainian", "uk", "ukr");
INSERT INTO "Languages" VALUES (62, "Urdu", "ur", "urd");
INSERT INTO "Languages" VALUES (63, "Uzbek", "uz", "uzb");
INSERT INTO "Languages" VALUES (64, "Vietnamese", "vi", "vie");

-- ----------------------------
-- Data for 'Studios'
-- ----------------------------
BEGIN;

INSERT INTO Studios VALUES
(1, "007"),
(2, "20th Century Fox Home Entertainment"),
(3, "20th Century Fox Studios"),
(4, "20th Century Fox Television"),
(5, "20th Century Fox"),
(6, "2929 Productions"),
(7, "3L Filmproduktion"),
(8, "3L Productions"),
(9, "40 Acres & A Mule Filmworks"),
(10, "40 Acres A Mule Filmworks"),
(11, "57th & Irving Productions"),
(12, "A and M Films"),
(13, "A Band Apart"),
(14, "A&E Home Video"),
(15, "A&E IndieFilms"),
(16, "A&E Television Networks"),
(17, "A&E"),
(18, "A&M Films"),
(19, "AAC Kids"),
(20, "Aardman Animations"),
(21, "Aardman"),
(22, "Abandon Entertainment"),
(23, "Abandon Pictures"),
(24, "ABC Australia"),
(25, "ABC Family"),
(26, "ABC"),
(27, "ABC1 (Australia)"),
(28, "ABC_hd"),
(29, "Absolute"),
(30, "Act III Communications"),
(31, "Adult Swim"),
(32, "adultswim"),
(33, "Afrodisia Filmes e Brinquedos para Adultos"),
(34, "After Dark Films"),
(35, "Alcon Entertainment"),
(36, "Alcon Films"),
(37, "Alcon"),
(38, "Alliance Atlantis Communications"),
(39, "Alliance Atlantis Home Video"),
(40, "Alliance Atlantis Productions"),
(41, "Alliance Atlantis"),
(42, "Alliance Communications Corporation"),
(43, "Allied Filmmakers"),
(44, "Alloy Entertainment"),
(45, "Alloy Productions"),
(46, "Allternativa Filmes X"),
(47, "Allumination Filmworks"),
(48, "Alphaville Films"),
(49, "Alphaville Productions"),
(50, "Alphaville"),
(51, "Ambience Entertainment"),
(52, "Ambience"),
(53, "Ambient Entertainment GmbH"),
(54, "Ambient Entertainment"),
(55, "Ambient Film"),
(56, "Ambient Information Systems"),
(57, "Amblin Entertainment"),
(58, "Amblin Television"),
(59, "Ambush Enterprises"),
(60, "Ambush Entertainment"),
(61, "Ambush Film Productions"),
(62, "AMC Pictures"),
(63, "AMC"),
(64, "American Broadcasting Company (ABC) Sports"),
(65, "American Broadcasting Company"),
(66, "American International Pictures (AIP)"),
(67, "American International Productions"),
(68, "American Movie Classics (AMC)"),
(69, "American Zoetrope"),
(70, "American_International"),
(71, "Anchor Bay Entertainment (UK)"),
(72, "Anchor Bay Entertainment"),
(73, "Anchor Bay Films"),
(74, "Anchor Bay"),
(75, "AND Syndicated Productions"),
(76, "Andrew Stevens Entertainment"),
(77, "Animal Planet"),
(78, "animalplanet"),
(79, "Anonymous Content"),
(80, "Antena 3 Films"),
(81, "ApolloMedia"),
(82, "Apple Film Productions"),
(83, "Arclight Films"),
(84, "Arclight Partners"),
(85, "Arclight Productions"),
(86, "Arclight"),
(87, "Ark Film Collective"),
(88, "Arte France"),
(89, "Arte"),
(90, "Artisan Entertainment"),
(91, "Artisan Films"),
(92, "Artisan Productions"),
(93, "Artisan"),
(94, "Artists United"),
(95, "Astoria Films"),
(96, "Asylum Films"),
(97, "Asylum Pictures"),
(98, "Asylum, The"),
(99, "Asylum"),
(100, "Atlas Entertainment"),
(101, "Atlas Film Company"),
(102, "Atlas Film"),
(103, "Atlas International Film"),
(104, "Atlas Media"),
(105, "Australian Film Commission"),
(106, "Australian Film Institute"),
(107, "Avnet"),
(108, "Avnet Kerner Productions"),
(109, "AvnetKerner Company, The"),
(110, "AVRO Television"),
(111, "Baa-Ram-Ewe"),
(112, "Babylonian Productions"),
(113, "Bad Hat Harry Productions"),
(114, "Bad Robot"),
(115, "Baker Street Productions"),
(116, "Bananeira Filmes"),
(117, "Bandai Entertainment Inc."),
(118, "Bandai Visual Company"),
(119, "Bauer Martinez Studios"),
(120, "Bavaria Atelier"),
(121, "Bavaria Entertainment"),
(122, "Bavaria Film International"),
(123, "Bavaria Film"),
(124, "Bavaria Media GmbH"),
(125, "Bavaria Media Television"),
(126, "Bavaria Pictures"),
(127, "Bavaria Studios"),
(128, "Bayerischer Rundfunk (BR)"),
(129, "Bayerischer Rundfunk"),
(130, "Bazmark Films"),
(131, "BBC Films"),
(132, "BBC Four"),
(133, "BBC HD"),
(134, "BBC One"),
(135, "BBC Three"),
(136, "BBC Two"),
(137, "BBC"),
(138, "BBC_hd"),
(139, "Beacon Communications"),
(140, "Beacon Films Inc."),
(141, "Beacon Pictures"),
(142, "Beacon"),
(143, "Bedford Falls Company, The"),
(144, "Bedford Falls Graphics Inc."),
(145, "Bedford Falls Productions"),
(146, "Bedford Falls"),
(147, "BET Pictures"),
(148, "BET"),
(149, "Big Talk Productions"),
(150, "Black Dog Films"),
(151, "Black Dog"),
(152, "Bleiberg Entertainment"),
(153, "Blu-Ray"),
(154, "Blue Sky Studios"),
(155, "Blue Star Movies"),
(156, "Blue Star Pictures"),
(157, "Blue Star Productions"),
(158, "Bob Yari Productions"),
(159, "Bold Films"),
(160, "Boll Kino Beteiligungs GmbH & Co. KG"),
(161, "Bossa Nova Films"),
(162, "BR Films"),
(163, "Brave New Work Filmproduktions"),
(164, "Bravo!"),
(165, "Bravo"),
(166, "Brightlight Pictures"),
(167, "British Broadcasting Corporation (BBC)"),
(168, "British Film Council"),
(169, "Broken Lizard Industries"),
(170, "Bucanero Filmes"),
(171, "Bud Spencer - Terence Hill Collection"),
(172, "Buena Vista (Austria) GmbH"),
(173, "Buena Vista Distribution Company"),
(174, "Buena Vista Film Distribution Company"),
(175, "Buena Vista Home Entertainment"),
(176, "Buena Vista Home Video"),
(177, "Buena Vista Imaging"),
(178, "Buena Vista International Television"),
(179, "Buena Vista International"),
(180, "Buena Vista Pictures Distribution"),
(181, "Buena Vista"),
(182, "Buzzfilms"),
(183, "Canal+ Distribution"),
(184, "Canal+ España"),
(185, "Canal+ Productions"),
(186, "Canal+"),
(187, "Cannes Film Festival"),
(188, "Cannon Entertainment"),
(189, "Cannon Film Distributors"),
(190, "Cannon Films"),
(191, "Cannon Group"),
(192, "Cannon Home Video"),
(193, "Cannon International"),
(194, "Cannon Video"),
(195, "Cannon"),
(196, "Capcom Company"),
(197, "Capital V productions"),
(198, "Capitol Films"),
(199, "Capitol Pictures"),
(200, "Capitol"),
(201, "Capitol_Films"),
(202, "Carolco Entertainment"),
(203, "Carolco Home Video"),
(204, "Carolco International N.V."),
(205, "Carolco Pictures"),
(206, "Carolco Studios Inc."),
(207, "Carsey-Werner Company"),
(208, "Carsey-Werner-Mandabach Productions"),
(209, "Cartoon Network Studios"),
(210, "Cartoon Network"),
(211, "Cartoon Network_hd"),
(212, "Casablanca Filmes"),
(213, "Castel Film Romania"),
(214, "Castel Film Studio"),
(215, "Castel Films"),
(216, "Castle Rock Entertainment"),
(217, "Castle Rock Television"),
(218, "Castle Rock"),
(219, "castlerock"),
(220, "CBC"),
(221, "CBS Films"),
(222, "CBS Paramount Network Television"),
(223, "CBS Productions"),
(224, "CBS"),
(225, "CBS_hd"),
(226, "Celador Films"),
(227, "Celador Productions"),
(228, "Central Partnership"),
(229, "Central_Film"),
(230, "Centre National de la Cin+matographie (CNC)"),
(231, "Centre National de la Cinematographie"),
(232, "Centre National de la Cin┌matographie (CNC)"),
(233, "Centropolis Entertainment"),
(234, "Centropolis Film Productions"),
(235, "Centropolis Television"),
(236, "Cetus Productions"),
(237, "Channel 4 International"),
(238, "Channel 4"),
(239, "Channel Four Films"),
(240, "Channel Four Productions"),
(241, "Channel Four"),
(242, "Cheyenne Enterprises"),
(243, "Cheyenne Films"),
(244, "Cheyenne Nation, The "),
(245, "Cheyenne Studios"),
(246, "China Star Entertainment"),
(247, "China Star Production Services Limited"),
(248, "China Star Worldwide Distribution"),
(249, "CiBy 2000"),
(250, "Cinemax"),
(251, "Cinemax_hd"),
(252, "Cinergi Pictures Entertainment"),
(253, "CJ Entertainment"),
(254, "Clasart Film + TV Produktions GmbH"),
(255, "Clasart Film- und Fernsehproduktion"),
(256, "CMT Films"),
(257, "Code Entertainment"),
(258, "Codeblack Entertainment"),
(259, "Codeblack Interactive"),
(260, "Columbia Pictures Corporation"),
(261, "Columbia Pictures Film Production Asia"),
(262, "Columbia Pictures Producciones Mexico"),
(263, "Columbia Pictures"),
(264, "Columbia TriStar Television"),
(265, "Columbia TriStar"),
(266, "Columbia"),
(267, "Comedy Central Films"),
(268, "Comedy Central"),
(269, "Comedy Central_hd"),
(270, "Compass Films"),
(271, "Compass International Pictures"),
(272, "Compass Pictures"),
(273, "Compass Productions"),
(274, "Concorde Films"),
(275, "Concorde Home Entertainment"),
(276, "Concorde Pictures"),
(277, "Concorde Video"),
(278, "Concorde"),
(279, "Condemned Productions"),
(280, "ConspiraþÒo Filmes"),
(281, "Conspiraτπo Filmes"),
(282, "Constantin Entertainment"),
(283, "Constantin Film Produktion"),
(284, "Constantin Film"),
(285, "Constantin"),
(286, "Contrabando Filmes"),
(287, "Criterion Collection, The "),
(288, "Criterion Collection"),
(289, "Crowvision Inc."),
(290, "CW Television Network"),
(291, "CW"),
(292, "Cyfrowy Polsat"),
(293, "Czolowka Film Studios"),
(294, "Czolowka"),
(295, "Danish Film Institute"),
(296, "Danjaq"),
(297, "Dark Horse Entertainment"),
(298, "Dark Horse Films"),
(299, "Dark Horse Indie"),
(300, "Dark Horse Pictures"),
(301, "Darko Entertainment"),
(302, "Darren Star Productions"),
(303, "Davis Entertainment"),
(304, "Davis-Films"),
(305, "Dawn Syndicated Productions"),
(306, "DC Comics"),
(307, "De Laurentiis Entertainment Group (DEG)"),
(308, "Destination Films"),
(309, "Deutsche Film (DEFA)"),
(310, "Digital Frontier"),
(311, "Digital Playground"),
(312, "Dimension Films"),
(313, "Dimension Home Video"),
(314, "Dino de Laurentiis Cinematografica"),
(315, "Dino De Laurentiis Company"),
(316, "Discovery Channel"),
(317, "Discovery"),
(318, "Disney Channel"),
(319, "Disney DVD"),
(320, "Disney Interactive"),
(321, "Disney"),
(322, "Downtown Filmes"),
(323, "Dreamworks Animation"),
(324, "DreamWorks Home Entertainment"),
(325, "Dreamworks Pictures"),
(326, "Dreamworks SKG"),
(327, "Drimtim Entertainment"),
(328, "DVD"),
(329, "E! Entertainment Television"),
(330, "E!"),
(331, "E4"),
(332, "Eden Rock Media"),
(333, "Eleven A Music Company"),
(334, "Elz+vir Films"),
(335, "Elz┌vir Films"),
(336, "EM Media"),
(337, "Emmett"),
(338, "Emmett Furla Films"),
(339, "Entertainment Tonight"),
(340, "EON Productions"),
(341, "Epic Productions"),
(342, "Epic"),
(343, "ESPN"),
(344, "Eurimages"),
(345, "Europa Corp."),
(346, "Europa Corp"),
(347, "EuropaCorp Distribution"),
(348, "Eurosport"),
(349, "EvaApollo Media"),
(350, "Excel Entertainment Group"),
(351, "Facilidades e Filmes"),
(352, "Fact"),
(353, "Fanes Film"),
(354, "Fantefilm"),
(355, "Figaro Film Production Ltd"),
(356, "Film Commission Torino-Piemonte"),
(357, "Film Department"),
(358, "Film i V)st"),
(359, "Film i V§st"),
(360, "Film i V⌡st"),
(361, "Film Media"),
(362, "Film Polski"),
(363, "Film Victoria"),
(364, "Film Workshop"),
(365, "Film4"),
(366, "Filmax Group"),
(367, "Filmax International"),
(368, "Filmax"),
(369, "FilmColony"),
(370, "FilmFour"),
(371, "Filmirage S.r.l."),
(372, "Filmoteka Narodowa Polski"),
(373, "Filmoteka Narodowa"),
(374, "Filmstiftung Nordrhein-Westfalen"),
(375, "Filmways Home Video"),
(376, "Filmways Pictures"),
(377, "Filmways Productions"),
(378, "Filmways Television"),
(379, "Filmways"),
(380, "First Look International"),
(381, "Focus Features"),
(382, "Food Network Canada"),
(383, "Food Network, The"),
(384, "Food Network"),
(385, "foodnetwork"),
(386, "Fortress Features"),
(387, "Fox 2000 Pictures"),
(388, "FOX 8"),
(389, "Fox Atomic"),
(390, "Fox Searchlight Pictures"),
(391, "Fox Searchlight"),
(392, "FOX"),
(393, "FOX_hd"),
(394, "Franchise Pictures"),
(395, "Fuji Television Network"),
(396, "Fuji TV"),
(397, "FX"),
(398, "FX_hd"),
(399, "Gary Sanchez Productions"),
(400, "Gaumont British Picture Corporation of America"),
(401, "Gaumont Buena Vista International"),
(402, "Gaumont Company"),
(403, "Gaumont International"),
(404, "Gaumont"),
(405, "Geffen Pictures"),
(406, "Ghibli International"),
(407, "Ghibli"),
(408, "Ghost House Pictures"),
(409, "Global Asylum, The"),
(410, "Globo Filmes"),
(411, "Go Films"),
(412, "Goldcrest Films International"),
(413, "Goldcrest Films"),
(414, "Goldcrest Pictures"),
(415, "Goldcrest Post Production London"),
(416, "Goldcrest Post Production New York"),
(417, "Goldcrest Production"),
(418, "Golden Harvest Company"),
(419, "Golden Harvest Pictures (China)"),
(420, "Golden Harvest Promotions"),
(421, "Golden Harvest"),
(422, "Gracie Films"),
(423, "Gramercy Pictures"),
(424, "Gramppo Filmes"),
(425, "Greenestreet"),
(426, "Grosvenor Park Impact Productions"),
(427, "Grosvenor Park Media"),
(428, "Grosvenor Park Productions"),
(429, "Grovesnor Park Film"),
(430, "Gullane Entertainment"),
(431, "Gullane Filmes"),
(432, "Gullane Pictures"),
(433, "Gutek Film"),
(434, "Hahn Film AG"),
(435, "Halas and Batchelor Cartoon Films"),
(436, "Halcyon Company, The"),
(437, "Halestorm Entertainment"),
(438, "HandMade Films"),
(439, "HanWay Films"),
(440, "Happinet Corporation"),
(441, "Happinet Pictures"),
(442, "Happinet"),
(443, "Happy Madison Productions"),
(444, "HBO"),
(445, "HBO_hd"),
(446, "HD-DVD"),
(447, "Hell's Kitchen Films"),
(448, "Hell's Kitchen International"),
(449, "History"),
(450, "History_hd"),
(451, "Hollywood Pictures"),
(452, "Hollywood"),
(453, "Icelandic Filmcompany, The"),
(454, "Icon Distribution"),
(455, "Icon Entertainment International"),
(456, "Icon Film Distribution"),
(457, "Icon Film"),
(458, "Icon Home Entertainment"),
(459, "Icon Pictures"),
(460, "Icon Productions"),
(461, "Icon"),
(462, "IFC Films"),
(463, "IKM Productions"),
(464, "Image Entertainment"),
(465, "ImageMovers"),
(466, "Imaginary Forces"),
(467, "Imagination Worldwide"),
(468, "Imagine Entertainment"),
(469, "Imagine Television"),
(470, "IMAX"),
(471, "Immortal Thoughts"),
(472, "Industrial Light and Magic"),
(473, "Infilm"),
(474, "Infinifilm"),
(475, "Insight Film Studios"),
(476, "Intense Productions"),
(477, "interActual"),
(478, "Intermedia Films"),
(479, "ITI Film Studio"),
(480, "ITV"),
(481, "ITV1"),
(482, "ITV2"),
(483, "Jawa"),
(484, "Jerry Bruckheimer Films"),
(485, "Jim Henson Company, The"),
(486, "Kadokawa"),
(487, "KADR Studio"),
(488, "Kamera"),
(489, "KBS"),
(490, "Kennedy"),
(491, "Kings Road Entertainment"),
(492, "Kinowelt Filmproduktion"),
(493, "Kinowelt Hungary"),
(494, "Kinowelt"),
(495, "Kushner-Locke Company, The"),
(496, "La Parti Productions"),
(497, "Ladd Company, The"),
(498, "Lakeshore Entertainment"),
(499, "Largo Entertainment"),
(500, "Largo Films");

INSERT INTO "Studios" VALUES
(501, "Largo Productions"),
(502, "Largo"),
(503, "Laurinfilm"),
(504, "League of Noble Peers"),
(505, "Left Turn Films"),
(506, "Left Turn Productions"),
(507, "Legendary Pictures"),
(508, "Lemon Films"),
(509, "Lightstorm Entertainment"),
(510, "Lightstorm"),
(511, "Lionheart Entertainment"),
(512, "Lions Gate Entertainment"),
(513, "Lions Gate Family Entertainment"),
(514, "Lions Gate Films Home Entertainment"),
(515, "Lions Gate Films"),
(516, "Lions Gate International"),
(517, "Lions Gate Studios"),
(518, "Lions Gate Television"),
(519, "Lions Gate"),
(520, "Lionsgate Australia"),
(521, "Lionsgate Productions"),
(522, "Lionsgate Records"),
(523, "Lionsgate Television"),
(524, "Lionsgate"),
(525, "Lionshare Entertainment"),
(526, "Liquid Filmes"),
(527, "Lisa Film"),
(528, "Lisa-Film"),
(529, "Live Entertainment"),
(530, "LivePlanet"),
(531, "Living Films"),
(532, "Lizard"),
(533, "ls.txt"),
(534, "Lucasfilm"),
(535, "M&M Productions"),
(536, "M6 Films"),
(537, "Madman Entertainment"),
(538, "Magnolia Management"),
(539, "Magnolia Pictures"),
(540, "Management 360"),
(541, "Mandalay Pictures"),
(542, "Mandate Pictures"),
(543, "Manga"),
(544, "Marshall Company, The"),
(545, "Marvel Enterprises"),
(546, "Marvel"),
(547, "Maverick Entertainment Group"),
(548, "Maverick Entertainment"),
(549, "Maverick Films"),
(550, "Maverick Records"),
(551, "Maverick Television"),
(552, "Maverick"),
(553, "Maya Entertainment"),
(554, "MDP Worldwide"),
(555, "Media 8 Entertainment"),
(556, "Media Asia Films"),
(557, "Mediapro"),
(558, "Medusa Communications"),
(559, "Medusa Distribuzione"),
(560, "Medusa Film"),
(561, "Medusa Home Entertainment"),
(562, "Medusa Productions"),
(563, "Medusa Produzione"),
(564, "Medusa Video"),
(565, "Medusa"),
(566, "Metalight Productions"),
(567, "Metro-Goldwyn-Mayer (MGM) Studios"),
(568, "Metro-Goldwyn-Mayer (MGM)"),
(569, "Metro-Goldwyn-Mayer Distributing Corporation (MGM)"),
(570, "Metro-Goldwyn-Mayer Studios"),
(571, "Metro-Goldwyn-Mayer"),
(572, "MGM"),
(573, "Mibac"),
(574, "Mike Zoss Productions"),
(575, "Milkshake Films"),
(576, "Millenium Pictures"),
(577, "Millenium Productions"),
(578, "Millenium"),
(579, "Millennium Films"),
(580, "Minds Eye Films"),
(581, "Miramax Films"),
(582, "Miramax Home Entertainment"),
(583, "Miramax International"),
(584, "Miramax Television"),
(585, "Miramax"),
(586, "mmprod"),
(587, "Monolith Films"),
(588, "Monolith Plus"),
(589, "Montecito Picture Company, The"),
(590, "Morgan Creek International"),
(591, "Morgan Creek Productions"),
(592, "Mosaic Entertainment"),
(593, "Mosaic Film Group"),
(594, "Mosaic Films North"),
(595, "Mosaic Films"),
(596, "Mosaic Management"),
(597, "Mosaic Media Group"),
(598, "Mosaic Movies"),
(599, "Mosaic Moving Pictures"),
(600, "Mosaic Productions"),
(601, "Mosaic"),
(602, "Mosfilm"),
(603, "Moura Filmes"),
(604, "MPP"),
(605, "MSNBC Films"),
(606, "MSNBC Network"),
(607, "MSNBC"),
(608, "MTV Films"),
(609, "MTV Networks Europe"),
(610, "MTV Networks"),
(611, "MTV"),
(612, "MTV_hd"),
(613, "Music Television (MTV)"),
(614, "Myriad Pictures"),
(615, "MyTV KaRo Team"),
(616, "MyTV"),
(617, "NALA Films"),
(618, "National Geographic"),
(619, "NBC Film"),
(620, "NBC Productions"),
(621, "NBC Studios"),
(622, "NBC Universal Global Networks"),
(623, "NBC Universal Television"),
(624, "NBC"),
(625, "NBC_hd"),
(626, "Network Ten"),
(627, "Neue Constantin Film"),
(628, "New Line Cinema"),
(629, "New Line Home Entertainment"),
(630, "New Line Home Video"),
(631, "New Line Productions"),
(632, "New Line Television"),
(633, "New Regency Pictures"),
(634, "New World Pictures"),
(635, "New Zealand Film Commission"),
(636, "New Zealand Film Corporation"),
(637, "New Zealand Film Production Fund"),
(638, "New Zealand Film Unit"),
(639, "newlinecinema"),
(640, "Newmarket Capital Group"),
(641, "Nickelodeon Animation Studios"),
(642, "Nickelodeon Movies"),
(643, "Nickelodeon Productions"),
(644, "Nickelodeon Studios"),
(645, "Nickelodeon"),
(646, "Nine Network"),
(647, "NonStop Sales AB"),
(648, "Nordisk Film"),
(649, "Nothing Studios"),
(650, "Nothing"),
(651, "Nu Image Films"),
(652, "Nu Image"),
(653, "NUIMAGE"),
(654, "O2 Filmes"),
(655, "Odd Lot Entertainment"),
(656, "Open Door Productions"),
(657, "Open Doors International"),
(658, "Orion Home Video"),
(659, "Orion Pictures Corporation"),
(660, "Orion Television Distribution"),
(661, "Orion Television Entertainment"),
(662, "Orion Television"),
(663, "Orion-Nova Productions"),
(664, "Overbrook Entertainment"),
(665, "Overbrook Management"),
(666, "Overbrook Television"),
(667, "Overture Entertainment LLC"),
(668, "Overture Films"),
(669, "Pandora Cinema"),
(670, "Pandora Filmproduktion"),
(671, "Pandora Films"),
(672, "Pandora Productions"),
(673, "Pandora"),
(674, "Paramount British Pictures"),
(675, "Paramount Classics"),
(676, "Paramount Home Entertainment"),
(677, "Paramount Home Video"),
(678, "Paramount Pictures"),
(679, "Paramount Studios"),
(680, "Paramount Television"),
(681, "Paramount Vantage"),
(682, "Paramount-Film"),
(683, "Paramount"),
(684, "Participant Productions"),
(685, "Pathe"),
(686, "PBS American Experience"),
(687, "PBS Home Video"),
(688, "PBS Kids"),
(689, "PBS Networks"),
(690, "PBS Pictures"),
(691, "PBS Television"),
(692, "PBS Video"),
(693, "PBS"),
(694, "Peace Arch Entertainment Group"),
(695, "Peace Arch Films"),
(696, "Peace Arch Home Entertainment"),
(697, "Peace Arch Motion Pictures"),
(698, "Peace Arch Releasing"),
(699, "PECF"),
(700, "Phoenix Pictures"),
(701, "Phoenix"),
(702, "Picturehouse Cinemas"),
(703, "Picturehouse Entertainment"),
(704, "Picturehouse Marketing"),
(705, "Picturehouse"),
(706, "PISF"),
(707, "Pixar Animation Studios"),
(708, "Platige Image"),
(709, "Platige"),
(710, "Playtone Productions"),
(711, "Playtone"),
(712, "Plum Films"),
(713, "Plum Pictures"),
(714, "polsat hd"),
(715, "polsat"),
(716, "PolyGram Film Distribution"),
(717, "PolyGram Film Entertainment"),
(718, "Polygram Filmed Entertainment"),
(719, "Polygram Studios"),
(720, "PolyGram Television"),
(721, "PolyGram Video"),
(722, "PolyGram"),
(723, "Premiere-Direkt"),
(724, "Premiere"),
(725, "Priority Records"),
(726, "PÒo Com Ovo Filmes"),
(727, "Pπo Com Ovo Filmes"),
(728, "Rai Cinema"),
(729, "Rai Cinemafiction"),
(730, "Rai Fiction"),
(731, "Rai Tre Radiotelevisione Italiana"),
(732, "Rai Uno Radiotelevisione"),
(733, "Rai"),
(734, "Rankin"),
(735, "RankinBass Productions"),
(736, "Rapi Films"),
(737, "Raw Feed"),
(738, "Red Circle Productions"),
(739, "Red Wagon Entertainment"),
(740, "Red Wagon Films"),
(741, "Red Wagon Productions"),
(742, "RedRum Entertainment"),
(743, "Redrum Films"),
(744, "Regal Film"),
(745, "Regal Films Company Ltd."),
(746, "Regal Films"),
(747, "Regal Multimedia"),
(748, "Regal Pictures"),
(749, "Regal Productions"),
(750, "Regal"),
(751, "Regency Enterprises"),
(752, "Regency Films"),
(753, "Regency Home Video"),
(754, "Regency International Pictures"),
(755, "Regency Productions"),
(756, "Regency Television"),
(757, "Relativity Films"),
(758, "Relativity Media"),
(759, "Relativity Pictures"),
(760, "Remarkable Films"),
(761, "Resident Evil Productions"),
(762, "Revolution Films"),
(763, "Revolution Pictures"),
(764, "Revolution Studios"),
(765, "Rhino Films"),
(766, "Rhombus Film"),
(767, "Rhombus Media"),
(768, "Rhombus"),
(769, "RKO Home Video"),
(770, "RKO Radio Films"),
(771, "RKO Radio Pictures"),
(772, "Roadside Attractions"),
(773, "Roadside Cinema"),
(774, "Roadside Entertainment"),
(775, "Roadside Pictures"),
(776, "Roadside Productions"),
(777, "Rogue Films"),
(778, "Rogue Pictures"),
(779, "RTL Television"),
(780, "RTL"),
(781, "Ruthless Records"),
(782, "Saga Film (I)"),
(783, "Saga Film"),
(784, "Saga Films"),
(785, "Sandrew Metronome Distribution"),
(786, "Sandrew"),
(787, "Sandrews"),
(788, "Saturn Films"),
(789, "Saul Zaentz Film Center"),
(790, "SBS"),
(791, "Science Fiction Channel"),
(792, "Science Fiction"),
(793, "SciFi"),
(794, "SciFi_hd"),
(795, "Scott Free Productions"),
(796, "Scott Free"),
(797, "Screen Australia"),
(798, "Screen Gems Television"),
(799, "Screen Gems"),
(800, "Screen West Midlands"),
(801, "Screener"),
(802, "screengems"),
(803, "Se-ma-for Studios"),
(804, "Se-ma-for"),
(805, "Seed Productions"),
(806, "Semafor"),
(807, "SenArt Films"),
(808, "Senator Distribution"),
(809, "Senator Film Produktion"),
(810, "Senator Film"),
(811, "Senator Home Entertainment"),
(812, "Senator International"),
(813, "Serendipity Films"),
(814, "Serendipity Point Films"),
(815, "Serendipity Productions"),
(816, "Serendipity"),
(817, "Seven Network"),
(818, "Shaw Brothers"),
(819, "Sherwood Foundation, The"),
(820, "Sherwood MacDonald Productions"),
(821, "Sherwood Pictures"),
(822, "Sherwood Productions Ltd."),
(823, "Sherwood Productions"),
(824, "Sherwood Schwartz Productions"),
(825, "Sherwood-Wadsworth Pictures"),
(826, "Sherwood"),
(827, "Shochiku Company"),
(828, "Shochiku Daiichi Kogyo"),
(829, "Shochiku Eizo Company"),
(830, "Shochiku Kinema (Kamata)"),
(831, "Shochiku Kinema"),
(832, "Shochiku Kyoto"),
(833, "Shochiku Ofuna"),
(834, "Shout Factory"),
(835, "Show Dog Productions"),
(836, "Showbox Entertainment"),
(837, "Showtime Australia"),
(838, "Showtime DVD"),
(839, "Showtime Entertainment"),
(840, "Showtime Original Pictures"),
(841, "Showtime Pictures Inc."),
(842, "Showtime Pictures"),
(843, "Showtime Video Ventures"),
(844, "Showtime"),
(845, "Showtime_hd"),
(846, "Sidney Kimmel Entertainment"),
(847, "Silver Nitrate Films"),
(848, "Silver Nitrate Pictures"),
(849, "Silver Nitrate Releasing"),
(850, "Silver Nitrate"),
(851, "Silver Pictures Television"),
(852, "Silver Pictures"),
(853, "Sky One HD"),
(854, "Sky One Productions"),
(855, "Sky One"),
(856, "Sky One_hd"),
(857, "sky1"),
(858, "SMC"),
(859, "Solar Film Productions"),
(860, "Solar Films"),
(861, "Solar Filmworks"),
(862, "Solar Productions"),
(863, "Solar-Film GmbH"),
(864, "Solar-Film"),
(865, "Sony Pictures Classics"),
(866, "Sony Pictures Entertainment"),
(867, "Sony Pictures Home Entertainment"),
(868, "Sony Pictures Studios"),
(869, "Sony Pictures Television International"),
(870, "Sony Pictures Television"),
(871, "Sony Pictures"),
(872, "Sony"),
(873, "sonypictures"),
(874, "Space Films"),
(875, "Space Productions"),
(876, "Space"),
(877, "Speed"),
(878, "Spike Productions"),
(879, "Spike TV"),
(880, "Spike"),
(881, "Spyglass Entertainment"),
(882, "Square Enix Company"),
(883, "Square Enix"),
(884, "Square-Enix"),
(885, "Stage 6 Films"),
(886, "Stage 6 Productions"),
(887, "Stage 6"),
(888, "Star Overseas"),
(889, "Starving Kappa Pictures"),
(890, "Starz!"),
(891, "Starz"),
(892, "Starz_hd"),
(893, "Steele Films"),
(894, "Storyline Entertainment"),
(895, "Storyline Productions"),
(896, "Studio Canal"),
(897, "Studio Eight Productions"),
(898, "Studio Filmowe Oko"),
(899, "Studio Filmowe Se-Ma-For"),
(900, "Studio Ghibli"),
(901, "StudioCanal"),
(902, "Studios USA Television"),
(903, "Studios USA"),
(904, "Subversive Cinema"),
(905, "Subversive Flix"),
(906, "Subversive Propaganda"),
(907, "Subversive"),
(908, "Summit Distribution"),
(909, "Summit Entertainment"),
(910, "Summit International"),
(911, "Summit Media"),
(912, "Superstation WGN"),
(913, "Svensk Filmindustri (SF)"),
(914, "Svenska Filminstitutet"),
(915, "Swedish Film Institute"),
(916, "Syfy"),
(917, "Syndicated Productions"),
(918, "Syndicated"),
(919, "Syrena Film"),
(920, "Syrena Films"),
(921, "Syrena"),
(922, "Tandem Communications"),
(923, "Tandem Films"),
(924, "Tandem Productions"),
(925, "Tatsunoko Productions Company"),
(926, "TBS Superstation"),
(927, "TBS"),
(928, "Telewizja Polska (TVP)"),
(929, "Telewizja Polska "),
(930, "Telos Films"),
(931, "Ten"),
(932, "The CW"),
(933, "The CW_hd"),
(934, "The Food Network"),
(935, "The WB"),
(936, "The Weinstein Company"),
(937, "ThinkFilm"),
(938, "This Is That Productions"),
(939, "This Is That"),
(940, "Thura Film"),
(941, "Tig Productions"),
(942, "Tiger Aspect Productions"),
(943, "Titan Media"),
(944, "Titan Productions"),
(945, "TLC"),
(946, "TNT Originals"),
(947, "TNT"),
(948, "TOBIS Film"),
(949, "Tobis-Filmverleih"),
(950, "Tobis"),
(951, "Toei Animation Company"),
(952, "Toei Company"),
(953, "Toei Inc."),
(954, "Toei International Company Ltd."),
(955, "Toei Picture Company Productions"),
(956, "Toei Tokyo"),
(957, "Toei Video Company"),
(958, "Toei"),
(959, "toho b"),
(960, "Toho Company"),
(961, "Toho Eiga (Tokyo)"),
(962, "Toho Eizo Co."),
(963, "Toho Film (Eiga) Co. Ltd."),
(964, "Toho Film Distributing Co. Ltd."),
(965, "Toho Kyoiku Eiga"),
(966, "Toho Pictures Inc."),
(967, "Toho Studios"),
(968, "Toho Video"),
(969, "Toho"),
(970, "Tokyo Broadcasting System"),
(971, "Tommy Boy Records"),
(972, "Too Askew Prod. Inc."),
(973, "Tor Films"),
(974, "Tor"),
(975, "Tornado Film"),
(976, "Tornado Films"),
(977, "Tornado"),
(978, "Touchstone Home Video"),
(979, "Touchstone Pictures"),
(980, "Touchstone Television"),
(981, "Touchstone"),
(982, "Trancas International Films"),
(983, "Travel Channel"),
(984, "Trigger Street Independent"),
(985, "Trigger Street Productions"),
(986, "Triggerstreet"),
(987, "Trimark Home Video"),
(988, "Trimark Pictures"),
(989, "Trimark Video"),
(990, "Trimark"),
(991, "TriStar Pictures"),
(992, "TriStar Television"),
(993, "Tristar"),
(994, "Troma Entertainment"),
(995, "Troma Team Video"),
(996, "Troma Video Entertainment GmbH"),
(997, "Troma"),
(998, "Turner Classic Movies (TCM)"),
(999, "Turner Entertainment");

INSERT INTO "Studios" VALUES
(1000, "Turner Home Entertainment"),
(1001, "Turner Network Television (TNT)"),
(1002, "Turner Pictures (I)"),
(1003, "Turner Studios"),
(1004, "Turner Television"),
(1005, "Turner"),
(1006, "TV 4"),
(1007, "TV2 Danmark"),
(1008, "TVN Turbo"),
(1009, "TVN"),
(1010, "TVP SA"),
(1011, "TVP"),
(1012, "TVP1"),
(1013, "TVP2"),
(1014, "Twentieth Century Fox Animation"),
(1015, "Twentieth Century Fox Archive"),
(1016, "Twentieth Century Fox Film Company"),
(1017, "Twentieth Century Fox Film"),
(1018, "Twentieth Century Fox Home Entertainment Germany"),
(1019, "Twentieth Century Fox Home Entertainment"),
(1020, "Twentieth Century Fox of Germany"),
(1021, "Twentieth Century Fox Television"),
(1022, "Twentieth Century Fox"),
(1023, "Twentieth Century-Fox Film Corporation"),
(1024, "Twentieth Century-Fox Productions"),
(1025, "Twentieth Television "),
(1026, "Twisted Pictures"),
(1027, "Two For Flinching Pictures"),
(1028, "Two for Flinching Productions LLC"),
(1029, "UGC Distribution"),
(1030, "UGC Films"),
(1031, "UGC International"),
(1032, "UGC YM"),
(1033, "UGC-Fox Distribution"),
(1034, "UGC"),
(1035, "UIP"),
(1036, "UK Film Council"),
(1037, "Unified Film Organization (UFO)"),
(1038, "United Artists Classics"),
(1039, "United Artists Corporation"),
(1040, "United Artists Europa"),
(1041, "United Artists Films"),
(1042, "United Artists Pictures "),
(1043, "United Artists Television"),
(1044, "United Artists"),
(1045, "United British Artists"),
(1046, "United Digital Artists"),
(1047, "United International Pictures (UIP)"),
(1048, "United International Pictures"),
(1049, "United Paramount Network (UPN)"),
(1050, "unitedartists"),
(1051, "Universal City Studios"),
(1052, "Universal Film Manufacturing Company"),
(1053, "Universal Films"),
(1054, "Universal Home Entertainment"),
(1055, "Universal Home Video"),
(1056, "Universal International Pictures (UI)"),
(1057, "Universal Media Studios (UMS)"),
(1058, "Universal Network Television"),
(1059, "Universal Pictures (Spain)"),
(1060, "Universal Pictures Benelux"),
(1061, "Universal Pictures Finland Oy"),
(1062, "Universal Pictures International (UPI)"),
(1063, "Universal Pictures Nordic"),
(1064, "Universal Pictures"),
(1065, "Universal Production Partners (UPP)"),
(1066, "Universal Studios Home Entertainment"),
(1067, "Universal Studios Home Video"),
(1068, "Universal Studios"),
(1069, "Universal Television"),
(1070, "Universal Title"),
(1071, "Universal TV"),
(1072, "Universal"),
(1073, "Universum Film (UFA)"),
(1074, "Universum Film"),
(1075, "Universum-Film GmbH"),
(1076, "UPN"),
(1077, "USA Cable Network"),
(1078, "USA Films"),
(1079, "USA Home Video (II)"),
(1080, "USA Network"),
(1081, "USA"),
(1082, "Versus Entertainment"),
(1083, "Versus Films"),
(1084, "Versus Ivan Incorporated"),
(1085, "Versus Media"),
(1086, "Versus Network"),
(1087, "Versus Production"),
(1088, "Versus"),
(1089, "VH1 Classic"),
(1090, "VH1 Original Movies"),
(1091, "VH1 Productions"),
(1092, "VH1 Television"),
(1093, "VH1"),
(1094, "View Askew Productions"),
(1095, "Village Roadshow"),
(1096, "Virtual Studios"),
(1097, "Virtual"),
(1098, "Vivendi Entertainment"),
(1099, "Voltage Pictures"),
(1100, "Walden Enterprises"),
(1101, "Walden Entertainment"),
(1102, "Walden Media"),
(1103, "Walden Productions"),
(1104, "Walden Woods Film Company "),
(1105, "Walt Disney Animation Australia"),
(1106, "Walt Disney Animation Studios"),
(1107, "Walt Disney Company, The"),
(1108, "Walt Disney Feature Animation"),
(1109, "Walt Disney Home Entertainment"),
(1110, "Walt Disney Home Video"),
(1111, "Walt Disney Pictures and Television"),
(1112, "Walt Disney Pictures"),
(1113, "Walt Disney Productions"),
(1114, "Walt Disney Records"),
(1115, "Walt Disney Studio Archives, The"),
(1116, "Walt Disney Studios Home Entertainment"),
(1117, "Walt Disney Studios Motion Pictures"),
(1118, "Walt Disney Studios"),
(1119, "Walt Disney Television Animation"),
(1120, "Walt Disney Television"),
(1121, "Walt Disney"),
(1122, "Warner Bros. Animation"),
(1123, "Warner Bros. Archives"),
(1124, "Warner Bros. Entertainment"),
(1125, "Warner Bros. Home Video"),
(1126, "Warner Bros. Motion Picture Imaging"),
(1127, "Warner Bros. Pictures"),
(1128, "Warner Bros. Studio Facilities"),
(1129, "Warner Bros. Studios"),
(1130, "Warner Bros. Television"),
(1131, "Warner Bros."),
(1132, "Warner Brothers Entertainment"),
(1133, "Warner Brothers First National Films "),
(1134, "Warner Brothers Pictures"),
(1135, "Warner Brothers Post-Production Services"),
(1136, "Warner Brothers-First National Productions"),
(1137, "Warner Brothers"),
(1138, "Warner Independent Pictures (WIP)"),
(1139, "Warner"),
(1140, "WB Television Network, The"),
(1141, "WB"),
(1142, "WDR"),
(1143, "WFDIF Film Archive"),
(1144, "wfdif"),
(1145, "wgn"),
(1146, "Wicked Pictures"),
(1147, "Wild Bunch Benelux"),
(1148, "Wild Bunch Distribution"),
(1149, "Wild Bunch Inc."),
(1150, "Wild Bunch"),
(1151, "Wingman Productions"),
(1152, "WingNut Films"),
(1153, "WIP"),
(1154, "Working Title Australia"),
(1155, "Working Title Films"),
(1156, "Working Title Television"),
(1157, "Working Title"),
(1158, "WWE Films"),
(1159, "WWE Studios"),
(1160, "X RAY Productions Inc."),
(1161, "X-Filme Creative Pool"),
(1162, "yari"),
(1163, "Yash Raj Films Design Cell"),
(1164, "Yash Raj Films International Ltd."),
(1165, "Yash Raj Films Internet Cell"),
(1166, "Yash Raj Films Publicity Team"),
(1167, "Yash Raj Films USA Inc."),
(1168, "Yash Raj Films"),
(1169, "Yash-Raj-Films"),
(1170, "Zazen Produþ§es"),
(1171, "Zazen Produτ⌡es"),
(1172, "ZDF Enterprises"),
(1173, "ZDF"),
(1174, "Zebra Film Studio"),
(1175, "Zebra Film"),
(1176, "Zebra Filmes"),
(1177, "Zebra Films"),
(1178, "Zebra Producciones"),
(1179, "zebra"),
(1180, "Zelig Films Distribution"),
(1181, "Zentropa Entertainments"),
(1182, "Zentropa International Berlin"),
(1183, "Zentropa Productions"),
(1184, "Zentropa Real ApS"),
(1185, "zentropa"),
(1186, "Zero Film GmbH"),
(1187, "Zero One Entertainment"),
(1188, "Zero One Film"),
(1189, "Zero One Zero"),
(1190, "Zodiac Films"),
(1191, "Zodiac Pictures International"),
(1192, "Zodiac Productions"),
(1193, "Zodiac Produzioni"),
(1194, "Zodiac Video"),
(1195, "Zodiac"),
(1196, "Zweites Deutsches Fernsehen (ZDF)");

COMMIT;