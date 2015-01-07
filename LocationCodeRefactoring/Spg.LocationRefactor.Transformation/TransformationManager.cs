using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.TextRegion;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Transformation
{
    public class TransformationManager
    {
        /// <summary>
        /// Return program able to transform locations.
        /// </summary>
        /// <param name="regionsBeforeEdit">Regions before edit list</param>
        /// <param name="regionsAfterEdit">Regions after edit list</param>
        /// <param name="color">Color of statement</param>
        /// <param name="prog">Locations program</param>
        /// <returns></returns>
        public static SynthesizedProgram TransformationProgram(List<Tuple<ListNode, ListNode>> examples)
        {  
            ASTProgram program = new ASTProgram();

            List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
            SynthesizedProgram validated = synthesizedProgs.Single();

            return validated;
        }
    }
}
