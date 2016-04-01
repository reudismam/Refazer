using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using TreeEdit.Spg.TreeEdit.Mapping;
using TreeEdit.Spg.TreeEdit.Script;
using TreeEdit.Spg.TreeEdit.Update;

namespace TreeEdit
{
    public class TreeEditMain
    {

        public void main()
        {
            //SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(
            //@"using System;

            //public class Test
            //{
            //    static bool ma()
            //    {
            //         bool a = false;
            //         if (a){
            //            return a;
            //         }
            //        return false;
            //    }
            //}").GetRoot();

            //SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(
            //@"using System;

            //public class Test
            //{
            //    static bool ma()
            //    {
            //         bool a = false;
            //         if (a){
            //            return a;
            //         }
            //        return false;
            //    }
            //}").GetRoot();

            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(i == 0) return ""Foo!"";
                }
            }").GetRoot();

            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {

                public String foo(int i)
                {
                     if(i == 0) return ""Bar"";
                     else if(i == -1) return ""Foo!"";
                }
            }").GetRoot();

            ITreeMapping mapping = new GumTreeMapping();
            var M = mapping.Mapping(inpTree, outTree);

            var generator = new EditScriptGenerator();
            var script = generator.EditScript(inpTree, outTree, M);

            TreeUpdate update = new TreeUpdate();
            update.UpdateTree(script, inpTree, M);
            Console.WriteLine();
        }
    }
}
