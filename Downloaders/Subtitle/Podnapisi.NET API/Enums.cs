using System.ComponentModel;

namespace Frost.PodnapisiNET {

    public enum StatusCode {
        Ok = 200,
        InvalidCredentials = 300,
        NoAuthorisation = 301,
        InvalidSession = 302,
        MovieNotFound = 400,
        InvalidFormat = 401,
        InvalidLanguage = 402,
        InvalidHash = 403,
        InvalidArchive = 404,
    }

    public enum Formats {
        MicroDVD,
        SAMI,
        SSA,
        SubRip,

        [Description("SubViewer 2.0")]
        SubViewer2,
        SubViewer,
        MPSub
    }

    // ReSharper disable InconsistentNaming
    public enum Fps {
        [Description("N/A")]
        NA,

        [Description("Timecode (PAL)")]
        PAL,

        [Description("23,976")]
        FPS23_976,

        [Description("23,97")]
        FPS23_97,

        [Description("25")]
        FPS25,

        [Description("29,97")]
        FPS29_97,

        [Description("Timecode (NTSC)")]
        NTSC
    }

    // ReSharper restore InconsistentNaming

    public enum LanguageId {
        Albanščina = 29,
        English = 02,
        Arabic = 12,
        SpanishArgentinian = 14,
        Belarus = 50,
        Bengali = 59,
        Bolgarian = 33,
        Bosanščina = 10,
        PortugeseBrazilian = 48,
        Danish = 24,
        Estonish = 20,
        Farsi = 52,
        Finish = 31,
        French = 8,
        Glenlandish = 57,
        Greek = 16,
        Hebrew = 22,
        Hindi = 42,
        Croatian = 38,
        Indonesian = 54,
        Irlandese = 49,
        Islandic = 6,
        Italian = 9,
        Japanese = 11,
        Catalon = 53,
        Kazakh = 58,
        Chinese = 17,
        Korean = 04,
        Latvian = 21,
        Lithuanian = 19,
        Hungarian = 15,
        Macedonian = 35,
        Malay = 55,
        Mandarin = 40,
        German = 05,
        Dutch = 23,
        Norwegian = 03,
        Polish = 26,
        Portugese = 32,
        Romanian = 13,
        Russian = 27,
        Singalese = 56,
        Slovak = 37,
        Slovene = 1,
        SerbianCyrilic = 47,
        Serbian = 36,
        Thai = 44,
        Turkish = 30,
        Ukrainian = 46,
        Vietnamese = 51,
        Czech = 07,
        Spanish = 28,
        Swedish = 25,
    }

}