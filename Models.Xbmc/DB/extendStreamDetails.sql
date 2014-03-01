BEGIN TRANSACTION;

CREATE TEMPORARY TABLE 
streamdetails_backup (
	idFile integer,
	iStreamType integer,
	strVideoCodec text,
	fVideoAspect float,
	iVideoWidth integer,
	iVideoHeight integer,
	strAudioCodec text,
	iAudioChannels integer,
	strAudioLanguage text,
	strSubtitleLanguage text,
	iVideoDuration integer
);

INSERT INTO streamdetails_backup SELECT * FROM streamdetails;
DROP TABLE streamdetails;

CREATE TABLE
streamdetails (
	idStream integer primary key not null,
	idFile integer,
	iStreamType integer,
	strVideoCodec text,
	fVideoAspect float,
	iVideoWidth integer,
	iVideoHeight integer,
	strAudioCodec text,
	iAudioChannels integer,
	strAudioLanguage text,
	strSubtitleLanguage text,
	iVideoDuration integer
);

INSERT INTO streamdetails(idFile, iStreamType, strVideoCodec, fVideoAspect, iVideoWidth, iVideoHeight, strAudioCodec, iAudioChannels, strAudioLanguage, strSubtitleLanguage, iVideoDuration) SELECT * FROM streamdetails_backup;
DROP TABLE streamdetails_backup;

COMMIT;