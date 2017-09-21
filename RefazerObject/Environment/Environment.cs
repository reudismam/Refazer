using System;

namespace RefazerObject.Environment
{
    public class Environment
    {
        /// <summary>
        /// returns the root of the experiment folder
        /// </summary>
        public static string ExpHome()
        {
            return System.Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);
        }
    }
}
