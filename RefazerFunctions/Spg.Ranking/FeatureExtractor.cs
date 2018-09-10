using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RefazerFunctions.Spg.Ranking
{
    public class FeatureExtractor
    {

        public static Dictionary<Feature, int> Extract(string program)
        {
            var dictionary = new Dictionary<Feature, int>();
            //put here the R code.
            string constNode = "ConstNode";
            var f1 = Regex.Matches(program, constNode);
            int v1 = f1.Count;
            dictionary.Add(Feature.Constants, v1);

            string reference = "Reference";
            var f2 = Regex.Matches(program, reference);
            int v2 = f2.Count;
            dictionary.Add(Feature.References, v2);

            string concretePattern = "Concrete";
            var f3 = Regex.Matches(program, concretePattern);
            int v3 = f3.Count;
            dictionary.Add(Feature.Concrete, v3);

            string abstractPattern = "Abstract";
            var f4 = Regex.Matches(program, abstractPattern);
            int v4 = f4.Count;
            dictionary.Add(Feature.Abstract, v4);

            string node = "Node";
            var f5 = Regex.Matches(program, node);
            int v5 = f5.Count;
            dictionary.Add(Feature.Nodes, v5);

            string pattern = "Pattern";
            var f6 = Regex.Matches(program, pattern);
            int v6 = f6.Count;
            dictionary.Add(Feature.Patterns, v6);

            string parentOne = "\\/\\[[0-9]\\]";
            var f7 = Regex.Matches(program, parentOne);
            int v7 = f7.Count;
            dictionary.Add(Feature.ParentOne, v7);

            string parentTwo = "\\/\\[[0-9]\\]\\/\\[[0-9]\\]";
            var f8 = Regex.Matches(program, parentTwo);
            int v8 = f8.Count;
            dictionary.Add(Feature.ParentTwo, v8);

            string parentThree = "\\/\\[[0-9]\\]\\/\\[[0-9]\\]\\/\\[[0-9]\\]";
            var f9 = Regex.Matches(program, parentThree);
            int v9 = f9.Count;
            dictionary.Add(Feature.ParentThree, v9);

            string nodeItSelf = "\\.";
            var f10 = Regex.Matches(program, nodeItSelf);
            int v10 = f10.Count;
            dictionary.Add(Feature.NodeItSelf, v10);

            string sizeProg = "\\(";
            var f11 = Regex.Matches(program, sizeProg);
            int v11 = f11.Count;
            dictionary.Add(Feature.Size, v11);

            string numberOp = "EditMap";
            var f12 = Regex.Matches(program, numberOp);
            int v12 = f12.Count;
            dictionary.Add(Feature.Operations, v12);

            //   int[,,,,,,,,,,,] vecFeature = new int[v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12];
            int[] vecFeat = { v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12 };
            //return vecFeat;
            return dictionary;
        }
    }
}