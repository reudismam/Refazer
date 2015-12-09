using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactor.Program;
using System.Linq;

namespace Spg.LocationRefactor.Operator.Map
{
    /// <summary>
    /// Map operator
    /// </summary>
    public class Merge : IOperator
    {
        public List<Prog> maps { get; set; }

        /// <summary>
        /// Execute map
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Matched statements</returns>
        public ListNode Execute(string input)
        {
            foreach (var map in maps)
            {
                ListNode ln = map.Execute(input);
                if (ln != null)
                {
                    return ln;
                }
            }
            return null;
        }

        /// <summary>
        /// Execute map
        /// </summary>
        /// <param name="input">Syntax node input</param>
        /// <returns>Result of map execution</returns>
        public ListNode Execute(SyntaxNode input)
        {
            foreach (var map in maps)
            {
                ListNode ln = map.Execute(input);
                if (ln != null)
                {
                    return ln;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieve region from input
        /// </summary>
        /// <param name="filtereds">Filtered regions</param>
        /// <returns>Region list</returns>
        private List<TRegion> RetrieveRegionBase(List<TRegion> filtereds)
        {
            List<TRegion> regions = new List<TRegion>();

            foreach (var map in maps)
            {
                var rs = (map.Ioperator as MapBase).RetrieveRegionBase(filtereds);
                regions.AddRange(regions);
            }

            return regions;
        }

        /// <summary>
        /// Retrieve region from input
        /// </summary>
        /// <returns>Region list</returns>
        public virtual List<TRegion> RetrieveRegion()
        {
            List<TRegion> regions = new List<TRegion>();

            foreach (var map in maps)
            {
                var rs = (map.Ioperator as MapBase).RetrieveRegion();
                regions.AddRange(regions);
            }

            return regions;
        }

        /// <summary>
        /// Retrieve region of the source code
        /// </summary>
        /// <param name="syntaxNode">Syntax node to be considered</param>
        /// <param name="sourceCode">Source code</param>
        /// <returns>List of region on the source code</returns>
        public List<TRegion> RetrieveRegion(SyntaxNode syntaxNode, string sourceCode)
        {
            List<TRegion> regions = new List<TRegion>();

            foreach (var map in maps)
            {
                var rs = (map.Ioperator as MapBase).RetrieveRegion(syntaxNode, sourceCode);
                regions.AddRange(rs);
            }

            regions = NonDuplicateRegions(regions);

            return regions;
        }


        /// <summary>
        /// Selected only non duplicate locations
        /// </summary>
        /// <param name="regions">All locations</param>
        /// <returns>Non duplicate locations</returns>
        private static List<TRegion> NonDuplicateRegions(List<TRegion> regions)
        {
            List<TRegion> removes = new List<TRegion>();
            foreach (TRegion rs in regions)
            {
                foreach (TRegion region in regions)
                {
                    if (rs != region && region.IsInside(rs))
                    {
                        removes.Add(region);
                    }
                }
            }
            List<TRegion> nonDup = regions.Except(removes).ToList();
            return nonDup;
        }

    }
}


