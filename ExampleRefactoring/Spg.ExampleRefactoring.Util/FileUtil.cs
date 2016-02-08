using System;
using System.IO;

namespace Spg.ExampleRefactoring.Util
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
            //string value = File.ReadAllText(path);
            //return value;
            ////string s = "";

            ////using (StreamReader sr = new StreamReader(path))
            ////{
            ////    string line;

            ////    while ((line = sr.ReadLine()) != null)
            ////    {
            ////        s += line;
            ////    }
            ////}

            ////return s;

            // Open the text file using a stream reader.
            using (StreamReader sr = new StreamReader(path))
            {
                // Read the stream to a string, and write the string to the console.
                String line = sr.ReadToEnd();
                return line;
            }
        }

        /// <summary>
        /// Write string data to a file
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="sourceCode">Source code</param>
        public static void WriteToFile(string path, string sourceCode)
        {
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
    }
}


