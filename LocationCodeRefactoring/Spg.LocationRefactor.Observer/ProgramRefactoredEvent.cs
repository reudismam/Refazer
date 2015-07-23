using System.Collections.Generic;
using Spg.LocationRefactor.Transform;

namespace Spg.LocationRefactor.Observer
{
    public class ProgramRefactoredEvent
    {
        public List<Transformation> transformations { get; set; }

        public ProgramRefactoredEvent(List<Transformation> transformations) {
            this.transformations = transformations;
        }
    }
}

