using System;
using System.Collections.Generic;
using Spg.ExampleRefactoring.Workspace;
using Spg.ExampleRefactoring.Data;
using Spg.ExampleRefactoring.Synthesis;

namespace ExampleRefactoring
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //WorkspaceManager manager = new WorkspaceManager();
            //manager.GetProjects(@"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject.sln");

            
            ASTProgram program = new ASTProgram();

            Console.WriteLine("CHOOSE A NUMBER BETWEEM 1 AND 10\n");

            String inputexamples = Console.ReadLine();
            System.IO.StreamWriter file = new System.IO.StreamWriter("hypothesis.txt");
            ExampleCommand command = null;
            while (!inputexamples.Equals("&"))
            {
                command = ExampleManager.GetCommand(inputexamples); 
                while (command == null)
                {
                    Console.WriteLine("WRITE OPTION");
                    inputexamples = Console.ReadLine();
                    command = ExampleManager.GetCommand(inputexamples);
                }

                List<Tuple<String, String>> examples = command.Train();

                List<Tuple<ListNode, ListNode>> data = ASTProgram.Examples(examples);

                List<SynthesizedProgram> hypothesis = program.GenerateStringProgram(data);

                Tuple<String, String> test = command.Test();

                while (!test.Equals("#"))
                {
                    String result = ASTProgram.TransformString(test.Item1, hypothesis[0]).transformation;
                    Console.WriteLine(result);
                    test = command.Test();
                    Console.ReadLine();
                }

                Console.WriteLine("CHOOSE A INPUT EXAMPLE DATASET OR & TO LEAVE: \n OPTIONS:\n (1) - EXTRACT VALUE \n (2) - FORMAT NAME \n (3) - CHANGE API \n");

                inputexamples = Console.ReadLine();
            }
            file.Close();
        }
    }
}
