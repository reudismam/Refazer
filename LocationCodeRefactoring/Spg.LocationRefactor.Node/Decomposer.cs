using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExampleRefactoring.Spg.ExampleRefactoring.Bean;
using Spg.ExampleRefactoring.Projects;
using Spg.ExampleRefactoring.Workspace;
using Spg.LocationRefactor.Location;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Utilities;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Controller;
using Spg.LocationRefactor.TextRegion;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactoring.Tok;

namespace Spg.LocationRefactor.Node
{
    /// <summary>
    /// Decompose element to be used on the filtering estage.
    /// The decomposition can be syntactical or semantical.
    /// </summary>
    public class Decomposer
    {
        /// <summary>
        /// Controler instance
        /// </summary>
        private static readonly EditorController Ctl = EditorController.GetInstance();

        /// <summary>
        /// Reference dictionary
        /// </summary>
        private readonly Dictionary<SelectionInfo, IEnumerable<SyntaxNode>> _dicReferences = new Dictionary<SelectionInfo, IEnumerable<SyntaxNode>>();

        /// <summary>
        /// Singletion instance
        /// </summary>
        private static Decomposer _instance;

        /// <summary>
        /// Constructor
        /// </summary>
        private Decomposer()
        {
        }

        /// <summary>
        /// Return singleton instance
        /// </summary>
        /// <returns>Singleton instance</returns>
        public static Decomposer GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Decomposer();
            }
            return _instance;
        }

        /// <summary>
        /// Initialize singleton instance
        /// </summary>
        public static void Init()
        {
            _instance = null;
        }

        /// <summary>
        /// Choose corresponding syntax node for the region
        /// </summary>
        /// <param name="statements">Statement list</param>
        /// <param name="regions">Region</param>
        /// <returns>Pair of region with the respectively syntax node</returns>
        private Dictionary<TRegion, SyntaxNode> ChoosePairs(List<SyntaxNode> statements, List<TRegion> regions)
        {
            Dictionary<TRegion, SyntaxNode> dicRegions = new Dictionary<TRegion, SyntaxNode>();

            foreach (SyntaxNode statement in statements)
            {
                foreach (TRegion region in regions)
                {
                    if (region.Length == 0)
                    {
                        dicRegions[region] = null;
                        continue;
                    }

                    string text = region.Text;
                    string pattern = Regex.Escape(text);
                    string statmentText = statement.GetText().ToString();
                    bool contains = Regex.IsMatch(statmentText, pattern);
                    if (contains)
                    {
                        if (statement.SpanStart <= region.Start && region.Start <= statement.Span.End)
                        {
                            if (!dicRegions.ContainsKey(region))
                            {
                                dicRegions.Add(region, statement);
                            }
                            else if (statement.GetText().Length < dicRegions[region].GetText().Length)
                            {
                                dicRegions[region] = statement;
                            }
                        }
                    }
                }
            }
            return dicRegions;
        }

        /// <summary>
        /// Decompose set of selected region to list of examples
        /// </summary>
        /// <param name="list">List of selected regions</param>
        /// <returns>Decomposed examples</returns>
        public List<Tuple<ListNode, ListNode>> DecomposeToOutput(List<TRegion> list)
        {
            List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();

            Dictionary<string, List<TRegion>> dicRegion = RegionManager.GetInstance().GroupRegionBySourceFile(list);
            foreach (var entry in dicRegion)
            {
                SyntaxTree tree = CSharpSyntaxTree.ParseText(entry.Key, path: entry.Value.First().Path);
                foreach (TRegion re in entry.Value)
                {
                    SyntaxNode node = tree.GetRoot();

                    Tuple<ListNode, ListNode> te;
                    if (re.Length != 0)
                    {
                        te = Example(node, re);
                    }
                    else
                    {
                        ListNode emptyNode = new ListNode(new List<SyntaxNodeOrToken>());
                        te = Tuple.Create(emptyNode, emptyNode);
                    }
                   
                    examples.Add(te);
                }
            }
            return examples;
        }

        /// <summary>
        /// Decompose set of selected region to list of examples
        /// </summary>
        /// <param name="list">List of selected regions</param>
        /// <returns>Decomposed examples</returns>
        public List<Tuple<ListNode, ListNode>> Decompose(List<TRegion> list)
        {
            List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();

            Dictionary<string, List<TRegion>> dicRegion = RegionManager.GetInstance().GroupRegionBySourceFile(list);
            foreach (var entry in dicRegion)
            {
                List<SyntaxNode> statements = RegionManager.GetInstance().SyntaxNodes(entry.Key, entry.Value);
                Dictionary<TRegion, SyntaxNode> pairs = ChoosePairs(statements, entry.Value);
                foreach (KeyValuePair<TRegion, SyntaxNode> pair in pairs)
                {
                    TRegion re = pair.Key;
                    SyntaxNode node = pair.Value;

                    Tuple<ListNode, ListNode> te;
                    if (node != null)
                    {
                        te = Example(node, re);
                    }
                    else
                    {
                        ListNode emptyNode = new ListNode(new List<SyntaxNodeOrToken>());
                        te = Tuple.Create(emptyNode, emptyNode);
                    }

                    examples.Add(te);
                }
            }
            return examples;
        }

        /// <summary>
        /// Covert the region on a method to an example ListNode
        /// </summary>
        /// <param name="syntaxNode">Syntax node</param>
        /// <param name="re">Region within the method</param>
        /// <param name="compact">Needs to be compacted</param>
        /// <returns>An example</returns>
        public Tuple<ListNode, ListNode> Example(SyntaxNode syntaxNode, TRegion re, bool compact = false)
        {
            List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
            if (compact)
            {
                list = ASTManager.EnumerateSyntaxNodesAndTokens2(syntaxNode, list);
            }
            else
            {
                list = ASTManager.EnumerateSyntaxNodesAndTokens(syntaxNode, list);
            }
            ListNode listNode = new ListNode(list);
            listNode.OriginalText = syntaxNode.GetText().ToString();

            SyntaxNodeOrToken node = list[0];

            int i = 0;
            while (re.Start > node.Span.Start)
            {
                node = list[i++];
            }

            int j = i;
            bool last = false;
            while (re.Start + re.Length >= node.Span.End)
            {
                if (node.Equals(list.Last()))
                {
                    last = true; break; //j reached the last element.
                }
                node = list[j++];
            }

            ListNode subNodes;
            int iIndex = Math.Max(i - 1, 0);
            if (last)
            {
                subNodes = ASTManager.SubNotes(listNode, iIndex, list.Count - iIndex);
            }
            else
            {
                List<SyntaxNodeOrToken> snort = new List<SyntaxNodeOrToken>();
                for (int k = iIndex; !list[k].Equals(node); k++)
                {
                    snort.Add(list[k]);
                }
                subNodes = new ListNode(snort);
            }

            //subNodes = ASTManager.SubNotes(listNode, Math.Max(i - 1, 0), ((j) - i));
            subNodes.OriginalText = re.Text;
            Tuple<ListNode, ListNode> t = Tuple.Create(listNode, subNodes);

            return t;
        }

        /// <summary>
        ///   Syntax nodes to be used on filtering
        /// </summary>
        /// <param name="name">Identifier name</param>
        /// <returns>Syntax nodes to be used on filtering</returns>
        internal IEnumerable<SyntaxNode> SyntaxNodesWithSemanticModel(DymToken name, List<SyntaxNode> Lcas)
        {
            //return null;
            if (name == null) return null;
     
            SelectionInfo info = new SelectionInfo(name.dynType.fullName, new List<TRegion>(Ctl.SelectedLocations));
            IEnumerable<SyntaxNode> output;
            if (!_dicReferences.TryGetValue(info, out output))
            {
                Console.WriteLine("Looking for references!! " + name);

                Dictionary<string, Dictionary<string, List<TextSpan>>> result = GetReferences(name);

                Dictionary<string, Dictionary<string, List<TextSpan>>> referencedSymbols =
                    ReferenceManager.GroupReferencesBySelection(result, Ctl.SelectedLocations);
                List<SyntaxNode> nodesList = new List<SyntaxNode>();
                Dictionary<string, List<TextSpan>> dictionary;

                if (referencedSymbols.Count == 1)
                {
                    dictionary = referencedSymbols.First().Value;
                }
                else
                {
                    dictionary = new Dictionary<string, List<TextSpan>>();
                    foreach (KeyValuePair<string, Dictionary<string, List<TextSpan>>> symbol in result)
                    {
                        foreach (KeyValuePair<string, List<TextSpan>> dic in symbol.Value)
                        {
                            if (!dictionary.ContainsKey(dic.Key))
                            {
                                dictionary.Add(dic.Key, dic.Value);
                            }
                            dictionary[dic.Key].AddRange(dic.Value);
                        }
                    }
                }
                //for each file
                foreach (var fileSpans in dictionary)
                {
                    string strTree = FileUtil.ReadFile(fileSpans.Key);
                    SyntaxTree fileTree = CSharpSyntaxTree.ParseText(strTree, path: fileSpans.Key);

                    var spans = fileSpans;
                    var nodes = from node in fileTree.GetRoot().DescendantNodesAndSelf()
                                where WithinLcas(node, Lcas) && WithinSpans(node, spans.Value)
                                select node;
                    nodesList.AddRange(nodes);
                }
                
                if (!result.Any() || !nodesList.Any())
                {
                    _dicReferences.Add(info, null);
                }
                else
                {
                    _dicReferences.Add(info, nodesList);
                }
            }
            return _dicReferences[info];
        }

        /// <summary>
        /// Generates references on the format: key referenced type, and values dictionary of referencee file and list of referecees.
        /// </summary>
        /// <param name="dymToken">Name to perform look up</param>
        /// <returns>Generates references on the format: key referenced type, and values dictionary of referencee file and list of referecees.</returns>
        internal static Dictionary<string, Dictionary<string, List<TextSpan>>> GetReferences(DymToken dymToken)
        {
            try
            {
                WorkspaceManager wsManager = WorkspaceManager.GetInstance();
                ProjectInformation pjInfo = Ctl.ProjectInformation;
                EditorController controller = EditorController.GetInstance();
           
                Dictionary<string, List<TRegion>> files = RegionManager.GetInstance().GroupRegionBySourceFile(controller.SelectedLocations);
                Dictionary<string, Dictionary<string, List<TextSpan>>> result = new Dictionary<string, Dictionary<string, List<TextSpan>>>();
                //if (files.Count == 1)
                //{
                //    result = wsManager.GetLocalReferences(pjInfo.ProjectPath.First(), pjInfo.SolutionPath, dymToken);
                //}

                //if (!result.Any())
                //{
                //    result = wsManager.GetDeclaredReferences(pjInfo.ProjectPath, pjInfo.SolutionPath, dymToken);
                //}

                result = wsManager.GetLocalReferences(pjInfo.ProjectPath.First(), pjInfo.SolutionPath, dymToken);
                return result;
            }
            catch (AggregateException)
            {
                Console.WriteLine("Could not find references for: " + dymToken);
            }

            return new Dictionary<string, Dictionary<string, List<TextSpan>>>();
        }

        /// <summary>
        /// Syntax nodes without semantical model
        /// </summary>
        /// <param name="tree">Syntax node root</param>
        /// <returns>Syntax nodes without semantical model</returns>
        internal static IEnumerable<SyntaxNode> SyntaxNodesWithoutSemanticModel(SyntaxNode tree, List<SyntaxNode> Lcas)
        {
            if (tree == null) throw new ArgumentNullException("tree");

            var nodes = from node in tree.DescendantNodesAndSelf()
                        where WithinLcas(node, Lcas)
                        select node;
            return nodes;
        }

        /// <summary>
        /// Syntax nodes without semantical model
        /// </summary>
        /// <param name="files">List of source files on the format (sourceCode, sourcePath)</param>
        /// <returns>Syntax nodes without semantical model</returns>
        internal static IEnumerable<SyntaxNode> SyntaxNodesWithoutSemanticModel(List<Tuple<string, string>> files, List<SyntaxNode> Lcas)
        {
            if (files == null) throw new ArgumentNullException("files");
            if (!files.Any()) throw new ArgumentException("Source files cannot be empty.", "files");

            List<SyntaxNode> listNodes = new List<SyntaxNode>();
            foreach (Tuple<string, string> fileTuple in files)
            {
                string strTree = FileUtil.ReadFile(fileTuple.Item2);
                SyntaxTree tree = CSharpSyntaxTree.ParseText(strTree, path: fileTuple.Item2);

                var nodes = from node in tree.GetRoot().DescendantNodesAndSelf()
                            where WithinLcas(node, Lcas)
                            select node;
                listNodes.AddRange(nodes);
            }
            return listNodes;
        }

        /// <summary>
        /// True if node intersects one of the spans
        /// </summary>
        /// <param name="node">Node to be analyzed</param>
        /// <param name="spans">List of spans</param>
        /// <returns>True is node intersects one of the spans</returns>
        internal static bool WithinSpans(SyntaxNode node, List<TextSpan> spans)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (spans == null) throw new ArgumentNullException("spans");
            if (!spans.Any()) throw new ArgumentException("Spans cannot by empty", "spans");

            foreach (var entry in spans)
            {
                if (node.Span.IntersectsWith(entry)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// True if node is of the same type of one of the lca of the region selection by the user
        /// </summary>
        /// <param name="node">Syntax node to be analyzed</param>
        /// <returns>True if node is of the same type of one of the lca of the region selection by the user</returns>
        internal static bool WithinLcas(SyntaxNode node, List<SyntaxNode> Lcas)
        {
            if (node == null) throw new ArgumentNullException("node");

            foreach (var lca in Lcas)
            {
                if (node.IsKind(lca.Kind()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}




