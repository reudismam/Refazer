using Spg.LocationRefactor.Operator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.LocationRefactor.Operator
{
    public interface IPredicateOperator: IOperator
    {
        /// <summary>
        /// Add a predicate to a operator
        /// </summary>
        void AddPredicate();
    }
}
