//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Spg.ExampleRefactoring.AST;
//using Spg.LocationRefactor.Learn;
//using System.Collections.Generic;
//using LocationCodeRefactoring.Br.Spg.Location;
//using Spg.LocationRefactor.TextRegion;

//namespace Spg.LocationRefactor.Operator
//{
//    public class MethodFilter: FilterBase
//    {

//        public MethodFilter(List<TRegion> list) : base(list)
//        {

//        }
//        public override string ToString()
//        {
//            return "MethodFilter(\n\t" + predicate.ToString() + ")";
//        }

//        /// <summary>
//        /// Method filter learner
//        /// </summary>
//        /// <returns>Method filter learner</returns>
//        protected override FilterLearnerBase GetFilterLearner(List<TRegion> list)
//        {
//            return new MethodFilterLearner(list);
//        }

//        /// <summary>
//        /// Syntax nodes
//        /// </summary>
//        /// <param name="sourceCode">Source code</param>
//        /// <returns>Syntax nodes</returns>
//        protected override IEnumerable<SyntaxNode> SyntaxNodes(string sourceCode)
//        {
//            //return Strategy.SyntaxElements(sourceCode, SyntaxKind.MethodDeclaration);
//            return Strategy.SyntaxElements(sourceCode, list);
//        }
//    }
//}
