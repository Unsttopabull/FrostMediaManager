BEGIN TRANSACTION;

CREATE TEMPORARY TABLE 
settings_backup (
	idFile INTEGER,
	Deinterlace BOOLEAN,
	ViewMode INTEGER,
	ZoomAmount FLOAT,
	PixelRatio FLOAT,
	VerticalShift FLOAT,
	AudioStream INTEGER,
	SubtitleStream INTEGER,
	SubtitleDelay FLOAT,
	SubtitlesOn BOOLEAN,
	Brightness FLOAT,
	Contrast FLOAT,
	Gamma FLOAT,
	VolumeAmplification FLOAT,
	AudioDelay FLOAT,
	OutputToAllSpeakers BOOLEAN,
	ResumeTime INTEGER,
	Crop BOOLEAN,
	CropLeft INTEGER,
	CropRight INTEGER,
	CropTop INTEGER,
	CropBottom INTEGER,
	Sharpness FLOAT,
	NoiseReduction FLOAT,
	NonLinStretch BOOLEAN,
	PostProcess BOOLEAN,
	ScalingMethod INTEGER,
	DeinterlaceMode INTEGER
 );


INSERT INTO settings_backup SELECT * FROM settings;
DROP TABLE settings;

CREATE TABLE
settings (
	idSetting INTEGER primary key NOT NULL,
	idFile INTEGER,
	Deinterlace BOOLEAN,
	ViewMode INTEGER,
	ZoomAmount FLOAT,
	PixelRatio FLOAT,
	VerticalShift FLOAT,
	AudioStream INTEGER,
	SubtitleStream INTEGER,
	SubtitleDelay FLOAT,
	SubtitlesOn BOOLEAN,
	Brightness FLOAT,
	Contrast FLOAT,
	Gamma FLOAT,
	VolumeAmplification FLOAT,
	AudioDelay FLOAT,
	OutputToAllSpeakers BOOLEAN,
	ResumeTime INTEGER,
	Crop BOOLEAN,
	CropLeft INTEGER,
	CropRight INTEGER,
	CropTop INTEGER,
	CropBottom INTEGER,
	Sharpness FLOAT,
	NoiseReduction FLOAT,
	NonLinStretch BOOLEAN,
	PostProcess BOOLEAN,
	ScalingMethod INTEGER,
	DeinterlaceMode INTEGER
);

INSERT INTO settings( idFile, Deinterlace,ViewMode,ZoomAmount, PixelRatio, VerticalShift, AudioStream, SubtitleStream, SubtitleDelay, SubtitlesOn, Brightness, Contrast, Gamma, VolumeAmplification, AudioDelay, OutputToAllSpeakers, ResumeTime, Crop, CropLeft, CropRight, CropTop, CropBottom, Sharpness, NoiseReduction, NonLinStretch, PostProcess, ScalingMethod, DeinterlaceMode) SELECT * FROM settings_backup;
DROP TABLE settings_backup;

COMMIT;