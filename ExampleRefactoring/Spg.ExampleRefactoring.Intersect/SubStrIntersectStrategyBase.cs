using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spg.ExampleRefactoring.Expression;

namespace Spg.ExampleRefactoring.Intersect
{
    internal class SubStrIntersectStrategyBase: IIntersectStrategy
    {
        public List<IExpression> GetExpressions(List<IExpression> expressions1, List<IExpression> expressions2)
        {
            throw new NotImplementedException();
        }
    }
}


