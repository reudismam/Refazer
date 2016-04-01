using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using TreeEdit.Spg.TreeEdit.Script;

namespace TreeEdit.Spg.TreeEdit.Update
{
    public class TreeUpdate
    {
        /// <summary>
        /// Map to operations in each syntax tree
        /// </summary>
        private Dictionary<SyntaxNodeOrToken, List<EditOperation>> _dict;

        /// <summary>
        /// Map to annotation to each edit operations
        /// </summary>
        private Dictionary<EditOperation, SyntaxAnnotation> _ann;

        /// <summary>
        /// Newest updated tree
        /// </summary>
        private SyntaxNodeOrToken _currentTree;

        /// <summary>
        /// Map element from T1 (before tree) to T2 (after tree) nodes
        /// </summary>
        private Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> _M;

        /// <summary>
        /// Map each annoted node
        /// </summary>
        private Dictionary<SyntaxNode, List<SyntaxAnnotation>> _annts;

        /// <summary>
        /// Indicate the edit operations that was processed.
        /// </summary>
        private Dictionary<EditOperation, bool> _processed;

        /// <summary>
        /// Update the tree following the edit script
        /// </summary>
        /// <param name="script">Edit script</param>
        /// <param name="tree">Tree to be updated</param>
        /// <param name="M">Mapping from each element for the before and after tree.</param>
        public void UpdateTree(List<EditOperation> script, SyntaxNodeOrToken tree, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            //Update attributes
            _currentTree = tree;
            _M = M;
            _dict = new Dictionary<SyntaxNodeOrToken, List<EditOperation>>();
            _ann = new Dictionary<EditOperation, SyntaxAnnotation>();
            _annts = new Dictionary<SyntaxNode, List<SyntaxAnnotation>>();
            _processed = new Dictionary<EditOperation, bool>();
            //Update atrributes

            CreateEditionDictionary(script);

            foreach (var item in _annts)
            {
                var annVisitor = new AddAnnotationRewriter(item.Key, item.Value);
                _currentTree = annVisitor.Visit(_currentTree.AsNode());
            }

            foreach (var item in _annts)
            {
                foreach (var s in script)
                {
                    if (s is Move)
                    {
                        Move mv = s as Move;
                        if (item.Key.Span.Start == mv.T1Node.Span.Start && item.Key.Span.Length == mv.T1Node.Span.Length)
                        {
                            mv.T1Node = _currentTree.AsNode().FindNode(mv.T1Node.Span);
                        }
                    }

                    if (s is Script.Update)
                    {
                        Script.Update up = s as Script.Update;
                        if (item.Key.Span.Start == up.T1Node.Span.Start && item.Key.Span.Length == up.T1Node.Span.Length)
                        {
                            up.T1Node = _currentTree.AsNode().FindNode(up.T1Node.Span);
                        }
                    }
                }
            }

            foreach (var item in script)
            {
                if (!_processed.ContainsKey(item))
                {
                    ProcessInsertOperation(item);

                    foreach (var a in _ann.Values)
                    {
                        var moveL = _currentTree.AsNode().GetAnnotatedNodes(a).ToList();
                        if (moveL.Any())
                        {
                            var move = moveL.First();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Process insert operations
        /// </summary>
        /// <param name="eop"></param>
        private void ProcessInsertOperation(EditOperation eop)
        {
            if (eop is Insert)
            {
                var x = UpdateTree(eop);

                //Use the annotation on our original node to find the new class declaration
                var changedNodeList = _currentTree.AsNode().GetAnnotatedNodes(_ann[eop]).ToList();

                var oldNode = changedNodeList.First();
                if (eop is Insert)
                {
                    Insert ins = eop as Insert;
                    var newNode = Update(oldNode, x.AsNode(), ins.K);
                    var visitor = new UpdateTreeRewriter(oldNode, newNode);
                    _currentTree = visitor.Visit(_currentTree.AsNode());
                    string text = _currentTree.AsNode().NormalizeWhitespace().GetText().ToString();
                }
            }
        }

        private void CreateEditionDictionary(List<EditOperation> script)
        {
            int id = 0;
            foreach (var s in script)
            {
                AnnotateMoveOperation(s, id);
                AnnotateInsertOperation(s, id);
                AnnotateUpdateOperation(s, id);

                id++;
                if (!_dict.ContainsKey(s.Parent))
                {
                    _dict[s.Parent] = new List<EditOperation>();
                }

                _dict[s.Parent].Add(s);
            }
        }

        /// <summary>
        /// Annotate move operations
        /// </summary>
        /// <param name="eop">Edition operation</param>
        /// <param name="id">Unique Id</param>
        private void AnnotateMoveOperation(EditOperation eop, int id)
        {
            if (eop is Move)
            {
                Move mv = eop as Move;
                SyntaxAnnotation sn = new SyntaxAnnotation("MV-" + id);
                _ann.Add(eop, sn);

                if (!_annts.ContainsKey(mv.T1Node.AsNode()))
                {
                    _annts[mv.T1Node.AsNode()] = new List<SyntaxAnnotation>();
                }
                _annts[mv.T1Node.AsNode()].Add(sn);
            }
        }

        /// <summary>
        /// Annotate update operations
        /// </summary>
        /// <param name="s">Edit operation</param>
        /// <param name="id">Unique id</param>
        private void AnnotateUpdateOperation(EditOperation s, int id)
        {
            if (s is Script.Update)
            {
                Script.Update update = s as Script.Update;
                SyntaxAnnotation sn = new SyntaxAnnotation("UP-" + id);
                _ann.Add(s, sn);

                if (!_annts.ContainsKey(update.T1Node.AsNode()))
                {
                    _annts[update.T1Node.AsNode()] = new List<SyntaxAnnotation>();
                }
                _annts[update.T1Node.AsNode()].Add(sn);
            }
        }

        /// <summary>
        /// Annotate insert operation
        /// </summary>
        /// <param name="eop">edit operation</param>
        /// <param name="id">Unique id</param>
        private void AnnotateInsertOperation(EditOperation eop, int id)
        {
            if (eop is Insert && !(eop is Move))
            {
                var insert = eop as Insert;
                var y = insert.T1Node.AsNode().Parent;
                var z = _M.ToList().Find(o => o.Value.Equals(y)).Key;

                SyntaxAnnotation upAnn = new SyntaxAnnotation("IS-" + id);
                _ann[eop] = upAnn;

                if (!_annts.ContainsKey(z.AsNode()))
                {
                    _annts.Add(z.AsNode(), new List<SyntaxAnnotation>());
                }
                _annts[z.AsNode()].Add(upAnn);
            }
        }

        private SyntaxNode Update(SyntaxNode node, SyntaxNode child, int k)
        {
            if (node is BlockSyntax)
            {
                BlockSyntax b = node as BlockSyntax;
                StatementSyntax st = child as StatementSyntax;
                b = b.AddStatements(st);
                node = b;
            }
            else
            {
                if (node.ChildNodes().Count() > 1)
                {
                    node = node.ReplaceNode(node.ChildNodes().ElementAt(k - 1), child);
                }
            }

            return node;
        }

        public SyntaxNodeOrToken UpdateTree(EditOperation operation)
        {
            _processed.Add(operation, true);

            if (operation is Insert)
            {
                Insert ins = operation as Insert;

                if (!_dict.ContainsKey(ins.T1Node))
                {
                    return ins.T1Node;
                }

                SyntaxNodeOrToken snot = ins.T1Node;
                SyntaxNode node = snot.AsNode();

                SyntaxNode newNode = node;
                foreach (EditOperation eop in _dict[ins.T1Node])
                {
                    if (eop is Insert)
                    {
                        Insert ies = eop as Insert;
                        var uptNode = UpdateTree(eop).AsNode();
                        var oldNode = newNode.ChildNodes().ElementAt(ies.K - 1);
                        if (!(eop is Move))
                        {          
                            newNode = newNode.ReplaceNode(oldNode, uptNode);
                        }else if (eop is Move)
                        {
                            var annotation = _ann[eop];
                            var moveL = _currentTree.AsNode().GetAnnotatedNodes(annotation).ToList();
                            var uptNodeMove = moveL.First();
                            newNode = newNode.ReplaceNode(oldNode, uptNodeMove);

                            string Treetext = _currentTree.ToString();

                            try
                            {
                                var visitor = new UpdateTreeRewriter(uptNode, null);
                                _currentTree = visitor.Visit(_currentTree.AsNode());
                            }
                            catch (Exception e)
                            {
                            }
                            string text = _currentTree.AsNode().NormalizeWhitespace().GetText().ToString();
                        }
                    }
                    else if (eop is Script.Update)
                    {
                        var uptNode = UpdateTree(eop).AsNode();
                        var oldNode = newNode.ChildNodes().First();
                        newNode = newNode.ReplaceNode(oldNode, uptNode);
                        var annotation = _ann[eop];
                        var moveL = _currentTree.AsNode().GetAnnotatedNodes(annotation).ToList();
                        var move = moveL.First();

                        var visitor = new UpdateTreeRewriter(move, uptNode);
                        _currentTree = visitor.Visit(_currentTree.AsNode());

                        TextSpan span = new TextSpan(move.Span.Start, uptNode.Span.Length);  
                        var update = _currentTree.AsNode().FindNode(span);

                        foreach (var item in _annts)
                        {
                            if (item.Value.Contains(annotation))
                            {
                                var annVisitor = new AddAnnotationRewriter(update, item.Value);
                                _currentTree = annVisitor.Visit(_currentTree.AsNode());
                            }
                        }

                        string text = _currentTree.AsNode().NormalizeWhitespace().GetText().ToString();
                    }
                }

                return newNode;
            }

            if (operation is Script.Update)
            {
                Script.Update update = operation as Script.Update;
                return update.To;
            }

            return null;
        }
    }
}