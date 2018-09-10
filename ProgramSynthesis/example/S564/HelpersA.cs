﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2017 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using ShareX.HelpersLib.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace ShareX.HelpersLib
{
    public static class Helpers
    {
        public const string Numbers = "0123456789"; // 48 ... 57
        public const string AlphabetCapital = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // 65 ... 90
        public const string Alphabet = "abcdefghijklmnopqrstuvwxyz"; // 97 ... 122
        public const string Alphanumeric = Numbers + AlphabetCapital + Alphabet;
        public const string AlphanumericInverse = Numbers + Alphabet + AlphabetCapital;
        public const string Hexadecimal = Numbers + "ABCDEF";
        public const string URLCharacters = Alphanumeric + "-._~"; // 45 46 95 126
        public const string URLPathCharacters = URLCharacters + "/"; // 47
        public const string ValidURLCharacters = URLPathCharacters + ":?#[]@!$&'()*+,;= ";

        public static readonly string[] ImageFileExtensions = new string[] { "jpg", "jpeg", "png", "gif", "bmp", "ico", "tif", "tiff" };
        public static readonly string[] TextFileExtensions = new string[] { "txt", "log", "nfo", "c", "cpp", "cc", "cxx", "h", "hpp", "hxx", "cs", "vb", "html", "htm", "xhtml", "xht", "xml", "css", "js", "php", "bat", "java", "lua", "py", "pl", "cfg", "ini", "dart" };
        public static readonly string[] VideoFileExtensions = new string[] { "mp4", "webm", "mkv", "avi", "vob", "ogv", "ogg", "mov", "qt", "wmv", "m4p", "m4v", "mpg", "mp2", "mpeg", "mpe", "mpv", "m2v", "m4v", "flv", "f4v" };

        public static readonly Version OSVersion = Environment.OSVersion.Version;

        /// <summary>Get file name extension without dot.</summary>
        public static string GetFilenameExtension(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                int pos = filePath.LastIndexOf('.');

                if (pos >= 0)
                {
                    return filePath.Substring(pos + 1);
                }
            }

            return null;
        }

        public static string GetFilenameSafe(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                int pos = filePath.LastIndexOf('\\');

                if (pos < 0)
                {
                    pos = filePath.LastIndexOf('/');
                }

                if (pos >= 0)
                {
                    return filePath.Substring(pos + 1);
                }
            }

            return filePath;
        }

        public static string ChangeFilenameExtension(string filePath, string extension)
        {
            if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(extension))
            {
                int pos = filePath.LastIndexOf('.');

                if (pos >= 0)
                {
                    filePath = filePath.Remove(pos);

                    extension = extension.Trim();
                    pos = extension.LastIndexOf('.');

                    if (pos >= 0)
                    {
                        extension = extension.Substring(pos + 1);
                    }

                    return filePath + "." + extension;
                }
            }

            return filePath;
        }

        public static string RenameFile(string filePath, string newFileName)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string directory = Path.GetDirectoryName(filePath);
                    string newPath = Path.Combine(directory, newFileName);
                    File.Move(filePath, newPath);
                    return newPath;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Rename error:\r\n" + e.ToString(), "ShareX - " + Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return filePath;
        }

        public static string AppendExtension(string filePath, string extension)
        {
            return filePath.TrimEnd('.') + '.' + extension.TrimStart('.');
        }

        public static bool CheckExtension(string filePath, IEnumerable<string> extensions)
        {
            string ext = GetFilenameExtension(filePath);

            if (!string.IsNullOrEmpty(ext))
            {
                return extensions.Any(x => ext.Equals(x, StringComparison.InvariantCultureIgnoreCase));
            }

            return false;
        }

        public static bool IsImageFile(string filePath)
        {
            return CheckExtension(filePath, ImageFileExtensions);
        }

        public static bool IsTextFile(string filePath)
        {
            return CheckExtension(filePath, TextFileExtensions);
        }

        public static bool IsVideoFile(string filePath)
        {
            return CheckExtension(filePath, VideoFileExtensions);
        }

        public static EDataType FindDataType(string filePath)
        {
            if (IsImageFile(filePath))
            {
                return EDataType.Image;
            }

            if (IsTextFile(filePath))
            {
                return EDataType.Text;
            }

            return EDataType.File;
        }

        public static string AddZeroes(string input, int digits = 2)
        {
            return input.PadLeft(digits, '0');
        }

        public static string AddZeroes(int number, int digits = 2)
        {
            return AddZeroes(number.ToString(), digits);
        }

        public static string HourTo12(int hour)
        {
            if (hour == 0)
            {
                return 12.ToString();
            }

            if (hour > 12)
            {
                return AddZeroes(hour - 12);
            }

            return AddZeroes(hour);
        }

        public static char GetRandomChar(string chars)
        {
            return chars[MathHelpers.Random(chars.Length - 1)];
        }

        public static string GetRandomString(string chars, int length)
        {
            StringBuilder sb = new StringBuilder();

            while (length-- > 0)
            {
                sb.Append(GetRandomChar(chars));
            }

            return sb.ToString();
        }

        public static string GetRandomNumber(int length)
        {
            return GetRandomString(Numbers, length);
        }

        public static string GetRandomAlphanumeric(int length)
        {
            return GetRandomString(Alphanumeric, length);
        }

        public static string GetRandomKey(int length = 5, int count = 3, char separator = '-')
        {
            return Enumerable.Range(1, (length + 1) * count - 1).Aggregate("", (x, index) => x += index % (length + 1) == 0 ? separator : GetRandomChar(Alphanumeric));
        }

        public static string GetAllCharacters()
        {
            return Encoding.UTF8.GetString(Enumerable.Range(1, 255).Select(i => (byte)i).ToArray());
        }

        public static string GetValidFileName(string fileName, string separator = "")
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            if (string.IsNullOrEmpty(separator))
            {
                return new string(fileName.Where(c => !invalidFileNameChars.Contains(c)).ToArray());
            }
            else
            {
                invalidFileNameChars.ForEach(x => fileName = fileName.Replace(x.ToString(), separator));
                return fileName.Trim().Replace(separator + separator, separator);
            }
        }

        public static string GetValidFolderPath(string folderPath)
        {
            char[] invalidPathChars = Path.GetInvalidPathChars();
            return new string(folderPath.Where(c => !invalidPathChars.Contains(c)).ToArray());
        }

        public static string GetValidFilePath(string filePath)
        {
            string folderPath = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            return GetValidFolderPath(folderPath) + Path.DirectorySeparatorChar + GetValidFileName(fileName);
        }

        public static string GetValidURL(string url, bool replaceSpace = false)
        {
            if (replaceSpace) url = url.Replace(' ', '_');
            return HttpUtility.UrlPathEncode(url);
        }

        public static string GetXMLValue(string input, string tag)
        {
            return Regex.Match(input, string.Format("(?<={0}>).+?(?=</{0})", tag)).Value;
        }

        public static string GetMimeType(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string ext = Path.GetExtension(fileName).ToLower();

                if (!string.IsNullOrEmpty(ext))
                {
                    string mimeType = MimeTypes.GetMimeType(ext);

                    if (!string.IsNullOrEmpty(mimeType))
                    {
                        return mimeType;
                    }

                    using (RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(ext))
                    {
                        if (regKey != null && regKey.GetValue("Content Type") != null)
                        {
                            mimeType = regKey.GetValue("Content Type").ToString();

                            if (!string.IsNullOrEmpty(mimeType))
                            {
                                return mimeType;
                            }
                        }
                    }
                }
            }

            return MimeTypes.DefaultMimeType;
        }

        public static T[] GetEnums<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static string[] GetEnumDescriptions<T>()
        {
            return Enum.GetValues(typeof(T)).OfType<Enum>().Select(x => x.GetDescription()).ToArray();
        }

        /*public static string[] GetLocalizedEnumDescriptions<T>()
        {
            Assembly assembly = typeof(T).Assembly;
            string resourcePath = assembly.GetName().Name + ".Properties.Resources";
            ResourceManager resourceManager = new ResourceManager(resourcePath, assembly);
            return GetLocalizedEnumDescriptions<T>(resourceManager);
        }*/

        public static string[] GetLocalizedEnumDescriptions<T>()
        {
            return GetLocalizedEnumDescriptions<T>(Resources.ResourceManager);
        }

        public static string[] GetLocalizedEnumDescriptions<T>(ResourceManager resourceManager)
        {
            return Enum.GetValues(typeof(T)).OfType<Enum>().Select(x => x.GetLocalizedDescription(resourceManager)).ToArray();
        }

        public static int GetEnumLength<T>()
        {
            return Enum.GetValues(typeof(T)).Length;
        }

        public static T GetEnumFromIndex<T>(int i)
        {
            return GetEnums<T>()[i];
        }

        public static string[] GetEnumNamesProper<T>()
        {
            string[] names = Enum.GetNames(typeof(T));
            string[] newNames = new string[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                newNames[i] = GetProperName(names[i]);
            }

            return newNames;
        }

        // returns a list of public static fields of the class type (similar to enum values)
        public static T[] GetValueFields<T>()
        {
            List<T> res = new List<T>();
            foreach (FieldInfo fi in typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (fi.FieldType != typeof(T)) continue;
                res.Add((T)fi.GetValue(null));
            }
            return res.ToArray();
        }

        // Example: "TopLeft" becomes "Top left"
        public static string GetProperName(string name)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];

                if (i > 0 && char.IsUpper(c))
                {
                    sb.Append(' ');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static bool OpenFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                try
                {
                    Process.Start(filePath);
                    return true;
                }
                catch (Exception e)
                {
                    DebugHelper.WriteException(e, $"OpenFile({filePath}) failed.");
                }
            }
            else
            {
                MessageBox.Show(Resources.Helpers_OpenFile_File_not_exist_ + Environment.NewLine + filePath, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return false;
        }

        public static bool OpenFolder(string folderPath)
        {
            if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath))
            {
                if (!folderPath.EndsWith(@"\"))
                {
                    folderPath += @"\";
                }

                try
                {
                    Process.Start(folderPath);
                    return true;
                }
                catch (Exception e)
                {
                    DebugHelper.WriteException(e, $"OpenFolder({folderPath}) failed.");
                }
            }
            else
            {
                MessageBox.Show(Resources.Helpers_OpenFolder_Folder_not_exist_ + Environment.NewLine + folderPath, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return false;
        }

        public static bool OpenFolderWithFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                try
                {
                    NativeMethods.OpenFolderAndSelectFile(filePath);
                    return true;
                }
                catch (Exception e)
                {
                    DebugHelper.WriteException(e, $"OpenFolderWithFile({filePath}) failed.");
                }
            }
            else
            {
                MessageBox.Show(Resources.Helpers_OpenFile_File_not_exist_ + Environment.NewLine + filePath, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return false;
        }

        /// <summary>
        /// If version1 newer than version2 = 1
        /// If version1 equal to version2 = 0
        /// If version1 older than version2 = -1
        /// </summary>
        public static int CompareVersion(string version1, string version2)
        {
            return NormalizeVersion(version1).CompareTo(NormalizeVersion(version2));
        }

        /// <summary>
        /// If version1 newer than version2 = 1
        /// If version1 equal to version2 = 0
        /// If version1 older than version2 = -1
        /// </summary>
        public static int CompareVersion(Version version1, Version version2)
        {
            return version1.Normalize().CompareTo(version2.Normalize());
        }

        /// <summary>
        /// If version newer than ApplicationVersion = 1
        /// If version equal to ApplicationVersion = 0
        /// If version older than ApplicationVersion = -1
        /// </summary>
        public static int CompareApplicationVersion(string version)
        {
            return CompareVersion(version, Application.ProductVersion);
        }

        private static Version NormalizeVersion(string version)
        {
            return Version.Parse(version).Normalize();
        }

        public static bool IsWindowsXP()
        {
            return OSVersion.Major == 5 && OSVersion.Minor == 1;
        }

        public static bool IsWindowsXPOrGreater()
        {
            return (OSVersion.Major == 5 && OSVersion.Minor >= 1) || OSVersion.Major > 5;
        }

        public static bool IsWindowsVista()
        {
            return OSVersion.Major == 6;
        }

        public static bool IsWindowsVistaOrGreater()
        {
            return OSVersion.Major >= 6;
        }

        public static bool IsWindows7()
        {
            return OSVersion.Major == 6 && OSVersion.Minor == 1;
        }

        public static bool IsWindows7OrGreater()
        {
            return (OSVersion.Major == 6 && OSVersion.Minor >= 1) || OSVersion.Major > 6;
        }

        public static bool IsWindows8()
        {
            return OSVersion.Major == 6 && OSVersion.Minor == 2;
        }

        public static bool IsWindows8OrGreater()
        {
            return (OSVersion.Major == 6 && OSVersion.Minor >= 2) || OSVersion.Major > 6;
        }

        public static bool IsWindows10OrGreater()
        {
            return OSVersion.Major >= 10;
        }

        public static bool IsDefaultInstallDir()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            return Application.ExecutablePath.StartsWith(path);
        }

        public static bool IsValidIPAddress(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return false;

            string pattern = @"(?<First>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Second>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Third>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Fourth>2[0-4]\d|25[0-5]|[01]?\d\d?)";

            return Regex.IsMatch(ip.Trim(), pattern);
        }

        public static string GetUniqueFilePath(string filepath)
        {
            if (File.Exists(filepath))
            {
                string folder = Path.GetDirectoryName(filepath);
                string filename = Path.GetFileNameWithoutExtension(filepath);
                string extension = Path.GetExtension(filepath);
                int number = 1;

                Match regex = Regex.Match(filepath, @"(.+) \((\d+)\)\.\w+");

                if (regex.Success)
                {
                    filename = regex.Groups[1].Value;
                    number = int.Parse(regex.Groups[2].Value);
                }

                do
                {
                    number++;
                    filepath = Path.Combine(folder, string.Format("{0} ({1}){2}", filename, number, extension));
                }
                while (File.Exists(filepath));
            }

            return filepath;
        }

        public static string ProperTimeSpan(TimeSpan ts)
        {
            string time = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            int hours = (int)ts.TotalHours;
            if (hours > 0) time = hours + ":" + time;
            return time;
        }

        public static object Clone(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                binaryFormatter.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                return binaryFormatter.Deserialize(ms);
            }
        }

        public static void PlaySoundAsync(Stream stream)
        {
            if (stream != null)
            {
                TaskEx.Run(() =>
                {
                    using (stream)
                    using (SoundPlayer soundPlayer = new SoundPlayer(stream))
                    {
                        soundPlayer.PlaySync();
                    }
                });
            }
        }

        public static void PlaySoundAsync(string filepath)
        {
            if (!string.IsNullOrEmpty(filepath) && File.Exists(filepath))
            {
                TaskEx.Run(() =>
                {
                    using (SoundPlayer soundPlayer = new SoundPlayer(filepath))
                    {
                        soundPlayer.PlaySync();
                    }
                });
            }
        }

        public static bool BrowseFile(TextBox tb, string initialDirectory = "", bool detectSpecialFolders = false)
        {
            return BrowseFile("ShareX - " + Resources.Helpers_BrowseFile_Choose_file, tb, initialDirectory, detectSpecialFolders);
        }

        public static bool BrowseFile(string title, TextBox tb, string initialDirectory = "", bool detectSpecialFolders = false)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = title;

                try
                {
                    string path = tb.Text;

                    if (!string.IsNullOrEmpty(path))
                    {
                        path = Path.GetDirectoryName(path);

                        if (Directory.Exists(path))
                        {
                            ofd.InitialDirectory = path;
                        }
                    }
                }
                finally
                {
                    if (string.IsNullOrEmpty(ofd.InitialDirectory) && !string.IsNullOrEmpty(initialDirectory))
                    {
                        ofd.InitialDirectory = initialDirectory;
                    }
                }

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    tb.Text = detectSpecialFolders ? GetVariableFolderPath(ofd.FileName) : ofd.FileName;
                    return true;
                }
            }

            return false;
        }

        public static bool BrowseFolder(TextBox tb, string initialDirectory = "", bool detectSpecialFolders = false)
        {
            return BrowseFolder("ShareX - " + Resources.Helpers_BrowseFolder_Choose_folder, tb, initialDirectory, detectSpecialFolders);
        }

        public static bool BrowseFolder(string title, TextBox tb, string initialDirectory = "", bool detectSpecialFolders = false)
        {
            using (FolderSelectDialog fsd = new FolderSelectDialog())
            {
                fsd.Title = title;

                string path = tb.Text;

                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    fsd.InitialDirectory = path;
                }
                else if (!string.IsNullOrEmpty(initialDirectory))
                {
                    fsd.InitialDirectory = initialDirectory;
                }

                if (fsd.ShowDialog())
                {
                    tb.Text = detectSpecialFolders ? GetVariableFolderPath(fsd.FileName) : fsd.FileName;
                    return true;
                }
            }

            return false;
        }

        public static string GetVariableFolderPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    GetEnums<Environment.SpecialFolder>().ForEach(x => path = path.Replace(Environment.GetFolderPath(x), $"%{x}%", StringComparison.InvariantCultureIgnoreCase));
                }
                catch (Exception e)
                {
                    DebugHelper.WriteException(e);
                }
            }

            return path;
        }

        public static string ExpandFolderVariables(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    GetEnums<Environment.SpecialFolder>().ForEach(x => path = path.Replace($"%{x}%", Environment.GetFolderPath(x), StringComparison.InvariantCultureIgnoreCase));
                    path = Environment.ExpandEnvironmentVariables(path);
                }
                catch (Exception e)
                {
                    DebugHelper.WriteException(e);
                }
            }

            return path;
        }

        public static bool WaitWhile(Func<bool> check, int interval, int timeout = -1)
        {
            Stopwatch timer = Stopwatch.StartNew();

            while (check())
            {
                if (timeout >= 0 && timer.ElapsedMilliseconds >= timeout)
                {
                    return false;
                }

                Thread.Sleep(interval);
            }

            return true;
        }

        public static void WaitWhileAsync(Func<bool> check, int interval, int timeout, Action onSuccess, int waitStart = 0)
        {
            bool result = false;

            TaskEx.Run(() =>
            {
                if (waitStart > 0)
                {
                    Thread.Sleep(waitStart);
                }

                result = WaitWhile(check, interval, timeout);
            },
            () =>
            {
                if (result) onSuccess();
            }, false);
        }

        public static bool IsFileLocked(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }

        public static void CreateDirectoryFromDirectoryPath(string path)
        {
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception e)
                {
                    DebugHelper.WriteException(e);
                    MessageBox.Show(Resources.Helpers_CreateDirectoryIfNotExist_Create_failed_ + "\r\n\r\n" + e, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void CreateDirectoryFromFilePath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                CreateDirectoryFromDirectoryPath(Path.GetDirectoryName(path));
            }
        }

        public static void BackupFileMonthly(string filepath, string destinationFolder)
        {
            if (!string.IsNullOrEmpty(filepath) && File.Exists(filepath))
            {
                string filename = Path.GetFileNameWithoutExtension(filepath);
                string extension = Path.GetExtension(filepath);
                string newFilename = string.Format("{0}-{1:yyyy-MM}{2}", filename, DateTime.Now, extension);
                string newFilepath = Path.Combine(destinationFolder, newFilename);

                if (!File.Exists(newFilepath))
                {
                    CreateDirectoryFromFilePath(newFilepath);
                    File.Copy(filepath, newFilepath, false);
                }
            }
        }

        public static void BackupFileWeekly(string filepath, string destinationFolder)
        {
            if (!string.IsNullOrEmpty(filepath) && File.Exists(filepath))
            {
                string filename = Path.GetFileNameWithoutExtension(filepath);
                DateTime dateTime = DateTime.Now;
                string extension = Path.GetExtension(filepath);
                string newFilename = string.Format("{0}-{1:yyyy-MM}-W{2:00}{3}", filename, dateTime, dateTime.WeekOfYear(), extension);
                string newFilepath = Path.Combine(destinationFolder, newFilename);

                if (!File.Exists(newFilepath))
                {
                    CreateDirectoryFromFilePath(newFilepath);
                    File.Copy(filepath, newFilepath, false);
                }
            }
        }

        public static string GetUniqueID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static Point GetPosition(ContentAlignment placement, Point offset, Size backgroundSize, Size objectSize)
        {
            int midX = backgroundSize.Width / 2 - objectSize.Width / 2;
            int midY = backgroundSize.Height / 2 - objectSize.Height / 2;
            int right = backgroundSize.Width - objectSize.Width;
            int bottom = backgroundSize.Height - objectSize.Height;

            switch (placement)
            {
                default:
                case ContentAlignment.TopLeft:
                    return new Point(offset.X, offset.Y);
                case ContentAlignment.TopCenter:
                    return new Point(midX, offset.Y);
                case ContentAlignment.TopRight:
                    return new Point(right - offset.X, offset.Y);
                case ContentAlignment.MiddleLeft:
                    return new Point(offset.X, midY);
                case ContentAlignment.MiddleCenter:
                    return new Point(midX, midY);
                case ContentAlignment.MiddleRight:
                    return new Point(right - offset.X, midY);
                case ContentAlignment.BottomLeft:
                    return new Point(offset.X, bottom - offset.Y);
                case ContentAlignment.BottomCenter:
                    return new Point(midX, bottom - offset.Y);
                case ContentAlignment.BottomRight:
                    return new Point(right - offset.X, bottom - offset.Y);
            }
        }

        public static Size MeasureText(string text, Font font)
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                return g.MeasureString(text, font).ToSize();
            }
        }

        public static Size MeasureText(string text, Font font, int width)
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                return g.MeasureString(text, font, width).ToSize();
            }
        }

        public static string SendPing(string host)
        {
            return SendPing(host, 1);
        }

        public static string SendPing(string host, int count)
        {
            string[] status = new string[count];

            using (Ping ping = new Ping())
            {
                PingReply reply;
                //byte[] buffer = Encoding.ASCII.GetBytes(new string('a', 32));
                for (int i = 0; i < count; i++)
                {
                    reply = ping.Send(host, 3000);
                    if (reply.Status == IPStatus.Success)
                    {
                        status[i] = reply.RoundtripTime.ToString() + " ms";
                    }
                    else
                    {
                        status[i] = "Timeout";
                    }
                    Thread.Sleep(100);
                }
            }

            return string.Join(", ", status);
        }

        public static string DownloadString(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Encoding = Encoding.UTF8;
                        wc.Headers.Add(HttpRequestHeader.UserAgent, ShareXResources.UserAgent);
                        wc.Proxy = HelpersOptions.CurrentProxy.GetWebProxy();
                        return wc.DownloadString(url);
                    }
                }
                catch (Exception e)
                {
                    DebugHelper.WriteException(e);
                    MessageBox.Show(Resources.Helpers_DownloadString_Download_failed_ + "\r\n" + e, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return null;
        }

        public static void SetDefaultUICulture(CultureInfo culture)
        {
            Type type = typeof(CultureInfo);

            try
            {
                // .NET 4.0
                type.InvokeMember("s_userDefaultUICulture", BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static, null, culture, new object[] { culture });
            }
            catch
            {
                try
                {
                    // .NET 2.0
                    type.InvokeMember("m_userDefaultUICulture", BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static, null, culture, new object[] { culture });
                }
                catch
                {
                    DebugHelper.WriteLine("SetDefaultUICulture failed: " + culture.DisplayName);
                }
            }
        }

        public static string GetAbsolutePath(string path)
        {
            path = ExpandFolderVariables(path);

            if (!Path.IsPathRooted(path)) // Is relative path?
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }

            return Path.GetFullPath(path);
        }

        public static string GetTempPath(string extension)
        {
            string path = Path.GetTempFileName();
            return Path.ChangeExtension(path, extension);
        }

        public static bool IsAdministrator()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static string RepeatGenerator(int count, Func<string> generator)
        {
            string result = "";
            for (int x = count; x > 0; x--)
            {
                result += generator();
            }
            return result;
        }

        public static DateTime UnixToDateTime(long unix)
        {
            long timeInTicks = unix * TimeSpan.TicksPerSecond;
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddTicks(timeInTicks);
        }

        public static long DateTimeToUnix(DateTime dateTime)
        {
            DateTime date = dateTime.ToUniversalTime();
            long ticks = date.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
            return ticks / TimeSpan.TicksPerSecond;
        }

        public static bool IsRunning(string name)
        {
            try
            {
                Mutex mutex = Mutex.OpenExisting(name);
                mutex.ReleaseMutex();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static void ShowError(Exception e)
        {
            MessageBox.Show(e.ToString(), "ShareX - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void CopyAll(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (!Directory.Exists(target.FullName))
            {
                Directory.CreateDirectory(target.FullName);
            }

            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);

            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }

        // http://goessner.net/articles/JsonPath/
        public static string ParseJSON(string text, string jsonPath)
        {
            return (string)JToken.Parse(text).SelectToken("$." + jsonPath);
        }

        public static T[] GetInstances<T>() where T : class
        {
            IEnumerable<T> instances = from t in Assembly.GetCallingAssembly().GetTypes()
                                       where t.IsClass && t.IsSubclassOf(typeof(T)) && t.GetConstructor(Type.EmptyTypes) != null
                                       select Activator.CreateInstance(t) as T;

            return instances.ToArray();
        }

        public static string GetWindowsProductName()
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

                if (rk != null)
                {
                    string productName = rk.GetValue("ProductName") as string;

                    if (!string.IsNullOrEmpty(productName))
                    {
                        return productName;
                    }
                }
            }
            catch
            {
            }

            return Environment.OSVersion.VersionString;
        }

        public static Cursor CreateCursor(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return new Cursor(ms);
            }
        }

        public static string EscapeCLIText(string text)
        {
            return string.Format("\"{0}\"", text.Replace("\\", "\\\\").Replace("\"", "\\\""));
        }
    }
}