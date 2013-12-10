using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.DetectFeatures;
using Frost.DetectFeatures.Util;
using Frost.SharpCharsetDetector;
using Frost.SharpLanguageDetect;
using File = System.IO.File;

namespace Frost.Tester {

    internal class Program {
        private const string FOLDER_PATH = @"E:\Torrenti\FILMI\Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi\";

        private const string FILE_PATH = @"E:\Torrenti\FILMI\Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi\Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi.avi";
        private const string FILE_PATH2 = @"E:\Torrenti\FILMI\Intersections 2013 CROSubs.DVDRip XViD juggs\Intersections 2013 CROSubs.DVDRip XViD juggs.avi";
        private const string FILE_PATH3 = @"Z:\Filmi\(500) Days of Summer (2009) - 500 dni z Summer\(500)Days of Summer.[2009].RETAIL.DVDRIP.XVID.[Eng]-DUQA.avi";
        private const string FILE_PATH4 = @"E:\Torrenti\FILMI\Oz.the.Great.and.Powerful.2013.SLOSubs.DVDRip.XviD-DrSi\Oz.the.Great.and.Powerful.2013.SLOSubs.DVDRip.XviD-DrSi.avi";
        private const string FILE_PATH5 = @"E:\Torrenti\FILMI\The Wolverine 2013 SLOSubs.EXTENDED BRRip XviD-ETRG\The Wolverine 2013 SLOSubs.EXTENDED BRRip XviD-ETRG.avi";
        private const string FILE_PATH6 = @"Z:\Filmi\Avatar (2009) - Avatar\Avatar.2009.1080p.Slosubs.BluRay.DTS.x264-ESiR.mkv";
        private static readonly string[] FileNames;
        private static readonly List<string> SubtitleFiles;
        private static readonly string Filler;
        private static readonly List<string> LocalSubtitleFiles;

        static Program() {
            Filler = string.Join("", Enumerable.Repeat("_", Console.BufferWidth));

            #region LocalSubs

            LocalSubtitleFiles = new List<string> {
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\A Lot Like Love.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\A Separation 2011.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\A Single Man.2010.DVDRip.XviD-T0XiC.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\A Walk To Remember[2002].DvdRip.[yddam].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\After the Wedding 2006 DVDRip Xvid fasamoo LKRG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Alfie.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\All About Eve - hrvatski tilovi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\American Pie 2[2001].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\American Pie 2[2001]ENG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\American Pie 3 The Wedding[2003].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\American Pie 4 Band Camp[2005].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\American Psycho.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\American.Pie.5.The.Naked Mile.[2006].SLOsub.DvDrip[Eng]-BugZ.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\American.Pie.Presents.Beta.House.2007.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Amores.Perros.2000.SLOSubs.DVDRip.XviD-DrSi.cd1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Amores.Perros.2000.SLOSubs.DVDRip.XviD-DrSi.cd2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Anchorman The legend of Ron Burgundy[2004].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Another Year[2010]DvDrip[Eng]-FXG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Another Year[Eng][Subs].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Atonement.2007.cd1.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Atonement.2007.cd2.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Atonement.DVDRip.XviD-SVD.CD1.slo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Atonement.DVDRip.XviD-SVD.CD2.slo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\attack.the.block-done Eng.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\attack.the.block-done.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Avatar.2009.1080p.Slosubs.BluRay.DTS.x264-ESiR.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Babel[2006]DvDrip[Eng]-aXXo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Barney's Version 2010 720p BRRip x264 RmD (HDScene Release).srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\batman.begins-phrax.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Biutiful.2010.DVDRip.XviD.5rFF.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Black.Swan.2010.DVDSCR.XviD-ViSiON.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Blue.Valentine.2010.DvdScr.AC3.Xvid {1337x}-Noir.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\carre-my.sassy.girl-xvid.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Celda.211.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Charlie And The Chocolate Factory (2005).srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Children Of Men.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\City of God CD1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\City of God CD2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Cleaner.2007.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Confessions.2010.JAP.DVDRip.XviD-MOC hrvatski.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Confessions.2010.JAP.DVDRip.XviD-MOC.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Confucius.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Cowboys.And.Aliens.EXTENDED.2011.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Crash.2004.SLOSub.DVDRip.Xvid-DrSi.cd1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Crash.2004.SLOSub.DVDRip.Xvid-DrSi.cd2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\dash-fighting.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Dead Man Walking [English] 1995.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Dear.John.slo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\despicable.me.dvdrip.xvid-imbt.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Detachment[2011]BRRip XviD-ETRG -srbski.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Disgrace.2008.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\District 9 (2009) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Django.Unchained.2012.SLOSubs.DVDSCR.XviD-metalcamp.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Dom.za.vesanje.Xvid.DVD.QQZ.CD1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Dom.za.vesanje.Xvid.DVD.QQZ.CD2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Drive.2011.SLOSubs.DVDSCR.XviD.AC3.HQ.Hive-CM8.srt.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\dvl-eotsm.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Eat.Pray.Love.2010.DC.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Efter brylluppet.2006.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Elizabeth.1998.DVDRip.DivX-GarlicClan.slo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Eng.The Red Shoes.1948.DVDRip.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Everybodys.Fine.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Fantastic Mr. Fox.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Feast.of.Love.2007.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Felon.2008.cd1.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Felon.2008.cd2.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Fight.Club.1999.BRRip.XviD.AC3-FLAWL3SS.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Gone Baby Gone.[2007].DVDRIP.XVID.[Eng]-DUQA.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Gran.Torino.2008.DvDRip-FxM.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Grbavica.2006.DVDRip.XviD-XPTO.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Green.Zone.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Guess.Who.2005.SLOSub.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\hangover2.1080.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Happy.Go.Lucky.[2008.Eng].DVDRip.DivX-LTT.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Happy.Go.Lucky.2008.LIMITED.DVDRip.XviD-AMIABLE-cd1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Happy.Go.Lucky.2008.LIMITED.DVDRip.XviD-AMIABLE-cd2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Happy-Go-Lucky Eng.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Happy-Go-Lucky Rom.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Hard.Candy.DVDRip.XviD-DiAMOND.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\hdt.cloud.atlas.2012.1080p.bluray.x264.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Headhunters(2011)English(Revised).srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Heartbeats.2010.FESTiVAL.DVDRip.XviD-LAP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\How.to.Lose.Friends.and.Alienate.People.2008.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\HR5.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Hugo 2011.720p.BrRip.X264.YIFY.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Hugo.3D.2011.1080p.BluRay.Half.OU.DTS.x264-HDMaNiAcS.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\I'm.Not.There[2007]DvDrip[Eng]-aXXo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Im_Not_There_Retail.Slo s podnapisi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Imagine That.2009.DvdRip.Xvid {1337x}-Noir.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Incendies.2010.DVDRip.XviD.AC3.HORiZON-ArtSubs.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\IpMan -aot.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Iron.Man.2.2010.DVDRip.XviD.AC3-ViSiON.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Iron.Man.3.2013.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Iron.Man.2008.SLOSubs.DVDRip.XviD-DrSi.cd1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Iron.Man.2008.SLOSubs.DVDRip.XviD-DrSi.cd2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Its.Kind.of.a.Funny.Story.2010.SLOSubs.BRRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Julie.and.Julia.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Killers.2010.R5.LiNE.Xvid-Noir.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Knowing.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\La.Piel.que.Habito.2011.BDRip.720p.x264-boofer.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\La.Piel.Que.Habito.2011.DVDRip.AC3.HORiZON-ArtSubsSLO.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Lebanon.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Leon-The Professional 1994 BDrip[A Release-Lounge H.264 By Titan].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Les Amours Imaginaires 2010 [FESTiVAL.DVDRip.XviD-miguel] [ENG].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Limitless.UNRATED.2011.SLOSubs.BRRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Lincoln.2012.DVDSCR.XViD.AC3-MAGNAT - kopija.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Lincoln.2012.DVDSCR.XViD.AC3-MAGNAT.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Little.Big.Soldier.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Love Actually CD1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Love Actually CD2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Lucky Number Slevin.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Man.On.Wire.2008.DVDRip.XviD.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Mar.Adentro (The.Sea.Inside) 2004.DVDRip.XviD.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Melancholia.[2011].DVD.Rip.Xvid.[StB].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Midnight in Paris.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Milk[2008]DvDrip[Eng]-FXG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Milk[Eng][Subs].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Moon.2009.LiMiTED.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\nep-wtwta-scr.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Never.Let.Me.Go.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Noruwei no mori CD 1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Noruwei no mori CD 2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Norwegian.Wood.2010.JAP.DVDRip.XviD.AC3-BAUM_en.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Oblivion.2013.SLOSubs.DVDRip.XviD-DrSi - kopija.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Oblivion.2013.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Old.Dogs.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Once.2006.SLOSubs.DVDRip.XviD.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\P.S.I.Love.You.2007.SLOSubs.DVDRip.XviD-DrSi.cd1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\P.S.I.Love.You.2007.SLOSubs.DVDRip.XviD-DrSi.cd2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Paha.Perhe.aka.Bad.Family.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Pan's.Labyrinth[2006]DvDrip[Eng.Sub]-aXXo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Pan's.Labyrinth[2006]DvDrip[Eng.Sub]-aXXo_ENG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Paul.2011.DVDRip.XviD-DiVERSiTY.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Perfume.The.Story.Of.A.Murderer.2006.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Pirates.Of.The.Caribbean-At.World's.End[2007]DvDrip[Eng]-aXXo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Potovanje.Skozi.Plasti.Zemlje.Slosub.Xvid.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Premonition.2007.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Pride_and_Prejudice[2005]DvDrip[Eng]-aXXo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Prince.Of.Persia.The.Sands.Of.Time.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\psig-melancholia.2011.dvdrip.xvid.subtitles.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Pt(BR).The.Red.Shoes.1948.DVDRip.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Public.Enemies.2009.DvDRip-FxM.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Push[2009]DvDrip[Eng]-FXG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Red.2010.BRRip.XviD.AC3-MAGNAT.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Remember Me (2010).srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Rise of the Guardians (2012)R5 DVDRip NL subs (Divx)NLtoppers.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Robin.Hood.Unrated.DC.2010.BRRip.XviD.AC3-MAGNAT CRO.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Robin.Hood.Unrated.DC.2010.BRRip.XviD.AC3-MAGNAT SER.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Robin.Hood.Unrated.DC.2010.BRRip.XviD.AC3-MAGNAT.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\RocknRolla[2008]DvDrip-aXXo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\RocknRolla_EN_FXG_23.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Roger Dodger.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Rush.Hour.3.2007.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\S01E01 - A Study In Pink.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\S01E02 - The Blind Banker.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\S01E03 - The Great Game.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Scott.Pilgrim.vs.the.World.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Seven.Pounds.2008.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Seven.Psychopaths.2012.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\SEVEN[1995]DvDrip[Eng]-NikonXP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Sex.Drive.2008.UNRATED.DVDRip.XviD-ST4R.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\shame.2011.limited.dvdrip.xvid-amiable.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Sherlock.Holmes.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Shes.Out.Of.My.League.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\She's.The.Man.2006.XviD.AC3.CD1-WAF.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\She's.The.Man.2006.XviD.AC3.CD2-WAF.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Shrek.Forever.After.2010.SLOSubs.TS.XviD.AC3-ViSiON.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Shutter.Island.2010.CROSubs.R5.LiNE.XviD-metalcamp.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Shutter.Island.2010.SLO.CRO.Subs.R5.LiNE.XviD-metalcamp.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Silver Linings Playbook 2012 CROSubs.R5 XViD-PSiG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Sin City.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Slumdog.Millionaire.2008.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Snatch.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Spartan.2004.CROSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Spartan.2004.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Stargate.Continuum.2008.SLOSubs.DVDRip.XviD-DrSi.cd1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Stargate.Continuum.2008.SLOSubs.DVDRip.XviD-DrSi.cd2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Step.Up.2.(The.Streets).2008.cd1.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Step.Up.2.(The.Streets).2008.cd2.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Street.Kings.2008.R5.cd1.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Street.Kings.2008.R5.cd1.DVDRip.XviD-bRiP-ENG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Street.Kings.2008.R5.cd2.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Street.Kings.2008.R5.cd2.DVDRip.XviD-bRiP-ENG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Sucker Punch 2011 Extended Cut 720p BluRay x264-Japhson.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Superbad[2007][Unrated Editon]DvDrip[Eng]-FXG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Superhero.Movie.2008.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\syriana1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\syriana2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Takers 2010 480p BRRip XviD AC3-FLAWL3SS.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Taking Chance[2009]DvDrip[Eng]-FXG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Taking Chance[Eng][Subs].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Tangled.2010.SLOSiNHRO.BRRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\target-beginners-xvid.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\target-beginners-xvid-ENG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Big Lebowski.1998.HDRip.x264-VLiS.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Big Sleep - hrvatski tilovi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Breakfast Club [1985] DvdRip [Eng] - Thizz.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Curious Case of Benjamin Button[2008]DvDrip[Eng]-FXG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Curious Case of Benjamin Button[Eng][Subs].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Dark Knight[Eng][Subs].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Dark KnightThe Dark Knight[2008]DvDrip[Eng]-FXG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Football Factory.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Notebook (2004) [ENG] [DVDrip].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Pacifier.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The People vs Larry Flynt.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Prestige 2006 BRRip x264 AC3-TheFalcon007 (Kingdom-Release) [ENG] .srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Prestige 2006 BRRip x264 AC3-TheFalcon007 (Kingdom-Release).srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Red Shoes - hravatski 1 verzija.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Shawshank Redemption dvd rip Xvid.Rets.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Tourist[2010]DvDrip[Eng]-FXG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Usual Suspects(Xvid).srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The White Ribbon[2009]DvDrip[Ger]-FXG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The White Ribbon[Eng][Subs].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The Woman in Black 2012 BRRip XviD AbSurdiTy.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.American.2010.BRRip.XviD-Warrior.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Art.Of.War.II.Betrayal.2008.STV.SLOSubs.DVDRip.XviD-DrSi.cd1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Art.Of.War.II.Betrayal.2008.STV.SLOSubs.DVDRip.XviD-DrSi.cd2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Artist.2011.DVDSCR.XviD.ZOMBiES.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Beaver.2011.720p.SLOSubs.BRRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Big.Sleep.1946.DVDRip.H264.AAC.Gopo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Big.Sleep.1946.DVDRip.H264.AAC.Gopo_EN.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Blind.Side.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Boat.That.Rocked.2009.DvDRip-FxM.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Brothers.Bloom.2009.DvDRip-FxM.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Da.Vinci.Code.2006.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Expendables.2.2012.SLOSubs.BRRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Expendables.2010.SLOSubs.BRRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Fall[2006]DvDrip-aXXo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Guard.2011.LiMiTED.DVDRip.XviD-ViP3R.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Hangover.2009.UNRATED.SLOSubs.BRRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Horsemen[2009]DvDrip[Eng]-FXG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Hurt.Locker.2008.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Interpreter.(V.O).[DvDRip].[WwW.LiMiTeDiVx.CoM].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Killing.Room.2009.DVDRip.XviD-VoMiT.NoRar.www.crazy-torrent.com.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Kingdom.2007.cd1.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Kingdom.2007.cd2.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Kings.Speech.2010.NORDiC.DvDRip.x264-Makavalios.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Last.House.On.The.Left.UNRATED.DvDRip-FxM.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Orphanage.2007.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Rebound.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Red.Shoes.1948.720p.BluRay.x264-CiNEFiLE - hrvatski verzija 2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Score.2001.SLOSubs.DVDRip.XviD-DrSi.CD1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Score.2001.SLOSubs.DVDRip.XviD-DrSi.CD2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Skin.I.Live.In.2011.720p.Bluray.x264.anoXmous_eng.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Skin.I.Live.In.2011.720p.Bluray.x264.anoXmous_swe.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Skin.I.Live.In.2011.LIMITED.BDRip.XviD-DoNE-CD1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Skin.I.Live.In.2011.LIMITED.BDRip.XviD-DoNE-CD2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Smurfs.2011.SLOSubs.BRRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Social.Network.2010.DVDSCR.XViD-WBZ.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\The.Town.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\There.Will.Be.Blood.2007.SLOSubs.DVDRip.XviD-DrSi.cd1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\There.Will.Be.Blood.2007.SLOSubs.DVDRip.XviD-DrSi.cd2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Thick.As.Thieves[2009].srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Thick.As.Thieves[2009]DvDrip-aXXo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Traitor.2008.SLOSubs.DVDRip.XviD-aXXo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Transporter 2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Transporter 3 (2008) DVDRip-HALESPONGE.SLO.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Triage.2009.DVDRip.XviD-Jaybob.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Tristan.and.Isolde[2006]DvDrip[Eng]-aXXo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\True Grit (2010) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\True.Grit.2010.BluRay.1080p.DTS.x264-CHD.Slovenian.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Un.Prophete.2009.DVDRip.XviD.AC3.5.1.HORiZON.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\United.93[2006]DvDrip[Eng]-aXXo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Unthinkable.2010.EXTENDED.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Up.In.The.Air.2009.SLOSubs.DVDSCR.RERIP.XviD-CAMELOT.cd1.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Up.In.The.Air.2009.SLOSubs.DVDSCR.RERIP.XviD-CAMELOT.cd2.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\V.For.Vendetta[2005]DvDrip[Eng]-aXXo.srt.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Valkyrie.2008.R5.LINE.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Wall_E.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Wanted.2008.R5.cd1.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Wanted.2008.R5.cd1.DVDRip.XviD-bRiP-ENG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Wanted.2008.R5.cd2.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Wanted.2008.R5.cd2.DVDRip.XviD-bRiP-ENG.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\When Harry Met Sally.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\When.A.Man.Falls.In.The.Forest.2007.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Whip It Eng.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Whip It Rom.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Whip.It.720p.BluRay.x264.HAPPY.NEW.YEAR-METiS.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Witless.Protection.2008.cd1.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Witless.Protection.2008.cd2.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Wreck.It.Ralph.2012.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\You.Dont.Know.Jack.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Zodiac 2007.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Zodiac.2007.cd1.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Zodiac.2007.cd2.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\Zwartboek.2006.SLOSub.DVDRip.XviD-bRiP.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\(500)Days of Summer.[2009].RETAIL.DVDRIP.XVID.[Eng]-DUQA.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\9.Songs.2004.SLOSubs.DVDRip.XviD-DJTimi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\21.Grams.2003.PROPER.LiMiTED.DVDRip.XviD.DEiTY.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\21[2008]R5_DvDrip[Eng]-NikonXp.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\28.Weeks.Later.2007.DVDRip.XviD-DrSi.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\50.50.2011.DVDScr.XviD-playXD.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\88.Minutes.2007.DVDRip.Eng-aXXo.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\300 (2007).DVDSCR.XVID.srt",
                @"C:\Users\Martin\Desktop\FMM\MovieSubs\500.Days.Of.Summer.BDRip.XviD-ARiGOLD.srt",
                @"E:\Torrenti\FILMI\Girl.Most.Likely.2012.CROSubs.HDRip.XviD-S4A\Girl.Most.Likely.2012.CROSubs.HDRip.XviD-S4A.srt",
                @"E:\Torrenti\FILMI\Girl.Most.Likely.2012.CROSubs.HDRip.XviD-S4A\Girl.Most.Likely.2012.ENGSubs.HDRip.XviD-S4A.srt",
                @"E:\Torrenti\FILMI\Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi\Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi.srt",
                @"E:\Torrenti\FILMI\Bachelorette.2012.SLOSubs.DVDRip.XviD-DrSi\Bachelorette.2012.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"E:\Torrenti\FILMI\Oz.the.Great.and.Powerful.2013.SLOSubs.DVDRip.XviD-DrSi\Oz.the.Great.and.Powerful.2013.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"E:\Torrenti\FILMI\Trance.2013.SLOSubs.DVDRip.XviD-DrSi\Trance.2013.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"E:\Torrenti\FILMI\G.I.Joe.Retaliation.2013.SLOSubs.DVDRip.XviD-DrSi\G.I.Joe.Retaliation.2013.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"E:\Torrenti\FILMI\Total.Recall.Extended.2012.SLOSubs.BRRip.XviD-DrSi\Total.Recall.Extended.2012.SLOSubs.BRRip.XviD-DrSi.srt",
                @"E:\Torrenti\FILMI\Snow.White.and.the.Huntsman.2012.EXTENDED.SLOSubs.DVDRip.XviD-DrSi\Snow.White.and.the.Huntsman.2012.EXTENDED.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"E:\Torrenti\FILMI\Upside.Down.2012.SLOSubs.DVDRip.XviD-DrSi\Upside.Down.2012.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"E:\Torrenti\FILMI\The.Odd.Life.of.Timothy.Green.2012.SLOSubs.DVDRip.XviD-DrSi\The.Odd.Life.of.Timothy.Green.2012.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"E:\Torrenti\FILMI\Seeking.a.Friend.for.the.End.of.the.World.2012.SLOSubs.DVDRip.XviD-DrSi\Seeking.a.Friend.for.the.End.of.the.World.2012.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"E:\Torrenti\FILMI\Now You See Me 2013 SloSubs  EXTENDED BRRiP XViD UNiQUE\Now You See Me 2013 EXTENDED BRRiP XViD UNiQUE.srt",
                @"E:\Torrenti\FILMI\Intersections 2013 CROSubs.DVDRip XViD juggs\Intersections 2013 CROSubs.DVDRip XViD juggs.srt",
                @"E:\Torrenti\FILMI\Kick-Ass.2.2013.SRBSubs.R6.HDRip.XviD-S4A\Kick-Ass.2.2013.SRBSubs.R6.HDRip.XviD-S4A.srt",
                @"E:\Torrenti\FILMI\Pacific Rim 2013 CROSubs.HDCam NewAudio XviD Feel-Free\Pacific Rim 2013 CROSubs.HDCam NewAudio XviD Feel-Free.srt",
                @"E:\Torrenti\FILMI\R I P D 2013 WEBRiP XViD UNiQUE (SilverTorrent)\R.I.P.D 2013-hrv.srt",
                @"E:\Torrenti\FILMI\Rise of the Guardians (2012)R5 DVDRip NL subs (Divx)NLtoppers\Rise of the Guardians (2012)R5 DVDRip NL subs (Divx)NLtoppers.srt",
                @"E:\Torrenti\FILMI\A Serious Man (2009) - Zresni se\A.Serious.Man.2009.LiMiTED.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"E:\Torrenti\FILMI\Silver Linings Playbook (2012) - Za dežjem posije sonce\Silver Linings Playbook 2012 CROSubs.R5 XViD-PSiG.srt",
                @"E:\Torrenti\FILMI\The.Hobbit.An.Unexpected.Journey.2012.SLOSubs.DVDSCRENER.XviD-metalcamp\The.Hobbit.An.Unexpected.Journey.2012.SLOSubs.DVDSCR.XviD-metalcamp.srt",
                @"E:\Torrenti\FILMI\Alan Partridge Alpha Papa 2013 720p CROSubs.BrRip x264 - YIFY\Alan Partridge Alpha Papa 2013 720p CROSubs.BrRip x264 - YIFY.srt",
                @"E:\Torrenti\FILMI\The Wolverine 2013 SLOSubs.EXTENDED BRRip XviD-ETRG\The Wolverine 2013 SLOSubs.EXTENDED BRRip XviD-ETRG.srt",
                @"E:\Torrenti\FILMI\Escape.Plan.2013.SRBSubs.CAM.XviD-Tr0uNcE\Escape.Plan.2013.SRBSubs.CAM.XviD-Tr0uNcE.srt",
                @"E:\Torrenti\FILMI\The World's End  2013 SRBSubs.BRRip XViD-ETRG\The World's End  2013 SRBSubs.BRRip XViD-ETRG.srt",
                @"E:\Torrenti\FILMI\The World's End  2013 SRBSubs.BRRip XViD-ETRG\The World's End  2013 ENGSubs.BRRip XViD-ETRG.srt",
                @"E:\Torrenti\FILMI\The.To.Do.List.2013.720p.BRRip.XviD.INSiDE\The.To.Do.List.2013.720p.BRRip.XviD.INSiDE.srt",
                @"E:\Torrenti\FILMI\Paranoia 2013 CROSubs.BRRip XViD-ETRG\Paranoia 2013 CROSubs.BRRip XViD-ETRG.srt",
                @"E:\Torrenti\FILMI\Paranoia 2013 CROSubs.BRRip XViD-ETRG\Paranoia 2013 ENGSubs.BRRip XViD-ETRG.srt",
                @"E:\Torrenti\FILMI\Hysteria.2011.SLOSubs.DVDRip.XviD-DrSi\Hysteria.2011.SLOSubs.DVDRip.XviD-DrSi.srt",
            };

            #endregion

            #region SubsFiles

            SubtitleFiles = new List<string> {
                @"Z:\Filmi\28 Weeks Later (2007) - 28 tednov pozneje\28.Weeks.Later.2007.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\28 Weeks Later (2007) - 28 tednov pozneje\ENG\28.Weeks.Later.2007.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\50 50 (2011) - 50 50\50.50.2011.DVDScr.XviD-playXD.srt",
                @"Z:\Filmi\88 Minutes (2007) - 88 minut\88.Minutes.2007.DVDRip.Eng-aXXo.srt",
                @"Z:\Filmi\300 (2006) - 300\300 (2007).DVDSCR.XVID.srt",
                @"Z:\Filmi\A Lot Like Love (2005) - Več kot ljubezen\A Lot Like Love.srt",
                @"Z:\Filmi\A Separation (2011) - Ločitev\A Separation 2011.srt",
                @"Z:\Filmi\A Single Man (2010) - Samski moški\A Single Man.2010.DVDRip.XviD-T0XiC.srt",
                @"Z:\Filmi\A Walk To Remember [2002] - Spomin v srcu\A Walk To Remember[2002].DvdRip.[yddam].srt",
                @"Z:\Filmi\ALFIE (2004) - Alfie\Alfie.srt",
                @"Z:\Filmi\All About Eve (1950) - Vse o Evi\All About Eve - hrvatski tilovi.srt",
                @"Z:\Filmi\American Pie 2 [2001] - Ameriška pita 2\American Pie 2[2001]ENG.srt",
                @"Z:\Filmi\American Pie 2 [2001] - Ameriška pita 2\American Pie 2[2001].srt",
                @"Z:\Filmi\American Pie 3 The Wedding [2003] - Ameriška pita 3 Poroka\American Pie 3 The Wedding[2003].srt",
                @"Z:\Filmi\American Pie 4 Band Camp [2005] - Ameriška pita 4 Glasbeni tabor\American Pie 4 Band Camp[2005].srt",
                @"Z:\Filmi\American Pie 5 The Naked Mile [2006] - Ameriška pita 5 Gola milija\American.Pie.5.The.Naked Mile.[2006].SLOsub.DvDrip[Eng]-BugZ.srt",
                @"Z:\Filmi\American Pie Presents Beta House (2007) - Ameriška pita 6\ENG\American.Pie.Presents.Beta.House.2007.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\American Pie Presents Beta House (2007) - Ameriška pita 6\American.Pie.Presents.Beta.House.2007.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\American Psycho (Uncut) (2000) - Ameriški psiho\American Psycho.srt",
                @"Z:\Filmi\Amores Perros (2000) - Pasja ljubezen\Amores.Perros.2000.SLOSubs.DVDRip.XviD-DrSi.cd1.srt",
                @"Z:\Filmi\Amores Perros (2000) - Pasja ljubezen\Amores.Perros.2000.SLOSubs.DVDRip.XviD-DrSi.cd2.srt",
                @"Z:\Filmi\Anchorman The legend of Ron Burgundy [2004] - Anchorman\ENG\Anchorman The legend of Ron Burgundy[2004].srt",
                @"Z:\Filmi\Anchorman The legend of Ron Burgundy [2004] - Anchorman\Anchorman The legend of Ron Burgundy[2004].srt",
                @"Z:\Filmi\Another Year (2010) - Se eno leto\Another Year[Eng][Subs].srt",
                @"Z:\Filmi\Another Year (2010) - Se eno leto\Another Year[2010]DvDrip[Eng]-FXG.srt",
                @"Z:\Filmi\Atonement (2007) - Pokora\Atonement.2007.cd1.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Atonement (2007) - Pokora\ENG\Atonement.2007.cd1.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Atonement (2007) - Pokora\ENG\Atonement.2007.cd2.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Atonement (2007) - Pokora\Atonement.2007.cd2.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Attack the Block - Napad na blok\attack.the.block-done.srt",
                @"Z:\Filmi\Attack the Block - Napad na blok\attack.the.block-done Eng.srt",
                @"Z:\Filmi\Avatar (2009) - Avatar\Avatar.2009.1080p.Slosubs.BluRay.DTS.x264-ESiR.srt",
                @"Z:\Filmi\Babel (2006) - Babilon\Babel[2006]DvDrip[Eng]-aXXo.srt",
                @"Z:\Filmi\Babel (2006) - Babilon\ENG\Babel[2006]DvDrip[Eng]-aXXo.srt",
                @"Z:\Filmi\Barney's Version (2010) - Barneyjeva različica\Barney's Version 2010 720p BRRip x264 RmD (HDScene Release).srt",
                @"Z:\Filmi\Batman Begins (2005) - Batman na začetku\batman.begins-phrax.srt",
                @"Z:\Filmi\Beginners (2011) - Začetniki\target-beginners-xvid.srt",
                @"Z:\Filmi\Beginners (2011) - Začetniki\target-beginners-xvid-ENG.srt",
                @"Z:\Filmi\Biutiful (2010) - Biutiful\Biutiful.2010.DVDRip.XviD.5rFF.srt",
                @"Z:\Filmi\Black Swan (2010) - Crni labod\Black.Swan.2010.DVDSCR.XviD-ViSiON.srt",
                @"Z:\Filmi\Blue Valentine (2010) - Blue Valentine\Blue.Valentine.2010.DvdScr.AC3.Xvid {1337x}-Noir.srt",
                @"Z:\Filmi\Cell 211 (2009) - Celica 211\Celda.211.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Charlie And The Chocolate Factory (2005) - Carli in tovarna cokolade\Charlie And The Chocolate Factory (2005).srt",
                @"Z:\Filmi\Children Of Men - Otroci clovestva\Children Of Men.srt",
                @"Z:\Filmi\City of God (2006) - Božje mesto\City of God CD1.srt",
                @"Z:\Filmi\City of God (2006) - Božje mesto\City of God CD2.srt",
                @"Z:\Filmi\Cleaner (2007) - Cistilec\Cleaner.2007.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Cloud Atlas (2012) - Atlas oblakov\hdt.cloud.atlas.2012.1080p.bluray.x264.srt",
                @"Z:\Filmi\Confucius (2010) - Konfucij\Confucius.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Cowboys And Aliens (2011) - Kavboji in vesoljci\Cowboys.And.Aliens.EXTENDED.2011.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Crash (2004) - Usodna nesreča\Crash.2004.SLOSub.DVDRip.Xvid-DrSi.cd1.srt",
                @"Z:\Filmi\Crash (2004) - Usodna nesreča\Crash.2004.SLOSub.DVDRip.Xvid-DrSi.cd2.srt",
                @"Z:\Filmi\Das Weisse Band (2008) - Beli trak\The White Ribbon[Eng][Subs].srt",
                @"Z:\Filmi\Das Weisse Band (2008) - Beli trak\The White Ribbon[2009]DvDrip[Ger]-FXG.srt",
                @"Z:\Filmi\Dead Man Walking (1995) - Zadnji sprehod\Dead Man Walking [English] 1995.srt",
                @"Z:\Filmi\Dear John (2010) - Samo tebe si zelim\Dear.John.slo.srt",
                @"Z:\Filmi\Despicable Me (2010) - Jaz, baraba\despicable.me.dvdrip.xvid-imbt.srt",
                @"Z:\Filmi\Detachment (2011) - Odtujenost\Detachment[2011]BRRip XviD-ETRG -srbski.srt",
                @"Z:\Filmi\Disgrace (2008) - Sramota\Disgrace.2008.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\District 9 (2009) - Okrožje 9\District 9 (2009) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro.srt",
                @"Z:\Filmi\Django Unchained (2012) - Django brez okov\Django.Unchained.2012.SLOSubs.DVDSCR.XviD-metalcamp.srt",
                @"Z:\Filmi\Drive (2011) - Vožnja\Drive.2011.SLOSubs.DVDSCR.XviD.AC3.HQ.Hive-CM8.srt.srt",
                @"Z:\Filmi\Eat Pray Love (2010) - Jej, moli, ljubi\Eat.Pray.Love.2010.DC.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Efter brylluppet (2006) - Po poroki\After the Wedding 2006 DVDRip Xvid fasamoo LKRG.srt",
                @"Z:\Filmi\Efter brylluppet (2006) - Po poroki\Efter brylluppet.2006.srt",
                @"Z:\Filmi\El Orfanato (2007) - Sirotišnica\The.Orphanage.2007.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Elizabeth (1998) - Elizabeta\Elizabeth.1998.DVDRip.DivX-GarlicClan.slo.srt",
                @"Z:\Filmi\Eternal Sunshine Of The Spotless Mind (2004) - Večno sonce brezmadežnega uma\dvl-eotsm.srt",
                @"Z:\Filmi\Everybody's Fine (2009) - Vsi so vredu\Everybodys.Fine.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Fantastic Mr Fox - Fantastični gospod Lisjak\Fantastic Mr. Fox.srt",
                @"Z:\Filmi\Feast of Love (2007) - Praznik ljubezni\Feast.of.Love.2007.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Felon (2008) - Zločinec\Felon.2008.cd1.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Felon (2008) - Zločinec\Felon.2008.cd2.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Felon (2008) - Zločinec\Sub.ENG\Felon.2008.cd1.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Felon (2008) - Zločinec\Sub.ENG\Felon.2008.cd2.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Fight Club (1999) - Klub golih pesti\Fight.Club.1999.BRRip.XviD.AC3-FLAWL3SS.srt",
                @"Z:\Filmi\Fighting (2009) - Borba\dash-fighting.srt",
                @"Z:\Filmi\Gone Baby Gone (2007) - Zbogom, punčka\Gone Baby Gone.[2007].DVDRIP.XVID.[Eng]-DUQA.srt",
                @"Z:\Filmi\Gone Baby Gone (2007) - Zbogom, punčka\ENG\Gone Baby Gone.[2007].DVDRIP.XVID.[Eng]-DUQA.srt",
                @"Z:\Filmi\Gran Torino (2008) - Gran Torino\Gran.Torino.2008.DvDRip-FxM.srt",
                @"Z:\Filmi\Grbavica (2006) - Grbavica\Grbavica.2006.DVDRip.XviD-XPTO.srt",
                @"Z:\Filmi\Green Zone (2010) - Zelena cona\Green.Zone.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Guess Who (2005) - Ugani kdo\Guess.Who.2005.SLOSub.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Hancock (2008) - Hancock\HR5.srt",
                @"Z:\Filmi\Happy-Go-Lucky (2008) - Kar-brez-skrbi\Subs\Happy-Go-Lucky Eng.srt",
                @"Z:\Filmi\Happy-Go-Lucky (2008) - Kar-brez-skrbi\Subs\Happy-Go-Lucky Rom.srt",
                @"Z:\Filmi\Happy-Go-Lucky (2008) - Kar-brez-skrbi\Happy.Go.Lucky.[2008.Eng].DVDRip.DivX-LTT.srt",
                @"Z:\Filmi\Happy-Go-Lucky (2008) - Kar-brez-skrbi\Happy.Go.Lucky.2008.LIMITED.DVDRip.XviD-AMIABLE-cd1.srt",
                @"Z:\Filmi\Happy-Go-Lucky (2008) - Kar-brez-skrbi\Happy.Go.Lucky.2008.LIMITED.DVDRip.XviD-AMIABLE-cd2.srt",
                @"Z:\Filmi\Hard Candy (2006) - Prepovedan sadež\Hard.Candy.DVDRip.XviD-DiAMOND.srt",
                @"Z:\Filmi\Headhunters (2011) - Lovci na glave\Headhunters(2011)English(Revised).srt",
                @"Z:\Filmi\How to Lose Friends and Alienate People (2008) - Kako izgubiti prijatelje in odtujiti ljudi\How.to.Lose.Friends.and.Alienate.People.2008.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Hugo (2011) - Hugo\Hugo 2011.720p.BrRip.X264.YIFY.srt",
                @"Z:\Filmi\Hugo (2011) - Hugo\Hugo.3D.2011.1080p.BluRay.Half.OU.DTS.x264-HDMaNiAcS.srt",
                @"Z:\Filmi\I'm Not There (2007) - Bob Dylan 7 obrazov\I'm.Not.There[2007]DvDrip[Eng]-aXXo.srt",
                @"Z:\Filmi\I'm Not There (2007) - Bob Dylan 7 obrazov\Im_Not_There_Retail.Slo s podnapisi.srt",
                @"Z:\Filmi\Imagine That (2009) - Predstavljaj si to\Imagine That.2009.DvdRip.Xvid {1337x}-Noir.srt",
                @"Z:\Filmi\Incendies (2010) - Zenska, ki poje\Incendies.2010.DVDRip.XviD.AC3.HORiZON-ArtSubs.srt",
                @"Z:\Filmi\Ip Man [2008] - Ip Man\IpMan -aot.srt",
                @"Z:\Filmi\Ip Man [2008] - Ip Man\OLD\IpMan -aot.srt",
                @"Z:\Filmi\Iron Man (2008) - Iron Man\Iron.Man.2008.SLOSubs.DVDRip.XviD-DrSi.cd2.srt",
                @"Z:\Filmi\Iron Man (2008) - Iron Man\Iron.Man.2008.SLOSubs.DVDRip.XviD-DrSi.cd1.srt",
                @"Z:\Filmi\Iron Man 2 (2010) - Iron Man 2\Iron.Man.2.2010.DVDRip.XviD.AC3-ViSiON.srt",
                @"Z:\Filmi\Iron Man 3 (2013) - Iron Man 3\Iron.Man.3.2013.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\It's Kind of a Funny Story (2010) - It's Kind of a Funny Story\Its.Kind.of.a.Funny.Story.2010.SLOSubs.BRRip.XviD-DrSi.srt",
                @"Z:\Filmi\Julie and Julia (2009) - Julie in Julia\Julie.and.Julia.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Killers (2010) - Morilci\Killers.2010.R5.LiNE.Xvid-Noir.srt",
                @"Z:\Filmi\Knowing (2009) - Prerokba\Knowing.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Kokuhaku Confessions (2010) - Priznanja\Confessions.2010.JAP.DVDRip.XviD-MOC.srt",
                @"Z:\Filmi\Kokuhaku Confessions (2010) - Priznanja\Confessions.2010.JAP.DVDRip.XviD-MOC hrvatski.srt",
                @"Z:\Filmi\La piel que habito (2011) - Koža v kateri zivim\The.Skin.I.Live.In.2011.720p.Bluray.x264.anoXmous_eng.srt",
                @"Z:\Filmi\La piel que habito (2011) - Koža v kateri zivim\The.Skin.I.Live.In.2011.720p.Bluray.x264.anoXmous_swe.srt",
                @"Z:\Filmi\La piel que habito (2011) - Koža v kateri zivim\La.Piel.que.Habito.2011.BDRip.720p.x264-boofer.srt",
                @"Z:\Filmi\La piel que habito (2011) - Koža v kateri zivim\La.Piel.Que.Habito.2011.DVDRip.AC3.HORiZON-ArtSubsSLO.srt",
                @"Z:\Filmi\La piel que habito (2011) - Koža v kateri zivim\The.Skin.I.Live.In.2011.LIMITED.BDRip.XviD-DoNE-CD1.srt",
                @"Z:\Filmi\La piel que habito (2011) - Koža v kateri zivim\The.Skin.I.Live.In.2011.LIMITED.BDRip.XviD-DoNE-CD2.srt",
                @"Z:\Filmi\Lebanon (2009) - Libanon\Lebanon.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Leon-The Professional (19949 - Leon\Leon-The Professional 1994 BDrip[A Release-Lounge H.264 By Titan].srt",
                @"Z:\Filmi\Les Amours Imaginaires (2010) - Namišljene ljubezni\Heartbeats.2010.FESTiVAL.DVDRip.XviD-LAP.srt",
                @"Z:\Filmi\Les Amours Imaginaires (2010) - Namišljene ljubezni\Les Amours Imaginaires 2010 [FESTiVAL.DVDRip.XviD-miguel] [ENG].srt",
                @"Z:\Filmi\Limitless (2011) - Odklenjen\Limitless.UNRATED.2011.SLOSubs.BRRip.XviD-DrSi.srt",
                @"Z:\Filmi\Lincoln (2012) - Lincoln\Lincoln.2012.DVDSCR.XViD.AC3-MAGNAT - kopija.srt",
                @"Z:\Filmi\Lincoln (2012) - Lincoln\Lincoln.2012.DVDSCR.XViD.AC3-MAGNAT.srt",
                @"Z:\Filmi\Little Big Soldier (2010) - Mali veliki vojak\Little.Big.Soldier.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Love Actually (2003) - Pravzaprav ljubezen\Love Actually CD1.srt",
                @"Z:\Filmi\Love Actually (2003) - Pravzaprav ljubezen\Love Actually CD2.srt",
                @"Z:\Filmi\Lucky Number Slevin (2006) - Srečnež Slevin\Lucky Number Slevin.srt",
                @"Z:\Filmi\Man On Wire (2008) - Clovek na zici\Man.On.Wire.2008.DVDRip.XviD.srt",
                @"Z:\Filmi\Mar Adentro (2004) - Morje v meni\Mar.Adentro (The.Sea.Inside) 2004.DVDRip.XviD.srt",
                @"Z:\Filmi\Melancholia (2011) - Melanholija\Subs\psig-melancholia.2011.dvdrip.xvid.subtitles.srt",
                @"Z:\Filmi\Melancholia (2011) - Melanholija\Melancholia.[2011].DVD.Rip.Xvid.[StB].srt",
                @"Z:\Filmi\Midnight in Paris (2011) - Polnoč v Parizu\Midnight in Paris.srt",
                @"Z:\Filmi\Milk [2008] - Milk\ENG\Milk[Eng][Subs].srt",
                @"Z:\Filmi\Milk [2008] - Milk\Milk[2008]DvDrip[Eng]-FXG.srt",
                @"Z:\Filmi\Moon (2009) - Luna\Moon.2009.LiMiTED.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\My Sassy Girl (2008)  - Moja cudna punca\ENG\carre-my.sassy.girl-xvid.srt",
                @"Z:\Filmi\Never Let Me Go (2010) - Ne zapusti me nikdar\Never.Let.Me.Go.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Norwegian Wood (2010) - Norveški gozd\Norwegian.Wood.2010.JAP.DVDRip.XviD.AC3-BAUM_en.srt",
                @"Z:\Filmi\Norwegian Wood (2010) - Norveški gozd\Noruwei no mori CD 1.srt",
                @"Z:\Filmi\Norwegian Wood (2010) - Norveški gozd\Noruwei no mori CD 2.srt",
                @"Z:\Filmi\Oblivion (2013) - Pozaba\Oblivion.2013.SLOSubs.DVDRip.XviD-DrSi - kopija.srt",
                @"Z:\Filmi\Oblivion (2013) - Pozaba\Oblivion.2013.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Old Dogs (2009) - Stara mačka\Old.Dogs.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Once (2006) - Enkrat\Once.2006.SLOSubs.DVDRip.XviD.srt",
                @"Z:\Filmi\P.S. I Love You (2007) - P.S. Ljubim te\P.S.I.Love.You.2007.SLOSubs.DVDRip.XviD-DrSi.cd1.srt",
                @"Z:\Filmi\P.S. I Love You (2007) - P.S. Ljubim te\P.S.I.Love.You.2007.SLOSubs.DVDRip.XviD-DrSi.cd2.srt",
                @"Z:\Filmi\Paha Perhe aka Bad Family (2010) - Slaba družina\Paha.Perhe.aka.Bad.Family.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Pan's Labyrinth (2006) - Panov labirint\Pan's.Labyrinth[2006]DvDrip[Eng.Sub]-aXXo.srt",
                @"Z:\Filmi\Pan's Labyrinth (2006) - Panov labirint\Pan's.Labyrinth[2006]DvDrip[Eng.Sub]-aXXo_ENG.srt",
                @"Z:\Filmi\Paul (2011) - Paul\Paul.2011.DVDRip.XviD-DiVERSiTY.srt",
                @"Z:\Filmi\People vs Larry Flynt (1996) - Ljudstvo proti Larryju Flintu\The People vs Larry Flynt.srt",
                @"Z:\Filmi\Perfume The Story Of A Murderer (2006) - Parfum Zgodba morilca\Perfume.The.Story.Of.A.Murderer.2006.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Pirates of the Carribean At World's End (2007) - Pirati s Karibov Na robu sveta\Pirates.Of.The.Caribbean-At.World's.End[2007]DvDrip[Eng]-aXXo.srt",
                @"Z:\Filmi\Potovanje skozi plasti Zemlje (TV 2006)\Potovanje.Skozi.Plasti.Zemlje.Slosub.Xvid.srt",
                @"Z:\Filmi\Premonition (2007) - Slutnja\Premonition.2007.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Pride and Prejudice (2005) - Prevzetnost in pristranost\Pride_and_Prejudice[2005]DvDrip[Eng]-aXXo.srt",
                @"Z:\Filmi\Prince Of Persia The Sands Of Time (2010)  - Perzijski princ Sipine casa\Prince.Of.Persia.The.Sands.Of.Time.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Public Enemies (2009) - Državni sovražniki\Public.Enemies.2009.DvDRip-FxM.srt",
                @"Z:\Filmi\Push [2009] - Udarec\ENG\Push[2009]DvDrip[Eng]-FXG.srt",
                @"Z:\Filmi\Push [2009] - Udarec\Push[2009]DvDrip[Eng]-FXG.srt",
                @"Z:\Filmi\Red (2010) - Upokojeni, oboroženi, nevarni (Red)\Red.2010.BRRip.XviD.AC3-MAGNAT.srt",
                @"Z:\Filmi\Remember Me (2010) - Ne pozabi me\Remember Me (2010).srt",
                @"Z:\Filmi\Rise of the Guardians (2012) - Pet legend\Rise of the Guardians (2012)R5 DVDRip NL subs (Divx)NLtoppers.srt",
                @"Z:\Filmi\Robin Hood (2010) - Robin Hood\Robin.Hood.Unrated.DC.2010.BRRip.XviD.AC3-MAGNAT.srt",
                @"Z:\Filmi\Robin Hood (2010) - Robin Hood\Robin.Hood.Unrated.DC.2010.BRRip.XviD.AC3-MAGNAT CRO.srt",
                @"Z:\Filmi\Robin Hood (2010) - Robin Hood\Robin.Hood.Unrated.DC.2010.BRRip.XviD.AC3-MAGNAT SER.srt",
                @"Z:\Filmi\RocknRolla [2008] - Rocknroller\ENG\RocknRolla_EN_FXG_23.srt",
                @"Z:\Filmi\RocknRolla [2008] - Rocknroller\RocknRolla[2008]DvDrip-aXXo.srt",
                @"Z:\Filmi\Roger Dodger (2002) - Roger Dodger\Roger Dodger.srt",
                @"Z:\Filmi\Rush Hour 3 (2007) - Ful gas 3\Rush.Hour.3.2007.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Scott Pilgrim vs. the World (2010) - Scott Pilgrim proti vsem\Scott.Pilgrim.vs.the.World.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\SEVEN [1995] - Sedem\SEVEN[1995]DvDrip[Eng]-NikonXP.srt",
                @"Z:\Filmi\Seven Pounds (2008) - Sedem duš\Seven.Pounds.2008.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Seven Psychopaths (2012) - Sedem psihopatov\Seven.Psychopaths.2012.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Sex Drive (2008) - Ljubezenski klic\Sex.Drive.2008.UNRATED.DVDRip.XviD-ST4R.srt",
                @"Z:\Filmi\Sex Drive (2008) - Ljubezenski klic\Subs.ENG\Sex.Drive.2008.UNRATED.DVDRip.XviD-ST4R.srt",
                @"Z:\Filmi\Shame (2011) - Sramota\shame.2011.limited.dvdrip.xvid-amiable.srt",
                @"Z:\Filmi\Sherlock (20010) TV Series Season 1\S01E01 - A Study In Pink.srt",
                @"Z:\Filmi\Sherlock (20010) TV Series Season 1\S01E02 - The Blind Banker.srt",
                @"Z:\Filmi\Sherlock (20010) TV Series Season 1\S01E03 - The Great Game.srt",
                @"Z:\Filmi\Sherlock Holmes (2009) - Sherlock Holmes\Sherlock.Holmes.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\She's Out Of My League (2010) - Predobra zame\Shes.Out.Of.My.League.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\She's The Man(2006) - Mlada nogometašica\She's.The.Man.2006.XviD.AC3.CD1-WAF.srt",
                @"Z:\Filmi\She's The Man(2006) - Mlada nogometašica\She's.The.Man.2006.XviD.AC3.CD2-WAF.srt",
                @"Z:\Filmi\Shrek Forever After (2010) - Shrek za vedno\Shrek.Forever.After.2010.SLOSubs.TS.XviD.AC3-ViSiON.srt",
                @"Z:\Filmi\Shutter Island (2010) - Zlovešči otok\Shutter.Island.2010.CROSubs.R5.LiNE.XviD-metalcamp.srt",
                @"Z:\Filmi\Shutter Island (2010) - Zlovešči otok\Shutter.Island.2010.SLO.CRO.Subs.R5.LiNE.XviD-metalcamp.srt",
                @"Z:\Filmi\Silver Linings Playbook (2012) - Za dežjem posije sonce\Silver Linings Playbook 2012 CROSubs.R5 XViD-PSiG.srt",
                @"Z:\Filmi\Sin City (2005) - Mesto greha\Sin City.srt",
                @"Z:\Filmi\Slumdog Millionaire (2008) - Revni milijonar\Slumdog.Millionaire.2008.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Snatch [2000] - Pljuni in jo stisni\Snatch.srt",
                @"Z:\Filmi\Spartan (2004) - Spartanec\Spartan.2004.CROSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Spartan (2004) - Spartanec\Spartan.2004.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Stargate Continuum (2008) - Zvezdna vrata, Continuum\Stargate.Continuum.2008.SLOSubs.DVDRip.XviD-DrSi.cd1.srt",
                @"Z:\Filmi\Stargate Continuum (2008) - Zvezdna vrata, Continuum\Stargate.Continuum.2008.SLOSubs.DVDRip.XviD-DrSi.cd2.srt",
                @"Z:\Filmi\Step Up 2 The Streets (2008) - Odpleši svoje sanje 2\Step.Up.2.(The.Streets).2008.cd1.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Step Up 2 The Streets (2008) - Odpleši svoje sanje 2\Step.Up.2.(The.Streets).2008.cd2.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Step Up 2 The Streets (2008) - Odpleši svoje sanje 2\Sub.ENG\Step.Up.2.(The.Streets).2008.cd1.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Step Up 2 The Streets (2008) - Odpleši svoje sanje 2\Sub.ENG\Step.Up.2.(The.Streets).2008.cd2.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Street Kings (2008) - Kralji ulice\Street.Kings.2008.R5.cd1.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Street Kings (2008) - Kralji ulice\Street.Kings.2008.R5.cd2.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Street Kings (2008) - Kralji ulice\ENG\Street.Kings.2008.R5.cd2.DVDRip.XviD-bRiP-ENG.srt",
                @"Z:\Filmi\Street Kings (2008) - Kralji ulice\ENG\Street.Kings.2008.R5.cd1.DVDRip.XviD-bRiP-ENG.srt",
                @"Z:\Filmi\Sucker Punch (2011) - Prikriti udarec\Sucker Punch 2011 Extended Cut 720p BluRay x264-Japhson.srt",
                @"Z:\Filmi\Superbad [2007] - Superhudo\Superbad[2007][Unrated Editon]DvDrip[Eng]-FXG.srt",
                @"Z:\Filmi\Superhero Movie (2008) - Film o superjunaku\Sub.ENG\Superhero.Movie.2008.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Superhero Movie (2008) - Film o superjunaku\Superhero.Movie.2008.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Syriana (2005) - Syriana\syriana1.srt",
                @"Z:\Filmi\Syriana (2005) - Syriana\syriana2.srt",
                @"Z:\Filmi\Takers (2010) - Tatovi\Takers 2010 480p BRRip XviD AC3-FLAWL3SS.srt",
                @"Z:\Filmi\Taking Chance (2009) - Spremljevalec\ENG\Taking Chance[Eng][Subs].srt",
                @"Z:\Filmi\Taking Chance (2009) - Spremljevalec\Taking Chance[2009]DvDrip[Eng]-FXG.srt",
                @"Z:\Filmi\Tangled (2010) - Zlatolaska\Tangled.2010.SLOSiNHRO.BRRip.XviD-DrSi.srt",
                @"Z:\Filmi\The American (2010) - Američan\The.American.2010.BRRip.XviD-Warrior.srt",
                @"Z:\Filmi\The American (2010) - Američan\Subs.ENG\The.American.2010.BRRip.XviD-Warrior.srt",
                @"Z:\Filmi\The Art Of War II Betrayal (2008) - Umetnost vojne Izdaja\The.Art.Of.War.II.Betrayal.2008.STV.SLOSubs.DVDRip.XviD-DrSi.cd1.srt",
                @"Z:\Filmi\The Art Of War II Betrayal (2008) - Umetnost vojne Izdaja\The.Art.Of.War.II.Betrayal.2008.STV.SLOSubs.DVDRip.XviD-DrSi.cd2.srt",
                @"Z:\Filmi\The Artist (2011) - Umetnik\The.Artist.2011.DVDSCR.XviD.ZOMBiES.srt",
                @"Z:\Filmi\The Beaver (2011) - Bober\The.Beaver.2011.720p.SLOSubs.BRRip.XviD-DrSi.srt",
                @"Z:\Filmi\The Big Lebowski (1998) - Veliki Lebowski\The Big Lebowski.1998.HDRip.x264-VLiS.srt",
                @"Z:\Filmi\The Big Sleep (1946) - Velik spanec\The.Big.Sleep.1946.DVDRip.H264.AAC.Gopo_EN.srt",
                @"Z:\Filmi\The Big Sleep (1946) - Velik spanec\The.Big.Sleep.1946.DVDRip.H264.AAC.Gopo.srt",
                @"Z:\Filmi\The Big Sleep (1946) - Velik spanec\The Big Sleep - hrvatski tilovi.srt",
                @"Z:\Filmi\The Blind Side (2009) - The Blind Side\The.Blind.Side.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\The Boat That Rocked (2009) - Piratski radio\ENG\The.Boat.That.Rocked.2009.DvDRip-FxM.srt",
                @"Z:\Filmi\The Boat That Rocked (2009) - Piratski radio\The.Boat.That.Rocked.2009.DvDRip-FxM.srt",
                @"Z:\Filmi\The Breakfast Club (1985) - Sobotni klub\The Breakfast Club [1985] DvdRip [Eng] - Thizz.srt",
                @"Z:\Filmi\The Brothers Bloom (2009) - Brata Bloom\The.Brothers.Bloom.2009.DvDRip-FxM.srt",
                @"Z:\Filmi\The Curious Case of Benjamin Button [2008] - Nenavaden primer Benjamina Buttona\ENG\The Curious Case of Benjamin Button[Eng][Subs].srt",
                @"Z:\Filmi\The Curious Case of Benjamin Button [2008] - Nenavaden primer Benjamina Buttona\The Curious Case of Benjamin Button[2008]DvDrip[Eng]-FXG.srt",
                @"Z:\Filmi\The Da Vinci Code (2006) - Da Vincijeva Sifra\The.Da.Vinci.Code.2006.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\The Dark Knight [2008] - Vitez teme\ENG\The Dark Knight[Eng][Subs].srt",
                @"Z:\Filmi\The Dark Knight [2008] - Vitez teme\The Dark KnightThe Dark Knight[2008]DvDrip[Eng]-FXG.srt",
                @"Z:\Filmi\The Fall (2006) - Padec\The.Fall[2006]DvDrip-aXXo.srt",
                @"Z:\Filmi\The Football Factory (2004) - The Football Factory\The Football Factory.srt",
                @"Z:\Filmi\The Guard (2011) - Policist\The.Guard.2011.LiMiTED.DVDRip.XviD-ViP3R.srt",
                @"Z:\Filmi\The Hangover (2009) - Prekrokana noč\The.Hangover.2009.UNRATED.SLOSubs.BRRip.XviD-DrSi.srt",
                @"Z:\Filmi\The Hangover 2 (2011) - Prekrokana noč 2\hangover2.1080.srt",
                @"Z:\Filmi\The Horsemen [2009] - Jezdeci\The.Horsemen[2009]DvDrip[Eng]-FXG.srt",
                @"Z:\Filmi\The Hurt Locker (2008) - Bombna misija\The.Hurt.Locker.2008.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\The Interpreter (2005) - Prevajalka\The.Interpreter.(V.O).[DvDRip].[WwW.LiMiTeDiVx.CoM].srt",
                @"Z:\Filmi\The Killing Room (2009) - Soba za ubijanje\The.Killing.Room.2009.DVDRip.XviD-VoMiT.NoRar.www.crazy-torrent.com.srt",
                @"Z:\Filmi\The Kingdom (2007) - Kraljestvo\The.Kingdom.2007.cd1.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\The Kingdom (2007) - Kraljestvo\The.Kingdom.2007.cd2.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\The King's Speech (2010) - Kraljev govor\The.Kings.Speech.2010.NORDiC.DvDRip.x264-Makavalios.srt",
                @"Z:\Filmi\The Last House On The Left (2009) - Zadnja hiša na levi\The.Last.House.On.The.Left.UNRATED.DvDRip-FxM.srt",
                @"Z:\Filmi\The Notebook (2004) - Beležnica\The Notebook (2004) [ENG] [DVDrip].srt",
                @"Z:\Filmi\The Pacifier (2005) - Misija Cucelj\The Pacifier.srt",
                @"Z:\Filmi\The Prestige (2006) - Nevidna sled\The Prestige 2006 BRRip x264 AC3-TheFalcon007 (Kingdom-Release).srt",
                @"Z:\Filmi\The Prestige (2006) - Nevidna sled\ENG\The Prestige 2006 BRRip x264 AC3-TheFalcon007 (Kingdom-Release) [ENG] .srt",
                @"Z:\Filmi\The Rebound (2009) - The Rebound\The.Rebound.2009.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\The Red Shoes (1948) - Rdeci cevlji\Eng.The Red Shoes.1948.DVDRip.srt",
                @"Z:\Filmi\The Red Shoes (1948) - Rdeci cevlji\Pt(BR).The.Red.Shoes.1948.DVDRip.srt",
                @"Z:\Filmi\The Red Shoes (1948) - Rdeci cevlji\The Red Shoes - hravatski 1 verzija.srt",
                @"Z:\Filmi\The Red Shoes (1948) - Rdeci cevlji\The.Red.Shoes.1948.720p.BluRay.x264-CiNEFiLE - hrvatski verzija 2.srt",
                @"Z:\Filmi\The Score (2001) - Zadetek\The.Score.2001.SLOSubs.DVDRip.XviD-DrSi.CD1.srt",
                @"Z:\Filmi\The Score (2001) - Zadetek\The.Score.2001.SLOSubs.DVDRip.XviD-DrSi.CD2.srt",
                @"Z:\Filmi\The Shawshank Redemption (1994) - Kaznilnica odrešitve\The Shawshank Redemption dvd rip Xvid.Rets.srt",
                @"Z:\Filmi\The Smurfs (2011) - Smrkci\The.Smurfs.2011.SLOSubs.BRRip.XviD-DrSi.srt",
                @"Z:\Filmi\The Social Network (2010) - Socialno omrežje\The.Social.Network.2010.DVDSCR.XViD-WBZ.srt",
                @"Z:\Filmi\The Tourist [2010] - Turist\The Tourist[2010]DvDrip[Eng]-FXG.srt",
                @"Z:\Filmi\The Town (2010) - Mesto\The.Town.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\The Transporter (2002) - Prenašalec 1\Transporter 3 (2008) DVDRip-HALESPONGE\Transporter 3 (2008) DVDRip-HALESPONGE.SLO.srt",
                @"Z:\Filmi\The Transporter (2002) - Prenašalec 1\TRANSPORTER 2 [2005]DvDrip[Eng]-NikonXp\Transporter 2.srt",
                @"Z:\Filmi\The Usual Suspects (1995) - Osumljenih 5\The Usual Suspects(Xvid).srt",
                @"Z:\Filmi\The Woman in Black (2012) - Zenska v crnem\The Woman in Black 2012 BRRip XviD AbSurdiTy.srt",
                @"Z:\Filmi\The.Expendables.2.2012.SLOSubs.BRRip.XviD-DrSi\The.Expendables.2.2012.SLOSubs.BRRip.XviD-DrSi.srt",
                @"Z:\Filmi\The.Expendables.2010.SLOSubs.BRRip.XviD-DrSi\The.Expendables.2010.SLOSubs.BRRip.XviD-DrSi.srt",
                @"Z:\Filmi\There Will Be Blood (2007) - Tekla bo kri\There.Will.Be.Blood.2007.SLOSubs.DVDRip.XviD-DrSi.cd1.srt",
                @"Z:\Filmi\There Will Be Blood (2007) - Tekla bo kri\There.Will.Be.Blood.2007.SLOSubs.DVDRip.XviD-DrSi.cd2.srt",
                @"Z:\Filmi\Thick As Thieves [2009] - Pretkani tatovi\d3676d0b24d0517f880250cfe13ef37f76aa7af2\Thick.As.Thieves[2009]DvDrip-aXXo.srt",
                @"Z:\Filmi\Thick As Thieves [2009] - Pretkani tatovi\Thick.As.Thieves[2009].srt",
                @"Z:\Filmi\Time of the Gypsies (1998) - Dom za vešanje\Dom.za.vesanje.Xvid.DVD.QQZ.CD1.srt",
                @"Z:\Filmi\Time of the Gypsies (1998) - Dom za vešanje\Dom.za.vesanje.Xvid.DVD.QQZ.CD2.srt",
                @"Z:\Filmi\Traitor (2008) - Izdajalec\Traitor.2008.SLOSubs.DVDRip.XviD-aXXo.srt",
                @"Z:\Filmi\Triage (2009) - Triaža\Triage.2009.DVDRip.XviD-Jaybob.srt",
                @"Z:\Filmi\Tristan and Isolde [2006] - Tristan in Izolda\Tristan.and.Isolde[2006]DvDrip[Eng]-aXXo.srt",
                @"Z:\Filmi\True Grit (2010) - Pravi pogum\True Grit (2010) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro.srt",
                @"Z:\Filmi\True Grit (2010) - Pravi pogum\True.Grit.2010.BluRay.1080p.DTS.x264-CHD.Slovenian.srt",
                @"Z:\Filmi\Un Prophete (2009) - Prerok\Un.Prophete.2009.DVDRip.XviD.AC3.5.1.HORiZON.srt",
                @"Z:\Filmi\United 93 (2006) - United 93\United.93[2006]DvDrip[Eng]-aXXo.srt",
                @"Z:\Filmi\Unthinkable (2010) - Nepojmljivo\Unthinkable.2010.EXTENDED.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Up In The Air (2009) - V zraku\Up.In.The.Air.2009.SLOSubs.DVDSCR.RERIP.XviD-CAMELOT.cd1.srt",
                @"Z:\Filmi\Up In The Air (2009) - V zraku\Up.In.The.Air.2009.SLOSubs.DVDSCR.RERIP.XviD-CAMELOT.cd2.srt",
                @"Z:\Filmi\V For Vendetta (2005) - V kot vroče maščevanje\V.For.Vendetta[2005]DvDrip[Eng]-aXXo.srt.srt",
                @"Z:\Filmi\Valkyrie (2008) -  Operacija Valkira\Valkyrie.2008.R5.LINE.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Wall E (2008) - Wall E\Wall_E.srt",
                @"Z:\Filmi\Wanted (2008) - Iskan\Wanted.2008.R5.cd1.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Wanted (2008) - Iskan\Wanted.2008.R5.cd2.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Wanted (2008) - Iskan\ENG\Wanted.2008.R5.cd2.DVDRip.XviD-bRiP-ENG.srt",
                @"Z:\Filmi\Wanted (2008) - Iskan\ENG\Wanted.2008.R5.cd1.DVDRip.XviD-bRiP-ENG.srt",
                @"Z:\Filmi\When A Man Falls In The Forest (2007) - Ko clovek pade v gozdu\When.A.Man.Falls.In.The.Forest.2007.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\When A Man Falls In The Forest (2007) - Ko clovek pade v gozdu\Sub.ENG\When.A.Man.Falls.In.The.Forest.2007.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\When Harry Met Sally (1989) - Ko je Harry srečal Sally\When Harry Met Sally.srt",
                @"Z:\Filmi\Where The Wild Things Are (2009) - V kraljestvu prečudnih zveri\nep-wtwta-scr.srt",
                @"Z:\Filmi\Whip It (2009) - Divje mrhe\Subtitles\Whip It Eng.srt",
                @"Z:\Filmi\Whip It (2009) - Divje mrhe\Subtitles\Whip It Rom.srt",
                @"Z:\Filmi\Whip It (2009) - Divje mrhe\Whip.It.720p.BluRay.x264.HAPPY.NEW.YEAR-METiS.srt",
                @"Z:\Filmi\Witness Protection [2008] - Zaščita prič\Witless.Protection.2008.cd1.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Witness Protection [2008] - Zaščita prič\Witless.Protection.2008.cd2.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Witness Protection [2008] - Zaščita prič\Subs.ENG\Witless.Protection.2008.cd1.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Witness Protection [2008] - Zaščita prič\Subs.ENG\Witless.Protection.2008.cd2.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\Wreck It Ralph (2012) - Razbijač Ralph\Wreck.It.Ralph.2012.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\You Dont Know Jack (2010) - Ne poznate Jacka\You.Dont.Know.Jack.2010.SLOSubs.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Zodiac (2007) - Zodiac\Zodiac 2007.srt",
                @"Z:\Filmi\Zodiac (2007) - Zodiac\ENG\Zodiac.2007.cd2.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Zodiac (2007) - Zodiac\ENG\Zodiac.2007.cd1.DVDRip.XviD-DrSi.srt",
                @"Z:\Filmi\Zwartboek (2006) - Crna knjiga\Zwartboek.2006.SLOSub.DVDRip.XviD-bRiP.srt",
                @"Z:\Filmi\(500) Days of Summer (2009) - 500 dni z Summer\(500)Days of Summer.[2009].RETAIL.DVDRIP.XVID.[Eng]-DUQA.srt",
                @"Z:\Filmi\(500) Days of Summer (2009) - 500 dni z Summer\500.Days.Of.Summer.BDRip.XviD-ARiGOLD.srt",
                @"Z:\Filmi\9 Songs (2004) - 9 orgazmov\9.Songs.2004.SLOSubs.DVDRip.XviD-DJTimi.srt",
                @"Z:\Filmi\21 (2008) - 21 Razpad Las Vegasa\21[2008]R5_DvDrip[Eng]-NikonXp.srt",
                @"Z:\Filmi\21 Grams (2003) - 21 gramov\21.Grams.2003.PROPER.LiMiTED.DVDRip.XviD.DEiTY.srt",
            };

            #endregion

            #region FileNames

            FileNames = new[] {
                "Oz.the.Great.and.Powerful.2013.SLOSubs.DVDRip.XviD-DrSi",
                "Trance.2013.SLOSubs.DVDRip.XviD-DrSi",
                "G.I.Joe.Retaliation.2013.SLOSubs.DVDRip.XviD-DrSi",
                "Total.Recall.Extended.2012.SLOSubs.BRRip.XviD-DrSi",
                "Snow.White.and.the.Huntsman.2012.EXTENDED.SLOSubs.DVDRip.XviD-DrSi",
                "Upside.Down.2012.SLOSubs.DVDRip.XviD-DrSi",
                "The.Odd.Life.of.Timothy.Green.2012.SLOSubs.DVDRip.XviD-DrSi",
                "Seeking.a.Friend.for.the.End.of.the.World.2012.SLOSubs.DVDRip.XviD-DrSi",
                "Bachelorette.2012.SLOSubs.DVDRip.XviD-DrSi",
                "Now You See Me 2013 EXTENDED BRRiP XViD UNiQUE",
                "Intersections 2013 CROSubs.DVDRip XViD juggs",
                "Kick-Ass.2.2013.SRBSubs.R6.HDRip.XviD-S4A",
                "RIPD",
                "Pacific Rim 2013 CROSubs.HDCam NewAudio XviD Feel-Free",
                "Rise of the Guardians (2012)R5 DVDRip NL subs (Divx)NLtoppers",
                "A.Serious.Man.2009.LiMiTED.SLOSubs.DVDRip.XviD-DrSi",
                "Silver Linings Playbook 2012 CROSubs.R5 XViD-PSiG",
                "The.Hobbit.An.Unexpected.Journey.2012.SLOSubs.DVDSCR.XviD-metalcamp",
                "The World's End  2013 SRBSubs.BRRip XViD-ETRG",
                "Escape.Plan.2013.SRBSubs.CAM.XviD-Tr0uNcE",
                "The Wolverine 2013 SLOSubs.EXTENDED BRRip XviD-ETRG",
                "The.To.Do.List.2013.720p.BRRip.XviD.INSiDE",
                "Paranoia 2013 CROSubs.BRRip XViD-ETRG",
                "sample",
                "Hysteria.2011.SLOSubs.DVDRip.XviD-DrSi",
                "Girl.Most.Likely.2012.CROSubs.HDRip.XviD-S4A",
                "Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi",
                "28.Weeks.Later.2007.DVDRip.XviD-DrSi",
                "50.50.2011.DVDScr.XviD-playXD",
                "88.Minutes.2007.DVDRip.Eng-aXXo",
                "300 (2007).DVDSCR.XVID",
                "A Lot Like Love",
                "A Separation 2011",
                "A Single Man.2010.DVDRip.XviD-T0XiC",
                "A Walk To Remember[2002].DvdRip.[yddam]",
                "Adulthood[2008]DvDrip-aXXo",
                "Alfie",
                "All About Eve",
                "American Pie 1[1999]",
                "American Pie 2[2001]",
                "American Pie 3 The Wedding[2003]",
                "American Pie 4 Band Camp[2005]",
                "American.Pie.5.The.Naked Mile.[2006].SLOsub.DvDrip[Eng]-BugZ",
                "American.Pie.Presents.Beta.House.2007.DVDRip.XviD-bRiP",
                "AMERICAN PSYCHO - Uncut [2000-Eng-DVDrip]-haSak",
                "Amores.Perros.2000.SLOSubs.DVDRip.XviD-DrSi.cd1",
                "Amores.Perros.2000.SLOSubs.DVDRip.XviD-DrSi.cd2",
                "Anchorman The legend of Ron Burgundy[2004]",
                "Another Year[2010]DvDrip[Eng]-FXG",
                "Atonement.2007.cd1.DVDRip.XviD-bRiP",
                "Atonement.2007.cd2.DVDRip.XviD-bRiP",
                "attack.the.block-done",
                "Avatar.2009.1080p.Slosubs.BluRay.DTS.x264-ESiR",
                "Babel[2006]DvDrip[Eng]-aXXo",
                "ltu-honey-xvid",
                "Barney's Version 2010 720p BRRip x264 RmD (HDScene Release)",
                "batman.begins-phrax",
                "target-beginners-xvid",
                "Biutiful.2010.DVDRip.XviD.5rFF",
                "Black.Swan.2010.DVDSCR.XviD-ViSiON",
                "Blue.Valentine.2010.DvdScr.AC3.Xvid {1337x}-Noir",
                "Celda.211.2009.SLOSubs.DVDRip.XviD-DrSi",
                "Charlie And The Chocolate Factory (2005)",
                "Children Of Men",
                "City of God CD1",
                "City of God CD2",
                "Cleaner.2007.DVDRip.XviD-bRiP",
                "hdt.cloud.atlas.2012.1080p.bluray.x264",
                "Confucius.2010.SLOSubs.DVDRip.XviD-DrSi",
                "Cowboys.And.Aliens.EXTENDED.2011.SLOSubs.DVDRip.XviD-DrSi",
                "Crash.2004.SLOSub.DVDRip.Xvid-DrSi.cd1",
                "Crash.2004.SLOSub.DVDRip.Xvid-DrSi.cd2",
                "The White Ribbon[2009]DvDrip[Ger]-FXG",
                "Dead Man Walking [English] 1995",
                "Dear John.2010.DvdRip.Xvid {1337x}-Noir",
                "despicable.me.dvdrip.xvid-imbt",
                "Detachment[2011]BRRip XviD-ETRG",
                "ExtraTorrentRG",
                "Disgrace.2008.SLOSubs.DVDRip.XviD-DrSi",
                "District 9 (2009) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro",
                "Django.Unchained.2012.SLOSubs.DVDSCR.XviD-metalcamp",
                "Drive.2011.SCR.XviD-playXD",
                "Eat.Pray.Love.2010.DC.SLOSubs.DVDRip.XviD-DrSi",
                "After the Wedding 2006 DVDRip Xvid fasamoo LKRG",
                "The.Orphanage.2007.SLOSubs.DVDRip.XviD-DrSi",
                "Elizabeth.1998.DVDRip.DivX-GarlicClan.slo",
                "dvl-eotsm",
                "Everybodys.Fine.2009.SLOSubs.DVDRip.XviD-DrSi",
                "mrush-mrfox",
                "Feast.of.Love.2007.DVDRip.XviD-bRiP",
                "Felon.2008.cd1.DVDRip.XviD-bRiP",
                "Felon.2008.cd2.DVDRip.XviD-bRiP",
                "Fight Club",
                "dash-fighting",
                "flight.2012.1080p.bluray.x264-sparks",
                "Gang",
                "Gone Baby Gone.[2007].DVDRIP.XVID.[Eng]-DUQA",
                "Gran.Torino.2008.DvDRip-FxM",
                "Grbavica.2006.DVDRip.XviD-XPTO",
                "Green.Zone.2010.SLOSubs.DVDRip.XviD-DrSi",
                "Gremo.mi.po.svoje.2010.SLOVENiAN.DVDRip.XviD-DrSi",
                "Guess.Who.2005.SLOSub.DVDRip.XviD-bRiP",
                "HR5",
                "Happy-Go-Lucky",
                "Hard.Candy.DVDRip.XviD-DiAMOND",
                "Headhunters.2011.BRRip.XviD-playXD",
                "Homeward Bound",
                "How.to.Lose.Friends.and.Alienate.People.2008.SLOSubs.DVDRip.XviD-DrSi",
                "Hugo 2011.720p.BrRip.X264.YIFY",
                "I'm.Not.There[2007]DvDrip[Eng]-aXXo",
                "Imagine That.2009.DvdRip.Xvid {1337x}-Noir",
                "Incendies.2010.DVDRip.XviD.AC3.HORiZON-ArtSubs",
                "Inglourious Basterds (2009) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro",
                "IpMan -aot",
                "Iron.Man.2008.SLOSubs.DVDRip.XviD-DrSi.cd1",
                "Iron.Man.2008.SLOSubs.DVDRip.XviD-DrSi.cd2",
                "Iron.Man.2.2010.DVDRip.XviD.AC3-ViSiON",
                "Iron.Man.3.2013.SLOSubs.DVDRip.XviD-DrSi",
                "Its.Kind.of.a.Funny.Story.2010.SLOSubs.BRRip.XviD-DrSi",
                "Julie.and.Julia.2009.SLOSubs.DVDRip.XviD-DrSi ",
                "Kidulthood.2006.DVDRip.XviD.(Cd1)-SAvAGE",
                "Kidulthood.2006.DVDRip.XviD.(Cd2)-SAvAGE",
                "Killers.2010.R5.LiNE.Xvid-Noir",
                "Knowing.2009.SLOSubs.DVDRip.XviD-DrSi",
                "Confessions.2010.JAP.DVDRip.XviD-MOC",
                "La.Delicatesse",
                "Almodovar_Bad.Education_DVDrip.Xvid_eng.subbed_m^M^m",
                "The.Skin.I.Live.In.2011.720p.Bluray.x264.anoXmous",
                "Lebanon.2009.SLOSubs.DVDRip.XviD-DrSi",
                "Leon-The Professional 1994 BDrip[A Release-Lounge H.264 By Titan]",
                "Les Amours Imaginaires 2010 [DVDRip.XviD-miguel] [FR]",
                "Limitless.UNRATED.2011.SLOSubs.BRRip.XviD-DrSi",
                "Lincoln.2012.DVDSCR.XViD.AC3-MAGNAT",
                "Little.Big.Soldier.2010.SLOSubs.DVDRip.XviD-DrSi",
                "Love Actually CD1",
                "Love Actually CD2",
                "Lucky.Number.Slevin.2006.SLOSubs.DVDRip.XviD-DrSi.cd1",
                "Lucky.Number.Slevin.2006.SLOSubs.DVDRip.XviD-DrSi.cd2",
                "Magnolia [1999] dvd rip nlx",
                "Man.On.Wire.2008.DVDRip.XviD",
                "Mar.Adentro (The.Sea.Inside) 2004.DVDRip.XviD",
                "psig-melancholia.2011.dvdrip.xvid",
                "Memento.DVDrip,XviD-contempt",
                "target-paris-xvid",
                "Milk[2008]DvDrip[Eng]-FXG",
                "Moon.2009.LiMiTED.SLOSubs.DVDRip.XviD-DrSi",
                "carre-my.sassy.girl-xvid",
                "Never.Let.Me.Go.2010.SLOSubs.DVDRip.XviD-DrSi",
                "Norwegian.Wood.2010.JAP.DVDRip.XviD.AC3-BAUM",
                "Oblivion.2013.SLOSubs.DVDRip.XviD-DrSi",
                "Old.Dogs.2009.SLOSubs.DVDRip.XviD-DrSi",
                "Once.2006.SLOSubs.DVDRip.XviD",
                "P.S.I.Love.You.2007.SLOSubs.DVDRip.XviD-DrSi.cd1",
                "P.S.I.Love.You.2007.SLOSubs.DVDRip.XviD-DrSi.cd2",
                "Paha.Perhe.aka.Bad.Family.2010.SLOSubs.DVDRip.XviD-DrSi",
                "Pan's.Labyrinth[2006]DvDrip[Eng.Sub]-aXXo",
                "Paul.2011.DVDRip.XviD-DiVERSiTY",
                "The People vs Larry Flynt",
                "Perfume.The.Story.Of.A.Murderer.2006.SLOSubs.DVDRip.XviD-DrSi",
                "Petelinji.Zajtrk.2007.DVDSCR.XviD-SpeeD-cd1",
                "Petelinji.Zajtrk.2007.DVDSCR.XviD-SpeeD-cd2",
                "Pirates.Of.The.Caribbean-At.World's.End[2007]DvDrip[Eng]-aXXo",
                "Pirates.of.the.Caribbean-Dead.Man's.Chest[2006]DvDrip",
                "poletje v školjki 1",
                "poletje v skoljki 2",
                "Potovanje.Skozi.Plasti.Zemlje.Slosub.Xvid",
                "Premonition.2007.DVDRip.XviD-DrSi",
                "Pride.and.Prejudice[2005]DvDrip[Eng]-aXXo",
                "Prince.Of.Persia.The.Sands.Of.Time.2010.SLOSubs.DVDRip.XviD-DrSi",
                "Public.Enemies.2009.DvDRip-FxM",
                "Push[2009]DvDrip[Eng]-FXG",
                "Red.2010.BRRip.XviD.AC3-MAGNAT",
                "Remember Me (2010)",
                "Reservoir Dogs_1992_DVDrip_XviD-Ekolb",
                "Robin.Hood.Unrated.DC.2010.BRRip.XviD.AC3-MAGNAT",
                "RocknRolla[2008]DvDrip-aXXo",
                "Roger Dodger",
                "Rush.Hour.3.2007.DVDRip.XviD-bRiP",
                "Scary Movie",
                "Scott.Pilgrim.vs.the.World.2010.SLOSubs.DVDRip.XviD-DrSi",
                "SEVEN[1995]DvDrip[Eng]-NikonXP",
                "Seven.Pounds.2008.SLOSubs.DVDRip.XviD-DrSi",
                "Seven.Psychopaths.2012.SLOSubs.DVDRip.XviD-DrSi",
                "Sex.Drive.2008.UNRATED.DVDRip.XviD-ST4R",
                "shame.2011.limited.dvdrip.xvid-amiable",
                "S01E01 - A Study In Pink",
                "S01E02 - The Blind Banker",
                "S01E03 - The Great Game",
                "Sherlock.Holmes.2009.SLOSubs.DVDRip.XviD-DrSi",
                "Sherlock Holmes A Game of Shadows (2011) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro",
                "Shes.Out.Of.My.League.2010.SLOSubs.DVDRip.XviD-DrSi",
                "dmd-shestm-cd1",
                "dmd-shestm-cd2",
                "Shrek.Forever.After.2010.SLOSubs.TS.XviD.AC3-ViSiON",
                "Shutter.Island.2010.SLOSubs.R5.LiNE.XviD-metalcamp",
                "Side.Effects.2013.720p.WEB-DL.X264-WEBiOS",
                "Sin City",
                "Slovenka.2009.SLOVENiAN.DVDRip.XviD-DrSi",
                "Slumdog.Millionaire.2008.SLOSubs.DVDRip.XviD-DrSi",
                "Snatch",
                "Sorority.Boys.DVDRip.2002-DEiTY.xvid",
                "Spartan.2004.SLOSubs.DVDRip.XviD-DrSi",
                "Star Trek",
                "Stargate.Continuum.2008.SLOSubs.DVDRip.XviD-DrSi.cd1",
                "Stargate.Continuum.2008.SLOSubs.DVDRip.XviD-DrSi.cd2",
                "Step.Up.2.(The.Streets).2008.cd1.DVDRip.XviD-bRiP",
                "Step.Up.2.(The.Streets).2008.cd2.DVDRip.XviD-bRiP",
                "Street.Kings.2008.R5.cd1.DVDRip.XviD-bRiP",
                "Street.Kings.2008.R5.cd2.DVDRip.XviD-bRiP",
                "Sucker Punch (2011) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro",
                "Superbad[2007][Unrated Editon]DvDrip[Eng]-FXG",
                "Superhero.Movie.2008.DVDRip.XviD-bRiP",
                "syriana1",
                "syriana2",
                "Takers 2010 480p BRRip XviD AC3-FLAWL3SS",
                "Taking Chance[2009]DvDrip[Eng]-FXG",
                "Tangled.2010.SLOSiNHRO.BRRip.XviD-DrSi",
                "The.Accused.1988.SWESUB.AC3.DVDRip.XviD-Martin",
                "The.American.2010.BRRip.XviD-Warrior",
                "The.American.President.DVDRiP.XviD-ASK",
                "The.Art.Of.War.II.Betrayal.2008.STV.SLOSubs.DVDRip.XviD-DrSi.cd1",
                "The.Art.Of.War.II.Betrayal.2008.STV.SLOSubs.DVDRip.XviD-DrSi.cd2",
                "The_Artist_2011_DVDSCR_XviD_-_ZOMBiES",
                "citrin-the.band.xvid",
                "The.Beaver.2011.720p.SLOSubs.BRRip.XviD-DrSi",
                "The Big Lebowski.1998.HDRip.x264-VLiS",
                "The.Big.Sleep.1946.DVDRip.H264.AAC.Gopo",
                "The.Blind.Side.2009.SLOSubs.DVDRip.XviD-DrSi",
                "The.Boat.That.Rocked.2009.DvDRip-FxM",
                "The Breakfast Club [1985] DvdRip [Eng] - Thizz",
                "The Bridge (2006) DVDRip XviD.[www.UsaBit.com]",
                "The.Brothers.Bloom.2009.DvDRip-FxM",
                "The Curious Case of Benjamin Button[2008]DvDrip[Eng]-FXG",
                "The.Da.Vinci.Code.2006.SLOSubs.DVDRip.XviD-DrSi",
                "The Dark KnightThe Dark Knight[2008]DvDrip[Eng]-FXG",
                "The.Fall[2006]DvDrip-aXXo",
                "The Football Factory",
                "The Gospel of Judas",
                "The.Guard.2011.LiMiTED.DVDRip.XviD-ViP3R",
                "The.Hangover.2009.UNRATED.SLOSubs.BRRip.XviD-DrSi",
                "The Hangover 2 (2011) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro",
                "The Horsemen[2009]DvDrip[Eng]-FXG",
                "The Human Stain 2003 SLOSUB DVDRip-DeeJayTaurus",
                "The.Hurt.Locker.2008.SLOSubs.DVDRip.XviD-DrSi",
                "The.Interpreter.(V.O).[DvDRip].[WwW.LiMiTeDiVx.CoM]",
                "The.Killing.Room.2009.DVDRip.XviD-VoMiT.NoRar.www.crazy-torrent.com",
                "The.Kingdom.2007.cd1.DVDRip.XviD-bRiP",
                "The.Kingdom.2007.cd2.DVDRip.XviD-bRiP",
                "The.Kings.Speech.2010.NORDiC.DvDRip.x264-Makavalios",
                "The.Last.House.On.The.Left.UNRATED.DvDRip-FxM",
                "The Notebook (2004) [ENG] [DVDrip]",
                "The Pacifier",
                "The Prestige 2006 BRRip x264 AC3-TheFalcon007 (Kingdom-Release)",
                "The.Rebound.2009.SLOSubs.DVDRip.XviD-DrSi",
                "The Red Shoes (1948) Eng",
                "The.Score.2001.SLOSubs.DVDRip.XviD-DrSi.CD1",
                "The.Score.2001.SLOSubs.DVDRip.XviD-DrSi.CD2",
                "The Shawshank Redemption dvd rip Xvid.Rets",
                "The.Smurfs.2011.SLOSubs.BRRip.XviD-DrSi",
                "The.Social.Network.2010.DVDSCR.XViD-WBZ",
                "The Taking Of Pelham 123[2009]DvDrip-LW",
                "The Tourist[2010]DvDrip[Eng]-FXG",
                "The.Town.2010.SLOSubs.DVDRip.XviD-DrSi",
                "The Transporter",
                "Transporter 3 (2008) DVDRip-HALESPONGE",
                "Transporter 2",
                "The Usual Suspects(Xvid)",
                "The Woman in Black 2012 BRRip XviD AbSurdiTy",
                "The.Expendables.2.2012.SLOSubs.BRRip.XviD-DrSi",
                "The.Expendables.2010.SLOSubs.BRRip.XviD-DrSi",
                "There.Will.Be.Blood.2007.SLOSubs.DVDRip.XviD-DrSi.cd2",
                "There.Will.Be.Blood.2007.SLOSubs.DVDRip.XviD-DrSi.cd1",
                "Thick.As.Thieves[2009]",
                "Dom.za.vesanje.Xvid.DVD.QQZ.CD1",
                "Dom.za.vesanje.Xvid.DVD.QQZ.CD2",
                "Traitor.2008.SLOSubs.DVDRip.XviD-aXXo",
                "Triage.2009.DVDRip.XviD-Jaybob",
                "Tristan.and.Isolde[2006]DvDrip[Eng]-aXXo",
                "True Grit (2010) DVDRip XviD-MAXSPEED www.torentz.3xforum.ro",
                "Un.Prophete.2009.DVDRip.XviD.AC3.5.1.HORiZON-ArtSubs",
                "United.93[2006]DvDrip[Eng]-aXXo",
                "Unthinkable.2010.EXTENDED.SLOSubs.DVDRip.XviD-DrSi",
                "Up.In.The.Air.2009.SLOSubs.DVDSCR.RERIP.XviD-CAMELOT.cd2",
                "Up.In.The.Air.2009.SLOSubs.DVDSCR.RERIP.XviD-CAMELOT.cd1",
                "V.For.Vendetta[2005]DvDrip[Eng]-aXXo",
                "Valkyrie.2008.R5.LINE.SLOSubs.DVDRip.XviD-DrSi",
                "Wall_E",
                "Wanted.2008.R5.cd1.DVDRip.XviD-bRiP",
                "Wanted.2008.R5.cd2.DVDRip.XviD-bRiP",
                "When.A.Man.Falls.In.The.Forest.2007.DVDRip.XviD-bRiP",
                "When Harry Met Sally",
                "nep-wtwta-scr",
                "Whip It",
                "Witless.Protection.2008.cd1.DVDRip.XviD-bRiP",
                "Witless.Protection.2008.cd2.DVDRip.XviD-bRiP",
                "Wreck.It.Ralph.2012.SLOSubs.DVDRip.XviD-DrSi",
                "You.Dont.Know.Jack.2010.SLOSubs.DVDRip.XviD-DrSi",
                "Zodiac 2007",
                "Zombieland (2009) DVDRip XviD-MAX www.torentz.3xforum.ro",
                "Zwartboek.2006.SLOSub.DVDRip.XviD-bRiP",
                "(500)Days of Summer.[2009].RETAIL.DVDRIP.XVID.[Eng]-DUQA",
                "9.Songs.2004.SLOSubs.DVDRip.XviD-DJTimi",
                "10 Things I Hate About You- Full Movie",
                "21[2008]R5_DvDrip[Eng]-NikonXp",
                "21.Grams.2003.PROPER.LiMiTED.DVDRip.XviD.DEiTY"
            };

            #endregion
        }

        private static void Main() {
            //FileNameParser fnp = new FileNameParser(@"C:\Users\Martin\Desktop\FMM\MovieSubs\Dead Man Walking [English] 1995.srt");
            //fnp.Parse();

            TestFileNameParser();
        }

        #region Language Detect

        private static async Task<string[]> TestLanguageDetectorAsync2() {
            DetectorFactory.LoadStaticProfiles();

            //IEnumerable<Task<string>> tasks = SubtitleFiles.Select(async subtitleFile => await DetectFileTextLang(subtitleFile));
            //return await Task.WhenAll(tasks);

            //int count = SubtitleFiles.Count;
            int count = LocalSubtitleFiles.Count;
            string[] langs = new string[count];

            for (int i = 0; i < count; i++) {
                langs[i] = await DetectFileTextLangAsync(LocalSubtitleFiles[i]);
            }

            return langs;
        }

        private static async Task<string[]> TestLanguageDetectorAsync() {
            DetectorFactory.LoadProfilesFromFolder("profiles");

            //int count = SubtitleFiles.Count;
            int count = LocalSubtitleFiles.Count;
            string[] langs = new string[count];
            for (int i = 0; i < count; i++) {
                langs[i] = await DetectFileTextLangAsync(LocalSubtitleFiles[i]);
            }

            return langs;
        }

        private static async Task<string> DetectFileTextLangAsync(string subtitleFile) {
            string lang;
            using (FileStream fs = File.OpenRead(subtitleFile)) {
                Encoding enc = await DetectEncodingAsync(fs);

                fs.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = (enc == null) ? new StreamReader(fs) : new StreamReader(fs, enc)) {
                    lang = await DetectLangAsync(sr);
                }
            }

            return lang;
        }

        private static Task<string> DetectLangAsync(StreamReader sr) {
            return Task.Run(() => {
                try {
                    Detector detector = DetectorFactory.Create(0.5);
                    detector.Append(sr);
                    return detector.Detect();
                }
                catch (LangDetectException) {
                    return null;
                }
            });
        }

        private static async Task<Encoding> DetectEncodingAsync(Stream fileStream) {
            byte[] firstKB = new byte[1024];
            int numBytes = await fileStream.ReadAsync(firstKB, 0, 1024);

            UniversalDetector cd = new UniversalDetector();
            cd.Feed(firstKB, 0, numBytes);
            cd.DataEnd();

            return cd.IsSupportedEncoding
                ? cd.DetectedEncoding
                : null;
        }

        #region LangDetect Old

        //private static string DetectLangSingleFile(string fileName, Encoding encoding) {
        //    DetectorFactory.LoadProfilesFromFolder("profiles");
        //    Task<string> detectLangAsync = DetectLangAsync(fileName, encoding);
        //    detectLangAsync.Wait();
        //    return detectLangAsync.Result;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private static Task<List<Language>> DetectLangAsync2(string subtitleFile, Encoding enc) {
        //    return Task.Run(() => {
        //        try {
        //            Detector detector = DetectorFactory.Create(0.5);
        //            detector.Verbose = true;
        //            detector.Append(enc == null ? new StreamReader(subtitleFile) : new StreamReader(subtitleFile, enc));
        //            return detector.GetProbabilities();
        //        }
        //        catch (LangDetectException e) {
        //            return null;
        //        }
        //    });
        //}

        //private static async Task<Encoding> DetectEncodingAsyncFn(string fn) {

        //    byte[] firstKB = new byte[1024];
        //    int numBytes = await File.OpenRead(fn).ReadAsync(firstKB, 0, 1024);

        //    UniversalDetector cd = new UniversalDetector();
        //    cd.Feed(firstKB, 0, numBytes);
        //    cd.DataEnd();

        //    return cd.IsSupportedEncoding
        //        ? cd.DetectedEncoding
        //        : null;
        //}


        //private static void TestLanguageDetector() {
        //    //string filler = string.Join("", Enumerable.Repeat("_", Console.BufferWidth));

        //    DetectorFactory.LoadProfilesFromFolder("profiles");
        //    //Stopwatch sw = new Stopwatch();
        //    foreach (string subtitleFile in SubtitleFiles) {
        //        //sw.Reset();
        //        //sw.Start();

        //        FileInfo fi = new FileInfo(subtitleFile);

        //        byte[] firstKB = new byte[1024];
        //        int numBytes = fi.OpenRead().Read(firstKB, 0, 1024);

        //        UniversalDetector cd = new UniversalDetector();
        //        cd.Feed(firstKB, 0, numBytes);
        //        cd.DataEnd();

        //        Encoding enc = null;
        //        if (cd.DetectedCharset != null) {
        //            try {
        //                enc = Encoding.GetEncoding(cd.DetectedCharset);
        //                //Console.WriteLine(@"Detected charset: {0}[{1}]", enc.EncodingName, enc.WebName);
        //            }
        //            catch (ArgumentException e) {
        //                //Console.Error.WriteLine(e.Message);
        //            }
        //        }

        //        string lang;
        //        try {
        //            Detector detector = DetectorFactory.Create(0.5);
        //            detector.Append(new StreamReader(subtitleFile, enc));
        //            lang = detector.Detect();
        //        }
        //        catch (LangDetectException) {
        //        }

        //        //sw.Stop();

        //        //Console.WriteLine(@"{0}: {1} ({2})", subtitleFile, sw.Elapsed, sw.ElapsedMilliseconds);
        //        //Console.WriteLine("Detecting took (including console outputs): "+sw.ElapsedMilliseconds);
        //        //Console.WriteLine(filler);
        //    }
        //}


        //private static void TestLanguageDetectorParallel() {
        //    //string filler = string.Join("", Enumerable.Repeat("_", Console.BufferWidth));

        //    DetectorFactory.LoadProfilesFromFolder("profiles");
        //    Parallel.ForEach(SubtitleFiles, subtitleFile => {

        //        using (FileStream fs = File.OpenRead(subtitleFile)) {
        //            byte[] firstKB = new byte[1024];
        //            int numBytes = fs.Read(firstKB, 0, 1024);

        //            UniversalDetector cd = new UniversalDetector();
        //            cd.Feed(firstKB, 0, numBytes);
        //            cd.DataEnd();

        //            Encoding enc = null;
        //            if (cd.IsSupportedEncoding) {
        //                enc = cd.DetectedEncoding;
        //            }

        //            fs.Seek(0, SeekOrigin.Begin);

        //            try {
        //                Detector detector = DetectorFactory.Create(0.5);
        //                using (StreamReader sr = (enc == null) ? new StreamReader(fs) : new StreamReader(fs, enc)) {
        //                    detector.Append(sr);
        //                }
        //                detector.Detect();
        //            }
        //            catch (LangDetectException) {
        //            }
        //        }
        //    });
        //}

        //private static void TestLanguageDetector_Parallel() {
        //    //string filler = string.Join("", Enumerable.Repeat("_", Console.BufferWidth));

        //    DetectorFactory.LoadStaticProfiles();
        //    Parallel.ForEach(SubtitleFiles, subtitleFile => {

        //        using (FileStream fs = File.OpenRead(subtitleFile)) {
        //            byte[] firstKB = new byte[1024];
        //            int numBytes = fs.Read(firstKB, 0, 1024);

        //            UniversalDetector cd = new UniversalDetector();
        //            cd.Feed(firstKB, 0, numBytes);
        //            cd.DataEnd();

        //            Encoding enc = null;
        //            if (cd.IsSupportedEncoding) {
        //                enc = cd.DetectedEncoding;
        //            }

        //            fs.Seek(0, SeekOrigin.Begin);

        //            try {
        //                Detector detector = DetectorFactory.Create(0.5);
        //                using (StreamReader sr = (enc == null) ? new StreamReader(fs) : new StreamReader(fs, enc)) {
        //                    detector.Append(sr);
        //                }
        //                detector.Detect();
        //            }
        //            catch (LangDetectException) {
        //            }
        //        }
        //    });
        //}

        //private static void TestLanguageDetectorParallel2() {

        //    DetectorFactory.LoadProfilesFromFolder("profiles");

        //    //
        //    Parallel.ForEach(SubtitleFiles, async subtitleFile => {
        //        Encoding enc = await DetectEncodingAsyncFn(subtitleFile);
        //        string lang = await DetectLangAsyncFn(subtitleFile, enc);
        //    });
        //}

        //private static void TestLanguageDetectorParallel3(IEnumerable<string> fileNames) {
        //    //DetectorFactory.LoadProfilesFromFolder("profiles");
        //    DetectorFactory.LoadStaticProfiles();

        //    List<Task> lst = new List<Task>();
        //    Parallel.ForEach(fileNames, fn => lst.Add(Detect(fn)));

        //    Task.WaitAll(lst.ToArray());
        //}

        //private static async Task Detect(string fn) {
        //    Encoding enc = await DetectEncodingAsyncFn(fn);
        //    string lang = await DetectLangAsyncFn(fn, enc);

        //    string w = lang;
        //    //Console.WriteLine(fn + ":");
        //    //Console.WriteLine("\tEncoding: {0}", (enc == null) ? "Unknown" : enc.WebName);
        //    //Console.WriteLine("\tLanguage: {0}", lang ?? "Unknown");
        //    //Console.WriteLine(filler);
        //}

        //private static async Task DetectSingle(string fn) {
        //    DetectorFactory.LoadProfilesFromFolder("profiles");

        //    Encoding enc = await DetectEncodingAsyncFn(fn);
        //    string lang = await DetectLangAsyncFn(fn, enc);

        //    Console.WriteLine(fn + ":");
        //    Console.WriteLine("\tEncoding: {0}", (enc == null) ? "Unknown" : enc.WebName);
        //    Console.WriteLine("\tLanguage: {0}", lang ?? "Unknown");
        //    Console.WriteLine(filler);
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private static Task<string> DetectLangAsyncFn(string subtitleFile, Encoding enc) {
        //    return Task.Run(() => {
        //        try {
        //            Detector detector = DetectorFactory.Create(0.5);

        //            detector.Append(enc == null ? new StreamReader(subtitleFile) : new StreamReader(subtitleFile, enc));
        //            return detector.Detect();
        //        }
        //        catch (LangDetectException e) {
        //            return null;
        //        }
        //    });
        //}

        #endregion

        #endregion

        #region FileNameParser / FeatureDetector

        private static void TestFileNameParser() {
            foreach (string fileName in LocalSubtitleFiles) {
                Console.WriteLine("FileName: {0}", fileName);
                //Console.Write(fileName + ";");

                FileNameParser fnp = new FileNameParser(fileName);
                FileNameInfo fileNameInfo = fnp.Parse();
                //if (fileNameInfo.VideoSources.Count > 1) {
                //    Console.Error.WriteLine(fileName);
                //}

                //if (string.IsNullOrEmpty(fileNameInfo.Title)) {
                //    Console.WriteLine("UNKNOWN;");
                //}
                //else {
                //    Console.WriteLine(fileNameInfo.Title + ";");
                //}

                Console.WriteLine("\tTitle:");

                if (string.IsNullOrEmpty(fileNameInfo.Title)) {
                    Console.WriteLine("\t\tUNKNOWN");
                }
                else {
                    Console.WriteLine("\t\t" + fileNameInfo.Title);
                }
                OutputDetected(fileNameInfo);
                Console.WriteLine();
                Console.WriteLine(Filler);
            }
        }

        private static void OutputDetected(FileNameInfo fnInfo) {
            if (fnInfo.ReleaseYear.Year != 1) {
                OutputString("Release Year:", fnInfo.ReleaseYear.Year.ToString(CultureInfo.InvariantCulture));
            }

            OutputString("Release Group: ", fnInfo.ReleaseGroup);
            OutputString("Edithion: ", fnInfo.Edithion);

            if (fnInfo.Part != 0) {
                OutputString("Part:", fnInfo.Part.ToString(CultureInfo.InvariantCulture));
                OutputString("Part Type:", fnInfo.PartType);
            }

            if (fnInfo.Language != null) {
                OutputString("Language: ", fnInfo.Language.EnglishName);
            }

            if (fnInfo.SubtitleLanguage != null) {
                OutputString("Subtitle Language: ", fnInfo.SubtitleLanguage.EnglishName);
            }

            OutputList("Specials:", fnInfo.Specials);

            OutputString("Genre: ",fnInfo.Genre);
            OutputString("ContentType: ",fnInfo.ContentType);

            if (fnInfo.DVDRegion != DVDRegion.Unknown) {
                OutputString("DVDRegion: ", fnInfo.DVDRegion.ToString());
            }

            OutputString("VideoSource: ", fnInfo.VideoSource);
            OutputString("VideoQuality: ", fnInfo.VideoQuality);
            OutputString("VideoCodec:", fnInfo.VideoCodec);

            OutputString("AudioSources:", fnInfo.AudioSources);
            OutputString("AudioQuality:", fnInfo.AudioQuality);
            OutputString("AudioCodec:",   fnInfo.AudioCodec);
        }

        public static void OutputString(string type, string str) {
            if (!string.IsNullOrEmpty(str)) {
                Console.WriteLine("\t"+type);
                Console.WriteLine("\t\t"+str);
            }
        }

        private static void OutputList(string type, ICollection<string> enumerable) {
            if (enumerable.Count != 0) {
                Console.WriteLine("\t"+type);
                foreach (string str in enumerable) {
                    Console.WriteLine("\t\t" + str);
                }
            }
        }

        public static void OutputIfNotDefault<T>(string type, T param) where T : struct {
            if (!Equals(param, default(T))) {
                Console.WriteLine(type);
                Console.WriteLine("\t\t"+param);
            }
        }

        public static void OutputIfNotNull<T>(string type, T param) where T : class {
            if (param != null) {
                Console.WriteLine(type + param);
            }
        }

        private static void TestFeatureDetector() {
            using (FeatureDetector fd = new FeatureDetector(FOLDER_PATH)) {
                List<Subtitle> subtitles = fd.GetSubtitlesForFile(FILE_PATH).ToList();
            }
        }

        #endregion

        #region Mediainfo

        /*
        private static void TestMediaInfoDisposed() {
            //WithoutCaching();

            MediaFile mf = new MediaFile(FILE_PATH, true);
            string fileName = mf.General.FileInfo.FileName;
            mf.Close();
            string fileName2 = mf.General.FileInfo.FileName;
            string folderName = mf.General.FileInfo.FolderPath;
        }

        private static void WithoutCaching() {
            MediaFile mf = new MediaFile(FILE_PATH, false);
            string fileName = mf.General.FileInfo.FileName;
            mf.Close();
            string fileName2 = mf.General.FileInfo.FileName;
        }

        private static void TestMediaInfoList() {
            //TestFoldernameList();

            TestFileNamesArray();
        }

        private static void TestFoldernameList() {
            string newLine = Environment.NewLine;

            using (MediaInfoList mil = new MediaInfoList(@"E:\Torrenti\FILMI\Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi\")) {
                string filler = string.Join("", Enumerable.Repeat("_", Console.BufferWidth));
                const string FORMAT = "{0}:{1}\t{2}";
                foreach (MediaListFile file in mil) {
                    Console.WriteLine(FORMAT, @"FileName:", newLine, file.General.FileInfo.FileName);
                    Console.WriteLine(FORMAT, @"Extension:", newLine, file.General.FileInfo.Extension);
                    Console.WriteLine(FORMAT, @"FolderPath:", newLine, file.General.FileInfo.FolderPath);
                    Console.Write(filler);
                }
            }
        }

        private static void TestFileNamesArray() {
            string[] fileNames = {FILE_PATH, FILE_PATH2, FILE_PATH3, FILE_PATH4, FILE_PATH5, FILE_PATH6};
            string[] fileNames2 = {FILE_PATH, FILE_PATH4};
            string fileNamesString = FILE_PATH + ";" + FILE_PATH4;

            using (MediaInfoList mil = new MediaInfoList(fileNames2)) {
                int numOpen = mil.NumberOfOpenFiles;
                int numOpen2 = 0;
                foreach (MediaListFile file in mil) {
                    string fullPath = file.General.FileInfo.FullPath;
                    file.Close();
                    long? fileSize = file.General.FileInfo.FileSize;
                    numOpen2 = mil.NumberOfOpenFiles;
                }
                numOpen2 = mil.NumberOfOpenFiles;
            }
        }

        private static void TestMediaInfoCacheEnumerable() {
            using (MediaFile mf = new MediaFile(FILE_PATH6, true)) {
                long? id = mf.Audio.ID;

                Dictionary<string, string> z = (Dictionary<string, string>) mf.Audio.CachedValues;
                z.Clear();

                id = mf.Audio.ID;
            }
        }

        private static void TestMediaInfoEnumerable() {
            using (MediaFile mf = new MediaFile(FILE_PATH6, true)) {
                foreach (MediaText stream in mf.Text) {
                    Console.WriteLine(stream.LanguageInfo.Full);
                    Console.WriteLine(stream.LanguageInfo.Full1);

                    Console.WriteLine(@"____________________________________________________________________");
                    Console.WriteLine();
                }
            }
        }

        private static void TestMediaInfo2() {
            StringBuilder sb = new StringBuilder(100000);
            using (MediaFile mf = new MediaFile(@"E:\Torrenti\FILMI\Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi\Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi.avi", true)) {
                foreach (KeyValuePair<string, string> kvp in mf.Audio.CachedValues) {
                    sb.AppendLine(kvp.Key + " : " + kvp.Value);
                }
            }
            Console.Write(sb.ToString());
        }

        public static string Format(IEnumerable<byte> data) {
            //storage for the resulting string
            string result = string.Empty;
            //iterate through the byte[]
            foreach (byte value in data) {
                //storage for the individual byte
                string binarybyte = Convert.ToString(value, 2);
                //if the binarybyte is not 8 characters long, its not a proper result
                while (binarybyte.Length < 8) {
                    //prepend the value with a 0
                    binarybyte = "0" + binarybyte;
                }
                //append the binarybyte to the result
                result += binarybyte;
            }
            //return the result
            return result;
        }

        private static void TestMediaInfo() {
            using (MediaFile mf = new MediaFile(FILE_PATH6, true)) {
                mf.Options.InformPreset = InformPreset.HTML;
                mf.Options.ShowAllInfo = true;

                LanguageInfo languageInfo = mf.Video.LanguageInfo;

                string name = mf.Audio.Codec.Name;
                string nameS = mf.Audio.Codec.NameString;

                TimeSpan? timeSpan = mf.Audio.Interleave.Duration;
                float? timeSpans = mf.Audio.Interleave.VideoFrames;

                foreach (KeyValuePair<string, string> kvp in mf.General.CachedValues) {
                    Console.WriteLine(@"{0} : {1}", kvp.Key, kvp.Value);
                }

                //DllTest(mf);
            }
            Console.WriteLine();
            Console.WriteLine("----------Končal----------");
            //Console.Read();
        }

        private static void DllTest(MediaFileBase mf) {
            StringBuilder sb = new StringBuilder(148000);
            sb.AppendLine(mf.Options.Custom("Info_Version", "0.7.13;MediaInfoDLL_Example_MSVC;0.7.13"));
            sb.AppendLine();

            sb.AppendLine("Info_Parameters");
            sb.AppendLine(mf.Info.KnownParameters);
            sb.AppendLine();

            sb.AppendLine("Info Codecs");
            sb.AppendLine(mf.Info.KnownCodecs);
            sb.AppendLine();

            sb.AppendLine("Open");

            sb.AppendLine();

            if (mf.IsOpen) {
                sb.AppendLine("Inform with Complete=false");
                mf.Options.ShowAllInfo = false;
                sb.AppendLine(mf.Inform());
                sb.AppendLine();

                sb.AppendLine("Inform with Complete=true");
                mf.Options.ShowAllInfo = true;
                sb.AppendLine(mf.Inform());
                sb.AppendLine();

                sb.AppendLine("Custom Inform");
                mf.Options.Inform = "General;Example : FileSize=%FileSize%";
                sb.AppendLine(mf.Inform());
                sb.AppendLine();

                sb.AppendLine("Get with Stream=General and Parameter=\"FileSize\"");
                sb.AppendLine(mf.General["FileSize"]);

                sb.AppendLine("GetI with Stream=General and Parameter=46");
                sb.AppendLine(mf.General[46]);

                sb.AppendLine("Count_Get with StreamKind=Stream_Audio");
                sb.AppendLine(mf.Audio.Count.ToString(CultureInfo.InvariantCulture));

                sb.AppendLine("Get with Stream=General and Parameter=\"AudioCount\"");
                sb.AppendLine(mf.General["AudioCount"]);

                sb.AppendLine("Get with Stream=Audio and Parameter=\"StreamCount\"");
                sb.AppendLine(mf.Audio["StreamCount"]);

                sb.AppendLine("Moj Get Codec");
                sb.AppendLine(mf.Audio.Codec.Name);

                sb.AppendLine("Moj Fomat");
                sb.AppendLine(mf.General.FormatInfo.Name);

                sb.AppendLine("Are there menues?");
                sb.AppendLine(mf.Menu.Any.ToString());

                sb.AppendLine("Close");

                Console.Write(sb.ToString());
            }
            else {
                sb.AppendLine("Napaka med odprianjem");
                Console.Write(sb.ToString());
            }
        }
        */

        #endregion
    }

}