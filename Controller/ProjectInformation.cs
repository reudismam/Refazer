using System.Collections.Generic;

namespace Controller
{
    /// <summary>
    /// Information related to the project
    /// </summary>
    public class ProjectInformation
    {
        /// <summary>
        /// Solution path of this project
        /// </summary>
        /// <returns>Solution path</returns>
        public string SolutionPath { get; set; }

        /// <summary>
        /// Project path
        /// </summary>
        /// <returns></returns>
        public List<string> ProjectPath { get; set; }

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static ProjectInformation _instance;

        private ProjectInformation()
        {
            ProjectPath = new List<string>();
        }

        /// <summary>
        /// Get a new instance of ProjectInformation
        /// </summary>
        /// <returns></returns>
        public static ProjectInformation GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ProjectInformation();
            }
            return _instance;
        }
    }
}

