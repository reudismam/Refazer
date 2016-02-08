using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExampleProject.Company.ExampleProject.AddIOParam
{
    class BaseIOParam
    {
        private Microsoft.CodeAnalysis.CSharp.CSharpCommandLineParser cSharpCommandLineParser;
        private string responseFile;
        private string[] args;
        private string baseDirectory;
        private string p;
        private string p1;
        private string p2;

        public BaseIOParam(Microsoft.CodeAnalysis.CSharp.CSharpCommandLineParser cSharpCommandLineParser, string responseFile, string[] args, string baseDirectory, string p)
        {
            // TODO: Complete member initialization
            this.cSharpCommandLineParser = cSharpCommandLineParser;
            this.responseFile = responseFile;
            this.args = args;
            this.baseDirectory = baseDirectory;
            this.p = p;
        }

        public BaseIOParam(Microsoft.CodeAnalysis.CSharp.CSharpCommandLineParser cSharpCommandLineParser, string responseFile, string[] args, string baseDirectory, string p1, string p2)
        {
            // TODO: Complete member initialization
            this.cSharpCommandLineParser = cSharpCommandLineParser;
            this.responseFile = responseFile;
            this.args = args;
            this.baseDirectory = baseDirectory;
            this.p1 = p1;
            this.p2 = p2;
        }

        protected static void New(Microsoft.CodeAnalysis.CSharp.CSharpCommandLineParser cSharpCommandLineParser, string responseFile, string[] args, string baseDirectory, object Nothing)
        {
            throw new NotImplementedException();
        }
    }
}
