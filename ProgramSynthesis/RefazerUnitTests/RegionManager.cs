using System.Collections.Generic;
using RefazerObject.Location;
using RefazerObject.Region;
using RefazerObject.Transformation;
using System;

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
        public Dictionary<string, List<Region>> GroupRegionBySourceFile(List<Region> list)
        {
            Dictionary<string, List<Region>> dic = new Dictionary<string, List<Region>>();
            foreach (var item in list)
            {
                List<Region> value;
                if (!dic.TryGetValue(item.Parent.Text, out value))
                {
                    value = new List<Region>();
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
        public Dictionary<string, List<Region>> GroupRegionBySourcePath(List<Region> list)
        {
            Dictionary<string, List<Region>> dic = new Dictionary<string, List<Region>>();
            foreach (var item in list)
            {
                string path = item.Path.ToUpperInvariant();
                List<Region> value;
                if (!dic.TryGetValue(path, out value))
                {
                    value = new List<Region>();
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
        public Dictionary<string, List<Tuple<Region, string, string>>> GroupTransformationsBySourcePath(List<Tuple<Region, string, string>> list)
        {
            Dictionary<string, List<Tuple<Region, string, string>>> dic = new Dictionary<string, List<Tuple<Region, string, string>>>();
            foreach (var item in list)
            {
                string path = item.Item1.Path.ToUpperInvariant();
                List<Tuple<Region, string, string>> value;
                if (!dic.TryGetValue(path, out value))
                {
                    value = new List<Tuple<Region, string, string>>();
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
        /// <returns>Transformations by source file</returns>
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


