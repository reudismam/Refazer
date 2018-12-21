using Antlr4.Runtime.Tree;

namespace TreeElement.Spg.Node
{
    public class ANTLRNode : RefazerNode
    {
        public IParseTree Value { get; set; }

        public ANTLRNode(IParseTree value)
        {
            Value = value;
        }

        /*public void method()
        {
            string input = "int main(){}";
            StringBuilder text = new StringBuilder(input);

            //// to type the EOF character and end the input: use CTRL+D, then press <enter>
            //while (!(input = Console.ReadLine()).Equals("."))
            //{
            //    text.AppendLine(input);
            //}

            AntlrInputStream inputStream = new AntlrInputStream(text.ToString());
            CLexer speakLexer = new CLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
            CParser speakParser = new CParser(commonTokenStream);

            var compilationUnit = speakParser.compilationUnit();
            var children = compilationUnit.children;
            var child = children.First();
            var start = child.SourceInterval.a;
            var ini = commonTokenStream.Get(start).StartIndex;
            var end = child.SourceInterval.b;
            var fin = commonTokenStream.Get(end).StartIndex;
            Console.WriteLine(start + " " + end);
        }*/
    }
}
