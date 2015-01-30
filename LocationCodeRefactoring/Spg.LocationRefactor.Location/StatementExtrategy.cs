//using System;
//using System.Collections.Generic;
//using Microsoft.CodeAnalysis;
//using Spg.LocationRefactor.TextRegion;

//namespace LocationCodeRefactoring.Spg.LocationRefactor.Location
//{
//    /// <summary>
//    /// Statement Strategy
//    /// </summary>
//    [Obsolete]
//    public class StatementStrategy : RegionManager
//    {
//        private static StatementStrategy _instance;

//        //private readonly Dictionary<string, List<SyntaxNode>> _computed;

//        ///// <summary>
//        ///// Constructor
//        ///// </summary>
//        private StatementStrategy()
//        {
//            //_computed = new Dictionary<string, List<SyntaxNode>>();
//        }

//        /// <summary>
//        /// Singleton instance
//        /// </summary>
//        /// <returns>Statement strategy instance</returns>
//        public static StatementStrategy GetInstance()
//        {
//            if (_instance == null)
//            {
//                _instance = new StatementStrategy();
//            }

//            return _instance;
//        }

//        /////// <summary>
//        /////// Syntax nodes 
//        /////// </summary>
//        /////// <param name="sourceCode">Source code</param>
//        /////// <returns>Syntax nodes</returns>
//        ////public override List<SyntaxNode> SyntaxNodes(string sourceCode, List<TRegion> list)
//        ////{
//        ////    List<SyntaxNode> nodes = null;
//        ////    if (!_computed.TryGetValue(sourceCode, out nodes))
//        ////    {
//        ////        nodes = SyntaxElements(sourceCode, list);
//        ////        _computed.Add(sourceCode, nodes);
//        ////    }

//        ////    return _computed[sourceCode];
//        ////}
//    }
//}
