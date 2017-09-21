namespace RefazerObject.Constants
{
    public class LogData
    {
        /// <summary>
        ///defines the path for the log file.
        /// </summary>
        public static string LogPath()
        {
            return Environment.Environment.ExpHome() + TestConstants.LogPathFile;
        }
    }
}
