using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Setting;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Location;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.Controller;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Transformation
{
    public class TransformationManager
    {
        protected readonly EditorController Controller = EditorController.GetInstance();
        /// <summary>
        /// Return program able to transform locations.
        /// </summary>
        /// <param name="regionsToApplyTransformation">Regions to apply transformation</param>
        /// <returns></returns>
        public virtual SynthesizedProgram TransformationProgram(List<TRegion> regionsToApplyTransformation)
        {
            List<Tuple<TRegion, TRegion>> exampleRegions = ExtractEditedRegions();
            var manager = new TransformationManager();
            List<Tuple<ListNode, ListNode>> examples = manager.ListNodes(exampleRegions);

            ASTProgram program = new ASTProgram();

            List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
            SynthesizedProgram validated = synthesizedProgs.Single();

            return validated;
        }

        /// <summary>
        /// Transform selection regions
        /// </summary>
        /// <returns></returns>
        public virtual List<Transformation> TransformProgram(bool compact)
        {
            List<Tuple<TRegion, TRegion>> exampleRegions = ExtractEditedRegions();

            EditorController controller = EditorController.GetInstance();
            List<Tuple<ListNode, ListNode>> examples = ListNodes(exampleRegions);

            SynthesizedProgram validated = LearnSynthesizerProgram(examples); //learn a synthesizer program

            var locations = controller.Locations; //previous locations

            Dictionary<string, List<CodeLocation>> groupLocation = Groups(locations); //location for each file

            var transformations = new List<Transformation>();
            foreach (KeyValuePair<string, List<CodeLocation>> item in groupLocation)
            {
                string text = Transform(validated, item.Value, compact);
                Tuple<string, string> beforeAfter = Tuple.Create(item.Value[0].SourceCode, text);
                Transformation transformation = new Transformation(beforeAfter, item.Key);
                transformations.Add(transformation);
            }
            return transformations;
        }

        /// <summary>
        /// Transform a program
        /// </summary>
        /// <param name="program">Synthesized program</param>
        /// <param name="locations">Locations</param>
        /// <param name="compact">Indicates if entry must be compacted or not</param>
        /// <returns>Transformed program</returns>
        public virtual string Transform(SynthesizedProgram program, List<CodeLocation> locations, bool compact)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(locations[0].SourceCode); // all code location have the same source code
            List<SyntaxNode> update = new List<SyntaxNode>();
            foreach (CodeLocation location in locations)
            {
                //SyntaxNode selection = strategy.SyntaxNodesRegion(location.SourceCode, location.Region);
                SyntaxNode selection = location.Region.Node;

                var decedents = from snode in tree.GetRoot().DescendantNodes()
                                where snode.Span.Start == selection.Span.Start && snode.Span.Length == selection.Span.Length
                                select snode;
                update.AddRange(decedents);
            }

            var text = FileUtil.ReadFile(locations[0].SourceClass);
            foreach (SyntaxNode node in update)
            {
                try
                {
                    List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
                    if (compact)
                    {
                        list = ASTManager.EnumerateSyntaxNodesAndTokens2(node, list);
                    }
                    else
                    {
                        list = ASTManager.EnumerateSyntaxNodesAndTokens(node, list);
                    }
                    
                    ListNode lnode = new ListNode(list);

                    ASTTransformation treeNode = ASTProgram.TransformString(lnode, program);
                    string transformation = treeNode.Transformation;
                    string nodeText = node.GetText().ToString();
                    string escaped = Regex.Escape(nodeText);
                    string replacement = Regex.Replace(text, escaped, transformation);
                    text = replacement;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            SyntaxTree treeFormat = CSharpSyntaxTree.ParseText(text);
            SyntaxNode nodeFormat = treeFormat.GetRoot();//.NormalizeWhitespace();
            text = nodeFormat.GetText().ToString();
            return text;
        }

        /// <summary>
        /// Group location by file path
        /// </summary>
        /// <param name="locations">Location</param>
        /// <returns>Grouping</returns>
        protected static Dictionary<string, List<CodeLocation>> Groups(List<CodeLocation> locations)
        {
            Dictionary<string, List<CodeLocation>> dic = new Dictionary<string, List<CodeLocation>>();
            foreach (CodeLocation location in locations)
            {
                List<CodeLocation> value;
                if (!dic.TryGetValue(location.SourceClass, out value))
                {
                    value = new List<CodeLocation>();
                    dic[location.SourceClass] = value;
                }
                value.Add(location);
            }
            return dic;
        }

        /// <summary>
        /// Learn a synthesizer program
        /// </summary>
        /// <param name="examples">Examples</param>
        /// <returns>Synthesizer program</returns>
        protected SynthesizedProgram LearnSynthesizerProgram(List<Tuple<ListNode, ListNode>> examples)
        {
            SynthesizerSetting setting = new SynthesizerSetting
            {
                DynamicTokens = true,
                Deviation = 2,
                ConsiderEmpty = true,
                ConsiderConstrStr = true,
                CreateTokenSeq = false
            }; //configure setting

            ASTProgram program = new ASTProgram(setting, examples); //create a new AST program and learn synthesizer
            List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
            SynthesizedProgram validated = synthesizedProgs.Single();
            return validated;
        }

        /// <summary>
        /// List node in the text regions
        /// </summary>
        /// <param name="examples">Text regions</param>
        /// <returns>Regions</returns>
        public List<Tuple<ListNode, ListNode>> ListNodes(List<Tuple<TRegion, TRegion>> examples)
        {
            List<Tuple<ListNode, ListNode>> nodes = new List<Tuple<ListNode, ListNode>>();
            foreach (Tuple<TRegion, TRegion> tuple in examples)
            {
                Tuple<SyntaxNode, SyntaxNode> tnode = Tuple.Create(tuple.Item1.Node, tuple.Item2.Node);
                Tuple<ListNode, ListNode> tlnode = ASTProgram.Example(tnode);
                nodes.Add(tlnode);
            }
            return nodes;
        }

        /// <summary>
        /// Decompose location
        /// </summary>
        /// <returns>Locations</returns>
        public virtual  List<Tuple<TRegion, TRegion>> ExtractEditedRegions()
        {
            RegionManager rManager = RegionManager.GetInstance();
            List<Tuple<SyntaxNode, SyntaxNode>> pairs = rManager.SyntaxNodesRegionBeforeAndAfterEditing(Controller.Locations);

            var examples = new List<Tuple<TRegion, TRegion>>();

            for (int i = 0; i < pairs.Count; i++)
            {
                string statementBefore = pairs[i].Item1.GetText().ToString();
                string statementAfter = pairs[i].Item2.GetText().ToString();
                Tuple<SyntaxNode, SyntaxNode> sn = Tuple.Create(pairs[i].Item1, pairs[i].Item2);
                Tuple<ListNode, ListNode> ln = ASTProgram.Example(sn);

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

        /// <summary>
        /// Look for a place to put this. Refactor
        /// </summary>
        /// <returns></returns>
        public Tuple<string, string> Transformation(CodeLocation location, SynthesizedProgram program)
        {
            TRegion region = location.Region;
            Tuple<SyntaxNode, SyntaxNode> node = Tuple.Create(region.Node, region.Node);
            try
            {
                ListNode lnode = ASTProgram.Example(node).Item1;
                ASTTransformation tree = ASTProgram.TransformString(lnode, program);
                string transformation = tree.Transformation;

                Tuple<string, string> transformedLocation = Tuple.Create(region.Text, transformation);
                return transformedLocation;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}






