using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Synthesis
{
    /// <summary>
    /// Boundary manager class
    /// </summary>
    public class BoundaryManager
    {

        /// <summary>
        /// Boundary points
        /// </summary>
        /// <returns></returns>
        public List<int> boundary { get; set; }

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static BoundaryManager instance;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="boundary"></param>
        private BoundaryManager(List<int> boundary)
        {
            this.boundary = boundary;
        }

        /// <summary>
        /// Get Instance
        /// </summary>
        /// <param name="boundary">Boundary Points</param>
        /// <returns>Instance</returns>
        public static BoundaryManager GetInstance(List<int> boundary)
        {
            if (instance == null)
            {
                instance = new BoundaryManager(boundary);
            }

            return instance;
        }

        /// <summary>
        /// Get instance
        /// </summary>
        /// <returns>Instance</returns>
        public static BoundaryManager GetInstance()
        {
            return instance;
        }

    }
}
