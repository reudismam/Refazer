using System;
using System.Collections.Generic;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactor.Transform;

namespace Spg.LocationRefactor.Location
{
    /// <summary>
    /// Strategy
    /// </summary>
    public class RegionManager
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static RegionManager _instance;

        /// <summary>
        /// Create a new instance
        /// </summary>
        public static void Init()
        {
            _instance = null;
        }

        /// <summary>
        /// Return the singleton instance
        /// </summary>
        /// <returns>Singleton instance</returns>
        public static RegionManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RegionManager();
            }
            return _instance;
        }

        /// <summary>
        /// Group region by source file
        /// </summary>
        /// <param name="list">List of no grouped regions</param>
        /// <returns>Regions grouped by source file</returns>
        public Dictionary<string, List<TRegion>> GroupRegionBySourceFile(List<TRegion> list)
        {
            Dictionary<string, List<TRegion>> dic = new Dictionary<string, List<TRegion>>();
            foreach (var item in list)
            {
                List<TRegion> value;
                if (!dic.TryGetValue(item.Parent.Text, out value))
                {
                    value = new List<TRegion>();
                    dic[item.Parent.Text] = value;
                }

                dic[item.Parent.Text].Add(item);
            }
            return dic;
        }

        /// <summary>
        /// Group region by source file
        /// </summary>
        /// <param name="list">List of no grouped regions</param>
        /// <returns>Regions grouped by source file</returns>
        public Dictionary<string, List<TRegion>> GroupRegionBySourcePath(List<TRegion> list)
        {
            Dictionary<string, List<TRegion>> dic = new Dictionary<string, List<TRegion>>();
            foreach (var item in list)
            {
                string path = item.Path.ToUpperInvariant();
                List<TRegion> value;
                if (!dic.TryGetValue(path, out value))
                {
                    value = new List<TRegion>();
                    dic[path] = value;
                }

                dic[path].Add(item);
            }
            return dic;
        }

        /// <summary>
        /// Group region by source file
        /// </summary>
        /// <param name="list">List of no grouped regions</param>
        /// <returns>Regions grouped by source file</returns>
        public Dictionary<string, List<CodeTransformation>> GroupTransformationsBySourcePath(List<CodeTransformation> list)
        {
            Dictionary<string, List<CodeTransformation>> dic = new Dictionary<string, List<CodeTransformation>>();
            foreach (var item in list)
            {
                string path = item.Location.SourceClass.ToUpperInvariant();
                List<CodeTransformation> value;
                if (!dic.TryGetValue(path, out value))
                {
                    value = new List<CodeTransformation>();
                    dic[path] = value;
                }

                dic[path].Add(item);
            }
            return dic;
        }

        /// <summary>
        /// Group location by source file
        /// </summary>
        /// <param name="list">Location list</param>
        /// <returns>Locations by source file</returns>
        public Dictionary<string, List<CodeLocation>> GroupLocationsBySourceFile(List<CodeLocation> list)
        {
            Dictionary<string, List<CodeLocation>> dic = new Dictionary<string, List<CodeLocation>>();
            foreach (var item in list)
            {
                List<CodeLocation> value;
                if (!dic.TryGetValue(item.SourceClass.ToUpperInvariant(), out value))
                {
                    value = new List<CodeLocation>();
                    dic[item.SourceClass.ToUpperInvariant()] = value;
                }

                dic[item.SourceClass.ToUpperInvariant()].Add(item);
            }
            return dic;
        }
    }
}


