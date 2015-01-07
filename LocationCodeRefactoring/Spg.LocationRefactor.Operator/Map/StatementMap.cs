using Microsoft.CodeAnalysis.CSharp;

namespace Spg.LocationRefactor.Operator
{
    public class StatementMap : MapBase
    {
        private SyntaxKind syntaxKind;

        public StatementMap(SyntaxKind syntaxKind)
        {
            this.syntaxKind = syntaxKind;
        }
        public override string ToString()
        {
            return "StatementMap(λ" + syntaxKind.ToString() + ": Pair(Pos(" + syntaxKind.ToString().Substring(0, 1) + ", p1), Pos(" + syntaxKind.ToString().Substring(0, 1) + ", p2)), " + syntaxKind.ToString().Substring(0, 1) + "S) "
                + "\n\tp1 = " + ((Pair)scalarExpression.ioperator).expression.p1.ToString()
                + "\n\tp2 = " + ((Pair)scalarExpression.ioperator).expression.p2.ToString()
                + "\n\t" + syntaxKind.ToString().Substring(0, 1) + "S=" + sequenceExpression.ToString();
        }
    }
}
