using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spg.LocationRefactor.Operator;
using Microsoft.CodeAnalysis.CSharp;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Learn
{
    public class StatementFilterLearner : FilterLearnerBase
    {

        private SyntaxKind syntaxKind;

        public StatementFilterLearner(SyntaxKind syntaxKind, List<TRegion> list) :base(list)
        {
            this.syntaxKind = syntaxKind;
        }
        protected override FilterBase GetFilter(List<TRegion> list)
        {
            return new StatementFilter(syntaxKind, list);
        }
    }
}
