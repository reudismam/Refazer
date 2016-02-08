using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleProject.Company.ExampleProject.ReturnToGet
{
    public class ReturnToGet
    {
        public sealed override ImmutableArray<string> GetFixableDiagnosticIds1()
        {
           return ImmutableArray<string>.Create(StaticTypeRulesDiagnosticAnalyzer.CA1052RuleId);
        }

        public sealed override ImmutableArray<string> GetFixableDiagnosticIds2()
        {
            return ImmutableArray<string>.Create(CA1001DiagnosticAnalyzer.RuleId);
        }

        public sealed override ImmutableArray<string> GetFixableDiagnosticIds3()
        {
            return ImmutableArray<string>.Create(CA1008DiagnosticAnalyzer.RuleId);
       }
    }
}
