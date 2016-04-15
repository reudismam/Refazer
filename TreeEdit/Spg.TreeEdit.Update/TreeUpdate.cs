﻿using System;
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
        public Dictionary<EditOperation, SyntaxAnnotation> Ann { get; set; }

        /// <summary>
        /// Newest updated tree
        /// </summary>
        public SyntaxNodeOrToken CurrentTree { get; set; }

        /// <summary>
        /// Map element from T1 (before tree) to T2 (after tree) nodes
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> _M;

        /// <summary>
        /// Map each annoted node
        /// </summary>
        private Dictionary<SyntaxNode, List<SyntaxAnnotation>> _annts;

        /// <summary>
        /// Indicate the edit operations that was processed.
        /// </summary>
        public Dictionary<EditOperation, bool> Processed;


        /// <summary>
        /// Update the tree following the edit script
        /// </summary>
        /// <param name="script">Edit script</param>
        /// <param name="tree">Tree to be updated</param>
        /// <param name="M">Mapping from each element for the before and after tree.</param>
        // ReSharper disable once InconsistentNaming
        public void UpdateTree(List<EditOperation> script, SyntaxNodeOrToken tree, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            PreProcessTree(script, tree, M);

            foreach (var item in script)
            {
                if (!Processed.ContainsKey(item))
                {
                    ProcessEditOperation(item);
                }
            }
        }

        public void PreProcessTree(List<EditOperation> script, SyntaxNodeOrToken tree, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            InitializeAtributes(tree, M);
            CreateEditionDictionary(script);
            Annotate(script); //We also need to update the dict.
        }

        /// <summary>
        /// Annotate the tree
        /// </summary>
        /// <param name="script">Edition script</param>
        private void Annotate(List<EditOperation> script)
        {
            AnnotateTree();
            AnnotateEditOperations(script);
        }

        /// <summary>
        /// Annotate current tree
        /// </summary>
        private void AnnotateTree()
        {
            foreach (var item in _annts)
            {
                var annVisitor = new AddAnnotationRewriter(item.Key, item.Value);
                CurrentTree = annVisitor.Visit(CurrentTree.AsNode());
            }
        }

        /// <summary>
        /// Annotate edit operations
        /// </summary>
        /// <param name="script">Edit script</param>
        private void AnnotateEditOperations(List<EditOperation> script)
        {
            foreach (var item in _annts)
            {
                foreach (var s in script)
                {
                    if (s is Move || s is Script.Update)
                    {
                        if (item.Key.Span.Start == s.T1Node.Span.Start && item.Key.Span.Length == s.T1Node.Span.Length)
                        {
                            s.T1Node = CurrentTree.AsNode().FindNode(s.T1Node.Span);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Intialize global attributes
        /// </summary>
        /// <param name="tree">Source tree</param>
        /// <param name="M">Mapping</param>
        // ReSharper disable once InconsistentNaming
        private void InitializeAtributes(SyntaxNodeOrToken tree, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            CurrentTree = tree;
            _M = M;
            _dict = new Dictionary<SyntaxNodeOrToken, List<EditOperation>>();
            Ann = new Dictionary<EditOperation, SyntaxAnnotation>();
            _annts = new Dictionary<SyntaxNode, List<SyntaxAnnotation>>();
            Processed = new Dictionary<EditOperation, bool>();
        }

        /// <summary>
        /// Process insert operations
        /// </summary>
        /// <param name="eop">Edit operation</param>
        public void ProcessEditOperation(EditOperation eop)
        {
            var updated = UpdateTree(eop);

            var changedNodeList = CurrentTree.AsNode().GetAnnotatedNodes(Ann[eop]).ToList();
            var oldNode = changedNodeList.First();

            var newNode = Update(oldNode, updated.AsNode(), eop.K).AsNode();

            var visitor = new UpdateTreeRewriter(oldNode, newNode);
            CurrentTree = visitor.Visit(CurrentTree.AsNode());
        }

        /// <summary>
        /// Create edition dictionary. The key contains the updated node 
        /// and the value, the list of operations in this node.
        /// </summary>
        /// <param name="script"></param>
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
                SyntaxAnnotation sn = new SyntaxAnnotation("MV-" + id);
                Ann.Add(eop, sn);

                if (!_annts.ContainsKey(eop.T1Node.AsNode()))
                {
                    _annts[eop.T1Node.AsNode()] = new List<SyntaxAnnotation>();
                }
                _annts[eop.T1Node.AsNode()].Add(sn);
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
                SyntaxAnnotation sn = new SyntaxAnnotation("UP-" + id);
                Ann.Add(s, sn);

                if (!_annts.ContainsKey(s.T1Node.AsNode()))
                {
                    _annts[s.T1Node.AsNode()] = new List<SyntaxAnnotation>();
                }
                _annts[s.T1Node.AsNode()].Add(sn);
            }
        }

        /// <summary>
        /// Annotate insert operation
        /// </summary>
        /// <param name="eop">edit operation</param>
        /// <param name="id">Unique id</param>
        private void AnnotateInsertOperation(EditOperation eop, int id)
        {
            if (eop is Insert)
            {
                var y = eop.T1Node.AsNode().Parent;
                var z = _M.ToList().Find(o => o.Value.Equals(y)).Key;

                SyntaxAnnotation upAnn = new SyntaxAnnotation("IS-" + id);
                Ann[eop] = upAnn;

                if (!_annts.ContainsKey(z.AsNode()))
                {
                    _annts.Add(z.AsNode(), new List<SyntaxAnnotation>());
                }
                _annts[z.AsNode()].Add(upAnn);
            }
        }

        /// <summary>
        /// Update a node
        /// </summary>
        /// <param name="parent">Parent node to be updated</param>
        /// <param name="child">Child node to be updated</param>
        /// <param name="k">Position of the child on node</param>
        /// <returns>Updated node</returns>
        private static SyntaxNodeOrToken Update(SyntaxNode parent, SyntaxNode child, int k)
        {
            if (parent is BlockSyntax)
            {
                var statements = parent.ChildNodes().ToList();
                parent = parent.RemoveNodes(parent.ChildNodes(), SyntaxRemoveOptions.KeepNoTrivia);
                BlockSyntax b = (BlockSyntax) parent;
                
                statements.Insert(k - 1, child);

                foreach(var syntaxNode in statements)
                {
                    var stt = (StatementSyntax) syntaxNode;
                    b = b.AddStatements(stt);
                }

                parent = b;
            }else if (parent is IfStatementSyntax && child is ElseClauseSyntax)
            {
                var ifStatement = (IfStatementSyntax) parent;
                var elseClase = (ElseClauseSyntax) child;

                parent = ifStatement.WithElse(elseClase);
            }
            else
            {
                if (parent.ChildNodes().Count() > 1)
                {
                    if (parent.ChildNodes().Count() >= k)
                    {
                        parent = parent.ReplaceNode(parent.ChildNodes().ElementAt(k - 1), child);
                    }
                    else
                    {
                        var schild = parent.ChildNodes().ElementAt(k - 2);
                        schild = parent.FindNode(schild.Span);
                        parent = parent.InsertNodesAfter(schild, new List<SyntaxNode>() { child });
                    }
                }
            }

            SyntaxNodeOrToken returnSot = parent;
            return returnSot;
        }

        public SyntaxNodeOrToken UpdateTree(EditOperation operation)
        {
            Processed.Add(operation, true);

            //process update operation
            if (operation is Script.Update)
            {
                Script.Update update = (Script.Update) operation;
                return update.To;
            }

            //process isolated operations
            if (!_dict.ContainsKey(operation.T1Node))
            {
                return operation.T1Node;
            }

            SyntaxNodeOrToken snot = operation.T1Node;
            SyntaxNode node = snot.AsNode();
            SyntaxNode newNode = node;

            foreach (EditOperation eop in _dict[operation.T1Node])
            {
                var uptNode = UpdateTree(eop).AsNode();
                newNode = ProcessEditOperation(eop, newNode, uptNode);
            }

            return newNode;
        }

        /// <summary>
        /// Process edit operation
        /// </summary>
        /// <param name="eop">Edit operation</param>
        /// <param name="currentNode">Current node</param>
        /// <param name="uptNode">Updated node</param>
        /// <returns>Node after running edit operation</returns>
        private SyntaxNode ProcessEditOperation(EditOperation eop, SyntaxNode currentNode, SyntaxNode uptNode)
        {
            if (eop is Insert)
            {
                currentNode = ProcessInsertOperation(eop, currentNode, uptNode);
            }

            if (eop is Move)
            {
                currentNode = ProcessMoveOperation(eop, currentNode, uptNode);
            }

            if (eop is Script.Update)
            {
                currentNode = ProcessUpdateOperation(eop, currentNode, uptNode);
            }
            return currentNode;
        }

        /// <summary>
        /// Process insert operation
        /// </summary>
        /// <param name="eop">Insert operation</param>
        /// <param name="currentNode">Current node</param>
        /// <param name="uptNode">Update node</param>
        /// <returns>Updated version of current node</returns>
        private static SyntaxNode ProcessInsertOperation(EditOperation eop, SyntaxNode currentNode, SyntaxNode uptNode)
        {
            var oldNode = currentNode.ChildNodes().ElementAt(eop.K - 1);
            currentNode = currentNode.ReplaceNode(oldNode, uptNode);
            return currentNode;
        }

        /// <summary>
        /// Process move operation
        /// </summary>
        /// <param name="eop">Move operation</param>
        /// <param name="currentNode">Current node</param>
        /// <param name="uptNode">New node</param>
        /// <returns>Updated version of current node</returns>
        private SyntaxNode ProcessMoveOperation(EditOperation eop, SyntaxNode currentNode, SyntaxNode uptNode)
        {
            var annotation = Ann[eop];
            var moveL = CurrentTree.AsNode().GetAnnotatedNodes(annotation).ToList();
            var uptNodeMove = moveL.First();
            var oldNode = currentNode.ChildNodes().ElementAt(eop.K - 1);
            currentNode = currentNode.ReplaceNode(oldNode, uptNodeMove);
            //oldNode.Replace
            try
            {
                var visitor = new UpdateTreeRewriter(uptNode, null);
                CurrentTree = visitor.Visit(CurrentTree.AsNode());
            }
            catch (Exception)
            {
                // ignored
            }

            return currentNode;
        }

        /// <summary>
        /// Process update operation
        /// </summary>
        /// <param name="eop">Update operation</param>
        /// <param name="currentNode">Current node</param>
        /// <param name="uptNode">Updated node</param>
        /// <returns>Updated version of the current node</returns>
        private SyntaxNode ProcessUpdateOperation(EditOperation eop, SyntaxNode currentNode, SyntaxNode uptNode)
        {
            var oldNode = currentNode.ChildNodes().First();
            currentNode = currentNode.ReplaceNode(oldNode, uptNode);
            var annotation = Ann[eop];
            var moveL = CurrentTree.AsNode().GetAnnotatedNodes(annotation).ToList();
            var move = moveL.First();

            var visitor = new UpdateTreeRewriter(move, uptNode);
            CurrentTree = visitor.Visit(CurrentTree.AsNode());

            TextSpan span = new TextSpan(move.Span.Start, uptNode.Span.Length);
            var update = CurrentTree.AsNode().FindNode(span);

            foreach (var item in _annts)
            {
                if (item.Value.Contains(annotation))
                {
                    var annVisitor = new AddAnnotationRewriter(update, item.Value);
                    CurrentTree = annVisitor.Visit(CurrentTree.AsNode());
                }
            }

            return currentNode;
        }
    }
}