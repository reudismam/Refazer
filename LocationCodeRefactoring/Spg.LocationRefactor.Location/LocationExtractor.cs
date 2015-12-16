using System;
using System.Collections.Generic;
using System.Linq;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Controller;
using Spg.LocationRefactor.Program;
using Spg.LocationRefactor.Transform;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Comparator;
using Spg.LocationRefactor.Learn;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Location
{
    /// <summary>
    /// Location extractor
    /// </summary>
    public class LocationExtractor
    {
        /// <summary>
        /// Learner
        /// </summary>
        private readonly Learner _learn;

        ///// <summary>
        ///// Solution path
        ///// </summary>
        //string _solutionPath;

        /// <summary>
        /// Controller
        /// </summary>
        public EditorController Controller = EditorController.GetInstance();

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationExtractor()
        {
            _learn = new Learner();
        }

        /// <summary>
        /// Decompose location
        /// </summary>
        /// <param name="regions">List of selected regions</param>
        /// <returns>Extracted locations</returns>
        public List<Prog> Extract(List<TRegion> regions)
        {
            List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();
            List<Prog> programs;

            if (regions.Count() == 1)
            {
                foreach (TRegion region in regions)
                {
                    Tuple<string, string> ex = Tuple.Create(region.Parent.Text, region.Text);
                    Tuple<ListNode, ListNode> lex = ASTProgram.Example(ex);
                    examples.Add(lex);
                }
                programs = _learn.LearnRegion(examples);
            }
            else
            {
                examples = _learn.Decompose(regions);
                programs = _learn.LearnSeqRegion(examples);
            }
            return programs;
        }

        /// <summary>
        /// Decompose method
        /// </summary>
        /// <param name="positiveRegions">Positives examples</param>
        /// <param name="negativeRegions">Negative examples</param>
        /// <returns>List of learned programs</returns>
        internal List<Prog> Extract(List<TRegion> positiveRegions, List<TRegion> negativeRegions)
        {
            List<Tuple<ListNode, ListNode>> positiveExamples = new List<Tuple<ListNode, ListNode>>();
            List<Prog> programs;

            if (positiveRegions.Count() == 1)
            {
                foreach (TRegion region in positiveRegions)
                {
                    Tuple<string, string> ex = Tuple.Create(region.Parent.Text, region.Text);
                    Tuple<ListNode, ListNode> lex = ASTProgram.Example(ex);
                    positiveExamples.Add(lex);
                }
                programs = _learn.LearnRegion(positiveExamples);
            }
            else
            {
                positiveExamples = _learn.Decompose(positiveRegions);
                var negativeExamples = _learn.Decompose(negativeRegions);
                programs = _learn.LearnSeqRegion(positiveExamples, negativeExamples);
            }

            //if (programs.Any())
            //{
            //    Console.WriteLine("Top ranked synthesized location program: " + programs.First());
            //}

            return programs;
        }

        /// <summary>
        /// Retrieve the list of region in input
        /// </summary>
        /// <param name="program">Program</param>
        /// <returns>Regions list</returns>
        public List<TRegion> RetrieveString(Prog program)
        {
            List<TRegion> regions = program.RetrieveString();
            return regions;
        }

        /// <summary>
        /// Retrieve the list of regions on input node
        /// </summary>
        /// <param name="program">Synthesis program</param>
        /// <param name="sourceCode">Source code</param>
        /// <param name="input">Input node</param>
        /// <returns>List of regions present on input</returns>
        public List<TRegion> RetrieveString(Prog program, string sourceCode, SyntaxNode input)
        {
            List<TRegion> regions = program.RetrieveString(input, sourceCode);
            return regions;
        }

        /// <summary>
        /// Transform selection regions
        /// </summary>
        /// <returns>Transformation</returns>
        public List<Transformation> TransformProgram(bool compact)
        {
            var manager = new MappedLocationBasedTransformationManager();
            return manager.TransformProgram(compact);
        }

        /// <summary>
        /// Decompose location
        /// </summary>
        /// <param name="codeBefore">Code before transformation</param>
        /// <param name="codeAfter">Code after transformation</param>
        /// <returns>Locations</returns>
        public List<Tuple<TRegion, TRegion>> ExtractEditedRegions(string codeBefore, string codeAfter)
        {
            RegionManager strategy = RegionManager.GetInstance();
            List<Tuple<SyntaxNode, SyntaxNode>> pairs = strategy.SyntaxNodesRegionBeforeAndAfterEditing(Controller.Locations);

            var examples = new List<Tuple<TRegion, TRegion>>();

            for (int i = 0; i < pairs.Count; i++)
            {
                string statementBefore = pairs[i].Item1.GetText().ToString();
                string statementAfter = pairs[i].Item2.GetText().ToString();
                Tuple<SyntaxNode, SyntaxNode> sn = Tuple.Create(pairs[i].Item1, pairs[i].Item2);
                Tuple<ListNode, ListNode > ln = ASTProgram.Example(sn);

                NodeComparer comparator = new NodeComparer();
                bool isEqual = comparator.SequenceEqual(ln.Item1, ln.Item2);
                if (!isEqual)
                {
                    TRegion regionBefore = new TRegion();
                    regionBefore.Text = statementBefore;
                    regionBefore.Node = pairs[i].Item1;

                    TRegion regionAfter = new TRegion();
                    regionAfter.Text = statementAfter;
                    regionAfter.Node = pairs[i].Item2;

                    Tuple<TRegion, TRegion> example = Tuple.Create(regionBefore, regionAfter);
                    examples.Add(example);
                }
            }
            return examples;
        }
    }
}





