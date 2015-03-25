using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//commit: 49cdaceb2828acc1f50223826d478a00a80a59e2
namespace ExampleProject.Company.ExampleProject.AddIOParam
{
    class AddIOParam : BaseIOParam
    {
        public AddIOParam(string responseFile, string baseDirectory, string[] args) : base(CSharpCommandLineParser.Default, responseFile, args, baseDirectory, Environment.GetEnvironmentVariable("LIB"), System.IO.Path.GetTempPath())
        {
        }

        public AddIOParam(string responseFIle, string baseDirectory, string[] args, object other) : base(CSharpCommandLineParser.Interactive, responseFIle, args, baseDirectory, null, System.IO.Path.GetTempPath())
        {
        }

        public object Nothing
        {
            get;
            set;
        }
    }
}