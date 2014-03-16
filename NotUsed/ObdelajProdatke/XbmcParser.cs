﻿using System;
using System.Collections.Generic;
using System.Linq;
using Frost.Common;
using Frost.Model.Xbmc.DB;
using Frost.Models.Frost.DB;

namespace Frost.ProcessDatabase {
    public class XbmcParser : MediaManager<XbmcMovie> {

        public XbmcParser() : base(DBSystem.XBMC) {
        }

        public XbmcParser(string dbLocation) : base(DBSystem.XBMC, dbLocation) {
        }

        public override IEnumerable<XbmcMovie> RawMovies {
            get { throw new NotImplementedException(); }
        }

        public override IEnumerable<Movie> Movies {
            get { throw new NotImplementedException(); }
        }

        protected override void Process(string dbLoc) {
            XbmcContainer xc = new XbmcContainer(dbLoc);
            var z = xc.Files.Where(f => f.Bookmark != null)
                            .Select(f => new {f.FileNameString, f.Bookmark.Id});

            //var k = from sd in xc.StreamDetails.OfType<XbmcSubtitleDetails>()
            //        where sd.SubtitleLanguage != null
            //        group sd by sd.File.FileNameString
            //        into sd2
            //        select new {
            //            sd2.Key,
            //            Languages = from sd3 in sd2 select sd3.SubtitleLanguage
            //        };
            var zz = z.ToArray();
        }
    }
}