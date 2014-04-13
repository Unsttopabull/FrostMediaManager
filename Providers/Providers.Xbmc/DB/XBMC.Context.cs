﻿using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Frost.Common.Util;
using Frost.Providers.Xbmc.DB.Art;
using Frost.Providers.Xbmc.DB.People;
using Frost.Providers.Xbmc.DB.StreamDetails;
using Frost.Providers.Xbmc.Properties;

namespace Frost.Providers.Xbmc.DB {

    /// <summary>Represents a context used for manipulation of the XBMC database.</summary>
    public class XbmcContainer : DbContext {
        private StreamWriter _sw;
        private const string WIN_XBMC_DB_LOC = @"XBMC\userdata\Database\";

        /// <summary>Initializes a new instance of the <see cref="XbmcContainer"/> class.</summary>
        public XbmcContainer() : base("name=XbmcEntities") {
            FixForEf();
            SetupLogging();
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcContainer"/> class.</summary>
        /// <param name="filePath">The path to the SQLite database file.</param>
        public XbmcContainer(string filePath) : base(new SQLiteConnection("data source=" + filePath), true) {
            FixForEf();
            SetupLogging();
        }

        private void FixForEf() {
            try {
                SQLiteConnection dbConnection = (SQLiteConnection) Database.Connection;
                dbConnection.Open();

                SQLiteCommand cmd = new SQLiteCommand(Resources.addEfTables, dbConnection);
                cmd.ExecuteNonQuery();

                DataTable cols = dbConnection.GetSchema("Columns");
                if (cols.Select("COLUMN_NAME='idStream' AND TABLE_NAME='streamdetails'").Length == 0) {
                    cmd = new SQLiteCommand(Resources.StreamDetailsPK, dbConnection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SQLiteException e) {

            }
            catch (Exception e) {
                
            }
        }

        private void SetupLogging() {
            // _sw = new StreamWriter(File.Create("xbmc.EF.log"));

            //Database.Log = s => Task.Run(() => {
            //    lock (_sw) {
            //        _sw.WriteLine("############################");
            //        _sw.WriteLine(s);
            //        _sw.WriteLine("############################");
            //        _sw.Flush();
            //    }
            //});
        }

        /// <summary>Gets or sets the information about the movies in the XBMC library.</summary>
        /// <value>The information about the movies in XBMC library.</value>
        public DbSet<XbmcDbMovie> Movies { get; set; }

        /// <summary>Gets or sets the information about people that participated in the movies in the XBMC library.</summary>
        /// <value>The information about people that participated in the movies in the XBMC library</value>
        public DbSet<XbmcPerson> People { get; set; }

        /// <summary>Gets or sets the information about files that contain the movies their subtitles in the XBMC library.</summary>
        /// <value>The information about files that contain the movies their subtitles in the XBMC library</value>
        public DbSet<XbmcFile> Files { get; set; }

        /// <summary>Gets or sets the information about folders and their roles and settings.</summary>
        /// <value>The information about folders and their roles and settings</value>
        public DbSet<XbmcPath> Paths { get; set; }

        /// <summary>Gets or sets the infromation about movie video/audio/subtitle stream details.</summary>
        /// <value>The infromation about movie video/audio/subtitle stream details.</value>
        public DbSet<XbmcDbStreamDetails> StreamDetails { get; set; }

        /// <summary>Gets or sets the information about movie collections and sets in the XBMC library.</summary>
        /// <value>The information about movie collections and sets in the XBMC library.</value>
        public DbSet<XbmcSet> Sets { get; set; }

        /// <summary>Gets or sets the information about genres of the movies in the XBMC library</summary>
        /// <value>The information about genres of the movies in the XBMC library</value>
        public DbSet<XbmcGenre> Genres { get; set; }

        /// <summary>Gets or sets the information about studios that procuced the movies in the XBMC library.</summary>
        /// <value>The information about studios that procuced the movies in the XBMC library.</value>
        public DbSet<XbmcStudio> Studios { get; set; }

        /// <summary>Gets or sets the information about contries the movies in the XBMC library were shot and/or produced in.</summary>
        /// <value>The information about contries the movies in the XBMC library were shot and/or produced in.</value>
        public DbSet<XbmcCountry> Countries { get; set; }

        ///// <summary>Gets or sets the information about tags in the XBMC library.</summary>
        ///// <value>The information about tags in the XBMC library.</value>
        //public DbSet<XbmcTag> Tags { get; set; }

        ///// <summary>Gets or sets the tag links.</summary>
        ///// <value>The tag links.</value>
        //public DbSet<XbmcTagLink> TagLinks { get; set; }

        ///<summary>Gets or sets the information about promotional images in the XBMC library.</summary>
        ///<value>The information about promotional images in the XBMC library.</value>
        public DbSet<XbmcArt> Art { get; set; }

        /// <summary>Gets or sets the information about XBMC settings about a particular file.</summary>
        /// <value>The information about XBMC settings about a particular file</value>
        public DbSet<XbmcSettings> Settings { get; set; }

        /// <summary>Gets or sets the database version and compression information.</summary>
        /// <value>The database version and compression information.</value>
        public DbSet<XbmcVersion> Version { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Configurations.Add(new XbmcArt.Configuration());
            //modelBuilder.Configurations.Add(new XbmcBookmark.Configuration());
            modelBuilder.Configurations.Add(new XbmcFile.Configuration());
            modelBuilder.Configurations.Add(new XbmcDbMovie.Configuration());
            modelBuilder.Configurations.Add(new XbmcDbStreamDetails.Configuration());
            modelBuilder.Configurations.Add(new XbmcSet.Configuration());
            modelBuilder.Configurations.Add(new XbmcPath.Configuration());
            modelBuilder.Configurations.Add(new XbmcMovieActor.Configuration());
            modelBuilder.Configurations.Add(new XbmcPerson.Configuration());
            modelBuilder.Configurations.Add(new XbmcGenre.Configuration());
            modelBuilder.Configurations.Add(new XbmcCountry.Configuration());
            modelBuilder.Configurations.Add(new XbmcStudio.Configuration());

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges() {
            try {
                EfLogger.LogChanges(this, "xbmc.log");
            }
            catch (Exception e) {
                
            }

            return base.SaveChanges();
        }

        public static string FindXbmcDB() {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string fn = Path.Combine(appData, WIN_XBMC_DB_LOC);

            string[] di = Directory.GetFiles(fn);

            //escapamo separatorje med mapami da regex ne pomotoma proba narobe razumeti vzorca
            fn = fn.Replace(@"\", @"\\");
            return di.FirstOrDefault(file => Regex.IsMatch(file, fn + @"MyVideos\d+\.db"));
        }

        ~XbmcContainer() {
            if (_sw != null) {
                try {
                    lock (_sw) {
                        _sw.Dispose();
                    }
                }
                catch (Exception e){
                    
                }
            }
        }
    }

}