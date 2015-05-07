using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Spg.ExampleRefactoring.Util;

namespace ExampleRefactoring.Spg.ExampleRefactoring.Util
{
    /// <summary>
    /// Json utility
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
            }
            catch (OutOfMemoryException)
            {
                file.Write(json);
                file.Close();
            }
        }

        /// <summary>
        /// Read Json object
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
