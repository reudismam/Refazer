using System;
using Newtonsoft.Json;
using TreeElement;

namespace RefazerUnitTests
{
    /// <summary>
    /// JSON utility
    /// </summary>
    public class JsonUtil<T>
    {
        /// <summary>
        /// Write object to data
        /// </summary>
        /// <param name="t">Object</param>
        /// <param name="path">File path</param>
        public static void Write(T t, string path)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            string json = "";
            try
            {
                json = JsonConvert.SerializeObject(t, Formatting.Indented,
                    new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
                file.Write(json);
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("Could not write to file: " + path);
            }
            finally
            {
                file.Close();
            }
        }

        /// <summary>
        /// Reads JSON objects
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Object</returns>
        public static T Read(string path)
        {
            string json = FileUtil.ReadFile(path);
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
    }
}

