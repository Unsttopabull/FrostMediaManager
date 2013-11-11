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
        Name,
        Text,
        Measure,
        Options,
        NameText,
        MeasureText,
        Info,
        HowTo
    }

    public enum InfoOptions {
        ShowInInform,
        Support,
        ShowInSupported,
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

    //public enum ShowFiles {
    //    Nothing,
    //    VideoAudio,
    //    VideoOnly,
    //    AudioOnly,
    //    TextOnly
    //}
}

//NameSpace
