using System.Collections.Generic;
using RefazerObject.Location;
using RefazerObject.Region;
using RefazerObject.Transformation;

namespace RefazerUnitTests
{
    internal class TestUtil
    {
        public static List<CodeLocation> GetAllLocationsOnCommit(List<Region> selections, List<CodeLocation> locations)
        {
            List<CodeLocation> metaLocList = new List<CodeLocation>();
            foreach (var metaLoc in locations)
            {
                metaLoc.Region.Path = metaLoc.SourceClass;
                foreach (Region metaSelec in selections)
                {
                    if (metaLoc.Region.Equals(metaSelec))
                    {
                        metaLocList.Add(metaLoc);
                    }
                }
            }
            return metaLocList;
        }

        public static List<CodeTransformation> GetAllTransformationsOnCommit(List<CodeTransformation> transformations, List<CodeLocation> locations)
        {
            List<CodeTransformation> metaLocList = new List<CodeTransformation>();

            foreach(var transformation in transformations)
            {
                Region tregion = transformation.Location.Region;

                foreach(var location in locations)
                {
                    Region lregion = location.Region;

                    if (tregion.Start == lregion.Start && tregion.Length == lregion.Length && tregion.Path.ToUpperInvariant().Equals(lregion.Path.ToUpperInvariant()))
                    {
                        metaLocList.Add(transformation);
                    }
                }

            }
            return metaLocList;
        }

        //public static Dictionary<string, List<Selection>> FilterLocationsNotPresentOnCommit(Dictionary<string, List<Selection>> dictionarySelection, List<CodeLocation> controllerLocations, List<CodeLocation> commitLocations )
        //{
        //    Dictionary<string, List<CodeLocation>> dicLocs = RegionManager.GetInstance().GroupLocationsBySourceFile(controllerLocations);
  
        //    List<Tuple<CodeLocation, Selection>> tupledList = new List<Tuple<CodeLocation, Selection>>();
        //    foreach (KeyValuePair<string, List<Selection>> item in dictionarySelection)
        //    {
        //        List<CodeLocation> codeLocList = dicLocs[item.Key.ToUpperInvariant()];
        //        for (int i = 0; i < item.Value.Count; i++)
        //        {
        //            Tuple<CodeLocation, Selection> t = Tuple.Create(codeLocList[i], item.Value[i]);
        //            tupledList.Add(t);
        //        }

        //    }

        //    dictionarySelection = new Dictionary<string, List<Selection>>();
        //    foreach (Tuple<CodeLocation, Selection> item in tupledList)
        //    {
        //        List<Selection> value;
        //        if (!dictionarySelection.ContainsKey(item.Item2.SourcePath))
        //        {
        //            value = new List<Selection>();
        //            dictionarySelection.Add(item.Item2.SourcePath, value);
        //        }
        //        if (commitLocations.Contains(item.Item1))
        //        {
        //            dictionarySelection[item.Item2.SourcePath].Add(item.Item2);
        //        }
        //    }
        //    return dictionarySelection;
        //}
    }
}