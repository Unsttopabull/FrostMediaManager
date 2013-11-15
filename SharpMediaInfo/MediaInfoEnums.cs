/*  Copyright (c) MediaArea.net SARL. All Rights Reserved.
 *
 *  Use of this source code is governed by a BSD-style license that can
 *  be found in the License.html file in the root of the source tree.
 */

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//
// Microsoft Visual C# wrapper for MediaInfo Library
// See MediaInfo.h for help
//
// To make it working, you must put MediaInfo.Dll
// in the executable folder
//
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++



#pragma warning disable 1591 // Disable XML documentation warnings

namespace Frost.SharpMediaInfo {

    public enum StreamKind {
        General,
        Video,
        Audio,
        Text,
        Other,
        Image,
        Menu,
    }

    public enum InfoKind {
        /// <summary>Unique name of parameter.</summary>
        Name,
        /// <summary>Value of parameter.</summary>
        Text,
        /// <summary>Unique name of measure unit of parameter.</summary>
        Measure,
        /// <summary>See <see cref="InfoOptions"/>.</summary>
        Options,
        /// <summary>Translated name of parameter.</summary>
        NameText,
        /// <summary>Translated name of measure unit.</summary>
        MeasureText,
        /// <summary>More information about the parameter.</summary>
        Info,
        /// <summary>How this parameter is supported, could be N (No), B (Beta), R (Read only), W (Read/Write)</summary>
        HowTo,
        /// <summary>Domain of this piece of information.</summary>
        Domain
    }

    public enum InfoOptions {
        /// <summary>Show this parameter in Inform()</summary>
        ShowInInform,
        Support,
        /// <summary>Internal use only (info : Must be showed in Info_Capacities() )</summary>
        ShowInSupported,
        /// <summary>Value return by a standard Get() can be : T (Text), I (Integer, warning up to 64 bits), F (Float), D (Date), B (Binary datas coded Base64) (Numbers are in Base 10).</summary>
        TypeOfValue
    }

    public enum InfoFileOptions {
        Nothing = 0x00,
        NoRecursive = 0x01,
        CloseAll = 0x02,
        Max = 0x04
    };

    public enum BlockMethod {
        Immediately,
        AfterLocalInfo
    }

    public enum Demux {
        All,
        Frame,
        Container,
        Elementary
    }

    public enum TraceFormat {
        CSV,
        Tree
    }

    public enum InformPreset {
        Text,
        HTML,
        XML,
        PBCore,
        ReVTMD,
        Mpeg7
    }

    public enum ScanType {
        Unknown,
        Progresive,
        Interlaced,
        MBAFF,
        Mixed
    }

    public enum BitOrFrameRateMode {
        Unknown,
        Constant,
        Variable
    }

    public enum CompressionMode {
        Unknown,
        Lossy,
        Lossless,
    }

    public enum AudioAlignment {
        Unknown,
        Split,
        Aligned
    }

    //public enum ShowFiles {
    //    Nothing,
    //    VideoAudio,
    //    VideoOnly,
    //    AudioOnly,
    //    TextOnly
    //}
}

//NameSpace
