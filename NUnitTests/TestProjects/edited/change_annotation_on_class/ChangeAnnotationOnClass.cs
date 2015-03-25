using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleProject.Company.ChangeAnnotationOnClass
{
    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    public class ChangeAnnotationOnClass
    {
    }

    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    public class A
    {

    }

    [DiagnosticAnalyzer]
    public class B
    {

    }

    [DiagnosticAnalyzer]
    public class C
    {

    }

    [DiagnosticAnalyzer]
    public class D
    {

    }

    [DiagnosticAnalyzer]
    public class E
    {

    }
}
