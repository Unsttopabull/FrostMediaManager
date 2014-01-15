using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SQLite;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using Frost.CinemaInfoParsers.Kolosej;
using Frost.CinemaInfoParsers.PlanetTus;
using Frost.Common;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.DetectFeatures;
using System.Diagnostics;
using Frost.DetectFeatures.Util;
using Frost.PodnapisiNET;
using Frost.PodnapisiNET.Models;
using Frost.SharpOpenSubtitles;
using Frost.SharpOpenSubtitles.Models.Checking;
using Frost.SharpOpenSubtitles.Models.Checking.Receive;
using Frost.SharpOpenSubtitles.Models.Movies.Receive;
using Frost.SharpOpenSubtitles.Models.UI;
using Frost.SharpOpenSubtitles.Models.UI.Receive;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Newtonsoft.Json;
using CompressionMode = Frost.Common.CompressionMode;
using File = System.IO.File;
using FileVo = Frost.Common.Models.DB.MovieVo.Files.File;
using FrameOrBitRateMode = Frost.Common.FrameOrBitRateMode;
using Movie = Frost.Common.Models.DB.MovieVo.Movie;
using Subtitle = Frost.Common.Models.DB.MovieVo.Files.Subtitle;

namespace Frost.Tester {

    internal class Program {
        private static readonly string[] FileNames2;
        private static readonly string Filler;

        static Program() {
            Filler = string.Join("", Enumerable.Repeat("_", Console.BufferWidth));

            #region Files2

            FileNames2 = new[] {
                @"E:\Torrenti\FILMI\Amazing.Grace.2006.SLOSub.DvdRip.Xvid\Amazing.Grace.2006.SLOSub.DvdRip.Xvid.avi",
                @"E:\Torrenti\FILMI\North.Country.2005.SLOSubs.DVDRip.XviD-messi1\North.Country.2005.SLOSubs.DVDRip.XviD-messi1.avi",
                @"E:\Torrenti\FILMI\Playing by Heart 1998\Playing by Heart 1998.avi",
                @"E:\Torrenti\FILMI\Prizzi's Honor 1985\Prizzi's Honor 1985.avi",
                @"E:\Torrenti\FILMI\Road.to.Perdition.2002.SLOSubs.DVDRip.XviD-DrSi\Road.to.Perdition.2002.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\Snow.Dogs.2002.SLOSubs.DVDRip.XviD-DrSi\Snow.Dogs.2002.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\Taken (2008) - Ugrabljena\Taken 2008.iso",
                @"E:\Torrenti\FILMI\Takers (2010) - Tatovi\Takers 2010 480p BRRip XviD AC3-FLAWL3SS.avi",
                @"E:\Torrenti\FILMI\Taking Chance (2009) - Spremljevalec\Taking Chance[2009]DvDrip[Eng]-FXG.avi",
                @"E:\Torrenti\FILMI\Tangled (2010) - Zlatolaska\Tangled.2010.SLOSiNHRO.BRRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\Terminal (2004) - Terminal\TERMINAL.iso",
                @"E:\Torrenti\FILMI\The A-Team .2010.CUSTOM.SLOSUB. R5.PAL.DVDR-FG\The A-Team .2010.CUSTOM.SLOSUB. R5.PAL.DVDR-FG.iso",
                @"E:\Torrenti\FILMI\The Accused (1988) - Obtožena\The.Accused.1988.SWESUB.AC3.DVDRip.XviD-Martin.avi",
                @"E:\Torrenti\FILMI\The American (2010) - Američan\The.American.2010.BRRip.XviD-Warrior.avi",
                @"E:\Torrenti\FILMI\The American President (1995) - Ameriški predsednik\The.American.President.DVDRiP.XviD-ASK.avi",
                @"E:\Torrenti\FILMI\The Art Of War II Betrayal (2008) - Umetnost vojne Izdaja\The.Art.Of.War.II.Betrayal.2008.STV.SLOSubs.DVDRip.XviD-DrSi.cd1.avi",
                @"E:\Torrenti\FILMI\The Art Of War II Betrayal (2008) - Umetnost vojne Izdaja\The.Art.Of.War.II.Betrayal.2008.STV.SLOSubs.DVDRip.XviD-DrSi.cd2.avi",
                @"E:\Torrenti\FILMI\The Artist (2011) - Umetnik\The_Artist_2011_DVDSCR_XviD_-_ZOMBiES.avi",
                @"E:\Torrenti\FILMI\The Band (2009) - Bend\citrin-the.band.xvid.avi",
                @"E:\Torrenti\FILMI\The Bank Job (2008) - Bančni rop\The Bank Job.iso",
                @"E:\Torrenti\FILMI\The Beaver (2011) - Bober\The.Beaver.2011.720p.SLOSubs.BRRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\The Big Lebowski (1998) - Veliki Lebowski\The Big Lebowski.1998.HDRip.x264-VLiS.mkv",
                @"E:\Torrenti\FILMI\The Big Sleep (1946) - Velik spanec\The.Big.Sleep.1946.DVDRip.H264.AAC.Gopo.mp4",
                @"E:\Torrenti\FILMI\The Blind Side (2009) - The Blind Side\The.Blind.Side.2009.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\The Blues Brothers (1980) - Brata Blues\The Blues Brothers.iso",
                @"E:\Torrenti\FILMI\The Boat That Rocked (2009) - Piratski radio\The.Boat.That.Rocked.2009.DvDRip-FxM.avi",
                @"E:\Torrenti\FILMI\The Breakfast Club (1985) - Sobotni klub\The Breakfast Club [1985] DvdRip [Eng] - Thizz.avi",
                @"E:\Torrenti\FILMI\The Bridge (2006) - Most\The Bridge (2006) DVDRip XviD.[www.UsaBit.com].avi",
                @"E:\Torrenti\FILMI\The Brothers Bloom (2009) - Brata Bloom\The.Brothers.Bloom.2009.DvDRip-FxM.avi",
                @"E:\Torrenti\FILMI\The Curious Case of Benjamin Button [2008] - Nenavaden primer Benjamina Buttona\The Curious Case of Benjamin Button[2008]DvDrip[Eng]-FXG.avi",
                @"E:\Torrenti\FILMI\The Da Vinci Code (2006) - Da Vincijeva Sifra\The.Da.Vinci.Code.2006.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\The Dark Knight [2008] - Vitez teme\The Dark KnightThe Dark Knight[2008]DvDrip[Eng]-FXG.avi",
                @"E:\Torrenti\FILMI\The Descendants (2011) - Potomci\The.Descendants.2011.CUSTOM.SLOSUB.NTSC.DVDR-DrSi.iso",
                @"E:\Torrenti\FILMI\The Duchess (2008) - Vojvodinja\TheDuches.iso",
                @"E:\Torrenti\FILMI\The Fall (2006) - Padec\The.Fall[2006]DvDrip-aXXo.avi",
                @"E:\Torrenti\FILMI\The Football Factory (2004) - The Football Factory\The Football Factory.avi",
                @"E:\Torrenti\FILMI\The Ghost Writer (2010) - Pisatelj v senci\The.Ghost.Writer.2010.CUSTOM.SLOSUB.NTSC.DVDR-DrSi.iso",
                @"E:\Torrenti\FILMI\The Gospel of Judas (TV 2006) - Judežev evangelij\The Gospel of Judas.avi",
                @"E:\Torrenti\FILMI\The Great Gatsby (2013) - Veliki Gatsby\The Great Gatsby (2013) - Veliki Gatsby.iso",
                @"E:\Torrenti\FILMI\The Guard (2011) - Policist\The.Guard.2011.LiMiTED.DVDRip.XviD-ViP3R.avi",
                @"E:\Torrenti\FILMI\The Hangover (2009) - Prekrokana noč\The.Hangover.2009.UNRATED.SLOSubs.BRRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\The Hangover 2 (2011) - Prekrokana noč 2\The Hangover 2 (2011) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro.avi",
                @"E:\Torrenti\FILMI\The Happening (2008) - Dogodek\The Happening.iso",
                @"E:\Torrenti\FILMI\The Hobbit The Desolation of Smaug 2013 SrbSubs DVDSCR XviD-MAXSPEED\The Hobbit The Desolation of Smaug (2013) DVDSCR XviD-MAXSPEED.avi",
                @"E:\Torrenti\FILMI\The holiday (2006) - Počitnice\The holiday.iso",
                @"E:\Torrenti\FILMI\The Horsemen [2009] - Jezdeci\The Horsemen[2009]DvDrip[Eng]-FXG.avi",
                @"E:\Torrenti\FILMI\The human stain (2003) - Cloveški madez\The Human Stain 2003 SLOSUB DVDRip-DeeJayTaurus.avi",
                @"E:\Torrenti\FILMI\The Hurt Locker (2008) - Bombna misija\The.Hurt.Locker.2008.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\The Interpreter (2005) - Prevajalka\The.Interpreter.(V.O).[DvDRip].[WwW.LiMiTeDiVx.CoM].avi",
                @"E:\Torrenti\FILMI\The Iron Lady (2011) - Zelezna Lady\The.Iron.Lady.2011.DVDR.PAL.SLOSUBS-DiSHON.iso",
                @"E:\Torrenti\FILMI\The Killing Room (2009) - Soba za ubijanje\The.Killing.Room.2009.DVDRip.XviD-VoMiT.NoRar.www.crazy-torrent.com.avi",
                @"E:\Torrenti\FILMI\The King's Speech (2010) - Kraljev govor\The.Kings.Speech.2010.NORDiC.DvDRip.x264-Makavalios.mkv",
                @"E:\Torrenti\FILMI\The Kingdom (2007) - Kraljestvo\The.Kingdom.2007.cd1.DVDRip.XviD-bRiP.avi",
                @"E:\Torrenti\FILMI\The Kingdom (2007) - Kraljestvo\The.Kingdom.2007.cd2.DVDRip.XviD-bRiP.avi",
                @"E:\Torrenti\FILMI\The Last House On The Left (2009) - Zadnja hiša na levi\The.Last.House.On.The.Left.UNRATED.DvDRip-FxM.avi",
                @"E:\Torrenti\FILMI\The Namesake (2007) - Usoda imena\The Namesake.iso",
                @"E:\Torrenti\FILMI\The Notebook (2004) - Beležnica\The Notebook (2004) [ENG] [DVDrip].avi",
                @"E:\Torrenti\FILMI\The Pacifier (2005) - Misija Cucelj\The Pacifier.avi",
                @"E:\Torrenti\FILMI\The Prestige (2006) - Nevidna sled\The Prestige 2006 BRRip x264 AC3-TheFalcon007 (Kingdom-Release).avi",
                @"E:\Torrenti\FILMI\The Rebound (2009) - The Rebound\The.Rebound.2009.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\The Red Shoes (1948) - Rdeci cevlji\The Red Shoes (1948) Eng.avi",
                @"E:\Torrenti\FILMI\The Score (2001) - Zadetek\The.Score.2001.SLOSubs.DVDRip.XviD-DrSi.CD1.avi",
                @"E:\Torrenti\FILMI\The Score (2001) - Zadetek\The.Score.2001.SLOSubs.DVDRip.XviD-DrSi.CD2.avi",
                @"E:\Torrenti\FILMI\The Shawshank Redemption (1994) - Kaznilnica odrešitve\The Shawshank Redemption dvd rip Xvid.Rets.avi",
                @"E:\Torrenti\FILMI\The Sixth Sense (1999) - Sesti cut\The.Sixth.Sense.1999.CUSTOM.SLOSUB.PAL.DVDR-DrSi.iso",
                @"E:\Torrenti\FILMI\The Smurfs (2011) - Smrkci\The.Smurfs.2011.SLOSubs.BRRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\The Social Network (2010) - Socialno omrežje\The.Social.Network.2010.DVDSCR.XViD-WBZ.avi",
                @"E:\Torrenti\FILMI\The Taking Of Pelham 123 [2009] - Ugrabitev Pelhama 123\The Taking Of Pelham 123[2009]DvDrip-LW.avi",
                @"E:\Torrenti\FILMI\The Tourist [2010] - Turist\The Tourist[2010]DvDrip[Eng]-FXG.avi",
                @"E:\Torrenti\FILMI\The Town (2010) - Mesto\The.Town.2010.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\The Transporter (2002) - Prenašalec 1\The Transporter.avi",
                @"E:\Torrenti\FILMI\The Transporter 2 (2005) - Prenašalec 2\The TRANSPORTER 2 [2005]DvDrip[Eng]-NikonXp.avi",
                @"E:\Torrenti\FILMI\The Transporter 3 (2008) - Prenašalec 3\Transporter 3 (2008) DVDRip-HALESPONGE.avi",
                @"E:\Torrenti\FILMI\The Tree Of Life (2011) - Drevo življenja\The.Tree.Of.Life.2011.DVDR.PAL.SLOSUBS-DiSHON.iso",
                @"E:\Torrenti\FILMI\The Usual Suspects (1995) - Osumljenih 5\The Usual Suspects(Xvid).avi",
                @"E:\Torrenti\FILMI\The Wayward Cloud - Pobegli oblak\The Wayward Cloud.iso",
                @"E:\Torrenti\FILMI\The Woman in Black (2012) - Zenska v crnem\The Woman in Black 2012 BRRip XviD AbSurdiTy.avi",
                @"E:\Torrenti\FILMI\The.Expendables.2.2012.SLOSubs.BRRip.XviD-DrSi\The.Expendables.2.2012.SLOSubs.BRRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\The.Expendables.2010.SLOSubs.BRRip.XviD-DrSi\The.Expendables.2010.SLOSubs.BRRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\The.Hoax.2006.SLOsubs.DVDrip.XviD.aXXo\The.Hoax.2006.SLOsubs.DVDrip.XviD.aXXo.avi",
                @"E:\Torrenti\FILMI\There Will Be Blood (2007) - Tekla bo kri\There.Will.Be.Blood.2007.SLOSubs.DVDRip.XviD-DrSi.cd1.avi",
                @"E:\Torrenti\FILMI\There Will Be Blood (2007) - Tekla bo kri\There.Will.Be.Blood.2007.SLOSubs.DVDRip.XviD-DrSi.cd2.avi",
                @"E:\Torrenti\FILMI\Thick As Thieves [2009] - Pretkani tatovi\Thick.As.Thieves[2009].avi",
                @"E:\Torrenti\FILMI\Thor The Dark World 2013 SRBSubs.New CAM MP3 x264-MaNuDiL\Thor The Dark World 2013 SRBSubs.New CAM MP3 x264-MaNuDiL.mkv",
                @"E:\Torrenti\FILMI\Time of the Gypsies (1998) - Dom za vešanje\Dom.za.vesanje.Xvid.DVD.QQZ.CD1.avi",
                @"E:\Torrenti\FILMI\Time of the Gypsies (1998) - Dom za vešanje\Dom.za.vesanje.Xvid.DVD.QQZ.CD2.avi",
                @"E:\Torrenti\FILMI\Tinker Tailor Soldier Spy (2011) - Kotlar, Krojac, Vojak, Vohun\Tinker.Tailor.Soldier.Spy.2011.DVDR.PAL.SLOSUBS-DiSHON.iso",
                @"E:\Torrenti\FILMI\Titus (1999) - Titus\Titus.iso",
                @"E:\Torrenti\FILMI\Total.Recall.Extended.2012.SLOSubs.BRRip.XviD-DrSi\Total.Recall.Extended.2012.SLOSubs.BRRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\Traitor (2008) - Izdajalec\Traitor.2008.SLOSubs.DVDRip.XviD-aXXo.avi",
                @"E:\Torrenti\FILMI\Trance.2013.SLOSubs.DVDRip.XviD-DrSi\Trance.2013.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\Triage (2009) - Triaža\Triage.2009.DVDRip.XviD-Jaybob.avi",
                @"E:\Torrenti\FILMI\Tristan and Isolde [2006] - Tristan in Izolda\Tristan.and.Isolde[2006]DvDrip[Eng]-aXXo.avi",
                @"E:\Torrenti\FILMI\True Grit (2010) - Pravi pogum\True Grit (2010) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro.avi",
                @"E:\Torrenti\FILMI\Un Prophete (2009) - Prerok\Un.Prophete.2009.DVDRip.XviD.AC3.5.1.HORiZON-ArtSubs.avi",
                @"E:\Torrenti\FILMI\United 93 (2006) - United 93\United.93[2006]DvDrip[Eng]-aXXo.avi",
                @"E:\Torrenti\FILMI\Unthinkable (2010) - Nepojmljivo\Unthinkable.2010.EXTENDED.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\Up In The Air (2009) - V zraku\Up.In.The.Air.2009.SLOSubs.DVDSCR.RERIP.XviD-CAMELOT.cd1.avi",
                @"E:\Torrenti\FILMI\Up In The Air (2009) - V zraku\Up.In.The.Air.2009.SLOSubs.DVDSCR.RERIP.XviD-CAMELOT.cd2.avi",
                @"E:\Torrenti\FILMI\Upside.Down.2012.SLOSubs.DVDRip.XviD-DrSi\Upside.Down.2012.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\V For Vendetta (2005) - V kot vroče maščevanje\V.For.Vendetta[2005]DvDrip[Eng]-aXXo.avi",
                @"E:\Torrenti\FILMI\Valkyrie (2008) -  Operacija Valkira\Valkyrie.2008.R5.LINE.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\Wall E (2008) - Wall E\Wall_E.avi",
                @"E:\Torrenti\FILMI\Wanted (2008) - Iskan\Wanted.2008.R5.cd1.DVDRip.XviD-bRiP.avi",
                @"E:\Torrenti\FILMI\Wanted (2008) - Iskan\Wanted.2008.R5.cd2.DVDRip.XviD-bRiP.avi",
                @"E:\Torrenti\FILMI\When A Man Falls In The Forest (2007) - Ko clovek pade v gozdu\When.A.Man.Falls.In.The.Forest.2007.DVDRip.XviD-bRiP.avi",
                @"E:\Torrenti\FILMI\When Harry Met Sally (1989) - Ko je Harry srečal Sally\When Harry Met Sally.avi",
                @"E:\Torrenti\FILMI\Where The Wild Things Are (2009) - V kraljestvu prečudnih zveri\nep-wtwta-scr.avi",
                @"E:\Torrenti\FILMI\Whip It (2009) - Divje mrhe\Whip It.mp4",
                @"E:\Torrenti\FILMI\Whip It (2009) - Divje mrhe\Audio 2 ch\Whip It audio trach 2ch.mp4",
                @"E:\Torrenti\FILMI\Wir Kinder vom Bahnhoff Zoo (1981) - Otroci s postaje Zoo\Wir Kinder vom Bahnhoff Zoo.iso",
                @"E:\Torrenti\FILMI\Witness Protection [2008] - Zaščita prič\Witless.Protection.2008.cd1.DVDRip.XviD-bRiP.avi",
                @"E:\Torrenti\FILMI\Witness Protection [2008] - Zaščita prič\Witless.Protection.2008.cd2.DVDRip.XviD-bRiP.avi",
                @"E:\Torrenti\FILMI\Wonder Boys\WonderBoys.avi",
                @"E:\Torrenti\FILMI\Wreck It Ralph (2012) - Razbijač Ralph\Wreck.It.Ralph.2012.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\You Dont Know Jack (2010) - Ne poznate Jacka\You.Dont.Know.Jack.2010.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"E:\Torrenti\FILMI\Zodiac (2007) - Zodiac\Zodiac 2007.avi",
                @"E:\Torrenti\FILMI\Zombieland (2009) - Dežela zombijev\Zombieland (2009) DVDRip XviD-MAX www.torentz.3xforum.ro.avi",
                @"E:\Torrenti\FILMI\Zwartboek (2006) - Crna knjiga\Zwartboek.2006.SLOSub.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\(500) Days of Summer (2009) - 500 dni z Summer\(500)Days of Summer.[2009].RETAIL.DVDRIP.XVID.[Eng]-DUQA.avi",
                @"F:\Torrenti\FILMI\10 things i hate about you (1999) - 10 razlogov, zakaj te sovražim\10 Things I Hate About You- Full Movie.AVI",
                @"F:\Torrenti\FILMI\2012 (2009) - 2012\2012.ISO",
                @"F:\Torrenti\FILMI\21 (2008) - 21 Razpad Las Vegasa\21[2008]R5_DvDrip[Eng]-NikonXp.avi",
                @"F:\Torrenti\FILMI\21 Grams (2003) - 21 gramov\21.Grams.2003.PROPER.LiMiTED.DVDRip.XviD.DEiTY.avi",
                @"F:\Torrenti\FILMI\28 Weeks Later (2007) - 28 tednov pozneje\28.Weeks.Later.2007.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\300 (2006) - 300\300 (2007).DVDSCR.XVID.avi",
                @"F:\Torrenti\FILMI\50 50 (2011) - 50 50\50.50.2011.DVDScr.XviD-playXD.avi",
                @"F:\Torrenti\FILMI\88 Minutes (2007) - 88 minut\88.Minutes.2007.DVDRip.Eng-aXXo.avi",
                @"F:\Torrenti\FILMI\9 Songs (2004) - 9 orgazmov\9.Songs.2004.SLOSubs.DVDRip.XviD-DJTimi.avi",
                @"F:\Torrenti\FILMI\A Lot Like Love (2005) - Več kot ljubezen\A Lot Like Love.avi",
                @"F:\Torrenti\FILMI\A Separation (2011) - Ločitev\A Separation 2011.avi",
                @"F:\Torrenti\FILMI\A Serious Man (2009) - Zresni se\A.Serious.Man.2009.LiMiTED.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\A Single Man (2010) - Samski moški\A Single Man.2010.DVDRip.XviD-T0XiC.avi",
                @"F:\Torrenti\FILMI\A Walk To Remember [2002] - Spomin v srcu\A Walk To Remember[2002].DvdRip.[yddam].avi",
                @"F:\Torrenti\FILMI\Adulthood [2008] - Odraslost\Adulthood[2008]DvDrip-aXXo.avi",
                @"F:\Torrenti\FILMI\Alan Partridge Alpha Papa 2013 720p CROSubs.BrRip x264 - YIFY\Alan Partridge Alpha Papa 2013 720p CROSubs.BrRip x264 - YIFY.mp4",
                @"F:\Torrenti\FILMI\ALFIE (2004) - Alfie\Alfie.avi",
                @"F:\Torrenti\FILMI\All About Eve (1950) - Vse o Evi\All About Eve.avi",
                @"F:\Torrenti\FILMI\Amen (2002) - Amen\Amen. 2002.iso",
                @"F:\Torrenti\FILMI\American History X (1999) - Generacija X\American History X.iso",
                @"F:\Torrenti\FILMI\American Pie 1 [1999] - Ameriška pita 1\American Pie 1[1999].avi",
                @"F:\Torrenti\FILMI\American Pie 2 [2001] - Ameriška pita 2\American Pie 2[2001].avi",
                @"F:\Torrenti\FILMI\American Pie 3 The Wedding [2003] - Ameriška pita 3 Poroka\American Pie 3 The Wedding[2003].avi",
                @"F:\Torrenti\FILMI\American Pie 4 Band Camp [2005] - Ameriška pita 4 Glasbeni tabor\American Pie 4 Band Camp[2005].avi",
                @"F:\Torrenti\FILMI\American Pie 5 The Naked Mile [2006] - Ameriška pita 5 Gola milija\American.Pie.5.The.Naked Mile.[2006].SLOsub.DvDrip[Eng]-BugZ.avi",
                @"F:\Torrenti\FILMI\American Pie Presents Beta House (2007) - Ameriška pita 6\American.Pie.Presents.Beta.House.2007.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\American Psycho (Uncut) (2000) - Ameriški psiho\AMERICAN PSYCHO - Uncut [2000-Eng-DVDrip]-haSak.avi",
                @"F:\Torrenti\FILMI\Amores Perros (2000) - Pasja ljubezen\Amores.Perros.2000.SLOSubs.DVDRip.XviD-DrSi.cd1.avi",
                @"F:\Torrenti\FILMI\Amores Perros (2000) - Pasja ljubezen\Amores.Perros.2000.SLOSubs.DVDRip.XviD-DrSi.cd2.avi",
                @"F:\Torrenti\FILMI\Anchorman The legend of Ron Burgundy [2004] - Anchorman\Anchorman The legend of Ron Burgundy[2004].avi",
                @"F:\Torrenti\FILMI\Another Year (2010) - Se eno leto\Another Year[2010]DvDrip[Eng]-FXG.avi",
                @"F:\Torrenti\FILMI\Argo (2012) - Argo\Argo.2012.PAL.MULTiSUBS.DVDR-DiSHON.iso",
                @"F:\Torrenti\FILMI\Atonement (2007) - Pokora\Atonement.2007.cd1.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\Atonement (2007) - Pokora\Atonement.2007.cd2.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\Attack the Block - Napad na blok\attack.the.block-done.avi",
                @"F:\Torrenti\FILMI\Avatar (2009) - Avatar\Avatar.2009.1080p.Slosubs.BluRay.DTS.x264-ESiR.mkv",
                @"F:\Torrenti\FILMI\Babel (2006) - Babilon\Babel[2006]DvDrip[Eng]-aXXo.avi",
                @"F:\Torrenti\FILMI\Bachelorette.2012.SLOSubs.DVDRip.XviD-DrSi\Bachelorette.2012.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Bal (2010) - Med\ltu-honey-xvid.avi",
                @"F:\Torrenti\FILMI\Barney's Version (2010) - Barneyjeva različica\Barney's Version 2010 720p BRRip x264 RmD (HDScene Release).mkv",
                @"F:\Torrenti\FILMI\Batman Begins (2005) - Batman na začetku\batman.begins-phrax.mp4",
                @"F:\Torrenti\FILMI\Becoming Jane (2008) - Ljubljena Jane\Becoming Jane.iso",
                @"F:\Torrenti\FILMI\Beginners (2011) - Začetniki\target-beginners-xvid.avi",
                @"F:\Torrenti\FILMI\Beowulf (2007) - Beowulf\Beowulf.2007.PAL.HD2DVD.DVDR.Custom.SLOSUBS-DiSHON.iso",
                @"F:\Torrenti\FILMI\Biutiful (2010) - Biutiful\Biutiful.2010.DVDRip.XviD.5rFF.avi",
                @"F:\Torrenti\FILMI\Black Hawk Down (2002) - Sestreljeni crni jastreb\Black Hawk Down.iso",
                @"F:\Torrenti\FILMI\Black Swan (2010) - Crni labod\Black.Swan.2010.DVDSCR.XviD-ViSiON.avi",
                @"F:\Torrenti\FILMI\Blue Valentine (2010) - Blue Valentine\Blue.Valentine.2010.DvdScr.AC3.Xvid {1337x}-Noir.avi",
                @"F:\Torrenti\FILMI\Cell 211 (2009) - Celica 211\Celda.211.2009.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Charlie And The Chocolate Factory (2005) - Carli in tovarna cokolade\Charlie And The Chocolate Factory (2005).avi",
                @"F:\Torrenti\FILMI\Children Of Men - Otroci clovestva\Children Of Men.avi",
                @"F:\Torrenti\FILMI\City of God (2006) - Božje mesto\City of God CD1.avi",
                @"F:\Torrenti\FILMI\City of God (2006) - Božje mesto\City of God CD2.avi",
                @"F:\Torrenti\FILMI\Cleaner (2007) - Cistilec\Cleaner.2007.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\Cloud Atlas (2012) - Atlas oblakov\hdt.cloud.atlas.2012.1080p.bluray.x264.mkv",
                @"F:\Torrenti\FILMI\Cold Mountain (2004) - Hladni vrh\Cold Mountain.iso",
                @"F:\Torrenti\FILMI\Collateral (2004) - Stranski učinki\Collateral.iso",
                @"F:\Torrenti\FILMI\Confucius (2010) - Konfucij\Confucius.2010.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Cowboys And Aliens (2011) - Kavboji in vesoljci\Cowboys.And.Aliens.EXTENDED.2011.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Crash (2004) - Usodna nesreča\Crash.2004.SLOSub.DVDRip.Xvid-DrSi.cd1.avi",
                @"F:\Torrenti\FILMI\Crash (2004) - Usodna nesreča\Crash.2004.SLOSub.DVDRip.Xvid-DrSi.cd2.avi",
                @"F:\Torrenti\FILMI\Das Weisse Band (2008) - Beli trak\The White Ribbon[2009]DvDrip[Ger]-FXG.avi",
                @"F:\Torrenti\FILMI\Dead Man Walking (1995) - Zadnji sprehod\Dead Man Walking [English] 1995.avi",
                @"F:\Torrenti\FILMI\Dear John (2010) - Samo tebe si zelim\Dear John.2010.DvdRip.Xvid {1337x}-Noir.avi",
                @"F:\Torrenti\FILMI\Deja Vu (2006) - Déja Vu\Deja Vu 2006.iso",
                @"F:\Torrenti\FILMI\Der Untergang (2005) - Propad\Der Untergang.iso",
                @"F:\Torrenti\FILMI\Despicable Me (2010) - Jaz, baraba\despicable.me.dvdrip.xvid-imbt.avi",
                @"F:\Torrenti\FILMI\Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi\Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi.avi",
                @"F:\Torrenti\FILMI\Detachment (2011) - Odtujenost\Detachment[2011]BRRip XviD-ETRG.avi",
                @"F:\Torrenti\FILMI\Disgrace (2008) - Sramota\Disgrace.2008.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\District 9 (2009) - Okrožje 9\District 9 (2009) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro.avi",
                @"F:\Torrenti\FILMI\Django Unchained (2012) - Django brez okov\Django.Unchained.2012.SLOSubs.DVDSCR.XviD-metalcamp.avi",
                @"F:\Torrenti\FILMI\Doubt (2008) - Dvom\DOUBT_DEU.iso",
                @"F:\Torrenti\FILMI\Drive (2011) - Vožnja\Drive.2011.SCR.XviD-playXD.avi",
                @"F:\Torrenti\FILMI\Eat Pray Love (2010) - Jej, moli, ljubi\Eat.Pray.Love.2010.DC.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Efter brylluppet (2006) - Po poroki\After the Wedding 2006 DVDRip Xvid fasamoo LKRG.avi",
                @"F:\Torrenti\FILMI\El Orfanato (2007) - Sirotišnica\The.Orphanage.2007.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Elizabeth (1998) - Elizabeta\Elizabeth.1998.DVDRip.DivX-GarlicClan.slo.avi",
                @"F:\Torrenti\FILMI\English Patient (1996) - Angleški pacient\English Patient.iso",
                @"F:\Torrenti\FILMI\Escape.Plan.2013.SRBSubs.CAM.XviD-Tr0uNcE\Escape.Plan.2013.SRBSubs.CAM.XviD-Tr0uNcE.avi",
                @"F:\Torrenti\FILMI\Eternal Sunshine Of The Spotless Mind (2004) - Večno sonce brezmadežnega uma\dvl-eotsm.avi",
                @"F:\Torrenti\FILMI\Everybody's Fine (2009) - Vsi so vredu\Everybodys.Fine.2009.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Factory Girl (2006)\Factory.Girl.2006.RETAiL.SLO.CROSub.PAL.DVD9-DrSi.iso",
                @"F:\Torrenti\FILMI\Fantastic Mr Fox - Fantastični gospod Lisjak\mrush-mrfox.avi",
                @"F:\Torrenti\FILMI\Fargo (1996) - Fargo\Fargo.1996.CUSTOM.SLOSUB.PAL.DVDR-DrSi.iso",
                @"F:\Torrenti\FILMI\Feast of Love (2007) - Praznik ljubezni\Feast.of.Love.2007.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\Felon (2008) - Zločinec\Felon.2008.cd1.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\Felon (2008) - Zločinec\Felon.2008.cd2.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\Fight Club (1999) - Klub golih pesti\Fight Club.mp4",
                @"F:\Torrenti\FILMI\Fighting (2009) - Borba\dash-fighting.avi",
                @"F:\Torrenti\FILMI\Finding Neverland (2005) - V iskanju dežele Nije\FindingNeverland.iso",
                @"F:\Torrenti\FILMI\Flags of our fathers (2006) - Zastave naših očetov\Flags of our fathers.iso",
                @"F:\Torrenti\FILMI\Flight (2012)\flight.2012.1080p.bluray.x264-sparks.mkv",
                @"F:\Torrenti\FILMI\G.I.Joe.Retaliation.2013.SLOSubs.DVDRip.XviD-DrSi\G.I.Joe.Retaliation.2013.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Gangster Squad (2013) - Gangsterska enota\Gang.avi",
                @"F:\Torrenti\FILMI\Girl.Most.Likely.2012.CROSubs.HDRip.XviD-S4A\Girl.Most.Likely.2012.CROSubs.HDRip.XviD-S4A.avi",
                @"F:\Torrenti\FILMI\Gomorra (2008) - Gomorra\Gomorra.iso",
                @"F:\Torrenti\FILMI\Gone Baby Gone (2007) - Zbogom, punčka\Gone Baby Gone.[2007].DVDRIP.XVID.[Eng]-DUQA.avi",
                @"F:\Torrenti\FILMI\Goodbye Bafana (2007) - Zbogom Bafana\Goodbye Bafana.iso",
                @"F:\Torrenti\FILMI\Gran Torino (2008) - Gran Torino\Gran.Torino.2008.DvDRip-FxM.avi",
                @"F:\Torrenti\FILMI\Grbavica (2006) - Grbavica\Grbavica.2006.DVDRip.XviD-XPTO.avi",
                @"F:\Torrenti\FILMI\Green Zone (2010) - Zelena cona\Green.Zone.2010.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Gremo mi po svoje (2010) - Gremo mi po svoje\Gremo.mi.po.svoje.2010.SLOVENiAN.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Guess Who (2005) - Ugani kdo\Guess.Who.2005.SLOSub.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\Hancock (2008) - Hancock\HR5.avi",
                @"F:\Torrenti\FILMI\Happy-Go-Lucky (2008) - Kar-brez-skrbi\Happy-Go-Lucky.mp4",
                @"F:\Torrenti\FILMI\Hard Candy (2006) - Prepovedan sadež\Hard.Candy.DVDRip.XviD-DiAMOND.avi",
                @"F:\Torrenti\FILMI\Head In the clouds (2004) - Z glavo v oblakih\HEADINTHECLOUDS.ISO",
                @"F:\Torrenti\FILMI\Headhunters (2011) - Lovci na glave\Headhunters.2011.BRRip.XviD-playXD.avi",
                @"F:\Torrenti\FILMI\Homeward Bound (1993) - Neverjetna pot domov\Homeward Bound.avi",
                @"F:\Torrenti\FILMI\How to Lose Friends and Alienate People (2008) - Kako izgubiti prijatelje in odtujiti ljudi\How.to.Lose.Friends.and.Alienate.People.2008.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Hugo (2011) - Hugo\Hugo 2011.720p.BrRip.X264.YIFY.mp4",
                @"F:\Torrenti\FILMI\Hysteria.2011.SLOSubs.DVDRip.XviD-DrSi\Hysteria.2011.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\I am legend (2007) - Jaz Legenda\I_AM_LEGEND.ISO",
                @"F:\Torrenti\FILMI\I'm Not There (2007) - Bob Dylan 7 obrazov\I'm.Not.There[2007]DvDrip[Eng]-aXXo.avi",
                @"F:\Torrenti\FILMI\Imagine That (2009) - Predstavljaj si to\Imagine That.2009.DvdRip.Xvid {1337x}-Noir.avi",
                @"F:\Torrenti\FILMI\In Bruges (2008) - V Brugesu\In Bruges.iso",
                @"F:\Torrenti\FILMI\Incendies (2010) - Zenska, ki poje\Incendies.2010.DVDRip.XviD.AC3.HORiZON-ArtSubs.avi",
                @"F:\Torrenti\FILMI\Inglourious Basterds (2009) - Naslavne barabe\Inglourious Basterds (2009) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro.avi",
                @"F:\Torrenti\FILMI\Intersections 2013 CROSubs.DVDRip XViD juggs\Intersections 2013 CROSubs.DVDRip XViD juggs.avi",
                @"F:\Torrenti\FILMI\Ip Man [2008] - Ip Man\IpMan -aot.avi",
                @"F:\Torrenti\FILMI\Iron Man (2008) - Iron Man\Iron.Man.2008.SLOSubs.DVDRip.XviD-DrSi.cd1.avi",
                @"F:\Torrenti\FILMI\Iron Man (2008) - Iron Man\Iron.Man.2008.SLOSubs.DVDRip.XviD-DrSi.cd2.avi",
                @"F:\Torrenti\FILMI\Iron Man 2 (2010) - Iron Man 2\Iron.Man.2.2010.DVDRip.XviD.AC3-ViSiON.avi",
                @"F:\Torrenti\FILMI\Iron Man 3 (2013) - Iron Man 3\Iron.Man.3.2013.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\It's Kind of a Funny Story (2010) - It's Kind of a Funny Story\Its.Kind.of.a.Funny.Story.2010.SLOSubs.BRRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Julie and Julia (2009) - Julie in Julia\Julie.and.Julia.2009.SLOSubs.DVDRip.XviD-DrSi .avi",
                @"F:\Torrenti\FILMI\Kick-Ass.2.2013.SRBSubs.R6.HDRip.XviD-S4A\Kick-Ass.2.2013.SRBSubs.R6.HDRip.XviD-S4A.avi",
                @"F:\Torrenti\FILMI\Kidulthood (2006) - Otroštvo\Kidulthood.2006.DVDRip.XviD.(Cd1)-SAvAGE.avi",
                @"F:\Torrenti\FILMI\Kidulthood (2006) - Otroštvo\Kidulthood.2006.DVDRip.XviD.(Cd2)-SAvAGE.avi",
                @"F:\Torrenti\FILMI\Killers (2010) - Morilci\Killers.2010.R5.LiNE.Xvid-Noir.avi",
                @"F:\Torrenti\FILMI\Knowing (2009) - Prerokba\Knowing.2009.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Ko To Tamo Peva\Ko To Tamo Peva.iso",
                @"F:\Torrenti\FILMI\Kokuhaku Confessions (2010) - Priznanja\Confessions.2010.JAP.DVDRip.XviD-MOC.avi",
                @"F:\Torrenti\FILMI\La Délicatesse (2011)\La.Delicatesse.avi",
                @"F:\Torrenti\FILMI\La mala educación (2004) - Slaba vzgoja\Almodovar_Bad.Education_DVDrip.Xvid_eng.subbed_m^M^m.avi",
                @"F:\Torrenti\FILMI\La piel que habito (2011) - Koža v kateri zivim\The.Skin.I.Live.In.2011.720p.Bluray.x264.anoXmous.mp4",
                @"F:\Torrenti\FILMI\La Vita E Bella (1997) - Zivljenje je lepo\La Vita E Bella.iso",
                @"F:\Torrenti\FILMI\Lebanon (2009) - Libanon\Lebanon.2009.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Leon-The Professional (19949 - Leon\Leon-The Professional 1994 BDrip[A Release-Lounge H.264 By Titan].mp4",
                @"F:\Torrenti\FILMI\Les Amours Imaginaires (2010) - Namišljene ljubezni\Les Amours Imaginaires 2010 [DVDRip.XviD-miguel] [FR].avi",
                @"F:\Torrenti\FILMI\Limitless (2011) - Odklenjen\Limitless.UNRATED.2011.SLOSubs.BRRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Lincoln (2012) - Lincoln\Lincoln.2012.DVDSCR.XViD.AC3-MAGNAT.avi",
                @"F:\Torrenti\FILMI\Little Big Soldier (2010) - Mali veliki vojak\Little.Big.Soldier.2010.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Little children (2007) - Mali otroci\LITTLE_CHILDREN.ISO",
                @"F:\Torrenti\FILMI\Looper (2012) - Casovna zanka\Looper.2012.CUSTOM.SLOSUB.NTSC.DVDR-DrSi.iso",
                @"F:\Torrenti\FILMI\Love Actually (2003) - Pravzaprav ljubezen\Love Actually CD1.avi",
                @"F:\Torrenti\FILMI\Love Actually (2003) - Pravzaprav ljubezen\Love Actually CD2.avi",
                @"F:\Torrenti\FILMI\Lucky Number Slevin (2006) - Srečnež Slevin\Lucky.Number.Slevin.2006.SLOSubs.DVDRip.XviD-DrSi.cd1.avi",
                @"F:\Torrenti\FILMI\Lucky Number Slevin (2006) - Srečnež Slevin\Lucky.Number.Slevin.2006.SLOSubs.DVDRip.XviD-DrSi.cd2.avi",
                @"F:\Torrenti\FILMI\Magnolia [1999] -  Magnolija\Magnolia [1999] dvd rip nlx.avi",
                @"F:\Torrenti\FILMI\Man On Wire (2008) - Clovek na zici\Man.On.Wire.2008.DVDRip.XviD.avi",
                @"F:\Torrenti\FILMI\Mar Adentro (2004) - Morje v meni\Mar.Adentro (The.Sea.Inside) 2004.DVDRip.XviD.avi",
                @"F:\Torrenti\FILMI\Melancholia (2011) - Melanholija\psig-melancholia.2011.dvdrip.xvid.avi",
                @"F:\Torrenti\FILMI\Memento (2000) - Memento\Memento.DVDrip,XviD-contempt.avi",
                @"F:\Torrenti\FILMI\Midnight in Paris (2011) - Polnoč v Parizu\target-paris-xvid.avi",
                @"F:\Torrenti\FILMI\Milk [2008] - Milk\Milk[2008]DvDrip[Eng]-FXG.avi",
                @"F:\Torrenti\FILMI\Moon (2009) - Luna\Moon.2009.LiMiTED.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Mr.73 (2008) - MR73\Mr.73.iso",
                @"F:\Torrenti\FILMI\My Sassy Girl (2008)  - Moja cudna punca\carre-my.sassy.girl-xvid.avi",
                @"F:\Torrenti\FILMI\Ned Kelly (2003) - Ned Kelly\Ned Kelly.iso",
                @"F:\Torrenti\FILMI\Never Let Me Go (2010) - Ne zapusti me nikdar\Never.Let.Me.Go.2010.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Norwegian Wood (2010) - Norveški gozd\Norwegian.Wood.2010.JAP.DVDRip.XviD.AC3-BAUM.avi",
                @"F:\Torrenti\FILMI\Now You See Me 2013 SloSubs  EXTENDED BRRiP XViD UNiQUE\Now You See Me 2013 EXTENDED BRRiP XViD UNiQUE.avi",
                @"F:\Torrenti\FILMI\Oblivion (2013) - Pozaba\Oblivion.2013.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Old Dogs (2009) - Stara mačka\Old.Dogs.2009.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Once (2006) - Enkrat\Once.2006.SLOSubs.DVDRip.XviD.avi",
                @"F:\Torrenti\FILMI\Oz.the.Great.and.Powerful.2013.SLOSubs.DVDRip.XviD-DrSi\Oz.the.Great.and.Powerful.2013.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\P.S. I Love You (2007) - P.S. Ljubim te\P.S.I.Love.You.2007.SLOSubs.DVDRip.XviD-DrSi.cd1.avi",
                @"F:\Torrenti\FILMI\P.S. I Love You (2007) - P.S. Ljubim te\P.S.I.Love.You.2007.SLOSubs.DVDRip.XviD-DrSi.cd2.avi",
                @"F:\Torrenti\FILMI\Pacific Rim 2013 CROSubs.HDCam NewAudio XviD Feel-Free\Pacific Rim 2013 CROSubs.HDCam NewAudio XviD Feel-Free.avi",
                @"F:\Torrenti\FILMI\Paha Perhe aka Bad Family (2010) - Slaba družina\Paha.Perhe.aka.Bad.Family.2010.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Pan's Labyrinth (2006) - Panov labirint\Pan's.Labyrinth[2006]DvDrip[Eng.Sub]-aXXo.avi",
                @"F:\Torrenti\FILMI\Paranoia 2013 CROSubs.BRRip XViD-ETRG\Paranoia 2013 CROSubs.BRRip XViD-ETRG.avi",
                @"F:\Torrenti\FILMI\Paranoia 2013 CROSubs.BRRip XViD-ETRG\sample.avi",
                @"F:\Torrenti\FILMI\Paul (2011) - Paul\Paul.2011.DVDRip.XviD-DiVERSiTY.avi",
                @"F:\Torrenti\FILMI\People vs Larry Flynt (1996) - Ljudstvo proti Larryju Flintu\The People vs Larry Flynt.avi",
                @"F:\Torrenti\FILMI\Perfume The Story Of A Murderer (2006) - Parfum Zgodba morilca\Perfume.The.Story.Of.A.Murderer.2006.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Petelinji zajtrk (2007)\Petelinji.Zajtrk.2007.DVDSCR.XviD-SpeeD-cd1.avi",
                @"F:\Torrenti\FILMI\Petelinji zajtrk (2007)\Petelinji.Zajtrk.2007.DVDSCR.XviD-SpeeD-cd2.avi",
                @"F:\Torrenti\FILMI\Pirates of the Carribean At World's End (2007) - Pirati s Karibov Na robu sveta\Pirates.Of.The.Caribbean-At.World's.End[2007]DvDrip[Eng]-aXXo.avi",
                @"F:\Torrenti\FILMI\Pirates of the Carribean Dead Man's Chest (2006) - Pirati s Karibov Mrtvečeva skrinja\Pirates.of.the.Caribbean-Dead.Man's.Chest[2006]DvDrip.avi",
                @"F:\Torrenti\FILMI\Potovanje skozi plasti Zemlje (TV 2006)\Potovanje.Skozi.Plasti.Zemlje.Slosub.Xvid.avi",
                @"F:\Torrenti\FILMI\Premonition (2007) - Slutnja\Premonition.2007.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Pride and Prejudice (2005) - Prevzetnost in pristranost\Pride.and.Prejudice[2005]DvDrip[Eng]-aXXo.avi",
                @"F:\Torrenti\FILMI\Prince Of Persia The Sands Of Time (2010)  - Perzijski princ Sipine casa\Prince.Of.Persia.The.Sands.Of.Time.2010.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Public Enemies (2009) - Državni sovražniki\Public.Enemies.2009.DvDRip-FxM.avi",
                @"F:\Torrenti\FILMI\Push [2009] - Udarec\Push[2009]DvDrip[Eng]-FXG.avi",
                @"F:\Torrenti\FILMI\R I P D 2013 WEBRiP XViD UNiQUE (SilverTorrent)\RIPD.avi",
                @"F:\Torrenti\FILMI\Rane (2003) - Rane\Rane.iso",
                @"F:\Torrenti\FILMI\Red (2010) - Upokojeni, oboroženi, nevarni (Red)\Red.2010.BRRip.XviD.AC3-MAGNAT.avi",
                @"F:\Torrenti\FILMI\Remember Me (2010) - Ne pozabi me\Remember Me (2010).avi",
                @"F:\Torrenti\FILMI\Reservoir Dogs (1992) - Stekli psi\Reservoir Dogs_1992_DVDrip_XviD-Ekolb.avi",
                @"F:\Torrenti\FILMI\Rise of the Guardians (2012) - Pet legend\Rise of the Guardians (2012)R5 DVDRip NL subs (Divx)NLtoppers.avi",
                @"F:\Torrenti\FILMI\Rise of the Guardians (2012)R5 DVDRip NL subs (Divx)NLtoppers\Rise of the Guardians (2012)R5 DVDRip NL subs (Divx)NLtoppers.avi",
                @"F:\Torrenti\FILMI\Robin Hood (2010) - Robin Hood\Robin.Hood.Unrated.DC.2010.BRRip.XviD.AC3-MAGNAT.avi",
                @"F:\Torrenti\FILMI\RocknRolla [2008] - Rocknroller\RocknRolla[2008]DvDrip-aXXo.avi",
                @"F:\Torrenti\FILMI\Roger Dodger (2002) - Roger Dodger\Roger Dodger.avi",
                @"F:\Torrenti\FILMI\Rush Hour 3 (2007) - Ful gas 3\Rush.Hour.3.2007.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\Salt (2010) - Salt\Salt.2010.R5.LiNE.CUSTOM.SLOSUB.PAL.DVDR-metalcamp.iso",
                @"F:\Torrenti\FILMI\Scary Movie (2000) - Film, da te kap\Scary Movie.avi",
                @"F:\Torrenti\FILMI\Scott Pilgrim vs. the World (2010) - Scott Pilgrim proti vsem\Scott.Pilgrim.vs.the.World.2010.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Seeking.a.Friend.for.the.End.of.the.World.2012.SLOSubs.DVDRip.XviD-DrSi\Seeking.a.Friend.for.the.End.of.the.World.2012.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Seven Pounds (2008) - Sedem duš\Seven.Pounds.2008.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Seven Psychopaths (2012) - Sedem psihopatov\Seven.Psychopaths.2012.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\SEVEN [1995] - Sedem\SEVEN[1995]DvDrip[Eng]-NikonXP.avi",
                @"F:\Torrenti\FILMI\Sex Drive (2008) - Ljubezenski klic\Sex.Drive.2008.UNRATED.DVDRip.XviD-ST4R.avi",
                @"F:\Torrenti\FILMI\Shakespeare in Love (1998) - Zaljubljeni Shakespeare\Shakespeare in Love.iso",
                @"F:\Torrenti\FILMI\Shame (2011) - Sramota\shame.2011.limited.dvdrip.xvid-amiable.avi",
                @"F:\Torrenti\FILMI\She's Out Of My League (2010) - Predobra zame\Shes.Out.Of.My.League.2010.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\She's The Man(2006) - Mlada nogometašica\dmd-shestm-cd1.avi",
                @"F:\Torrenti\FILMI\She's The Man(2006) - Mlada nogometašica\dmd-shestm-cd2.avi",
                @"F:\Torrenti\FILMI\Sherlock Holmes (2009) - Sherlock Holmes\Sherlock.Holmes.2009.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Sherlock Holmes A Game of Shadows (2011) - Sherlock Holmes Igra senc\Sherlock Holmes A Game of Shadows (2011) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro.avi",
                @"F:\Torrenti\FILMI\Shrek Forever After (2010) - Shrek za vedno\Shrek.Forever.After.2010.SLOSubs.TS.XviD.AC3-ViSiON.avi",
                @"F:\Torrenti\FILMI\Shutter Island (2010) - Zlovešči otok\Shutter.Island.2010.SLOSubs.R5.LiNE.XviD-metalcamp.avi",
                @"F:\Torrenti\FILMI\Side Effects (2013) - Stranski učinki\Side.Effects.2013.720p.WEB-DL.X264-WEBiOS.mkv",
                @"F:\Torrenti\FILMI\Silver Linings Playbook (2012) - Za dežjem posije sonce\Silver Linings Playbook 2012 CROSubs.R5 XViD-PSiG.avi",
                @"F:\Torrenti\FILMI\Sin City (2005) - Mesto greha\Sin City.avi",
                @"F:\Torrenti\FILMI\Skyfall (2012) - Skyfall\Skyfall.2012.iNT.CUSTOM.SLOSUB.NTSC.DVDR-DrSi.iso",
                @"F:\Torrenti\FILMI\Slovenka (2009)\Slovenka.2009.SLOVENiAN.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Slumdog Millionaire (2008) - Revni milijonar\Slumdog.Millionaire.2008.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Snatch [2000] - Pljuni in jo stisni\Snatch.avi",
                @"F:\Torrenti\FILMI\Snow.White.and.the.Huntsman.2012.EXTENDED.SLOSubs.DVDRip.XviD-DrSi\Snow.White.and.the.Huntsman.2012.EXTENDED.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\Sorority Boys (2002) - Sestre\Sorority.Boys.DVDRip.2002-DEiTY.xvid.avi",
                @"F:\Torrenti\FILMI\Spartan (2004) - Spartanec\Spartan.2004.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\STAR TREK 2 The Wrath Of Khan (1982) - Zvezdne steze 2 Khanov srd\StarTrek_The_Wrath_of_Khan.MULTISUBS.PAL.DVDR-SiHQ.iso",
                @"F:\Torrenti\FILMI\STAR TREK 8 First Contact (1996) - Zvezdne steze 8 Prvi stik\StarTrek_First_Contact.MULTISUBS.PAL.DVDR.iso",
                @"F:\Torrenti\FILMI\Star Trek Into Darkness (2013) - Zvezdne steze V temo\Star Trek.avi",
                @"F:\Torrenti\FILMI\Stargate Continuum (2008) - Zvezdna vrata, Continuum\Stargate.Continuum.2008.SLOSubs.DVDRip.XviD-DrSi.cd1.avi",
                @"F:\Torrenti\FILMI\Stargate Continuum (2008) - Zvezdna vrata, Continuum\Stargate.Continuum.2008.SLOSubs.DVDRip.XviD-DrSi.cd2.avi",
                @"F:\Torrenti\FILMI\Stargate The Ark Of Truth (2008) - Zvezdna vrata, Skrinja resnice\Stargate_the_ark_of_truth.iso",
                @"F:\Torrenti\FILMI\Step Up 2 The Streets (2008) - Odpleši svoje sanje 2\Step.Up.2.(The.Streets).2008.cd1.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\Step Up 2 The Streets (2008) - Odpleši svoje sanje 2\Step.Up.2.(The.Streets).2008.cd2.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\Street Kings (2008) - Kralji ulice\Street.Kings.2008.R5.cd1.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\Street Kings (2008) - Kralji ulice\Street.Kings.2008.R5.cd2.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\Sucker Punch (2011) - Prikriti udarec\sample.avi",
                @"F:\Torrenti\FILMI\Sucker Punch (2011) - Prikriti udarec\Sucker Punch (2011) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro.avi",
                @"F:\Torrenti\FILMI\Superbad [2007] - Superhudo\Superbad[2007][Unrated Editon]DvDrip[Eng]-FXG.avi",
                @"F:\Torrenti\FILMI\Superhero Movie (2008) - Film o superjunaku\Superhero.Movie.2008.DVDRip.XviD-bRiP.avi",
                @"F:\Torrenti\FILMI\Syriana (2005) - Syriana\syriana1.avi",
                @"F:\Torrenti\FILMI\Syriana (2005) - Syriana\syriana2.avi",
                @"F:\Torrenti\FILMI\The Great Gatsby (2013) - Veliki Gatsby\The Great Gatsby (2013) - Veliki Gatsby.iso",
                @"F:\Torrenti\FILMI\The Hunger Games (2012) - Igre lakote\THE HUNGER GAMES.ISO",
                @"F:\Torrenti\FILMI\The Wolverine 2013 SLOSubs.EXTENDED BRRip XviD-ETRG\The Wolverine 2013 SLOSubs.EXTENDED BRRip XviD-ETRG.avi",
                @"F:\Torrenti\FILMI\The World's End  2013 SRBSubs.BRRip XViD-ETRG\The World's End  2013 SRBSubs.BRRip XViD-ETRG.avi",
                @"F:\Torrenti\FILMI\The.Hobbit.An.Unexpected.Journey.2012.SLOSubs.DVDSCRENER.XviD-metalcamp\The.Hobbit.An.Unexpected.Journey.2012.SLOSubs.DVDSCR.XviD-metalcamp.avi",
                @"F:\Torrenti\FILMI\The.Odd.Life.of.Timothy.Green.2012.SLOSubs.DVDRip.XviD-DrSi\The.Odd.Life.of.Timothy.Green.2012.SLOSubs.DVDRip.XviD-DrSi.avi",
                @"F:\Torrenti\FILMI\The.To.Do.List.2013.720p.BRRip.XviD.INSiDE\The.To.Do.List.2013.720p.BRRip.XviD.INSiDE.avi",
            };

            #endregion
        }

        private static void Main() {
            EntityFrameworkProfiler.Initialize();

            FileStream debugLog = File.Create("debugSearch.txt");
            Debug.Listeners.Add(new TextWriterTraceListener(debugLog));
            Debug.Listeners.Add(new ConsoleTraceListener());
            Debug.AutoFlush = true;

            //Test();
            TestMediaSearcher();
            //TestOpenSubtitlesProtocol();
            //TestDB();
            //TestMovie();

            Console.WriteLine(Filler);
            Console.WriteLine("\tFIN");
            Console.WriteLine(Filler);
            Console.Read();
        }

        private static void Test() {
            const string file = @"F:\Torrenti\FILMI\[REQ]Ne.Joči.Peter.1964.DVD-R\VIDEO_TS\VIDEO_TS.IFO";
            FileNameParser fnp = new FileNameParser(file, true);
            FileNameInfo info = fnp.Parse();
        }

        private static void TestMediaSearcher() {
            FeatureDetector ms = new FeatureDetector(@"E:\Torrenti\FILMI", @"F:\Torrenti\FILMI");
            ms.Search();
        }

        private static void TestMovie() {
            MovieVoContainer mvc = new MovieVoContainer();
            Movie mov = new Movie();
            mov.Title = "Amazing Grace";
            mvc.Movies.Add(mov);

            FileVo file = new FileVo("Amazing.Grace.2006.SLOSub.DvdRip.Xvid", "avi", @"E:\Torrenti\FILMI\Amazing.Grace.2006.SLOSub.DvdRip.Xvid\");
            Language lang = new Language("Slovene", "sl", "slv");
            mov.Subtitles.Add(new Subtitle(file, lang, "SubRip"));

            try {
                mvc.SaveChanges();
            }
            catch (DbEntityValidationException e) {
            }
        }

        private static void TestDB() {
            //Console.ForegroundColor = ConsoleColor.White;
            //SQLiteLog.Enabled = true;
            //SQLiteLog.Initialize();
            //SQLiteLog.Log += SQLiteLog_Log;


            TestFeatureDetectorVideoDebugDB();

            //try {
            //    mvc.SaveChanges();
            //}
            //catch (DbUpdateException e) {
            //    Exception innerException = e.InnerException.InnerException;
            //    IEnumerable<DbEntityEntry> entries = e.Entries;

            //    Console.Error.WriteLine();
            //    Console.Error.WriteLine(e.InnerException.InnerException);
            //}
            //catch (InvalidOperationException e) {
            //    Console.Error.WriteLine(e.Message);

            //    if (e.InnerException != null) {
            //        Console.Error.WriteLine(e.InnerException.Message);
            //    }
            //}
        }

        static void SQLiteLog_Log(object sender, LogEventArgs e) {
            Console.WriteLine(e.Message);
        }

        private static void TestFeatureDetectorVideoDebugDB() {
            FeatureDetector fd = new FeatureDetector();
            //const string fileName = @"E:\Torrenti\FILMI\The Big Lebowski (1998) - Veliki Lebowski\The Big Lebowski.1998.HDRip.x264-VLiS.mkv";
            //fd.Detect(fileName);
            DetectMany(fd);
        }

        private static void DetectMany(FeatureDetector fd) {
            int i = 0;
            foreach (string fileName in FileNames2) {
                try{
                    fd.Detect(fileName);

                    Console.WriteLine("{0}: \t{1}", ++i, Path.GetFileName(fileName));
                }
                catch(FileNotFoundException e){
                    if (File.Exists(e.FileName)) {
                        ConsoleColor clr = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine("FILE EXITS");

                        Console.ForegroundColor = clr;
                    }
                    else {
                        Console.WriteLine("{0}: \t{1} NOT FOUND", ++i, Path.GetFileName(fileName));
                    }
                }
            }
        }

        private static void DetectManyP(FeatureDetector fd) {
            Parallel.For(0, FileNames2.Length, i => {
                string fileName = FileNames2[i];
                try {
                    fd.Detect(fileName);

                    Console.WriteLine("{0}: \t{1}", ++i, Path.GetFileName(fileName));
                }
                catch (FileNotFoundException e) {
                    Console.WriteLine("{0}: \t{1} NOT FOUND", ++i, Path.GetFileName(fileName));
                }
            });
        }

        private static List<Movie> Deserialize(string fn) {
            JsonSerializer jser = new JsonSerializer();
            return jser.Deserialize<List<Movie>>(new JsonTextReader(new StreamReader(File.OpenRead(fn))));
        }

        private static void Serialize(IEnumerable<Movie> movies) {
            JsonSerializer jser = new JsonSerializer();

            using (FileStream fileStream = File.Create("movie_.js")) {
                using (StreamWriter streamWriter = new StreamWriter(fileStream)) {
                    using (JsonTextWriter writer = new JsonTextWriter(streamWriter)) {
                        jser.Serialize(writer, movies);
                    }
                }
            }
        }

        private static void TestTusParser() {
            PlanetTusClient pcli = new PlanetTusClient();
            pcli.Parse();
            pcli.PullAllMovieInfo();
            pcli.Serialize("tus_info.js");
        }

        private static void TestKolosejParser() {
            KolosejClient kcli = new KolosejClient();
            kcli.Parse();
            kcli.PullAllMovieInfo();
            kcli.Serialize("kolosej_info.js");
        }

        private static void TestPodnapisiSubtitlesProtocol() {
            PodnapisiNetClient pcli = new PodnapisiNetClient();
            LogInInfo inInfo = pcli.Session.Initiate("FMM");

            FilterInfo info = pcli.Session.AuthenticateAnonymous();

            FilterInfo filterInfo = pcli.Subtitles.SetFilters(LanguageId.Slovene);
            DownloadInfo downloadInfo = pcli.Subtitles.Download(200294);

            //200294
        }

        private static void TestOpenSubtitlesProtocol() {
            OpenSubtitlesClient cli = new OpenSubtitlesClient(false);
            cli.Session.LogInAnonymous("sl", "OS Test User Agent");
            ImdbMovieDetailsInfo imdbDetails = cli.Movie.GetImdbDetails(0993846);
        }

        private static TimeSpan TestFeatureDetector() {
            Stopwatch sw = Stopwatch.StartNew();
            IEnumerable<Movie> movies = TestFeatureDetectorVideoDebug();
            //IEnumerable<Movie> moviesA = TestFeatureDetectorVideoDebugAsyncP();
            sw.Stop();

            foreach (Movie movie in movies) {
                OutputMovie(movie);
                Debug.WriteLine(Filler);
            }
            return sw.Elapsed;
        }

        #region FeatureDetector

        private static IEnumerable<Movie> TestFeatureDetectorVideoDebug() {
            List<Movie> movies = new List<Movie>();

            FeatureDetector fd = new FeatureDetector();
            int i = 0;
            foreach (string fileName in FileNames2) {
                Movie info = fd.Detect(fileName);

                if (info != null) {
                    movies.Add(info);
                }

                Console.WriteLine("{0}: \t{1}", ++i, Path.GetFileName(fileName));
            }
            return movies;
        }

        private static void OutputMovie(Movie movie) {
            Video video = movie.Videos.FirstOrDefault();
            if (video != null) {
                OutputFileInfo(video.File);
            }

            OutputMovieInfo(movie);

            OutputVideo(movie.Videos.FirstOrDefault());
            OutputAudio(movie.Audios.FirstOrDefault());
            OutputSubtitles(movie.Subtitles);
            Debug.WriteLine(Filler);
        }

        private static void OutputMovieInfo(Movie movie) {
            Debug.WriteLine("Movie:");
            Debug.Indent();

            Debug.WriteLineIf(movie.Title != null, "Title: " + movie.Title);
            Debug.WriteLineIf(movie.OriginalTitle != null, "Original Title: " + movie.OriginalTitle);
            Debug.WriteLineIf(movie.SortTitle != null, "Sort Title: " + movie.SortTitle);

            Debug.WriteLineIf(movie.ReleaseYear != null, "Release Year: " + movie.ReleaseYear);
            //Debug.WriteLineIf(movie.ReleaseDate != default(DateTime), "Release Date: " + movie.ReleaseDate);

            Debug.WriteLineIf(!string.IsNullOrEmpty(movie.Edithion), "Edithion: " + movie.Edithion);
            Debug.WriteLineIf(movie.DvdRegion != DVDRegion.Unknown, "DVD Region: " + movie.DvdRegion);

            Debug.WriteLine("Play count: " + movie.PlayCount);
            //Debug.WriteLineIf(movie.LastPlayed != default(DateTime), "Last Played: " + movie.LastPlayed);
            //Debug.WriteLineIf(movie.LastPlayed != default(DateTime), "Premiered: " + movie.Premiered);
            //Debug.WriteLineIf(movie.Aired != default(DateTime), "Aired: " + movie.Aired);

            Debug.WriteLineIf(!string.IsNullOrEmpty(movie.Trailer), "Trailer: " + movie.Trailer);

            Debug.WriteLineIf(movie.Top250.HasValue, "Top250: " + movie.Top250);
            Debug.WriteLineIf(movie.Runtime.HasValue, "Runtime: " + movie.Runtime);
            Debug.WriteLine("Watched: " + movie.Watched);

            Debug.WriteLineIf(movie.RatingAverage.HasValue, "AVG Rating: " + movie.RatingAverage);
            Debug.WriteLineIf(!string.IsNullOrEmpty(movie.ImdbID), "ImdbID: " + movie.ImdbID);
            Debug.WriteLineIf(!string.IsNullOrEmpty(movie.TmdbID), "TmdbID: " + movie.TmdbID);
            Debug.WriteLineIf(!string.IsNullOrEmpty(movie.ReleaseGroup), "Release Group: " + movie.ReleaseGroup);
            Debug.WriteLineIf(movie.IsMultipart, "Is Multipart: " + movie.IsMultipart);
            Debug.WriteLineIf(!string.IsNullOrEmpty(movie.PartTypes), "Part Type: " + movie.PartTypes);
            Debug.WriteLineIf(movie.Specials.Count > 0, "Specials: " + string.Join(", ", movie.Specials));

            Debug.Unindent();
        }

        private static void OutputAudio(Audio a) {
            if (a == null) {
                return;
            }

            Debug.WriteLine("Audio:");
            Debug.Indent();

            Debug.WriteLineIf(a.Language != null, "Language: " + a.Language);
            Debug.WriteLineIf(!string.IsNullOrEmpty(a.Source), "Source: " + a.Source);
            Debug.WriteLineIf(!string.IsNullOrEmpty(a.Type), "Type: " + a.Source);

            Debug.WriteLineIf(a.NumberOfChannels.HasValue, "Number of channels: " + a.NumberOfChannels);
            Debug.WriteLineIf(!string.IsNullOrEmpty(a.ChannelSetup), "Channel Setup: " + a.ChannelSetup);
            Debug.WriteLineIf(!string.IsNullOrEmpty(a.ChannelPositions), "Channel Positions: " + a.ChannelPositions);

            Debug.WriteLineIf(!string.IsNullOrEmpty(a.Codec), "Codec: " + a.Codec);
            Debug.WriteLineIf(a.BitRate.HasValue, "BitRate: " + a.BitRate);
            Debug.WriteLineIf(a.BitRateMode != FrameOrBitRateMode.Unknown, "BitRate Mode: " + a.BitRateMode);
            Debug.WriteLineIf(a.SamplingRate.HasValue, "Sampling Rate: " + a.SamplingRate);
            Debug.WriteLineIf(a.CompressionMode != CompressionMode.Unknown, "Compression Mode: " + a.CompressionMode);
            Debug.WriteLineIf(a.Duration.HasValue,
                string.Format("Durration: {0} ({1:hh'h 'mm'm 'ss's 'fff'ms'})", a.Duration ?? 0,
                    TimeSpan.FromMilliseconds(a.Duration ?? 0)));

            Debug.Unindent();
        }

        private static void OutputSubtitles(IEnumerable<Subtitle> subtitles) {
            Debug.WriteLine("Subtitles:");
            Debug.Indent();

            foreach (Subtitle subtitle in subtitles) {
                Debug.WriteLine(subtitle);
            }

            Debug.Unindent();
        }

        private static void OutputVideo(Video video) {
            if (video == null) {
                return;
            }

            Debug.WriteLine("Video:");
            Debug.Indent();

            Debug.WriteLineIf(video.Language != null, "Language: " + video.Language);
            Debug.WriteLineIf(!string.IsNullOrEmpty(video.Source), "Source: " + video.Source);
            Debug.WriteLineIf(!string.IsNullOrEmpty(video.Type), "Type: " + video.Type);
            Debug.WriteLineIf(!string.IsNullOrEmpty(video.Resolution), "Resoulution: " + video.Resolution);
            Debug.WriteLineIf(video.FPS.HasValue, "FPS: " + video.FPS);
            Debug.WriteLineIf(video.BitRate.HasValue, "BitRate: " + video.BitRate);
            Debug.WriteLineIf(video.BitRateMode != FrameOrBitRateMode.Unknown, "BitRateMode: " + video.BitRateMode);
            Debug.WriteLineIf(video.BitDepth.HasValue, "BitDepth: " + video.BitDepth);
            Debug.WriteLineIf(video.CompressionMode != CompressionMode.Unknown, "CompressionMode: " + video.CompressionMode);

            Debug.WriteLineIf(video.Duration.HasValue,
                              string.Format("Durration: {0} ({1:hh'h 'mm'm 'ss's 'fff'ms'})",
                                            video.Duration ?? 0,
                                            TimeSpan.FromMilliseconds(video.Duration ?? 0)
                                           )
                              );

            Debug.WriteLine("ScanType: " + video.ScanType);
            Debug.WriteLineIf(!string.IsNullOrEmpty(video.ColorSpace), "ColorSpace: " + video.ColorSpace);
            Debug.WriteLineIf(!string.IsNullOrEmpty(video.ChromaSubsampling),
                "Chroma Subsampling: " + video.ChromaSubsampling);
            Debug.WriteLineIf(!string.IsNullOrEmpty(video.Codec), "Codec: " + video.Codec);
            Debug.WriteLineIf(video.Aspect.HasValue, "Aspect: " + video.Aspect);
            Debug.WriteLineIf(!string.IsNullOrEmpty(video.AspectCommercialName), "AspectName: " + video.AspectCommercialName);
            Debug.WriteLineIf(video.Width.HasValue, "Width: " + video.Width);
            Debug.WriteLineIf(video.Height.HasValue, "Height: " + video.Height);

            Debug.Unindent();
        }

        private static void OutputFileInfo(FileVo file) {
            if (file != null) {
                Debug.WriteLine("File: ");
                Debug.Indent();

                Debug.WriteLine("FileName: " + file.Name);
                Debug.WriteLine("Extension: " + file.Extension);
                Debug.WriteLine("FolderPath: " + file.FolderPath);
                Debug.WriteLineIf(file.Size != null, "FileSize: " + (file.Size ?? 0).FormatFileSizeAsString());

                Debug.Unindent();
            }
        }

        #endregion
    }

}