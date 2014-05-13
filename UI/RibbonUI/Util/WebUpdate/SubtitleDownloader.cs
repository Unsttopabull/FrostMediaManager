using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using Frost.Common.Models.Provider;
using Frost.Common.Util.ISO;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models.Subtitles;
using log4net;
using RibbonUI.Design.Models;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Util.WebUpdate {

    public class SubtitleDownloader {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SubtitleDownloader));
        private readonly ISubtitleInfo _info;
        private readonly ObservableMovie _movie;

        public SubtitleDownloader(ISubtitleInfo info, ObservableMovie movie) {
            _info = info;
            _movie = movie;
        }

        public async Task Download() {
            if (string.IsNullOrEmpty(_movie.DirectoryPath)) {
                if (Log.IsErrorEnabled) {
                    Log.Error(string.Format("Movie \"{0}\" has no folder path available.", _movie.Title));
                }
                throw new Exception(Gettext.T("Unknown directory path"));
            }

            string filePath = GetName();
            bool success;
            if (!string.IsNullOrEmpty(_info.SubtitleGZipDownloadLink)) {
                success = await DownloadGzip(filePath);
            }
            else if (!string.IsNullOrEmpty(_info.SubtitleZipDownloadLink)) {
                success = await DownloadZip(filePath);
            }
            else if (!string.IsNullOrEmpty(_info.SubtitleFileDownloadLink)) {
                success = await DownloadFile(filePath);
            }
            else if (!string.IsNullOrEmpty(_info.SubtitlesLink)) {
                try {
                    Process.Start(_info.SubtitlesLink);
                }
                catch (Exception e) {
                    if (Log.IsWarnEnabled) {
                        Log.Warn(string.Format("Failed to open subtitle download link (user interaction required) with path: \"{0}\"", _info.SubtitlesLink));
                    }

                    MessageBox.Show(Gettext.T("Failed to open the subtitle download site"));
                }
                success = false;
            }
            else {
                MessageBox.Show(Gettext.T("No download information available."));
                return;
            }

            if (success && _movie["Subtitles"]) {
                try {
                    await Task.Run(() => AddSubtitleToMovie(filePath));
                }
                catch (Exception e) {
                    if (Log.IsWarnEnabled) {
                        Log.Warn(string.Format("Failed to add downloaded subtitle info to the movie \"{0}\"", _movie.Title));
                    }

                    MessageBox.Show(Gettext.T("Provider failed to save the subtitle information"));
                }
            }
        }

        private void AddSubtitleToMovie(string filePath) {
            FileInfo fi;
            try {
                fi = new FileInfo(filePath);
            }
            catch (SecurityException e) {
                if (Log.IsErrorEnabled) {
                    Log.Warn(string.Format("Failed to access subtitle file information. (Don't have security privileges) \"{0}\"", _movie.Title), e);
                }

                MessageBox.Show(Gettext.T("Failed to access subtitle file information. Don't have security privileges."));
                return;
            }
            catch (UnauthorizedAccessException e) {
                if (Log.IsErrorEnabled) {
                    Log.Warn(string.Format("Failed to access subtitle file information. (Access unauthorized) \"{0}\"", _movie.Title), e);
                }

                MessageBox.Show(Gettext.T("Failed to access subtitle file information. Access unauthorized."));
                return;
            }
            catch (IOException e) {
                if (Log.IsErrorEnabled) {
                    Log.Warn(string.Format("Failed to access subtitle file information. (IO Error) \"{0}\"", _movie.Title), e);
                }

                MessageBox.Show(Gettext.T("Failed to access subtitle file information."));
                return;
            }
            catch (Exception e) {
                if (Log.IsErrorEnabled) {
                    Log.Warn(string.Format("Failed to access subtitle file information. (Unknown error) \"{0}\"", _movie.Title), e);
                }

                MessageBox.Show(Gettext.T("Failed to obtain subtitle file information."));
                return;
            }

            DesingFile file = new DesingFile {
                Extension = fi.Extension,
                FolderPath = fi.DirectoryName,
                FullPath = fi.FullName,
                Name = Path.GetFileNameWithoutExtension(fi.FullName),
                Size = fi.Length
            };

            DesignSubtitle ds = new DesignSubtitle(file) {
                EmbededInVideo = false,
                ForHearingImpaired = _info.IsForHearingImpaired.HasValue && _info.IsForHearingImpaired.Value,
                Format = _info.SubtitleFormat
            };

            ISOLanguageCode code = null;
            if (!string.IsNullOrEmpty(_info.ISO639Language)) {
                code = ISOLanguageCodes.Instance.GetByISOCode(_info.ISO639Language);
            }
            else if (!string.IsNullOrEmpty(_info.LanguageName)) {
                code = ISOLanguageCodes.Instance.GetByEnglishName(_info.LanguageName);
            }

            if (code != null) {
                ds.Language = new DesignLanguage(code);
            }

            if (!string.IsNullOrEmpty(_info.SubtitleHash)) {
                ds.MD5 = _info.SubtitleHash;
            }
            else {
                ComputeMD5(fi, ds);
            }

            try {
                _movie.AddSubtitle(ds);
            }
            catch (Exception e) {
                if (Log.IsWarnEnabled) {
                    Log.Warn(string.Format("Failed to add downloaded subtitle info to the movie \"{0}\"", _movie.Title));
                }

                MessageBox.Show(Gettext.T("Provider failed to save the subtitle information"));
            }
        }

        private static void ComputeMD5(FileInfo fi, ISubtitle ds) {
            string md5Hash = null;
            try {
                using (FileStream fs = fi.Create()) {
                    using (MD5 md5 = MD5.Create()) {
                        md5Hash = md5.ComputeHash(fs).Aggregate("", (str, b) => str + b.ToString("x2"));
                    }
                }
            }
            catch (Exception e) {
                if (Log.IsWarnEnabled) {
                    Log.Warn(string.Format("Failed to compute subtitle MD5 Hash for file: \"{0}\"", fi.FullName), e);
                }
            }

            ds.MD5 = md5Hash;
        }

        private string GetName() {
            string subName;
            if (!GetSubFileName(out subName)) {
                subName = Path.Combine(_movie.DirectoryPath, _info.FileName);
            }
            else {
                subName += "." + _info.SubtitleFormat;
            }

            if (File.Exists(subName)) {
                string newFileName = string.Format("{0}_{1}.{2}", Path.GetFileNameWithoutExtension(subName), DateTime.Now.Ticks, _info.SubtitleFormat);
                newFileName = Path.Combine(_movie.DirectoryPath, string.IsNullOrEmpty(newFileName)
                                                                     ? string.Format("{0}_{1}", DateTime.Now.Ticks, subName)
                                                                     : newFileName);

                File.Move(subName, newFileName);
            }
            return subName;
        }

        private async Task<bool> DownloadFile(string filePath) {
            using (WebClient wc = new WebClient()) {
                try {
                    await wc.DownloadFileTaskAsync(_info.SubtitleFileDownloadLink, filePath);
                }
                catch (Exception e) {
                    if (Log.IsErrorEnabled) {
                        Log.Error(string.Format("Failed to download subtitle using direct link: \"{0}\".", _info.SubtitleFileDownloadLink), e);
                    }

                    MessageBox.Show(Gettext.T("Failed to download the subtitle file.\n\nError message:\n") + e.Message);
                    return false;
                }
            }
            return true;
        }

        private async Task<bool> DownloadGzip(string filePath) {
            byte[] data;
            using (WebClient wc = new WebClient()) {
                try {
                    data = await wc.DownloadDataTaskAsync(_info.SubtitleGZipDownloadLink);
                }
                catch (Exception e) {
                    if (Log.IsErrorEnabled) {
                        Log.Error(string.Format("Failed to download subtitle using GZip link: \"{0}\".", _info.SubtitleGZipDownloadLink), e);
                    }

                    MessageBox.Show(Gettext.T("Failed to download the subtitle archive.\n\nError message:\n") + e.Message);
                    return false;
                }
            }

            bool fileWriteFailure = false;
            try {
                byte[] dataDecompressed;
                using (MemoryStream ms = new MemoryStream(data)) {
                    using (GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress)) {
                        using (MemoryStream msDecompressed = new MemoryStream()) {
                            gzip.CopyTo(msDecompressed);
                            dataDecompressed = msDecompressed.ToArray();
                        }
                    }
                }

                if (string.IsNullOrEmpty(_info.SubtitleHash)) {
                    using (MD5 md5 = MD5.Create()) {
                        _info.SubtitleHash = md5.ComputeHash(dataDecompressed)
                                                .Aggregate("", (str, b) => str + b.ToString("x2"));
                    }
                }

                try {
                    File.WriteAllBytes(filePath, dataDecompressed);
                }
                catch {
                    fileWriteFailure = true;
                    throw;
                }
            }
            catch (Exception e) {
                if (Log.IsErrorEnabled) {
                    Log.Error(fileWriteFailure
                                    ? string.Format("Failed to write subtitle file to drive with the following path:\n\"{0}\"", filePath)
                                    : "Failed to decompress subtitle archive.", e);
                }

                MessageBox.Show(fileWriteFailure
                                    ? string.Format(Gettext.T("Failed to write subtitle file to drive with the following path:\n\"{0}\""), filePath)
                                    : Gettext.T("Failed to decompress subtitle archive."));
                return false;
            }
            return true;
        }

        private async Task<bool> DownloadZip(string path) {
            byte[] data;
            using (WebClient wc = new WebClient()) {
                try {
                    data = await wc.DownloadDataTaskAsync(_info.SubtitleZipDownloadLink);
                }
                catch (Exception e) {
                    if (Log.IsErrorEnabled) {
                        Log.Error(string.Format("Failed to download subtitle using ZIP link: \"{0}\".", _info.SubtitleZipDownloadLink), e);
                    }

                    MessageBox.Show(Gettext.T("Failed to download the subtitle archive.\n\nError message:\n") + e.Message);
                    return false;
                }
            }

            bool failedWriteFile = false;
            try {
                using (ZipArchive za = new ZipArchive(new MemoryStream(data), ZipArchiveMode.Read)) {
                    ZipArchiveEntry entry = za.GetEntry(_info.FileName);


                    if (string.IsNullOrEmpty(_info.SubtitleHash)) {
                        using (Stream s = entry.Open()) {
                            using (MD5 md5 = MD5.Create()) {
                                _info.SubtitleHash = md5.ComputeHash(s).Aggregate("", (str, b) => str + b.ToString("x2"));
                            }
                        }
                    }

                    try {
                        entry.ExtractToFile(path);
                    }
                    catch {
                        failedWriteFile = true;
                        throw;
                    }
                }
            }
            catch (Exception e) {
                if (Log.IsErrorEnabled) {
                    Log.Error(failedWriteFile
                                    ? string.Format("Failed to write subtitle file to drive with the following path:\n\"{0}\"", path)
                                    : "Failed to decompress subtitle archive.", e);
                }

                MessageBox.Show(failedWriteFile
                                    ? string.Format(Gettext.T("Failed to write subtitle file to drive with the following path:\n\"{0}\""), path)
                                    : Gettext.T("Failed to decompress subtitle archive."));
                return false;
            }
            return true;
        }

        private bool GetSubFileName(out string nfoName) {
            try {
                MovieVideo video = _movie.Videos.FirstOrDefault(v => v != null && v.File != null);
                if (video != null) {
                    nfoName = GetFileNameWithoutExtension(video.File.FullPath);
                    return true;
                }

                MovieAudio audio = _movie.Audios.FirstOrDefault(a => a != null && a.File != null);
                if (audio != null) {
                    nfoName = GetFileNameWithoutExtension(audio.File.FullPath);
                    return true;
                }

                MovieSubtitle subtitle = _movie.Subtitles.FirstOrDefault(s => s != null && s.File != null);
                if (subtitle != null) {
                    nfoName = GetFileNameWithoutExtension(subtitle.File.FullPath);
                    return true;
                }
            }
            catch (Exception e) {
                if (Log.IsWarnEnabled) {
                    Log.Warn(string.Format("Exception has occured while trying to obtain movie file name for movie \"{0}\".", _movie.Title), e);
                }

                nfoName = null;
                return false;
            }

            nfoName = null;
            return false;
        }

        private static string GetFileNameWithoutExtension(string fullPath) {
            if (!Path.HasExtension(fullPath)) {
                return fullPath;
            }

            int idx = fullPath.LastIndexOf('.');
            fullPath = fullPath.Substring(0, idx);
            return fullPath;
        }
    }

}