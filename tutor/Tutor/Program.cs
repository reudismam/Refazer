using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython;
using IronPython.Compiler;
using IronPython.Compiler.Ast;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using Microsoft.Scripting.Runtime;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Tutor
{
    class Program
    {
        static void Main(string[] args)
        {
            var py = Python.CreateEngine();

            var ast1 = ParseFile(@"b.py", py);
            var ast2 = ParseFile(@"a.py", py);
            var zss = new PythonZss(ast1, ast2);
            /*SyntaxNodeOrToken ast1 = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(i == 0) return ""Foo!"";
                }
            }").GetRoot();

            SyntaxNodeOrToken ast2 = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(i == 0) return ""Bar"";
                     else if(i == -1) return ""Foo!"";
                }
            }").GetRoot();
            var zss = new CSharpZss(ast1, ast2);*/

            var result = zss.Compute();
            Console.Out.WriteLine(result.Item1);
            var edits = result.Item2;
            foreach (var edit in edits)
            {
                Console.Out.WriteLine(edit);
            }
            Console.ReadKey();
        }

        static PythonAst ParseFile(string path, ScriptEngine py)
        {
            var src = HostingHelpers.GetSourceUnit(py.CreateScriptSourceFromFile(path));
            var pylc = HostingHelpers.GetLanguageContext(py);
            var parser = Parser.CreateParser(new CompilerContext(src, pylc.GetCompilerOptions(), ErrorSink.Default),
                (PythonOptions)pylc.Options);
            return parser.ParseFile(true);
        }


    }
}
