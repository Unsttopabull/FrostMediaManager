using System;
using System.Collections;
using System.Collections.Generic;
using Frost.DetectFeatures.Util.AspectRatio;

namespace Frost.DetectFeatures.Util {

    //list of aspect ratios adapted from https://code.google.com/p/moviejukebox/source/browse/trunk/moviejukebox/yamj/src/main/java/com/moviejukebox/tools/AspectRatioTools.java

    public static class AspectRatioDetector{
        private static readonly AspectRatioInfo[] KnownAspectRatios;
        private static readonly AspectRatioComparer _aspectRatioComparer = new AspectRatioComparer();

        static AspectRatioDetector() {
            //sorted array of Aspect ratios from lowest minimal Aspect ratio to the highest minimal Aspect ratio
            KnownAspectRatios = new[] {
                new AspectRatioInfo(@"1:01", 1.000f, 1.075f, 1.0f),
                new AspectRatioInfo(@"Movietone", 1.076f, 1.200f, 1.2f),
                new AspectRatioInfo(@"SVGA", 1.201f, 1.292f, 1.3f),
                new AspectRatioInfo(@"SDTV (4:3)", 1.293f, 1.352f, 1.3f),
                new AspectRatioInfo(@"Academy Ratio", 1.353f, 1.400f, 1.4f),
                new AspectRatioInfo(@"IMAX", 1.401f, 1.465f, 1.4f),
                new AspectRatioInfo(@"VistaVision", 1.466f, 1.510f, 1.5f),
                new AspectRatioInfo(@"Demeny Gaumont", 1.511f, 1.538f, 1.5f),
                new AspectRatioInfo(@"Widescreen", 1.539f, 1.578f, 1.6f),
                new AspectRatioInfo(@"Widescreen PC", 1.579f, 1.633f, 1.6f),
                new AspectRatioInfo(@"15:09", 1.634f, 1.698f, 1.7f),
                new AspectRatioInfo(@"Natural Vision", 1.699f, 1.740f, 1.7f),
                new AspectRatioInfo(@"Widescreen", 1.741f, 1.764f, 1.8f),
                new AspectRatioInfo(@"HDTV (16:9)", 1.765f, 1.814f, 1.8f),
                new AspectRatioInfo(@"Cinema film", 1.815f, 1.925f, 1.9f),
                new AspectRatioInfo(@"SuperScope Univisium", 1.926f, 2.025f, 2.0f),
                new AspectRatioInfo(@"Magnifilm", 2.026f, 2.075f, 2.1f),
                new AspectRatioInfo(@"11:10", 2.076f, 2.115f, 2.1f),
                new AspectRatioInfo(@"Fox Grandeur", 2.116f, 2.155f, 2.1f),
                new AspectRatioInfo(@"Magnifilm", 2.156f, 2.190f, 2.2f),
                new AspectRatioInfo(@"70mm standard", 2.191f, 2.205f, 2.2f),
                new AspectRatioInfo(@"MPEG-2 for 2.20", 2.206f, 2.272f, 2.2f),
                new AspectRatioInfo(@"21:09", 2.273f, 2.342f, 2.3f),
                new AspectRatioInfo(@"CinemaScope", 2.343f, 2.360f, 2.4f),
                new AspectRatioInfo(@"21:9 Cinema Display", 2.361f, 2.380f, 2.4f),
                new AspectRatioInfo(@"Scope", 2.381f, 2.395f, 2.4f),
                new AspectRatioInfo(@"Scope", 2.396f, 2.460f, 2.4f),
                new AspectRatioInfo(@"Panoramico Alberini", 2.461f, 2.535f, 2.5f),
                new AspectRatioInfo(@"Original CinemaScope", 2.536f, 2.555f, 2.6f),
                new AspectRatioInfo(@"Original CinemaScope", 2.556f, 2.575f, 2.6f),
                new AspectRatioInfo(@"Cinerama full", 2.576f, 2.595f, 2.6f),
                new AspectRatioInfo(@"Cinemiracle", 2.596f, 2.633f, 2.6f),
                new AspectRatioInfo(@"Super 16mm", 2.634f, 2.673f, 2.7f),
                new AspectRatioInfo(@"Cinerama", 2.674f, 2.720f, 2.7f),
                new AspectRatioInfo(@"Ultra Panavision 70", 2.721f, 2.825f, 2.8f),
                new AspectRatioInfo(@"Ultra Panavision 70", 2.826f, 2.910f, 2.9f),
                new AspectRatioInfo(@"MGM Camera 65", 2.911f, 2.965f, 2.9f),
                new AspectRatioInfo(@"3:01", 2.966f, 3.500f, 3.0f),
                new AspectRatioInfo(@"PolyVision", 3.501f, 8.000f, 4.0f),
                new AspectRatioInfo(@"Circle Vision 360", 8.001f, 12.000f, 12.0f)
            };
        }

        public static AspectRatioInfo GetKnownAspectRatio(float aspect) {
            int idx = Array.BinarySearch(KnownAspectRatios, aspect, _aspectRatioComparer);
            return (idx > 0)
                ? KnownAspectRatios[idx]
                : null;
        }
    }

}