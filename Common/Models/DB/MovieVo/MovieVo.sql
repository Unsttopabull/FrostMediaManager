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
where IsMultipart = 1;


-- ----------------------------
-- Data for 'Countries'
-- ----------------------------"
INSERT INTO "Countries" VALUES (1, "Afghanistan", "AF", "AFG");
INSERT INTO "Countries" VALUES (2, "Åland Islands", "AX", "ALA");
INSERT INTO "Countries" VALUES (3, "Albania", "AL", "ALB");
INSERT INTO "Countries" VALUES (4, "Algeria", "DZ", "DZA");
INSERT INTO "Countries" VALUES (5, "American Samoa", "AS", "ASM");
INSERT INTO "Countries" VALUES (6, "Andorra", "AD", "AND");
INSERT INTO "Countries" VALUES (7, "Angola", "AO", "AGO");
INSERT INTO "Countries" VALUES (8, "Anguilla", "AI", "AIA");
INSERT INTO "Countries" VALUES (9, "Antarctica", "AQ", "ATA");
INSERT INTO "Countries" VALUES (10, "Antigua and Barbuda", "AG", "ATG");
INSERT INTO "Countries" VALUES (11, "Argentina", "AR", "ARG");
INSERT INTO "Countries" VALUES (12, "Armenia", "AM", "ARM");
INSERT INTO "Countries" VALUES (13, "Aruba", "AW", "ABW");
INSERT INTO "Countries" VALUES (14, "Australia", "AU", "AUS");
INSERT INTO "Countries" VALUES (15, "Austria", "AT", "AUT");
INSERT INTO "Countries" VALUES (16, "Azerbaijan", "AZ", "AZE");
INSERT INTO "Countries" VALUES (17, "Bahamas", "BS", "BHS");
INSERT INTO "Countries" VALUES (18, "Bahrain", "BH", "BHR");
INSERT INTO "Countries" VALUES (19, "Bangladesh", "BD", "BGD");
INSERT INTO "Countries" VALUES (20, "Barbados", "BB", "BRB");
INSERT INTO "Countries" VALUES (21, "Belarus", "BY", "BLR");
INSERT INTO "Countries" VALUES (22, "Belgium", "BE", "BEL");
INSERT INTO "Countries" VALUES (23, "Belize", "BZ", "BLZ");
INSERT INTO "Countries" VALUES (24, "Benin", "BJ", "BEN");
INSERT INTO "Countries" VALUES (25, "Bermuda", "BM", "BMU");
INSERT INTO "Countries" VALUES (26, "Bhutan", "BT", "BTN");
INSERT INTO "Countries" VALUES (27, "Bolivia", "BO", "BOL");
INSERT INTO "Countries" VALUES (28, "Bonaire, Sint Eustatius and Saba", "BQ", "BES");
INSERT INTO "Countries" VALUES (29, "Bosnia and Herzegovina", "BA", "BIH");
INSERT INTO "Countries" VALUES (30, "Botswana", "BW", "BWA");
INSERT INTO "Countries" VALUES (31, "Bouvet Island", "BV", "BVT");
INSERT INTO "Countries" VALUES (32, "Brazil", "BR", "BRA");
INSERT INTO "Countries" VALUES (33, "British Indian Ocean Territory", "IO", "IOT");
INSERT INTO "Countries" VALUES (34, "Brunei Darussalam", "BN", "BRN");
INSERT INTO "Countries" VALUES (35, "Bulgaria", "BG", "BGR");
INSERT INTO "Countries" VALUES (36, "Burkina Faso", "BF", "BFA");
INSERT INTO "Countries" VALUES (37, "Burundi", "BI", "BDI");
INSERT INTO "Countries" VALUES (38, "Cambodia", "KH", "KHM");
INSERT INTO "Countries" VALUES (39, "Cameroon", "CM", "CMR");
INSERT INTO "Countries" VALUES (40, "Canada", "CA", "CAN");
INSERT INTO "Countries" VALUES (41, "Cape Verde", "CV", "CPV");
INSERT INTO "Countries" VALUES (42, "Cayman Islands", "KY", "CYM");
INSERT INTO "Countries" VALUES (43, "Central African Republic", "CF", "CAF");
INSERT INTO "Countries" VALUES (44, "Chad", "TD", "TCD");
INSERT INTO "Countries" VALUES (45, "Chile", "CL", "CHL");
INSERT INTO "Countries" VALUES (46, "China", "CN", "CHN");
INSERT INTO "Countries" VALUES (47, "Christmas Island", "CX", "CXR");
INSERT INTO "Countries" VALUES (48, "Cocos (Keeling) Islands", "CC", "CCK");
INSERT INTO "Countries" VALUES (49, "Colombia", "CO", "COL");
INSERT INTO "Countries" VALUES (50, "Comoros", "KM", "COM");
INSERT INTO "Countries" VALUES (51, "Congo", "CG", "COG");
INSERT INTO "Countries" VALUES (52, "Congo (the Democratic Republic of the)", "CD", "COD");
INSERT INTO "Countries" VALUES (53, "Cook Islands", "CK", "COK");
INSERT INTO "Countries" VALUES (54, "Costa Rica", "CR", "CRI");
INSERT INTO "Countries" VALUES (55, "Côte d'Ivoire", "CI", "CIV");
INSERT INTO "Countries" VALUES (56, "Croatia", "HR", "HRV");
INSERT INTO "Countries" VALUES (57, "Cuba", "CU", "CUB");
INSERT INTO "Countries" VALUES (58, "Curaçao", "CW", "CUW");
INSERT INTO "Countries" VALUES (59, "Cyprus", "CY", "CYP");
INSERT INTO "Countries" VALUES (60, "Czech Republic", "CZ", "CZE");
INSERT INTO "Countries" VALUES (61, "Denmark", "DK", "DNK");
INSERT INTO "Countries" VALUES (62, "Djibouti", "DJ", "DJI");
INSERT INTO "Countries" VALUES (63, "Dominica", "DM", "DMA");
INSERT INTO "Countries" VALUES (64, "Dominican Republic", "DO", "DOM");
INSERT INTO "Countries" VALUES (65, "Ecuador", "EC", "ECU");
INSERT INTO "Countries" VALUES (66, "Egypt", "EG", "EGY");
INSERT INTO "Countries" VALUES (67, "El Salvador", "SV", "SLV");
INSERT INTO "Countries" VALUES (68, "Equatorial Guinea", "GQ", "GNQ");
INSERT INTO "Countries" VALUES (69, "Eritrea", "ER", "ERI");
INSERT INTO "Countries" VALUES (70, "Estonia", "EE", "EST");
INSERT INTO "Countries" VALUES (71, "Ethiopia", "ET", "ETH");
INSERT INTO "Countries" VALUES (72, "Falkland Islands [Malvinas]", "FK", "FLK");
INSERT INTO "Countries" VALUES (73, "Faroe Islands", "FO", "FRO");
INSERT INTO "Countries" VALUES (74, "Fiji", "FJ", "FJI");
INSERT INTO "Countries" VALUES (75, "Finland", "FI", "FIN");
INSERT INTO "Countries" VALUES (76, "France", "FR", "FRA");
INSERT INTO "Countries" VALUES (77, "French Guiana", "GF", "GUF");
INSERT INTO "Countries" VALUES (78, "French Polynesia", "PF", "PYF");
INSERT INTO "Countries" VALUES (79, "French Southern Territories", "TF", "ATF");
INSERT INTO "Countries" VALUES (80, "Gabon", "GA", "GAB");
INSERT INTO "Countries" VALUES (81, "Gambia", "GM", "GMB");
INSERT INTO "Countries" VALUES (82, "Georgia", "GE", "GEO");
INSERT INTO "Countries" VALUES (83, "Germany", "DE", "DEU");
INSERT INTO "Countries" VALUES (84, "Ghana", "GH", "GHA");
INSERT INTO "Countries" VALUES (85, "Gibraltar", "GI", "GIB");
INSERT INTO "Countries" VALUES (86, "Greece", "GR", "GRC");
INSERT INTO "Countries" VALUES (87, "Greenland", "GL", "GRL");
INSERT INTO "Countries" VALUES (88, "Grenada", "GD", "GRD");
INSERT INTO "Countries" VALUES (89, "Guadeloupe", "GP", "GLP");
INSERT INTO "Countries" VALUES (90, "Guam", "GU", "GUM");
INSERT INTO "Countries" VALUES (91, "Guatemala", "GT", "GTM");
INSERT INTO "Countries" VALUES (92, "Guernsey", "GG", "GGY");
INSERT INTO "Countries" VALUES (93, "Guinea", "GN", "GIN");
INSERT INTO "Countries" VALUES (94, "Guinea-Bissau", "GW", "GNB");
INSERT INTO "Countries" VALUES (95, "Guyana", "GY", "GUY");
INSERT INTO "Countries" VALUES (96, "Haiti", "HT", "HTI");
INSERT INTO "Countries" VALUES (97, "Heard Island and McDonald Islands", "HM", "HMD");
INSERT INTO "Countries" VALUES (98, "Holy See [Vatican City State]", "VA", "VAT");
INSERT INTO "Countries" VALUES (99, "Honduras", "HN", "HND");
INSERT INTO "Countries" VALUES (100, "Hong Kong", "HK", "HKG");
INSERT INTO "Countries" VALUES (101, "Hungary", "HU", "HUN");
INSERT INTO "Countries" VALUES (102, "Iceland", "IS", "ISL");
INSERT INTO "Countries" VALUES (103, "India", "IN", "IND");
INSERT INTO "Countries" VALUES (104, "Indonesia", "ID", "IDN");
INSERT INTO "Countries" VALUES (105, "Iran", "IR", "IRN");
INSERT INTO "Countries" VALUES (106, "Iraq", "IQ", "IRQ");
INSERT INTO "Countries" VALUES (107, "Ireland", "IE", "IRL");
INSERT INTO "Countries" VALUES (108, "Isle of Man", "IM", "IMN");
INSERT INTO "Countries" VALUES (109, "Israel", "IL", "ISR");
INSERT INTO "Countries" VALUES (110, "Italy", "IT", "ITA");
INSERT INTO "Countries" VALUES (111, "Jamaica", "JM", "JAM");
INSERT INTO "Countries" VALUES (112, "Japan", "JP", "JPN");
INSERT INTO "Countries" VALUES (113, "Jersey", "JE", "JEY");
INSERT INTO "Countries" VALUES (114, "Jordan", "JO", "JOR");
INSERT INTO "Countries" VALUES (115, "Kazakhstan", "KZ", "KAZ");
INSERT INTO "Countries" VALUES (116, "Kenya", "KE", "KEN");
INSERT INTO "Countries" VALUES (117, "Kiribati", "KI", "KIR");
INSERT INTO "Countries" VALUES (118, "Korea (the Democratic People's Republic of)", "KP", "PRK");
INSERT INTO "Countries" VALUES (119, "South Korea", "KR", "KOR");
INSERT INTO "Countries" VALUES (120, "Kuwait", "KW", "KWT");
INSERT INTO "Countries" VALUES (121, "Kyrgyzstan", "KG", "KGZ");
INSERT INTO "Countries" VALUES (122, "Lao People's Democratic Republic", "LA", "LAO");
INSERT INTO "Countries" VALUES (123, "Latvia", "LV", "LVA");
INSERT INTO "Countries" VALUES (124, "Lebanon", "LB", "LBN");
INSERT INTO "Countries" VALUES (125, "Lesotho", "LS", "LSO");
INSERT INTO "Countries" VALUES (126, "Liberia", "LR", "LBR");
INSERT INTO "Countries" VALUES (127, "Libya", "LY", "LBY");
INSERT INTO "Countries" VALUES (128, "Liechtenstein", "LI", "LIE");
INSERT INTO "Countries" VALUES (129, "Lithuania", "LT", "LTU");
INSERT INTO "Countries" VALUES (130, "Luxembourg", "LU", "LUX");
INSERT INTO "Countries" VALUES (131, "Macao", "MO", "MAC");
INSERT INTO "Countries" VALUES (132, "Macedonia", "MK", "MKD");
INSERT INTO "Countries" VALUES (133, "Madagascar", "MG", "MDG");
INSERT INTO "Countries" VALUES (134, "Malawi", "MW", "MWI");
INSERT INTO "Countries" VALUES (135, "Malaysia", "MY", "MYS");
INSERT INTO "Countries" VALUES (136, "Maldives", "MV", "MDV");
INSERT INTO "Countries" VALUES (137, "Mali", "ML", "MLI");
INSERT INTO "Countries" VALUES (138, "Malta", "MT", "MLT");
INSERT INTO "Countries" VALUES (139, "Marshall Islands", "MH", "MHL");
INSERT INTO "Countries" VALUES (140, "Martinique", "MQ", "MTQ");
INSERT INTO "Countries" VALUES (141, "Mauritania", "MR", "MRT");
INSERT INTO "Countries" VALUES (142, "Mauritius", "MU", "MUS");
INSERT INTO "Countries" VALUES (143, "Mayotte", "YT", "MYT");
INSERT INTO "Countries" VALUES (144, "Mexico", "MX", "MEX");
INSERT INTO "Countries" VALUES (145, "Micronesia", "FM", "FSM");
INSERT INTO "Countries" VALUES (146, "Moldova", "MD", "MDA");
INSERT INTO "Countries" VALUES (147, "Monaco", "MC", "MCO");
INSERT INTO "Countries" VALUES (148, "Mongolia", "MN", "MNG");
INSERT INTO "Countries" VALUES (149, "Montenegro", "ME", "MNE");
INSERT INTO "Countries" VALUES (150, "Montserrat", "MS", "MSR");
INSERT INTO "Countries" VALUES (151, "Morocco", "MA", "MAR");
INSERT INTO "Countries" VALUES (152, "Mozambique", "MZ", "MOZ");
INSERT INTO "Countries" VALUES (153, "Myanmar", "MM", "MMR");
INSERT INTO "Countries" VALUES (154, "Namibia", "NA", "NAM");
INSERT INTO "Countries" VALUES (155, "Nauru", "NR", "NRU");
INSERT INTO "Countries" VALUES (156, "Nepal", "NP", "NPL");
INSERT INTO "Countries" VALUES (157, "Netherlands", "NL", "NLD");
INSERT INTO "Countries" VALUES (158, "New Caledonia", "NC", "NCL");
INSERT INTO "Countries" VALUES (159, "New Zealand", "NZ", "NZL");
INSERT INTO "Countries" VALUES (160, "Nicaragua", "NI", "NIC");
INSERT INTO "Countries" VALUES (161, "Niger", "NE", "NER");
INSERT INTO "Countries" VALUES (162, "Nigeria", "NG", "NGA");
INSERT INTO "Countries" VALUES (163, "Niue", "NU", "NIU");
INSERT INTO "Countries" VALUES (164, "Norfolk Island", "NF", "NFK");
INSERT INTO "Countries" VALUES (165, "Northern Mariana Islands", "MP", "MNP");
INSERT INTO "Countries" VALUES (166, "Norway", "NO", "NOR");
INSERT INTO "Countries" VALUES (167, "Oman", "OM", "OMN");
INSERT INTO "Countries" VALUES (168, "Pakistan", "PK", "PAK");
INSERT INTO "Countries" VALUES (169, "Palau", "PW", "PLW");
INSERT INTO "Countries" VALUES (170, "Palestine, State of", "PS", "PSE");
INSERT INTO "Countries" VALUES (171, "Panama", "PA", "PAN");
INSERT INTO "Countries" VALUES (172, "Papua New Guinea", "PG", "PNG");
INSERT INTO "Countries" VALUES (173, "Paraguay", "PY", "PRY");
INSERT INTO "Countries" VALUES (174, "Peru", "PE", "PER");
INSERT INTO "Countries" VALUES (175, "Philippines", "PH", "PHL");
INSERT INTO "Countries" VALUES (176, "Pitcairn", "PN", "PCN");
INSERT INTO "Countries" VALUES (177, "Poland", "PL", "POL");
INSERT INTO "Countries" VALUES (178, "Portugal", "PT", "PRT");
INSERT INTO "Countries" VALUES (179, "Puerto Rico", "PR", "PRI");
INSERT INTO "Countries" VALUES (180, "Qatar", "QA", "QAT");
INSERT INTO "Countries" VALUES (181, "Réunion", "RE", "REU");
INSERT INTO "Countries" VALUES (182, "Romania", "RO", "ROU");
INSERT INTO "Countries" VALUES (183, "Russian Federation", "RU", "RUS");
INSERT INTO "Countries" VALUES (184, "Rwanda", "RW", "RWA");
INSERT INTO "Countries" VALUES (185, "Saint Barthélemy", "BL", "BLM");
INSERT INTO "Countries" VALUES (186, "Saint Helena, Ascension and Tristan da Cunha", "SH", "SHN");
INSERT INTO "Countries" VALUES (187, "Saint Kitts and Nevis", "KN", "KNA");
INSERT INTO "Countries" VALUES (188, "Saint Lucia", "LC", "LCA");
INSERT INTO "Countries" VALUES (189, "Saint Martin (French part)", "MF", "MAF");
INSERT INTO "Countries" VALUES (190, "Saint Pierre and Miquelon", "PM", "SPM");
INSERT INTO "Countries" VALUES (191, "Saint Vincent and the Grenadines", "VC", "VCT");
INSERT INTO "Countries" VALUES (192, "Samoa", "WS", "WSM");
INSERT INTO "Countries" VALUES (193, "San Marino", "SM", "SMR");
INSERT INTO "Countries" VALUES (194, "Sao Tome and Principe", "ST", "STP");
INSERT INTO "Countries" VALUES (195, "Saudi Arabia", "SA", "SAU");
INSERT INTO "Countries" VALUES (196, "Senegal", "SN", "SEN");
INSERT INTO "Countries" VALUES (197, "Serbia", "RS", "SRB");
INSERT INTO "Countries" VALUES (198, "Seychelles", "SC", "SYC");
INSERT INTO "Countries" VALUES (199, "Sierra Leone", "SL", "SLE");
INSERT INTO "Countries" VALUES (200, "Singapore", "SG", "SGP");
INSERT INTO "Countries" VALUES (201, "Sint Maarten (Dutch part)", "SX", "SXM");
INSERT INTO "Countries" VALUES (202, "Slovakia", "SK", "SVK");
INSERT INTO "Countries" VALUES (203, "Slovenia", "SI", "SVN");
INSERT INTO "Countries" VALUES (204, "Solomon Islands", "SB", "SLB");
INSERT INTO "Countries" VALUES (205, "Somalia", "SO", "SOM");
INSERT INTO "Countries" VALUES (206, "South Africa", "ZA", "ZAF");
INSERT INTO "Countries" VALUES (207, "South Georgia and the South Sandwich Islands", "GS", "SGS");
INSERT INTO "Countries" VALUES (208, "South Sudan", "SS", "SSD");
INSERT INTO "Countries" VALUES (209, "Spain", "ES", "ESP");
INSERT INTO "Countries" VALUES (210, "Sri Lanka", "LK", "LKA");
INSERT INTO "Countries" VALUES (211, "Sudan", "SD", "SDN");
INSERT INTO "Countries" VALUES (212, "Suriname", "SR", "SUR");
INSERT INTO "Countries" VALUES (213, "Svalbard and Jan Mayen", "SJ", "SJM");
INSERT INTO "Countries" VALUES (214, "Swaziland", "SZ", "SWZ");
INSERT INTO "Countries" VALUES (215, "Sweden", "SE", "SWE");
INSERT INTO "Countries" VALUES (216, "Switzerland", "CH", "CHE");
INSERT INTO "Countries" VALUES (217, "Syrian Arab Republic", "SY", "SYR");
INSERT INTO "Countries" VALUES (218, "Taiwan (Province of China)", "TW", "TWN");
INSERT INTO "Countries" VALUES (219, "Tajikistan", "TJ", "TJK");
INSERT INTO "Countries" VALUES (220, "Tanzania, United Republic of", "TZ", "TZA");
INSERT INTO "Countries" VALUES (221, "Thailand", "TH", "THA");
INSERT INTO "Countries" VALUES (222, "Timor-Leste", "TL", "TLS");
INSERT INTO "Countries" VALUES (223, "Togo", "TG", "TGO");
INSERT INTO "Countries" VALUES (224, "Tokelau", "TK", "TKL");
INSERT INTO "Countries" VALUES (225, "Tonga", "TO", "TON");
INSERT INTO "Countries" VALUES (226, "Trinidad and Tobago", "TT", "TTO");
INSERT INTO "Countries" VALUES (227, "Tunisia", "TN", "TUN");
INSERT INTO "Countries" VALUES (228, "Turkey", "TR", "TUR");
INSERT INTO "Countries" VALUES (229, "Turkmenistan", "TM", "TKM");
INSERT INTO "Countries" VALUES (230, "Turks and Caicos Islands", "TC", "TCA");
INSERT INTO "Countries" VALUES (231, "Tuvalu", "TV", "TUV");
INSERT INTO "Countries" VALUES (232, "Uganda", "UG", "UGA");
INSERT INTO "Countries" VALUES (233, "Ukraine", "UA", "UKR");
INSERT INTO "Countries" VALUES (234, "United Arab Emirates", "AE", "ARE");
INSERT INTO "Countries" VALUES (235, "United Kingdom", "GB", "GBR");
INSERT INTO "Countries" VALUES (236, "United States", "US", "USA");
INSERT INTO "Countries" VALUES (237, "United States Minor Outlying Islands", "UM", "UMI");
INSERT INTO "Countries" VALUES (238, "Uruguay", "UY", "URY");
INSERT INTO "Countries" VALUES (239, "Uzbekistan", "UZ", "UZB");
INSERT INTO "Countries" VALUES (240, "Vanuatu", "VU", "VUT");
INSERT INTO "Countries" VALUES (241, "Venezuela, Bolivarian Republic of ", "VE", "VEN");
INSERT INTO "Countries" VALUES (242, "Viet Nam", "VN", "VNM");
INSERT INTO "Countries" VALUES (243, "Virgin Islands (British)", "VG", "VGB");
INSERT INTO "Countries" VALUES (244, "Virgin Islands (U.S.)", "VI", "VIR");
INSERT INTO "Countries" VALUES (245, "Wallis and Futuna", "WF", "WLF");
INSERT INTO "Countries" VALUES (246, "Western Sahara*", "EH", "ESH");
INSERT INTO "Countries" VALUES (247, "Yemen", "YE", "YEM");
INSERT INTO "Countries" VALUES (248, "Zambia", "ZM", "ZMB");
INSERT INTO "Countries" VALUES (249, "Zimbabwe", "ZW", "ZWE");

-- ----------------------------
-- Data for 'Countries'
-- ----------------------------
INSERT INTO "Languages" VALUES (9, "Afrikaans", "af", "afr");
INSERT INTO "Languages" VALUES (21, "Arabic", "ar", "ara");
INSERT INTO "Languages" VALUES (23, "Armenian", "hy", "arm");
INSERT INTO "Languages" VALUES (36, "Azerbaijani", "az", "aze");
INSERT INTO "Languages" VALUES (47, "Belarusian", "be", "bel");
INSERT INTO "Languages" VALUES (49, "Bengali", "bn", "ben");
INSERT INTO "Languages" VALUES (58, "Bosnian", "bs", "bos");
INSERT INTO "Languages" VALUES (64, "Bulgarian", "bg", "bul");
INSERT INTO "Languages" VALUES (70, "Catalan, Valencian", "ca", "cat");
INSERT INTO "Languages" VALUES (74, "Czech", "cs", "cze");
INSERT INTO "Languages" VALUES (79, "Chinese", "zh", "chi");
INSERT INTO "Languages" VALUES (98, "Danish", "da", "dan");
INSERT INTO "Languages" VALUES (111, "Dutch", "nl", "dut");
INSERT INTO "Languages" VALUES (118, "English", "en", "eng");
INSERT INTO "Languages" VALUES (120, "Estonian", "et", "est");
INSERT INTO "Languages" VALUES (128, "Finnish", "fi", "fin");
INSERT INTO "Languages" VALUES (131, "French", "fr", "fre");
INSERT INTO "Languages" VALUES (141, "Georgian", "ka", "geo");
INSERT INTO "Languages" VALUES (142, "German", "de", "ger");
INSERT INTO "Languages" VALUES (146, "Irish", "ga", "gle");
INSERT INTO "Languages" VALUES (153, "Guarani", "gn", "grn");
INSERT INTO "Languages" VALUES (161, "Hebrew", "he", "heb");
INSERT INTO "Languages" VALUES (165, "Hindi", "hi", "hin");
INSERT INTO "Languages" VALUES (169, "Croatian", "hr", "hrv");
INSERT INTO "Languages" VALUES (171, "Hungarian", "hu", "hun");
INSERT INTO "Languages" VALUES (175, "Icelandic", "is", "ice");
INSERT INTO "Languages" VALUES (184, "Indonesian", "id", "ind");
INSERT INTO "Languages" VALUES (190, "Italian", "it", "ita");
INSERT INTO "Languages" VALUES (193, "Japanese", "ja", "jpn");
INSERT INTO "Languages" VALUES (206, "Kazakh", "kk", "kaz");
INSERT INTO "Languages" VALUES (210, "Khmer", "km", "khm");
INSERT INTO "Languages" VALUES (214, "Kirghiz", "ky", "kir");
INSERT INTO "Languages" VALUES (219, "Korean", "ko", "kor");
INSERT INTO "Languages" VALUES (233, "Lao", "lo", "lao");
INSERT INTO "Languages" VALUES (235, "Latvian", "lv", "lav");
INSERT INTO "Languages" VALUES (239, "Lithuanian", "lt", "lit");
INSERT INTO "Languages" VALUES (242, "Luxembourgish", "lb", "ltz");
INSERT INTO "Languages" VALUES (250, "Macedonian", "mk", "mac");
INSERT INTO "Languages" VALUES (258, "Maori", "mi", "mao");
INSERT INTO "Languages" VALUES (262, "Malay", "ms", "may");
INSERT INTO "Languages" VALUES (271, "Maltese", "mt", "mlt");
INSERT INTO "Languages" VALUES (276, "Mongolian", "mn", "mon");
INSERT INTO "Languages" VALUES (294, "Nepali", "ne", "nep");
INSERT INTO "Languages" VALUES (303, "Norwegian", "no", "nor");
INSERT INTO "Languages" VALUES (328, "Persian", "fa", "per");
INSERT INTO "Languages" VALUES (332, "Polish", "pl", "pol");
INSERT INTO "Languages" VALUES (334, "Portuguese", "pt", "por");
INSERT INTO "Languages" VALUES (342, "Romansh", "rm", "roh");
INSERT INTO "Languages" VALUES (344, "Romanian", "ro", "rum");
INSERT INTO "Languages" VALUES (347, "Russian", "ru", "rus");
INSERT INTO "Languages" VALUES (365, "Sinhala", "si", "sin");
INSERT INTO "Languages" VALUES (369, "Slovak", "sk", "slo");
INSERT INTO "Languages" VALUES (370, "Slovenian", "sl", "slv");
INSERT INTO "Languages" VALUES (385, "Spanish", "es", "spa");
INSERT INTO "Languages" VALUES (388, "Serbian", "sr", "srp");
INSERT INTO "Languages" VALUES (397, "Swedish", "sv", "swe");
INSERT INTO "Languages" VALUES (407, "Tetum", null, "tet");
INSERT INTO "Languages" VALUES (410, "Thai", "th", "tha");
INSERT INTO "Languages" VALUES (425, "Turkmen", "tk", "tuk");
INSERT INTO "Languages" VALUES (428, "Turkish", "tr", "tur");
INSERT INTO "Languages" VALUES (436, "Ukrainian", "uk", "ukr");
INSERT INTO "Languages" VALUES (439, "Urdu", "ur", "urd");
INSERT INTO "Languages" VALUES (440, "Uzbek", "uz", "uzb");
INSERT INTO "Languages" VALUES (443, "Vietnamese", "vi", "vie");
