using System.Collections.Generic;
using Frost.Common;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;
using Frost.Common.Models.Provider.ISO;
using RibbonUI.Design.Models;

namespace RibbonUI.Design {

    public class DesignMoviesDataService : IMoviesDataService {
        private IEnumerable<IMovie> _movies;
        private IEnumerable<IFile> _files;
        private IEnumerable<IVideo> _videos;
        private IEnumerable<IAudio> _audios;
        private IEnumerable<ISubtitle> _subtitles;
        private IEnumerable<IArt> _art;
        private IEnumerable<ICountry> _countries;
        private IEnumerable<IStudio> _studios;
        private IEnumerable<IRating> _ratings;
        private IEnumerable<IPlot> _plots;
        private IEnumerable<IGenre> _genres;
        private IEnumerable<IAward> _awards;
        private IEnumerable<IPromotionalVideo> _promotionalVideos;
        private IEnumerable<ICertification> _certifications;
        private IEnumerable<IMovieSet> _sets;
        private IEnumerable<ILanguage> _languages;
        private IEnumerable<ISpecial> _specials;
        private IEnumerable<IPerson> _people;
        private IEnumerable<IActor> _actors;

        public DesignMoviesDataService() {
        }

        #region Movies

        public IEnumerable<IMovie> Movies {
            get {
                if (_movies != null) {
                    return _movies;
                }

                _movies = GetMovies();

                return _movies;
            }
        }

        private static IEnumerable<IMovie> GetMovies() {
            List<IMovie> movies = new List<IMovie>();

            #region Movie 1

            DesignMovie movie = new DesignMovie {
                Title = "Amazing Grace",
                OriginalTitle = null,
                SortTitle = null,
                Type = MovieType.Unknown,
                Goofs = null,
                Trivia = null,
                ReleaseYear = 2006,
                ReleaseDate = null,
                Edithion = null,
                DvdRegion = DVDRegion.Unknown,
                LastPlayed = null,
                Premiered = null,
                Aired = null,
                Trailer = null,
                Top250 = null,
                Runtime = 6779001,
                Watched = false,
                PlayCount = 0,
                RatingAverage = null,
                ImdbID = null,
                TmdbID = null,
                ReleaseGroup = null,
                IsMultipart = false,
                PartTypes = null,
                DirectoryPath = @"E:\Torrenti\FILMI\Amazing.Grace.2006.SLOSub.DvdRip.Xvid",
                NumberOfAudioChannels = 2,
                AudioCodec = "MP3",
                VideoResolution = null,
                VideoCodec = "mpeg4",
                HasTrailer = false,
                HasSubtitles = true,
                HasArt = false,
                HasNfo = true,
            };

            movie.AddSubtitle(new DesignSubtitle {
                Format = "SubRip",
                Encoding = "Windows-1252",
                EmbededInVideo = false,
                ForHearingImpaired = false,
            });

            movie.AddVideo(new DesignVideo {
                MovieHash = "2d6d9416661f72d8",
                Source = "DvdRip",
                Type = null,
                Resolution = null,
                ResolutionName = null,
                Standard = null,
                FPS = 25,
                BitRate = 728.2803f,
                BitRateMode = FrameOrBitRateMode.Unknown,
                BitDepth = 8,
                CompressionMode = CompressionMode.Lossy,
                Duration = 6779001,
                ScanType = ScanType.Progressive,
                ColorSpace = "YUV",
                ChromaSubsampling = "4:2:0",
                Format = "MPEG-4 Visual",
                Codec = "DivX 5",
                CodecId = "mpeg4",
                Aspect = 1.8400000333786,
                AspectCommercialName = "Cinema film",
                Width = 600,
                Height = 326,
                Language = null,
            });

            movie.AddAudio(new DesignAudio {
                Source = null,
                Type = null,
                ChannelSetup = null,
                NumberOfChannels = 2,
                ChannelPositions = null,
                Codec = "MPEG-1 Audio layer 3",
                CodecId = "MP3",
                BitRate = 109.375f,
                BitRateMode = FrameOrBitRateMode.Unknown,
                SamplingRate = 46,
                BitDepth = null,
                CompressionMode = CompressionMode.Lossy,
                Duration = 6778992,
                Language = null,
            });
            movies.Add(movie);

            #endregion

            #region Movie 2

            movie = new DesignMovie {
                Id = 2,
                Title = "North Country",
                OriginalTitle = null,
                SortTitle = null,
                Type = MovieType.Unknown,
                Goofs = null,
                Trivia = null,
                ReleaseYear = 2005,
                ReleaseDate = null,
                Edithion = null,
                DvdRegion = DVDRegion.Unknown,
                LastPlayed = null,
                Premiered = null,
                Aired = null,
                Trailer = null,
                Top250 = null,
                Runtime = 7267120,
                Watched = false,
                PlayCount = 0,
                RatingAverage = null,
                ImdbID = null,
                TmdbID = null,
                ReleaseGroup = "messi1",
                IsMultipart = false,
                PartTypes = null,
                DirectoryPath = @"E:\Torrenti\FILMI\North.Country.2005.SLOSubs.DVDRip.XviD-messi1",
                NumberOfAudioChannels = 6,
                AudioCodec = "AC3",
                VideoResolution = null,
                VideoCodec = "Xvid",
                Set = null,
                HasTrailer = false,
                HasSubtitles = true,
                HasArt = false,
                HasNfo = true,
            };

            movie.AddSubtitle(new DesignSubtitle {
                PodnapisiId = null,
                OpenSubtitlesId = null,
                MD5 = null,
                Format = "SubRip",
                Encoding = "Windows-1252",
                EmbededInVideo = false,
                ForHearingImpaired = false,
                Language = null,
            });

            movie.AddVideo(new DesignVideo {
                MovieHash = "3baa73642ad1eed6",
                Source = "DVDRip",
                Type = null,
                Resolution = null,
                ResolutionName = null,
                Standard = null,
                FPS = 25,
                BitRate = 1874.945f,
                BitRateMode = FrameOrBitRateMode.Unknown,
                BitDepth = 8,
                CompressionMode = CompressionMode.Lossy,
                Duration = 7267120,
                ScanType = ScanType.Progressive,
                ColorSpace = "YUV",
                ChromaSubsampling = "4:2:0",
                Format = "MPEG-4 Visual",
                Codec = "XviD",
                CodecId = "Xvid",
                Aspect = 1.875,
                AspectCommercialName = "Cinema film",
                Width = 720,
                Height = 384,
                Language = null,
            });

            movie.AddAudio(new DesignAudio {
                Source = null,
                Type = null,
                ChannelSetup = "3/2/0.1",
                NumberOfChannels = 6,
                ChannelPositions = "Front: L C R, Side: L R, LFE",
                Codec = "AC3",
                CodecId = "AC3",
                BitRate = 375,
                BitRateMode = FrameOrBitRateMode.Unknown,
                SamplingRate = 46,
                BitDepth = 16,
                CompressionMode = CompressionMode.Lossy,
                Duration = 7267058,
                Language = null,
            });
            movies.Add(movie);

            #endregion

            #region Movie 3

            movie = new DesignMovie {
                Title = "Playing By Heart",
                OriginalTitle = null,
                SortTitle = null,
                Type = MovieType.Unknown,
                Goofs = null,
                Trivia = null,
                ReleaseYear = 1998,
                ReleaseDate = null,
                Edithion = null,
                DvdRegion = DVDRegion.Unknown,
                LastPlayed = null,
                Premiered = null,
                Aired = null,
                Trailer = null,
                Top250 = null,
                Runtime = 6655560,
                Watched = false,
                PlayCount = 0,
                RatingAverage = null,
                ImdbID = null,
                TmdbID = null,
                ReleaseGroup = null,
                IsMultipart = false,
                PartTypes = null,
                DirectoryPath = @"E:\Torrenti\FILMI\Playing by Heart 1998",
                NumberOfAudioChannels = 2,
                AudioCodec = "MP3",
                VideoResolution = null,
                VideoCodec = "H264",
                Set = null,
                HasTrailer = false,
                HasSubtitles = false,
                HasArt = false,
                HasNfo = true,
            };
            movie.AddVideo(new DesignVideo {
                MovieHash = "a7197d681fdafc0",
                Source = null,
                Type = null,
                Resolution = null,
                ResolutionName = null,
                Standard = null,
                FPS = 25,
                BitRate = 559.543f,
                BitRateMode = FrameOrBitRateMode.Unknown,
                BitDepth = 8,
                CompressionMode = CompressionMode.Unknown,
                Duration = 6655560,
                ScanType = ScanType.Progressive,
                ColorSpace = "YUV",
                ChromaSubsampling = "4:2:0",
                Format = "AVC",
                Codec = "AVC",
                CodecId = "H264",
                Aspect = 1.33299994468689,
                AspectCommercialName = "SDTV (4:3)",
                Width = 704,
                Height = 528,
                Language = null,
            });

            movie.AddAudio(new DesignAudio {
                Source = null,
                Type = null,
                ChannelSetup = null,
                NumberOfChannels = 2,
                ChannelPositions = null,
                Codec = "MPEG-2 Audio layer 3",
                CodecId = "MP3",
                BitRate = 54.6875f,
                BitRateMode = FrameOrBitRateMode.Unknown,
                SamplingRate = 21,
                BitDepth = null,
                CompressionMode = CompressionMode.Lossy,
                Duration = 6655788,
                Language = null,
            });
            movies.Add(movie);

            #endregion

            #region Movie 4

            movie = new DesignMovie {
                Id = 4,
                Title = "Prizzi's Honor",
                OriginalTitle = null,
                SortTitle = null,
                Type = MovieType.Unknown,
                Goofs = null,
                Trivia = null,
                ReleaseYear = 1985,
                ReleaseDate = null,
                Edithion = null,
                DvdRegion = DVDRegion.Unknown,
                LastPlayed = null,
                Premiered = null,
                Aired = null,
                Trailer = null,
                Top250 = null,
                Runtime = 7401480,
                Watched = false,
                PlayCount = 0,
                RatingAverage = null,
                ImdbID = null,
                TmdbID = null,
                ReleaseGroup = null,
                IsMultipart = false,
                PartTypes = null,
                DirectoryPath = @"E:\Torrenti\FILMI\Prizzi's Honor 1985",
                NumberOfAudioChannels = 2,
                AudioCodec = "MP3",
                VideoResolution = null,
                VideoCodec = "H264",
                Set = null,
                HasTrailer = false,
                HasSubtitles = false,
                HasArt = false,
                HasNfo = true,
            };
            movie.AddVideo(new DesignVideo {
                MovieHash = "a7713ff462a8a14",
                Source = null,
                Type = null,
                Resolution = null,
                ResolutionName = null,
                Standard = null,
                FPS = 25,
                BitRate = 1517.729f,
                BitRateMode = FrameOrBitRateMode.Unknown,
                BitDepth = 8,
                CompressionMode = CompressionMode.Unknown,
                Duration = 7401480,
                ScanType = ScanType.Progressive,
                ColorSpace = "YUV",
                ChromaSubsampling = "4:2:0",
                Format = "AVC",
                Codec = "AVC",
                CodecId = "H264",
                Aspect = 1.77799999713898,
                AspectCommercialName = "HDTV (16:9)",
                Width = 1024,
                Height = 576,
                Language = null,
            });

            movie.AddAudio(new DesignAudio {
                Source = null,
                Type = null,
                ChannelSetup = null,
                NumberOfChannels = 2,
                ChannelPositions = null,
                Codec = "MPEG-2 Audio layer 3",
                CodecId = "MP3",
                BitRate = 54.6875f,
                BitRateMode = FrameOrBitRateMode.Unknown,
                SamplingRate = 21,
                BitDepth = null,
                CompressionMode = CompressionMode.Lossy,
                Duration = 7401656,
                Language = null,
            });

            movies.Add(movie);

            #endregion

            #region Movie 5

            movie = new DesignMovie {
                Id = 5,
                Title = "Road To Perdition",
                OriginalTitle = null,
                SortTitle = null,
                Type = MovieType.Unknown,
                Goofs = null,
                Trivia = null,
                ReleaseYear = 2002,
                ReleaseDate = null,
                Edithion = null,
                DvdRegion = DVDRegion.Unknown,
                LastPlayed = null,
                Premiered = null,
                Aired = null,
                Trailer = null,
                Top250 = null,
                Runtime = 6727720,
                Watched = false,
                PlayCount = 0,
                RatingAverage = null,
                ImdbID = null,
                TmdbID = null,
                ReleaseGroup = "DrSi",
                IsMultipart = false,
                PartTypes = null,
                DirectoryPath = @"E:\Torrenti\FILMI\Road.to.Perdition.2002.SLOSubs.DVDRip.XviD-DrSi",
                NumberOfAudioChannels = 6,
                AudioCodec = "AC3",
                VideoResolution = null,
                VideoCodec = "Xvid",
                Set = null,
                HasTrailer = false,
                HasSubtitles = true,
                HasArt = false,
                HasNfo = true,
            };

            movie.AddSubtitle(
                new DesignSubtitle {
                    PodnapisiId = null,
                    OpenSubtitlesId = null,
                    MD5 = null,
                    Format = "SubRip",
                    Encoding = "Windows-1252",
                    EmbededInVideo = false,
                    ForHearingImpaired = false,
                    Language = null,
                });

            movie.AddVideo(new DesignVideo {
                MovieHash = "114479ecb2f7975e",
                Source = "DVDRip",
                Type = null,
                Resolution = null,
                ResolutionName = null,
                Standard = null,
                FPS = 25,
                BitRate = 1261.126f,
                BitRateMode = FrameOrBitRateMode.Unknown,
                BitDepth = 8,
                CompressionMode = CompressionMode.Lossy,
                Duration = 6727720,
                ScanType = ScanType.Progressive,
                ColorSpace = "YUV",
                ChromaSubsampling = "4:2:0",
                Format = "MPEG-4 Visual",
                Codec = "XviD",
                CodecId = "Xvid",
                Aspect = 2.36800003051758,
                AspectCommercialName = "21:9 Cinema Display",
                Width = 720,
                Height = 304,
                Language = null,
            });

            movie.AddAudio(new DesignAudio {
                Source = null,
                Type = null,
                ChannelSetup = "3/2/0.1",
                NumberOfChannels = 6,
                ChannelPositions = "Front: L C R, Side: L R, LFE",
                Codec = "AC3",
                CodecId = "AC3",
                BitRate = 437.5f,
                BitRateMode = FrameOrBitRateMode.Unknown,
                SamplingRate = 46,
                BitDepth = 16,
                CompressionMode = CompressionMode.Lossy,
                Duration = 6727631,
                Language = null,
            });

            movies.Add(movie);

            #endregion

            return movies;
        }

        #endregion

        public IEnumerable<IFile> Files {
            get {
                if (_files != null) {
                    return _files;
                }

                //Create files
                return _files;
            }
        }

        public IEnumerable<IVideo> Videos {
            get {
                if (_videos != null) {
                    return _videos;
                }

                //Create videos

                return _videos;
            }
        }

        public IEnumerable<IAudio> Audios {
            get {
                if (_audios != null) {
                    return _audios;
                }

                _audios = new List<IAudio> {
                    new DesignAudio {
                        Source = null,
                        Type = null,
                        ChannelSetup = "3/2/0.1",
                        NumberOfChannels = 6,
                        ChannelPositions = "Front: L C R, Side: L R, LFE",
                        Codec = "AC3",
                        CodecId = "AC3",
                        BitRate = 437.5f,
                        BitRateMode = FrameOrBitRateMode.Unknown,
                        SamplingRate = 46,
                        BitDepth = 16,
                        CompressionMode = CompressionMode.Lossy,
                        Duration = 6727631,
                        Language = null,
                    },
                    new DesignAudio {
                        Source = null,
                        Type = null,
                        ChannelSetup = null,
                        NumberOfChannels = 2,
                        ChannelPositions = null,
                        Codec = "MPEG-2 Audio layer 3",
                        CodecId = "MP3",
                        BitRate = 54.6875f,
                        BitRateMode = FrameOrBitRateMode.Unknown,
                        SamplingRate = 21,
                        BitDepth = null,
                        CompressionMode = CompressionMode.Lossy,
                        Duration = 7401656,
                        Language = null,
                    },
                    new DesignAudio {
                        Source = null,
                        Type = null,
                        ChannelSetup = null,
                        NumberOfChannels = 2,
                        ChannelPositions = null,
                        Codec = "MPEG-2 Audio layer 3",
                        CodecId = "MP3",
                        BitRate = 54.6875f,
                        BitRateMode = FrameOrBitRateMode.Unknown,
                        SamplingRate = 21,
                        BitDepth = null,
                        CompressionMode = CompressionMode.Lossy,
                        Duration = 6655788,
                        Language = null,
                    },
                    new DesignAudio {
                        Source = null,
                        Type = null,
                        ChannelSetup = "3/2/0.1",
                        NumberOfChannels = 6,
                        ChannelPositions = "Front: L C R, Side: L R, LFE",
                        Codec = "AC3",
                        CodecId = "AC3",
                        BitRate = 375,
                        BitRateMode = FrameOrBitRateMode.Unknown,
                        SamplingRate = 46,
                        BitDepth = 16,
                        CompressionMode = CompressionMode.Lossy,
                        Duration = 7267058,
                        Language = null,
                    },
                    new DesignAudio {
                        Source = null,
                        Type = null,
                        ChannelSetup = null,
                        NumberOfChannels = 2,
                        ChannelPositions = null,
                        Codec = "MPEG-1 Audio layer 3",
                        CodecId = "MP3",
                        BitRate = 109.375f,
                        BitRateMode = FrameOrBitRateMode.Unknown,
                        SamplingRate = 46,
                        BitDepth = null,
                        CompressionMode = CompressionMode.Lossy,
                        Duration = 6778992,
                        Language = null,
                    }
                };

                return _audios;
            }
        }

        public IEnumerable<ISubtitle> Subtitles {
            get {
                if (_subtitles == null) {
                    //Create videos
                }
                return _subtitles;
            }
        }

        public IEnumerable<IArt> Art {
            get {
                if (_art != null) {
                    return _art;
                }

                //Create videos

                return _art;
            }
        }

        public IEnumerable<ICountry> Countries {
            get {
                if (_countries != null) {
                    return _countries;
                }

                _countries = new List<ICountry> {
                    new DesignCountry { ISO3166 = new ISO3166 { Alpha2 = "us", Alpha3 = "usa" }, Name = "United States" },
                    new DesignCountry { ISO3166 = new ISO3166 { Alpha2 = "br", Alpha3 = "bra" }, Name = "Brazil" },
                    new DesignCountry { ISO3166 = new ISO3166 { Alpha2 = "bg", Alpha3 = "bgr" }, Name = "Bulgaria" },
                    new DesignCountry { ISO3166 = new ISO3166 { Alpha2 = "ca", Alpha3 = "can" }, Name = "Canada" },
                    new DesignCountry { ISO3166 = new ISO3166 { Alpha2 = "fi", Alpha3 = "fin" }, Name = "Finland" },
                    new DesignCountry { ISO3166 = new ISO3166 { Alpha2 = "fr", Alpha3 = "fra" }, Name = "France" },
                    new DesignCountry { ISO3166 = new ISO3166 { Alpha2 = "gb", Alpha3 = "gbr" }, Name = "United Kingdom" }
                };

                return _countries;
            }
        }

        public IEnumerable<IStudio> Studios {
            get {
                if (_studios != null) {
                    return _studios;
                }

                _studios = new List<IStudio> {
                    new DesignStudio { Name = "20th Century Fox Home entertainment" },
                    new DesignStudio { Name = "A&E" },
                    new DesignStudio { Name = "Animal Planet" },
                    new DesignStudio { Name = "Archlight Films" },
                    new DesignStudio { Name = "Castle Rock" },
                    new DesignStudio { Name = "Cinemax" },
                    new DesignStudio { Name = "Columbia" },
                };

                return _studios;
            }
        }

        public IEnumerable<IRating> Ratings {
            get {
                if (_ratings != null) {
                    return _ratings;
                }

                //Create videos

                return _ratings;
            }
        }

        public IEnumerable<IPlot> Plots {
            get {
                if (_plots != null) {
                    return _plots;
                }

                //Create videos

                return _plots;
            }
        }

        public IEnumerable<IGenre> Genres {
            get {
                if (_genres != null) {
                    return _genres;
                }

                _genres = new List<IGenre> {
                    new DesignGenre { Name = "Action" },
                    new DesignGenre { Name = "Adventure" },
                    new DesignGenre { Name = "Science Fiction" },
                    new DesignGenre { Name = "Thriller" },
                    new DesignGenre { Name = "Drama" },
                    new DesignGenre { Name = "Mystery" },
                    new DesignGenre { Name = "Romance" },
                    new DesignGenre { Name = "Comedy" },
                    new DesignGenre { Name = "Foreign" },
                    new DesignGenre { Name = "Fantasy" },
                    new DesignGenre { Name = "Family" },
                    new DesignGenre { Name = "Animation" },
                };

                return _genres;
            }
        }

        public IEnumerable<IAward> Awards {
            get {
                if (_awards != null) {
                    return _awards;
                }

                //Create videos

                return _awards;
            }
        }

        public IEnumerable<IPromotionalVideo> PromotionalVideos {
            get {
                if (_promotionalVideos != null) {
                    return _promotionalVideos;
                }

                //Create videos

                return _promotionalVideos;
            }
        }

        public IEnumerable<ICertification> Certifications {
            get {
                if (_certifications != null) {
                    return _certifications;
                }

                return new List<ICertification> {
                    new DesignCertification {
                        Country = new DesignCountry {
                            ISO3166 = new ISO3166("us", "usa"),
                            Name = "United States"
                        },
                        Rating = "R"
                    },
                    new DesignCertification {
                        Country = new DesignCountry {
                            ISO3166 = new ISO3166("gb", "gbr"),
                            Name = "United Kingdom"
                        },
                        Rating = "15"
                    },
                    new DesignCertification {
                        Country = new DesignCountry {
                            ISO3166 = new ISO3166("de", "deu"),
                            Name = "Germany"
                        },
                        Rating = "16"
                    }
                };
            }
        }

        public IEnumerable<IMovieSet> Sets {
            get {
                if (_sets != null) {
                    return _sets;
                }

                _sets = new List<IMovieSet> {
                    new DesignSet { Name = "G.I. Joe (Live-Action Series)" },
                    new DesignSet { Name = "Kick-Ass Collection" },
                    new DesignSet { Name = "The Hunger Games Collection" },
                    new DesignSet { Name = "Pacific Rim Collection" },
                    new DesignSet { Name = "The Hobbit Collection" },
                };

                return _sets;
            }
        }

        public IEnumerable<ILanguage> Languages {
            get {
                if (_languages != null) {
                    return _languages;
                }

                _languages = new List<ILanguage> {
                    new DesignLanguage { ISO639 = new ISO639 { Alpha2 = "en", Alpha3 = "eng" }, Name = "English" },
                    new DesignLanguage { ISO639 = new ISO639 { Alpha2 = "fi", Alpha3 = "fin" }, Name = "Finnish" },
                    new DesignLanguage { ISO639 = new ISO639 { Alpha2 = "bg", Alpha3 = "bgr" }, Name = "Bulgaria" },
                    new DesignLanguage { ISO639 = new ISO639 { Alpha2 = "hr", Alpha3 = "hrv" }, Name = "Croatian" },
                    new DesignLanguage { ISO639 = new ISO639 { Alpha2 = "fi", Alpha3 = "fin" }, Name = "Finland" },
                    new DesignLanguage { ISO639 = new ISO639 { Alpha2 = "fr", Alpha3 = "fre" }, Name = "French" },
                    new DesignLanguage { ISO639 = new ISO639 { Alpha2 = "sl", Alpha3 = "slv" }, Name = "Slovene" }
                };

                return _languages;
            }
        }

        public IEnumerable<ISpecial> Specials {
            get {
                if (_specials != null) {
                    return _specials;
                }

                return _specials;
            }
        }

        public IEnumerable<IPerson> People {
            get {
                if (_people != null) {
                    return _people;
                }

                _people = new List<IPerson> {
                    new DesignPerson { ImdbID = "nm0603628", Name = "Pierre Morel" },
                    new DesignPerson { ImdbID = "nm0603629", Name = "John Luessenhop" },
                    new DesignPerson { ImdbID = "nm0603630", Name = "Nathan Greno" },
                    new DesignPerson { ImdbID = "nm0603631", Name = "Steven Spielberg" },
                    new DesignPerson { ImdbID = "nm0603632", Name = "Roger Donaldson" },
                    new DesignPerson { ImdbID = "nm0603633", Name = "John Lee Hancock" },
                    new DesignPerson { ImdbID = "nm0603634", Name = "Rian Johnson" },
                    new DesignPerson { ImdbID = "nm0603635", Name = "Zach Braff", Thumb = "http://cf2.imgobject.com/t/p/original/zCAwpjYyWnbYKmMBxd0qCEZWyuY.jpg" },
                    new DesignPerson { ImdbID = "nm0603636", Name = "John Cho", Thumb = "http://cf2.imgobject.com/t/p/original/wlA5pkKGun8BS7GjCqXrzthTOk4.jpg" },
                    new DesignPerson { ImdbID = "nm0603636", Name = "Colin Farrell", Thumb = "http://cf2.imgobject.com/t/p/original/tFMWlEeZNmgG8WsWgPm4Mjd6Tgc.jpg" },
                };

                return _people;
            }
        }

        public IEnumerable<IActor> Actors {
            get {
                if (_actors != null) {
                    return _actors;
                }

                //Create Actors

                return _actors;
            }
        }

        public void SaveDetected(MovieInfo movieInfo) {
            
        }

        public bool HasUnsavedChanges() {
            return false;
        }

        public void SaveChanges() {
        }

        public void Dispose() {
        }
    }

}