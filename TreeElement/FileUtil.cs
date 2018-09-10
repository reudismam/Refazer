using System;
using System.IO;
using System.Linq;

namespace TreeElement
{
    /// <summary>
    /// Manage files
    /// </summary>
    public static class FileUtil
    {
        /// <summary>
        /// Read a file and return a string as its content.
        /// </summary>
        /// <returns>String representing the content of the file</returns>
        public static String ReadFile(string path)
        {
            //path = NormalizePath(path);
            // Open the text file using a stream reader.
            using (StreamReader sr = new StreamReader(path))
            {
                // Read the stream to a string, and write the string to the console.
                String line = sr.ReadToEnd();
                return line;
            }
        }
        private static string NormalizePath(string path)
        {
            var result = path.Split(@"\".ToArray(), StringSplitOptions.None);
            var mypath = @"C:\Users\SPG-09\Documents\";
            var newresult = result.ToList().GetRange(4, result.Count() - 4).ToArray();
            //Insert code to remove specific computer path
            var newpath = Path.Combine(newresult);
            var resultPath = mypath + newpath;
            return resultPath;
        }
        /// <summary>
        /// Write string data to a file
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="sourceCode">Source code</param>
        public static void WriteToFile(string path, string sourceCode)
        {
            int index = path.LastIndexOf('/');
            if (index != -1) {
                string folder = path.Substring(0, index);
                Directory.CreateDirectory(folder);
            }
            StreamWriter file = new StreamWriter(path);
            file.Write(sourceCode);
            file.Close();
        }

        public static void AppendToFile(string path, string text)
        {
            File.AppendAllText(path, text);
        }

        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }

        /// <summary>
        /// Specifies the locations where this solution is on.
        /// </summary>
        public static string GetBasePath()
        {
            string startupPath = Directory.GetCurrentDirectory();
            string result = GetBasePath(startupPath);
            return result;
        }

        /// <summary>
        /// Specifies the locations where this solution is on.
        /// </summary>
        private static string GetBasePath(string executionPath)
        {
            var path = Path.GetFullPath(executionPath);
            var directory = Directory.CreateDirectory(path);
            while (directory != null && !directory.EnumerateFiles().Any(o => o.Extension.Contains("sln")))
            {
                directory = directory.Parent;
            }
            if (directory == null)
            {
                throw new Exception("The path to the folder of the solution could not be found.");
            }
            return directory.FullName;
        }
    }
}


