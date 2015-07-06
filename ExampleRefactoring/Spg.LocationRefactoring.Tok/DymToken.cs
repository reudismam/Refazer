using Spg.ExampleRefactoring.AST;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Tok;

namespace Spg.LocationRefactoring.Tok
{
    /// <summary>
    /// Represent dynamic token
    /// </summary>
    public class DymToken: Token
    {
        private bool _getFullyQualifiedName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">Dynamic token</param>
        /// <param name="getFullyQualifiedName">Indicate if fully qualified name have to be used</param>
        public DymToken(SyntaxNodeOrToken token, bool getFullyQualifiedName) : base(token)
        {
            this._getFullyQualifiedName = getFullyQualifiedName;
        }

        /// <summary>
        /// Match
        /// </summary>
        /// <param name="st">syntax node or token</param>
        /// <returns>True if a match exists</returns>
        public override bool Match(SyntaxNodeOrToken st)
        {
            DymToken dymToken = new DymToken(st, true);
            return Equals(dymToken);
        }

        /// <summary>
        /// Return a node comparer
        /// </summary>
        /// <returns>Node comparer</returns>
        public override ComparerBase Comparer()
        {
            return new NodeComparer();
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return "DymTok(" + token + ")";
        }

        /// <summary>
        /// Hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            //return this.ToString().GetHashCode();
            return base.GetHashCode();
        }

        /// <summary>
        /// Two DymToken are equal
        /// </summary>
        /// <param name="obj">Dynamic token</param>
        /// <returns>True if dynamic tokens are equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is DymToken))
            {
                return false;
            }
            //var x = token.SyntaxTree.GetRoot().FindNode();
            ////var x = model.GetDeclaredSymbol(token.Parent);
            //var property = token.Parent as IdentifierNameSyntax;

            //if (property != null)
            //    Console.WriteLine("Property: " + property.Identifier);
            //property.Identifier.
            //ProjectInformation information = ProjectInformation.GetInstance();

            //if (_getFullyQualifiedName)
            //{
            //    string fullName = WorkspaceManager.GetInstance()
            //        .GetSemanticModel(information.ProjectPath, information.SolutionPath,
            //            this.token.Parent, this.token.ToString());
            //}

            DymToken st = (DymToken)obj;
            ComparerBase comparator = new NodeComparer();
            return comparator.IsEqual(this.token, st.token) && ASTManager.Parent(this.token).RawKind == ASTManager.Parent(st.token).RawKind;
        }
    }
}

