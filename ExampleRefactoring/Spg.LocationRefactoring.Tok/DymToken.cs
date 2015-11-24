using Spg.ExampleRefactoring.AST;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Tok;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spg.ExampleRefactoring.Projects;
using Spg.ExampleRefactoring.Workspace;
using System.Windows.Forms;
using Microsoft.CodeAnalysis.CSharp;

namespace Spg.LocationRefactoring.Tok
{
    /// <summary>
    /// Represent dynamic token
    /// </summary>
    public class DymToken : Token
    {
        private bool _getFullyQualifiedName;

        DynType dynType { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">Dynamic token</param>
        /// <param name="getFullyQualifiedName">Indicate if fully qualified name have to be used</param>
        public DymToken(SyntaxNodeOrToken token, bool getFullyQualifiedName) : base(token)
        {
            _getFullyQualifiedName = getFullyQualifiedName;

            ProjectInformation information = ProjectInformation.GetInstance();

            string fullName = null;
            if (_getFullyQualifiedName)
            {
                if (token.IsKind(SyntaxKind.IdentifierToken))
                {
                    fullName = WorkspaceManager.GetInstance()
                        .GetFullyQualifiedName(information.SolutionPath,
                            this.token.Parent);
                    dynType = new DynType(fullName, DynType.FULLNAME);
                }
                else if(token.IsKind(SyntaxKind.StringLiteralToken))
                {
                    dynType = new DynType(token.ToFullString(), DynType.STRING);
                }
                else
                {
                    dynType = new DynType(token.ToFullString(), DynType.NUMBER);
                }
            }
            Console.WriteLine(fullName);
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

            DymToken st = (DymToken)obj;
            ComparerBase comparator = new NodeComparer();
            return comparator.IsEqual(this.token, st.token) && ASTManager.Parent(this.token).RawKind == ASTManager.Parent(st.token).RawKind;
        }
    }
}

