using System;
using System.Collections.Generic;
using NUnit.Framework;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Synthesis;

namespace Spg.NUnitTests.ASTProgramTests
{
    [TestFixture]
    public class TranformationTests
    {
        [Test]
        public void BoundaryPointsTest()
        {
            string input = @"
            public void Method(){
                if (type == typeof(int))
                {
                    return FormatLiteral((int)obj, useHexadecimalNumbers);
                }
            }
            ";

            string output = @"
            public void Method(){
                if (type == typeof(int))
                {
                    return FormatLiteral((int)obj, options);
                }
            }
            ";

            Tuple<string, string> tuple = Tuple.Create(input, output);
            Tuple<ListNode, ListNode> tln = ASTProgram.Example(tuple);

            List<int> indexes = SynthesisManager.CreateBoundaryPoints(input, output, tln);
            int[] spected = { 0, 24, 25, 29 };
            Assert.AreEqual(indexes.ToArray(), spected);
        }

        [Test]
        public void ConstructCombinationsTest()
        {
            List<IPosition> positions = new List<IPosition>();
            for(int i = 0; i < 10; i++)
            {
                CPos cpos = new CPos(i);
                positions.Add(cpos);
            }

            List<Tuple<IPosition, IPosition>> expressions = ASTProgram.ConstructCombinations(positions, positions);

            Assert.AreEqual(expressions.Count, 100);
        }
    }
}