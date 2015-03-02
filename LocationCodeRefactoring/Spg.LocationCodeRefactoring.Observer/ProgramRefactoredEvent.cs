using System.Collections.Generic;
using LocationCodeRefactoring.Spg.LocationRefactor.Transformation;

namespace Spg.LocationCodeRefactoring.Observer
{
    public class ProgramRefactoredEvent
    {
        public List<Transformation> transformations { get; set; }

        public ProgramRefactoredEvent(List<Transformation> transformations) {
            this.transformations = transformations;
        }
    }
}