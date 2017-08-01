using System.Collections.Generic;
using System.Linq;

namespace RefazerFunctions.Spg.Config
{
    public class SynthesisConfig
    {
        /// <summary>
        /// Singleton Instance
        /// </summary>
        private static SynthesisConfig _instance;
        /// <summary>
        /// Defines if tokens will be considered, default true
        /// </summary>
        public bool UseTokens { get; set; }
        /// <summary>
        /// Defines the minimum number of descendants that a node must contains, default 40
        /// </summary>
        public int DescendantsParentThreshouldForContext { get; set; } 
        /// <summary>
        /// Defines levels to be considered in context, default [0, 1, 2]
        /// </summary>
        public List<int> LevelsForContext { get; set; }

        public bool CreateLog { get; set; }
        public bool BoundGeneratedPrograms { get; set; }

        private SynthesisConfig()
        {
            UseTokens = true;
            CreateLog = true;
            BoundGeneratedPrograms = true;
            DescendantsParentThreshouldForContext = 40;
            LevelsForContext = Enumerable.Range(0, 3).ToList();
        }

        public static SynthesisConfig GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SynthesisConfig();
            }
            return _instance;
        }
    }
}
