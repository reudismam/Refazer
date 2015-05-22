using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis.Text;
using Spg.LocationRefactor.TextRegion;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Location
{
    internal class ReferenceManager
    {
        /// <summary>
        /// Identify on the dictionary what entry corresponds to the element selection by the user.
        /// </summary>
        /// <param name="dictionary">Dictionary</param>
        /// <param name="selection">Selection</param>
        /// <returns>Regions grouped by selection</returns>
        internal static Dictionary<string, Dictionary<string, List<TextSpan>>> GroupReferencesBySelection(Dictionary<string, Dictionary<string, List<TextSpan>>> dictionary, List<TRegion> selection)
        {
            Dictionary<string, Dictionary<string, List<TextSpan>>> result = new Dictionary<string, Dictionary<string, List<TextSpan>>>();
            foreach (KeyValuePair<string, Dictionary<string, List<TextSpan>>> dictReferences in dictionary)
            {
                Dictionary<string, List<TextSpan>> fileLocationDictionary = dictReferences.Value;
                foreach (TRegion region in selection)
                {
                    string path = Path.GetFullPath(region.Path).ToUpperInvariant();
                    if (fileLocationDictionary.ContainsKey(path))
                    {
                        List<TextSpan> listSpans = fileLocationDictionary[path];
                        foreach (TextSpan span in listSpans)
                        {
                            TRegion spanRegion = new TRegion();
                            spanRegion.Start = span.Start;
                            spanRegion.Length = span.Length;

                            if (region.IntersectWith(spanRegion))
                            {
                                if (!result.ContainsKey(dictReferences.Key))
                                {
                                    result.Add(dictReferences.Key, fileLocationDictionary);
                                }
                                else
                                {
                                    Console.Write("Key already exist on the dictionary.");
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
