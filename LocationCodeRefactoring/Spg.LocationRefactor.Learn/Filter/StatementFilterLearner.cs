using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spg.LocationRefactor.Operator;
using Microsoft.CodeAnalysis.CSharp;

namespace Spg.LocationRefactor.Learn
{
    public class StatementFilterLearner : FilterLearnerBase
    {

        private SyntaxKind syntaxKind;

        public StatementFilterLearner(SyntaxKind syntaxKind)
        {
            this.syntaxKind = syntaxKind;
        }
        protected override FilterBase GetFilter()
        {
            return new StatementFilter(syntaxKind);
        }
    }
}
