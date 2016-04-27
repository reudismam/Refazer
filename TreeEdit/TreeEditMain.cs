using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using Spg.TreeEdit.Node;
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
                     if(i == 2){}
                     if(i == 0) return ""Foo!"";
                }
            }").GetRoot();

            //var comparer = new CSharpTreeComparer();
            //var treeEdits = comparer.ComputeEditScript(inpTree.AsNode(), outTree.AsNode());

            var inpNode = ConverterHelper.ConvertCSharpToTreeNode(inpTree);
            var outNode = ConverterHelper.ConvertCSharpToTreeNode(outTree);
            var mapping = new GumTreeMapping<SyntaxNodeOrToken>();
            var M = mapping.Mapping(inpNode, outNode);

            var generator = new EditScriptGenerator<SyntaxNodeOrToken>();
            
            var script = generator.EditScript(inpNode, outNode, M);

            TreeUpdate update = new TreeUpdate();
            update.UpdateTree(script, inpTree/*, M*/);
            Console.WriteLine();
        }
    }
}
